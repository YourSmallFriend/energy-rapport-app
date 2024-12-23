﻿namespace energy_raport_app;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class DbClass
{
    private static string connectionString;

    public DbClass(string connectionString)
    {
        DbClass.connectionString = connectionString;
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

                string query = "SELECT id, naam, gebruikersnaam, email, wachtwoord_hash, aanmaakdatum FROM gebruikers";
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
                                gebruikersnaam = reader.GetString("gebruikersnaam"),
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
    public User GetUser(string gebruikersnaam)
    {
        User user = null;

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT id, naam, gebruikersnaam, email, wachtwoord_hash, aanmaakdatum FROM gebruikers WHERE gebruikersnaam = @gebruikersnaam";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@gebruikersnaam", gebruikersnaam);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32("id"),
                                Naam = reader.GetString("naam"),
                                gebruikersnaam = reader.GetString("gebruikersnaam"),
                                Email = reader.GetString("email"),
                                wachtwoord_hash = reader.GetString("wachtwoord_hash"),
                                AanmaakDatum = reader.GetDateTime("aanmaakdatum")
                            };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fout bij het ophalen van gebruiker: " + ex.Message);
        }

        return user;
    }
    public User GetUser(int id)
    {
        User user = null;

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT id, naam, gebruikersnaam, email, wachtwoord_hash, aanmaakdatum FROM gebruikers WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32("id"),
                                Naam = reader.GetString("naam"),
                                gebruikersnaam = reader.GetString("gebruikersnaam"),
                                Email = reader.GetString("email"),
                                wachtwoord_hash = reader.GetString("wachtwoord_hash"),
                                AanmaakDatum = reader.GetDateTime("aanmaakdatum")
                            };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fout bij het ophalen van gebruiker: " + ex.Message);
        }

        return user;
    }

    // Methode om een gebruiker toe te voegen aan de database
    public void AddUser(User user)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO gebruikers (naam, gebruikersnaam, email, wachtwoord_hash, aanmaakdatum) VALUES (@naam, @gebruikersnaam, @email, @wachtwoord_hash, @aanmaakdatum)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@naam", user.Naam);
                    command.Parameters.AddWithValue("@gebruikersnaam", user.gebruikersnaam);
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
     // Methode om stroomgegevens in te voegen
    public void AddElectricityData(int gebruiker_id, DateTime opnameDatum, int standNormaal, int standDal, int terugleveringNormaal, int terugleveringDal)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO stroomverbruik 
                                (gebruiker_id, opnamedatum, stand_normaal, stand_dal, teruglevering_normaal, teruglevering_dal) 
                                VALUES (@gebruiker_id, @opnameDatum, @standNormaal, @standDal, @terugleveringNormaal, @terugleveringDal)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@gebruiker_id", gebruiker_id);
                    command.Parameters.AddWithValue("@opnameDatum", opnameDatum);
                    command.Parameters.AddWithValue("@standNormaal", standNormaal);
                    command.Parameters.AddWithValue("@standDal", standDal);
                    command.Parameters.AddWithValue("@terugleveringNormaal", terugleveringNormaal);
                    command.Parameters.AddWithValue("@terugleveringDal", terugleveringDal);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fout bij het toevoegen van stroomgegevens: " + ex.Message);
        }
    }

    // Methode om gasgegevens in te voegen
    public void AddGasData(int gebruiker_id, DateTime opnameDatum, int gasStand)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO gasverbruik 
                                (gebruiker_id, opnamedatum, gas_stand) 
                                VALUES (@gebruiker_id, @opnameDatum, @gasStand)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@gebruiker_id", gebruiker_id);
                    command.Parameters.AddWithValue("@opnameDatum", opnameDatum);
                    command.Parameters.AddWithValue("@gasStand", gasStand);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fout bij het toevoegen van gasgegevens: " + ex.Message);
        }
    }
    
    // haal gas gegevens op 
    public static List<GasClass.GasData> GetGasData(int gebruiker_id)
    {
        List<GasClass.GasData> gasData = new List<GasClass.GasData>();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT gebruiker_id, opnamedatum, gas_stand FROM gasverbruik";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gasData.Add(new GasClass.GasData
                            {
                                gebruiker_id = reader.GetInt32("gebruiker_id"),
                                opnamedatum = reader.GetDateTime("opnamedatum"),
                                gas_stand = reader.GetInt32("gas_stand")
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fout bij het ophalen van gasgegevens: " + ex.Message);
        }

        return gasData;
    } 
    
    // haal stroom gegevens op
    public static List<ElectroClass.ElectricityData> GetElectricityData(int gebruiker_id)
    {
        List<ElectroClass.ElectricityData> electricityData = new List<ElectroClass.ElectricityData>();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT gebruiker_id, opnamedatum, stand_normaal, stand_dal, teruglevering_normaal, teruglevering_dal FROM stroomverbruik";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            electricityData.Add(new ElectroClass.ElectricityData
                            {
                                Id = reader.GetInt32("gebruiker_id"),
                                OpnameDatum = reader.GetDateTime("opnamedatum"),
                                StandNormaal = reader.GetInt32("stand_normaal"),
                                StandDal = reader.GetInt32("stand_dal"),
                                TerugleveringNormaal = reader.GetInt32("teruglevering_normaal"),
                                TerugleveringDal = reader.GetInt32("teruglevering_dal")
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fout bij het ophalen van stroomgegevens: " + ex.Message);
        }

        return electricityData;
    }
    
}
