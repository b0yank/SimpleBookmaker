namespace SimpleBookmaker.Services.BetResolvers
{
    using Contracts;
    using SimpleBookmaker.Data.Core.Enums;
    using System;
    using System.Linq;
    using System.Reflection;

    public class PlayerGameBetResolverFactory : IPlayerGameBetResolverFactory
    {
        public IPlayerGameBetResolver GetResolver(PlayerGameBetType betType)
        {
            var resolverType = Assembly.GetAssembly(typeof(PlayerGameBetResolverFactory))
                .GetTypes()
                .FirstOrDefault(t => t.IsClass
                    && (typeof(IPlayerGameBetResolver).IsAssignableFrom(t))
                    && t.Name == betType.ToString());

            if (resolverType == null)
            {
                return null;
            }

            return (IPlayerGameBetResolver)Activator.CreateInstance(resolverType);
        }
    }
}
