using Eto.Forms;
using Eto.Drawing;

namespace energy_raport_app
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Title = "Login";
            MinimumSize = new Size(300, 200);

            var usernameLabel = new Label { Text = "Username:" };
            var usernameTextBox = new TextBox();

            var passwordLabel = new Label { Text = "Password:" };
            var passwordTextBox = new PasswordBox();

            var loginButton = new Button { Text = "Login" };
            loginButton.Click += (sender, e) => HandleLogin(usernameTextBox.Text, passwordTextBox.Text);

            Content = new StackLayout
            {
                Padding = 10,
                Items =
                {
                    new StackLayoutItem(usernameLabel, HorizontalAlignment.Left),
                    new StackLayoutItem(usernameTextBox, HorizontalAlignment.Stretch),
                    new StackLayoutItem(passwordLabel, HorizontalAlignment.Left),
                    new StackLayoutItem(passwordTextBox, HorizontalAlignment.Stretch),
                    new StackLayoutItem(loginButton, HorizontalAlignment.Center)
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

        private void HandleLogin(string username, string password)
        {
            // Handle login logic here
            MessageBox.Show($"Username: {username}\nPassword: {password}", "Login Info");
        }
    }
}