using System;
using System.Collections.Generic;
using System.Linq;

namespace energy_raport_app
{
    public class GasClass
    {
        public class GasData
        {
            public int gebruiker_id { get; set; }
            public DateTime opnamedatum { get; set; }
            public int gas_stand { get; set; }

            public override string ToString()
            {
                return $"gebruiker_id: {gebruiker_id}, opnameDatum: {opnamedatum}, stand: {gas_stand}";
            }
        }

        public static List<EnergyData> ConvertGasData(List<GasData> gasData)
        {
            return gasData.Select(g => new EnergyData
            {
                OpnameDatum = g.opnamedatum,
                Stand = g.gas_stand
            }).ToList();
        }
    }
}