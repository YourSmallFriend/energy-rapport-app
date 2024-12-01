using System;

namespace energy_raport_app;

public class GasClass
{
    public class GasData
    {
        public int gebruiker_id { get; set; }
        public DateTime OpnameDatum { get; set; }
        public int gas_stand { get; set; }
        
        public override string ToString()
        {
            return $"ID: {gebruiker_id}, OpnameDatum: {OpnameDatum}, Stand: {gas_stand}";
        }
    }
}