using System;
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

            // Create the bar chart
            _cartesianChart = new CartesianChart
            {
                Dock = DockStyle.Fill
            };

            // Add controls to the form
            Controls.Add(_cartesianChart);
            Controls.Add(_viewSelector);
            Controls.Add(_yearSelector);
            Controls.Add(label);

            // Load initial data
            LoadYearSelector();
            LoadChartData("Day", DateTime.Now.Year); // Default view
        }

        private void LoadYearSelector()
        {
            var gasData = DbClass.GetGasData(_user.Id);
            if (gasData == null || !gasData.Any()) return;

            // Extract only the years that have data
            var years = gasData
                .Select(d => d.opnamedatum.Year) // Kies alleen de jaren
                .Distinct() // Zorg ervoor dat elk jaar maar één keer voorkomt
                .OrderByDescending(y => y) // Sorteer van recent naar oud
                .ToList();

            _yearSelector.Items.Clear();

            // Voeg alleen jaren toe waarvoor data beschikbaar is
            foreach (var year in years)
            {
                _yearSelector.Items.Add(year);
            }

            // Set current year as default if available
            if (_yearSelector.Items.Count > 0)
            {
                _yearSelector.SelectedIndex = 0; // Standaard het eerste jaar selecteren
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

        private void LoadChartData(string view, int year)
        {
            var gasData = DbClass.GetGasData(_user.Id);
            if (gasData == null || !gasData.Any())
            {
                MessageBox.Show("No gas data available.");
                return;
            }

            // Filter data by selected year
            gasData = gasData.Where(d => d.opnamedatum.Year == year).ToList();

            // Controleer alle datums in de dataset
            foreach (var data in gasData)
            {
                // Als de datum ongeldige waarden heeft, toon deze in de debug
                if (data.opnamedatum == null || data.opnamedatum.Year < 1900 || data.opnamedatum.Year > 9999)
                {
                    MessageBox.Show($"Invalid date encountered: {data.opnamedatum}. This record will be skipped.");
                    gasData.Remove(data); // Verwijder de ongeldige datum
                }
            }

            // Group data based on the selected view
            DateTimePoint[] dataPoints;
            Func<double, string> xAxisLabelFormatter;

            switch (view)
            {
                case "Day":
                    dataPoints = gasData
                        .Select(d => new DateTimePoint(d.opnamedatum, d.gas_stand))
                        .ToArray();
                    xAxisLabelFormatter = value =>
                    {
                        var date = DateTime.FromOADate(value);
                        return date.ToString("dd MMM yyyy"); // Bijvoorbeeld: "05 Dec 2024"
                    };
                    break;

                case "Month":
                    dataPoints = gasData
                        .GroupBy(d => d.opnamedatum.ToString("yyyy-MM"))
                        .Select(g => new DateTimePoint(DateTime.Parse(g.Key + "-01"), g.Sum(d => d.gas_stand)))
                        .ToArray();
                    xAxisLabelFormatter = value =>
                    {
                        var date = DateTime.FromOADate(value);
                        return date.ToString("MMM yyyy"); // Bijvoorbeeld: "Dec 2024"
                    };
                    break;

                case "Year":
                    dataPoints = gasData
                        .GroupBy(d => d.opnamedatum.Year)
                        .Select(g => new DateTimePoint(new DateTime(g.Key, 1, 1), g.Sum(d => d.gas_stand)))
                        .ToArray();
                    xAxisLabelFormatter = value =>
                    {
                        var date = DateTime.FromOADate(value);
                        return date.ToString("yyyy"); // Bijvoorbeeld: "2024"
                    };
                    break;

                default:  // day view
                    dataPoints = gasData
                        .Select(d => new DateTimePoint(d.opnamedatum, d.gas_stand))
                        .ToArray();
                    xAxisLabelFormatter = value =>
                    {
                        var date = DateTime.FromOADate(value);
                        return date.ToString("dd MMM yyyy"); // Bijvoorbeeld: "05 Dec 2024"
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
                    LabelsRotation = 15, // Draai de labels iets voor betere leesbaarheid
                    TextSize = 12
                }
            };

            // Herteken de grafiek
            _cartesianChart.Update();
        }
    }
}
