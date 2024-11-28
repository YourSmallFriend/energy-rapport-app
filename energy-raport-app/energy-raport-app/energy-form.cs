using Eto.Forms;
using Eto.Drawing;
namespace energy_raport_app
{
    public class EnergyForm : Form
    {
        public EnergyForm()
        {
            Title = "Energy Raport";
            MinimumSize = new Size(200, 200);
            
            var login = new Button { Text = "Login" };

            Content = new StackLayout
            {
                Padding = 10,
                Items =
                {
                    login,
                    // add more controls here
                }
            };

            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();

            var aboutCommand = new Command { MenuText = "About..." };
            aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

            // create menu
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
}