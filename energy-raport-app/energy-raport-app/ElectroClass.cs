using System;

namespace energy_raport_app;

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
}