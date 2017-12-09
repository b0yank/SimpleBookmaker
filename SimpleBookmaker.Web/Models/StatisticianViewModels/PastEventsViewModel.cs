namespace SimpleBookmaker.Web.Models.StatisticianViewModels
{
    using Services.Models.Game;
    using Services.Models.Tournament;
    using System.Collections.Generic;

    public class PastEventsViewModel
    {
        public IEnumerable<TournamentStatsListModel> PastTournaments { get; set; }

        public IEnumerable<GameStatsListModel> PastGames { get; set; }
    }
}
