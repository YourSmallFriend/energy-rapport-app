using System;
using Eto.Forms;
using Eto.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace energy_raport_app
{
    public class MainForm : Form
    {
        private DbClass db;

        public MainForm()
        {
            db = new DbClass("Server=localhost;Database=energydb;Uid=root;Pwd=;");

            Title = "Login";
            MinimumSize = new Size(300, 200);

            var usernameLabel = new Label { Text = "Email:" };
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

        private void HandleLogin(string email, string password)
        {
            if (email == "admin" && password == "admin")
            {
                OpenAdminScreen();
            }
            else
            {
                var user = VerifyCredentials(email, password);
                if (user != null)
                {
                    OpenUserAccount(user);
                }
                else
                {
                    MessageBox.Show("Invalid email or password", "Login Failed", MessageBoxButtons.OK, MessageBoxType.Error);
                }
            }
        }

        private User VerifyCredentials(string email, string password)
        {
            var hashedPassword = HashPassword(password);
            var users = db.GetUsers();
            return users.FirstOrDefault(u => u.Email == email && u.wachtwoord_hash == hashedPassword);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private void OpenUserAccount(User user)
        {
            // Open user account form or dashboard
            var userDashboard = new UserDashboard(user);
            userDashboard.Show();
        }

        private void OpenAdminScreen()
        {
            var adminScreen = new AdminScreen();
            adminScreen.Show();
        }
    }
}