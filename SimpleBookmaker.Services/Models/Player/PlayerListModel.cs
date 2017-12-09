namespace SimpleBookmaker.Services.Models.Player
{
    using Data.Models;
    using Infrastructure.AutoMapper;

    public class PlayerListModel : IMapFrom<Player>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
