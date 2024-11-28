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

                string query = "SELECT id, naam, email, aanmaakdatum FROM gebruikers";
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
}

