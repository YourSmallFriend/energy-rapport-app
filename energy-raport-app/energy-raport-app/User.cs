using System;

namespace energy_raport_app;

public partial class User
{
    public int Id { get; set; }
    public string Naam { get; set; }
    public string Email { get; set; }
    public string wachtwoord_hash { get; set; }
    public DateTime AanmaakDatum { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Naam: {Naam}, Email: {Email}, Aangemaakt op: {AanmaakDatum}";
    }
}