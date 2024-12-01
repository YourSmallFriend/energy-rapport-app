using System;

namespace energy_raport_app;

public class GasClass
{
    public class GasData
    {
        public int gebruiker_id { get; set; }
        public DateTime opnamedatum { get; set; }
        public int gas_stand { get; set; }
        
        public override string ToString()
        {
            return $"gebruiker_id: {gebruiker_id}, OpnameDatum: {opnamedatum}, Stand: {gas_stand}";
        }
    }
}