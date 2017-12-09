namespace SimpleBookmaker.Services.Models.Bet
{
    using Data.Core.Enums;

    public class GamePlayerPossibleBetListModel
    {
        public string BetCondition { get; set; }

        public PlayerGameBetType BetType { get; set; }
    }
}
