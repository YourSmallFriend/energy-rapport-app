using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;

namespace energy_raport_app
{
    public class UserDashboard : Form
    {
        private readonly User _user;
        private CartesianChart _cartesianChart;
        private readonly ComboBox _viewSelector;
        private readonly ComboBox _yearSelector;
        private readonly Button _switchDataButton;
        private bool _showGasData = true;

        // Fixed prices for gas and electricity
        private const double GasPricePerUnit = 0.79; // Example price in euros per unit
        private const double ElectricityPricePerUnit = 0.21; // Example price in euros per unit

        public UserDashboard(User user)
        {
            _user = user;

            // Window settings
            Text = "User Dashboard";
            MinimumSize = new Size(600, 400);

            // Welcome label
            var label = new Label
            {
                Text = $"Hello {_user.Naam}!",
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top
            };

            // Year selector
            _yearSelector = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _yearSelector.SelectedIndexChanged += YearSelector_SelectedIndexChanged;

            // View selector
            _viewSelector = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "Day", "Month", "Year" }
            };
            _viewSelector.SelectedIndexChanged += ViewSelector_SelectedIndexChanged;
            _viewSelector.SelectedIndex = 0; // Default to "Day"

            // Switch data button
            _switchDataButton = new Button
            {
                Text = "Switch to Electricity Data",
                Dock = DockStyle.Top
            };
            _switchDataButton.Click += SwitchDataButton_Click;

            // Create the bar chart
            _cartesianChart = new CartesianChart
            {
                Dock = DockStyle.Fill
            };

            // Add controls to the form
            Controls.Add(_cartesianChart);
            Controls.Add(_switchDataButton);
            Controls.Add(_viewSelector);
            Controls.Add(_yearSelector);
            Controls.Add(label);

            // Load initial data
            LoadYearSelector();
            LoadChartData("Day", DateTime.Now.Year); // Default view
        }

        private void LoadYearSelector()
        {
            var data = _showGasData ? GasClass.ConvertGasData(DbClass.GetGasData(_user.Id)) : ElectroClass.ConvertElectricityData(DbClass.GetElectricityData(_user.Id));
            if (data == null || !data.Any()) return;

            // Extract only the years that have data
            var years = data
                .Select(d => d.OpnameDatum.Year) // Choose only the years
                .Distinct() // Ensure each year appears only once
                .OrderByDescending(y => y) // Sort from recent to old
                .ToList();

            _yearSelector.Items.Clear();

            // Add only years for which data is available
            foreach (var year in years)
            {
                _yearSelector.Items.Add(year);
            }

            // Set current year as default if available
            if (_yearSelector.Items.Count > 0)
            {
                _yearSelector.SelectedIndex = 0; // Default to the first year
            }
        }

        private void ViewSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_viewSelector.SelectedItem == null || _yearSelector.SelectedItem == null) return;

            var selectedView = _viewSelector.SelectedItem.ToString();
            var selectedYear = (int)_yearSelector.SelectedItem;

            LoadChartData(selectedView, selectedYear);
        }

        private void YearSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_viewSelector.SelectedItem == null || _yearSelector.SelectedItem == null) return;

            var selectedView = _viewSelector.SelectedItem.ToString();
            var selectedYear = (int)_yearSelector.SelectedItem;

            LoadChartData(selectedView, selectedYear);
        }

        private void SwitchDataButton_Click(object sender, EventArgs e)
        {
            _showGasData = !_showGasData;
            _switchDataButton.Text = _showGasData ? "Switch to Electricity Data" : "Switch to Gas Data";
            LoadYearSelector();
            LoadChartData(_viewSelector.SelectedItem.ToString(), (int)_yearSelector.SelectedItem);
        }

        private void LoadChartData(string view, int year)
        {
            List<EnergyData> data = _showGasData
                ? GasClass.ConvertGasData(DbClass.GetGasData(_user.Id))
                : ElectroClass.ConvertElectricityData(DbClass.GetElectricityData(_user.Id));

            if (data == null || !data.Any())
            {
                MessageBox.Show(_showGasData ? "No gas data available." : "No electricity data available.");
                return;
            }

            // Filter data by selected year
            data = data.Where(d => d.OpnameDatum.Year == year).ToList();

            // Group data based on the selected view
            DateTimePoint[] dataPoints;
            Func<double, string> xAxisLabelFormatter;

            switch (view)
            {
                case "Day":
                    dataPoints = data
                        .Select(d => new DateTimePoint(d.OpnameDatum, d.Stand * (_showGasData ? GasPricePerUnit : ElectricityPricePerUnit)))
                        .ToArray();
                    xAxisLabelFormatter = value =>
                    {
                        var date = DateTime.FromOADate(value);
                        return date.ToString("dd MMM yyyy");
                    };
                    break;

                case "Month":
                    dataPoints = data
                        .GroupBy(d => d.OpnameDatum.ToString("yyyy-MM"))
                        .Select(g => new DateTimePoint(DateTime.Parse(g.Key + "-01"), g.Sum(d => d.Stand) * (_showGasData ? GasPricePerUnit : ElectricityPricePerUnit)))
                        .ToArray();
                    xAxisLabelFormatter = value =>
                    {
                        var date = DateTime.FromOADate(value);
                        return date.ToString("MMM yyyy");
                    };
                    break;

                case "Year":
                    dataPoints = data
                        .GroupBy(d => d.OpnameDatum.Year)
                        .Select(g => new DateTimePoint(new DateTime(g.Key, 1, 1), g.Sum(d => d.Stand) * (_showGasData ? GasPricePerUnit : ElectricityPricePerUnit)))
                        .ToArray();
                    xAxisLabelFormatter = value =>
                    {
                        var date = DateTime.FromOADate(value);
                        return date.ToString("yyyy");
                    };
                    break;

                default: // Default to "Day"
                    dataPoints = data
                        .Select(d => new DateTimePoint(d.OpnameDatum, d.Stand * (_showGasData ? GasPricePerUnit : ElectricityPricePerUnit)))
                        .ToArray();
                    xAxisLabelFormatter = value =>
                    {
                        var date = DateTime.FromOADate(value);
                        return date.ToString("dd MMM yyyy");
                    };
                    break;
            }

            // Update the chart
            _cartesianChart.Series = new ISeries[]
            {
                new ColumnSeries<DateTimePoint>
                {
                    Values = dataPoints
                }
            };

            // Update the X-axis with custom labels
            _cartesianChart.XAxes = new[]
            {
                new Axis
                {
                    Labeler = xAxisLabelFormatter,
                    LabelsRotation = 15,
                    TextSize = 12
                }
            };

            // Redraw the chart
            _cartesianChart.Update();
        }
    }
}
