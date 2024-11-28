namespace energy_raport_app;
using Eto.Forms;
using Eto.Drawing;

public class UserDashboard: Form
{
    public UserDashboard()
    {
        Title = "User Dashboard";
        MinimumSize = new Size(400, 400);

        var label = new Label { Text = "Hello User!" };

        Content = new StackLayout
        {
            Padding = 10,
            Items =
            {
                label
            }
        };

        var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
        quitCommand.Executed += (sender, e) => Application.Instance.Quit();

        var aboutCommand = new Command { MenuText = "About..." };
        aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

        Menu = new MenuBar
        {
            Items =
            {
                // File submenu
                // new SubMenuItem { Text = "&Edit", Items = { /* commands/items */ } },
                // new SubMenuItem { Text = "&View", Items = { /* commands/items */ } },
            },
            ApplicationItems =
            {
                // application (OS X) or file menu (others)
                new ButtonMenuItem { Text = "&Preferences..." },
            },
            QuitItem = quitCommand,
            AboutItem = aboutCommand
        };
    }
}