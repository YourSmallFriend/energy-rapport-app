using System;
using System.Linq;
using energy_raport_app;
using Eto.Drawing;
using Eto.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Eto;

public class UserDashboard : Form
{
    private User _user;

    public UserDashboard(User user)
    {
        _user = user;

    // Window settings
    Title = "User Dashboard";
    MinimumSize = new Size(600, 400);

    // Welcome label
    var label = new Label
    {
        Text = $"Hello {_user.Naam}!",
        Font = SystemFonts.Bold(14),
        TextAlignment = TextAlignment.Center
    };

    try
    {
        // Fetch gas data for the user
        var gasData = DbClass.GetGasData(gebruiker_id: _user.Id).Where(g => g.gebruiker_id == _user.Id).ToList();

        // Log the fetched data
        Console.WriteLine("Fetched gas data:");
        foreach (var data in gasData)
        {
            Console.WriteLine($"Date: {data.opnamedatum}, Gas Stand: {data.gas_stand}");
        }

        // Check if there is gas data
        if (gasData.Any())
        {
            var gasPlot = new PlotView
            {
                Model = new PlotModel
                {
                    Title = "Gas Usage",
                    Axes =
                    {
                        new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "dd/MM/yyyy" },
                        new LinearAxis { Position = AxisPosition.Left, Title = "Gas Usage" }
                    },
                    Series =
                    {
                        new LineSeries
                        {
                            Title = "Gas Usage",
                            ItemsSource = gasData,
                            DataFieldX = "opnamedatum",
                            DataFieldY = "gas_stand"
                        }
                    }
                }
            };

            // Add content to the layout
            Content = new StackLayout
            {
                Padding = 10,
                Spacing = 10,
                Items =
                {
                    label,
                    gasPlot
                }
            };
        }
        else
        {
            // Show a message if there is no gas data
            Content = new StackLayout
            {
                Padding = 10,
                Spacing = 10,
                Items =
                {
                    label,
                    new Label { Text = "Geen gasdata beschikbaar.", TextAlignment = TextAlignment.Center }
                }
            };
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error: " + ex.Message);
        MessageBox.Show("Stack Trace: " + ex.StackTrace);

        Content = new StackLayout
        {
            Padding = 10,
            Spacing = 10,
            Items =
            {
                label,
                new Label { Text = "Er is een fout opgetreden: " + ex.Message, TextAlignment = TextAlignment.Center }
            }
        };
    }
        // Menubalk
        Menu = CreateMenu();
    }
   
    private MenuBar CreateMenu()
    {
        // Maak menu-items
        var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
        quitCommand.Executed += (sender, e) => Application.Instance.Quit();

        var aboutCommand = new Command { MenuText = "About..." };
        aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

        // Maak de menubalk
        return new MenuBar
        {
            ApplicationItems =
            {
                new ButtonMenuItem { Text = "&Preferences..." }
            },
            QuitItem = quitCommand,
            AboutItem = aboutCommand
        };
    }
}
