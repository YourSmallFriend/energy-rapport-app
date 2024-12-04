using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Eto.Drawing;
using Eto.Forms;

namespace energy_raport_app
{
    public class AdminScreen : Form
    {
        private DbClass db;
        private GridView userGrid;
        private Button openUserScreenButton;

        public AdminScreen()
        {
            db = new DbClass("Server=localhost;Database=energydb;Uid=root;Pwd=;");

            Title = "Admin Screen";
            MinimumSize = new Size(600, 400);

            var label = new Label { Text = "Hello Admin!" };

            // Add user button
            var addUserButton = new Button { Text = "Add User" };
            addUserButton.Click += (sender, e) => new AddUserDialog().ShowDialog(this);

            // Open user screen button
            openUserScreenButton = new Button { Text = "Open User Screen", Enabled = false };
            openUserScreenButton.Click += OpenUserScreenButton_Click;

            // Create DataGrid
            userGrid = new GridView
            {
                DataStore = GetUserList(),
                Columns =
                {
                    new GridColumn { DataCell = new TextBoxCell { Binding = Binding.Property<User, string>(u => u.Id.ToString()) }, HeaderText = "ID" },
                    new GridColumn { DataCell = new TextBoxCell { Binding = Binding.Property<User, string>(u => u.gebruikersnaam) }, HeaderText = "Gebruikersnaam" },
                    new GridColumn { DataCell = new TextBoxCell { Binding = Binding.Property<User, string>(u => u.Naam) }, HeaderText = "Naam" }
                }
            };
            userGrid.SelectionChanged += UserGrid_SelectionChanged;

            Content = new StackLayout
            {
                Padding = 10,
                Items =
                {
                    label,
                    addUserButton,
                    openUserScreenButton,
                    userGrid
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

        private void UserGrid_SelectionChanged(object sender, EventArgs e)
        {
            openUserScreenButton.Enabled = userGrid.SelectedItem != null;
        }

        private void OpenUserScreenButton_Click(object sender, EventArgs e)
        {
            var selectedUser = userGrid.SelectedItem as User;
            if (selectedUser != null)
            {
                var userScreen = new UserScreen(selectedUser, db);
                userScreen.Show();
            }
        }

        private List<User> GetUserList()
        {
            return db.GetUsers();
        }

        public void RefreshUserGrid()
        {
            userGrid.DataStore = GetUserList();
        }
    }

    public class AddUserDialog : Dialog
    {
        public AddUserDialog()
        {
            Title = "Add User";
            ClientSize = new Size(200, 200);

            var nameLabel = new Label { Text = "Name:" };
            var nameTextBox = new TextBox();

            var gebruikersnaamLabel = new Label { Text = "Gebruikersnaam:" };
            var gebruikersnaamTextBox = new TextBox();

            var emailLabel = new Label { Text = "Email:" };
            var emailTextBox = new TextBox();

            var passwordLabel = new Label { Text = "Password:" };
            var passwordTextBox = new PasswordBox();

            var addButton = new Button { Text = "Add" };
            addButton.Click += (sender, e) => HandleAddUser(nameTextBox.Text, gebruikersnaamTextBox.Text, emailTextBox.Text, passwordTextBox.Text);

            Content = new StackLayout
            {
                Padding = 10,
                Items =
                {
                    new StackLayoutItem(nameLabel, HorizontalAlignment.Left),
                    new StackLayoutItem(nameTextBox, HorizontalAlignment.Stretch),
                    new StackLayoutItem(gebruikersnaamLabel, HorizontalAlignment.Left),
                    new StackLayoutItem(gebruikersnaamTextBox, HorizontalAlignment.Stretch),
                    new StackLayoutItem(emailLabel, HorizontalAlignment.Left),
                    new StackLayoutItem(emailTextBox, HorizontalAlignment.Stretch),
                    new StackLayoutItem(passwordLabel, HorizontalAlignment.Left),
                    new StackLayoutItem(passwordTextBox, HorizontalAlignment.Stretch),
                    new StackLayoutItem(addButton, HorizontalAlignment.Center)
                }
            };
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

        private void HandleAddUser(string name, string gebruikersnaam, string email, string password)
        {
            var hashedPassword = HashPassword(password);

            var user = new User
            {
                Naam = name,
                gebruikersnaam = gebruikersnaam,
                Email = email,
                wachtwoord_hash = hashedPassword,
                AanmaakDatum = DateTime.Now
            };

            var db = new DbClass("Server=localhost;Database=energydb;Uid=root;Pwd=;");
            db.AddUser(user);
            MessageBox.Show("User added successfully!");
        }

        public void ShowDialog(AdminScreen adminScreen)
        {
            ShowModal();
            adminScreen.RefreshUserGrid();
        }
    }
}