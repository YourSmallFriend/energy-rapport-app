using System;
using System.Collections.Generic;
using System.Linq;

namespace energy_raport_app
{
    public class ElectroClass
    {
        public class ElectricityData
        {
            public int Id { get; set; }
            public DateTime OpnameDatum { get; set; }
            public int StandNormaal { get; set; }
            public int StandDal { get; set; }
            public int TerugleveringNormaal { get; set; }
            public int TerugleveringDal { get; set; }

            public override string ToString()
            {
                return $"ID: {Id}, OpnameDatum: {OpnameDatum}, StandNormaal: {StandNormaal}, StandDal: {StandDal}, TerugleveringNormaal: {TerugleveringNormaal}, TerugleveringDal: {TerugleveringDal}";
            }
        }

        public static List<EnergyData> ConvertElectricityData(List<ElectricityData> electricityData)
        {
            return electricityData.Select(e => new EnergyData
            {
                OpnameDatum = e.OpnameDatum,
                Stand = e.StandNormaal + e.StandDal - e.TerugleveringNormaal - e.TerugleveringDal
            }).ToList();
        }
    }
}