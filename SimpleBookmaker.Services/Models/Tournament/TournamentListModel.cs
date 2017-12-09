namespace SimpleBookmaker.Services.Models.Tournament
{
    using Data.Models;
    using Infrastructure.AutoMapper;

    public class TournamentListModel : IMapFrom<Tournament>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
