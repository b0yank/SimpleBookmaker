namespace SimpleBookmaker.Web.Areas.Admin.Models.Player
{
    using Services.Models.Player;
    using System.Collections.Generic;

    public class TeamPlayersListModel
    {
        public IEnumerable<PlayerListModel> Players { get; set; }

        public int TeamId { get; set; }

        public string Team { get; set; }
    }
}
