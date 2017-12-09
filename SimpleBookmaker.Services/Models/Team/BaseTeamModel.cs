namespace SimpleBookmaker.Services.Models.Team
{
    using Data.Models;
    using Infrastructure.AutoMapper;

    public class BaseTeamModel : IMapFrom<Team>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
