using energy_raport_app;
using Eto.Drawing;
using Eto.Forms;

public class UserDashboard : Form
{
    private User _user;

    public UserDashboard(User user)
    {
        _user = user;

        Title = "User Dashboard";
        MinimumSize = new Size(400, 400);

        var label = new Label { Text = $"Hello {_user.Naam}!" };

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