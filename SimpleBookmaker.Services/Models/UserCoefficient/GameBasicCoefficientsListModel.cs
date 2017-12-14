namespace SimpleBookmaker.Services.Models.UserCoefficient
{
    using Game;
    using System;

    public class GameBasicCoefficientsListModel
    {
        public int Id { get; set; }

        public GameTeamsModel Teams { get;set; }

        public DateTime Kickoff { get; set; }

        public UserGameCoefficientModel CoefficientHome { get; set; }

        public UserGameCoefficientModel CoefficientDraw { get; set; }

        public UserGameCoefficientModel CoefficientAway { get; set; }
    }
}
