// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using ExtCore.Data.Abstractions;

namespace BaseTest.Common
{
    /// <summary>
    /// A class that mimics System.IServiceProvider but provides access only to
    /// what is injected to unit tests thanks to DatabaseFixture, here an instance is passed directly to constructor.
    /// </summary>
    public class MockedServiceProvider : IServiceProvider
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