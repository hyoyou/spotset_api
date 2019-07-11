using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Moq;

namespace SpotSet.Api.Tests.Mocks
{
    public class MockConfiguration : IConfiguration
    {
        public IConfigurationSection GetSection(string key)
        {
            return new Mock<IConfigurationSection>().Object;
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new System.NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new System.NotImplementedException();
        }

        public string this[string key]
        {
            get => key;
            set => throw new System.NotImplementedException();
        }
    }
}