using System;
using Eto.Drawing;
using Eto.Forms;
using System.Security.Cryptography;
using System.Text;

namespace energy_raport_app;

public class AdminScreen : Form
{
    //zeg hallo
    public AdminScreen()
    {
        Title = "Admin Screen";
        MinimumSize = new Size(400, 400);

        var label = new Label { Text = "Hello Admin!" };
        
        //add user button
        var addUserButton = new Button { Text = "Add User" };
        addUserButton.Click += (sender, e) => new AddUserDialog().ShowDialog(this);

        Content = new StackLayout
        {
            Padding = 10,
            Items =
            {
                label,
                addUserButton
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

public class AddUserDialog
{
    //add user dialog
    public AddUserDialog()
    {
        var dialog = new Dialog { Title = "Add User" };
        dialog.ClientSize = new Size(200, 200);

        var nameLabel = new Label { Text = "Name:" };
        var nameTextBox = new TextBox();

        var emailLabel = new Label { Text = "Email:" };
        var emailTextBox = new TextBox();

        var passwordLabel = new Label { Text = "Password:" };
        var passwordTextBox = new PasswordBox();

        var addButton = new Button { Text = "Add" };
        addButton.Click += (sender, e) => HandleAddUser(nameTextBox.Text, emailTextBox.Text, passwordTextBox.Text);

        dialog.Content = new StackLayout
        {
            Padding = 10,
            Items =
            {
                new StackLayoutItem(nameLabel, HorizontalAlignment.Left),
                new StackLayoutItem(nameTextBox, HorizontalAlignment.Stretch),
                new StackLayoutItem(emailLabel, HorizontalAlignment.Left),
                new StackLayoutItem(emailTextBox, HorizontalAlignment.Stretch),
                new StackLayoutItem(passwordLabel, HorizontalAlignment.Left),
                new StackLayoutItem(passwordTextBox, HorizontalAlignment.Stretch),
                new StackLayoutItem(addButton, HorizontalAlignment.Center)
            }
        };

        dialog.ShowModal();
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

    private void HandleAddUser(string name, string email, string password)
    {
        var hashedPassword = HashPassword(password);

        var user = new User
        {
            Naam = name,
            Email = email,
            wachtwoord_hash = hashedPassword,
            AanmaakDatum = DateTime.Now
        };

        var db = new DbClass("Server=localhost;Database=energydb;Uid=root;Pwd=;");
        db.AddUser(user);
    }

    public void ShowDialog(AdminScreen adminScreen)
    {
        throw new System.NotImplementedException();
    }
}