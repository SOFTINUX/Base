using System;
using ExtCore.Data.Abstractions;
using Xunit;

namespace SecurityTest.Util
{
    /// <summary>
    /// A class that mimics System.IServiceProvider but provides access only to
    /// what is injected to unit tests thanks to DatabaseFixture, here an instance is passed directly to constructor.
    /// </summary>
    public class MockedServiceProvider : System.IServiceProvider
    {
        private readonly DatabaseFixture _fixture;
        public MockedServiceProvider(DatabaseFixture fixture_)
        {
            _fixture = fixture_;
        }

        object IServiceProvider.GetService(Type serviceType)
        {
           if(serviceType == typeof(IStorage)) {
               return _fixture.DatabaseContext.Storage;
           }
           throw new NotImplementedException("Type not provided by simple service provider mockup: " + serviceType);
        }
    }
}