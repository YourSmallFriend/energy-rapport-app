using System;
using System.IO;
using energy_raport_app;
using Eto.Drawing;
using Eto.Forms;

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

        // WebView control for Google Chart
        var webView = new WebView
        {
            Size = new Size(900, 500)
        };

        // Load the HTML file
        var htmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chart.html");
        webView.Url = new Uri("file:///C:/Users/samuel/OneDrive/Documenten/GitHub/energy-rapport-app/energy-raport-app/energy-raport-app/chart.html");

        // Add content to the layout
        Content = new StackLayout
        {
            Padding = 10,
            Spacing = 10,
            Items =
            {
                label,
                webView
            }
        };

        // Menubalk
        Menu = CreateMenu();
    }

    private MenuBar CreateMenu()
    {
        var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
        quitCommand.Executed += (sender, e) => Application.Instance.Quit();

        var aboutCommand = new Command { MenuText = "About..." };
        aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

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