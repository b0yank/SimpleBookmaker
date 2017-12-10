namespace SimpleBookmaker.Services.Models.Coefficient
{
    using System;

    public class GameBasicCoefficientsListModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Kickoff { get; set; }

        public int CoefficientHomeId { get; set; }
        public double CoefficientHome { get; set; }

        public int CoefficientDrawId { get; set; }
        public double CoefficientDraw { get; set; }

        public int CoefficientAwayId { get; set; }
        public double CoefficientAway { get; set; }
    }
}
