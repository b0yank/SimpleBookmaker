namespace SimpleBookmaker.Web.Areas.Admin.Models.Tournament
{
    using Data.Models;
    using Services.Infrastructure.AutoMapper;

    public class TournamentEditModel : TournamentAddModel, IMapFrom<Tournament>
    {
        public int Id { get; set; }
    }
}
