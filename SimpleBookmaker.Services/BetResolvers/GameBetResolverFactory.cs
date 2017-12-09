namespace SimpleBookmaker.Services.BetResolvers
{
    using Contracts;
    using Data.Core.Enums;
    using System;
    using System.Linq;
    using System.Reflection;

    public class GameBetResolverFactory : IGameBetResolverFactory
    {
        public IGameBetResolver GetResolver(GameBetType betType)
        {
            var resolverType = Assembly.GetAssembly(typeof(GameBetResolverFactory))
                .GetTypes()
                .FirstOrDefault(t => t.IsClass
                    && (typeof(IGameBetResolver).IsAssignableFrom(t))
                    && t.Name == betType.ToString());

            if (resolverType == null)
            {
                return null;
            }

            return (IGameBetResolver) Activator.CreateInstance(resolverType);
        }
    }
}
