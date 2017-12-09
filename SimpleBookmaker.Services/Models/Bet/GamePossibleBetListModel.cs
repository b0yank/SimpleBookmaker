namespace SimpleBookmaker.Services.Models.Bet
{
    using Data.Core.Enums;
    using System.Collections.Generic;

    public class GamePossibleBetListModel
    {
        public GameBetType BetType { get; set; }

        public ICollection<BetSide> BetSides { get; set; }

        public string BetCondition { get; set; }
    }
}
