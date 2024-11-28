namespace energy_raport_app;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class DbClass
{
    private string connectionString;

    // database connectie string om met de mysql database te verbinden var connectString = "Server=localhost;Database=Huisarts;Uid=root;Pwd=;";
    public DbClass(string connectionString)
    {
        var connectString = "Server=localhost;Database=energydb;Uid=root;Pwd=;";
        this.connectionString = connectionString;
    }

    // Methode om alle gebruikers uit de database op te halen
    public List<User> GetUsers()
    {
        List<User> users = new List<User>();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT id, naam, email,wachtwoord_hash, aanmaakdatum FROM gebruikers";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Id = reader.GetInt32("id"),
                                Naam = reader.GetString("naam"),
                                Email = reader.GetString("email"),
                                wachtwoord_hash = reader.GetString("wachtwoord_hash"),
                                AanmaakDatum = reader.GetDateTime("aanmaakdatum")
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fout bij het ophalen van gebruikers: " + ex.Message);
        }

        return users;
    }

    // Methode om een gebruiker toe te voegen aan de database
    public void AddUser(User user)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO gebruikers (naam, email, wachtwoord_hash, aanmaakdatum) VALUES (@naam, @email, @wachtwoord_hash, @aanmaakdatum)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@naam", user.Naam);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@wachtwoord_hash", user.wachtwoord_hash);
                    command.Parameters.AddWithValue("@aanmaakdatum", user.AanmaakDatum);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fout bij het toevoegen van gebruiker: " + ex.Message);
        }
    }
}

