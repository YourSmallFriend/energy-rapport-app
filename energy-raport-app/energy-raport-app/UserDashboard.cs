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

        // Vensterinstellingen
        Title = "User Dashboard";
        MinimumSize = new Size(600, 400);

        // Welkomstlabel
        var label = new Label
        {
            Text = $"Hello {_user.Naam}!",
            Font = SystemFonts.Bold(14),
            TextAlignment = TextAlignment.Center
        };

        //Maak een grafiek voor de gas data van de gebruiker doe dit door de id van de gebruiker te vergelijken met de gebruiker id in de gas data
        var gasData = DbClass.GetGasData(gebruikerId: _user.Id).Where(g => g.gebruiker_id == _user.Id).ToList();
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
                        DataFieldX = "OpnameDatum",
                        DataFieldY = "gas_stand"
                    }
                }
            }
        };
        
        // Voeg inhoud toe aan de layout
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
