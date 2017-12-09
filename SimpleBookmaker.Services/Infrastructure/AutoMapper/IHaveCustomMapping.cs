using AutoMapper;

namespace SimpleBookmaker.Services.Infrastructure.AutoMapper
{
    public interface IHaveCustomMapping
    {
        void ConfigureMapping(Profile mapper);
    }
}
