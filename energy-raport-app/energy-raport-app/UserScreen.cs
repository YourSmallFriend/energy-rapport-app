using System;
using System.IO;
using energy_raport_app;
using Eto.Drawing;
using Eto.Forms;

public class UserScreen : Form
{
    private DbClass db;

    public UserScreen(User user)
    {

        Title = "User Screen";
        ClientSize = new Size(400, 300);

        var nameLabel = new Label { Text = "Name: " + user.Naam };
        var gebruikersnaamLabel = new Label { Text = "Gebruikersnaam: " + user.gebruikersnaam };

        // Knoppen voor import
        var importElectricityButton = new Button { Text = "Importeer Stroom CSV" };
        importElectricityButton.Click += ImportElectricityCsv;

        var importGasButton = new Button { Text = "Importeer Gas CSV" };
        importGasButton.Click += ImportGasCsv;

        // Layout
        Content = new StackLayout
        {
            Padding = 10,
            Items =
            {
                nameLabel,
                gebruikersnaamLabel,
                importElectricityButton,
                importGasButton
            }
        };
    }

    private void ImportElectricityCsv(object sender, EventArgs e)
{
    var openFileDialog = new OpenFileDialog
    {
        Title = "Selecteer stroom CSV-bestand",
        Filters = { new FileFilter("CSV Files", ".csv") }
    };

    if (openFileDialog.ShowDialog(this) == DialogResult.Ok)
    {
        try
        {
            var lines = File.ReadAllLines(openFileDialog.FileName);

            // Sla de eerste regel over (headers)
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                var values = line.Split(';');

                DateTime opnameDatum = DateTime.Parse(values[0]);
                int standNormaal = int.Parse(values[1]);
                int standDal = int.Parse(values[2]);
                int terugleveringNormaal = int.Parse(values[3]);
                int terugleveringDal = int.Parse(values[4]);

                // Voeg stroomgegevens toe voor de huidige gebruiker (bijvoorbeeld user.Id = 1)
                db.AddElectricityData(1, opnameDatum, standNormaal, standDal, terugleveringNormaal, terugleveringDal);
            }

            MessageBox.Show(this, "Stroomgegevens succesvol geïmporteerd!", MessageBoxType.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Fout bij importeren: " + ex.Message, MessageBoxType.Error);
        }
    }
}

private void ImportGasCsv(object sender, EventArgs e)
{
    var openFileDialog = new OpenFileDialog
    {
        Title = "Selecteer gas CSV-bestand",
        Filters = { new FileFilter("CSV Files", ".csv") }
    };

    if (openFileDialog.ShowDialog(this) == DialogResult.Ok)
    {
        try
        {
            var lines = File.ReadAllLines(openFileDialog.FileName);

            // Sla de eerste regel over (headers)
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                var values = line.Split(';');

                DateTime opnameDatum = DateTime.Parse(values[0]);
                int gasStand = int.Parse(values[1]);

                // Voeg gasgegevens toe voor de huidige gebruiker (bijvoorbeeld user.Id = 1)
                db.AddGasData(1, opnameDatum, gasStand);
            }

            MessageBox.Show(this, "Gasgegevens succesvol geïmporteerd!", MessageBoxType.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "Fout bij importeren: " + ex.Message, MessageBoxType.Error);
        }
    }
}

}
