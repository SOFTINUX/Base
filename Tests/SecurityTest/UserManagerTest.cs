// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using Security;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Security.Enums.Debug;
using SecurityTest.Util;
using Xunit;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class UserManagerTest : BaseTest
    {
        public UserManagerTest(DatabaseFixture fixture_)
        {
            _fixture = fixture_;

        }

        [Fact]
        public void TestNoCredentialType()
        {
           // no need of transaction (no db write)
            UserManager userManager = new UserManager(_fixture.DatabaseContext,
                ((TestContext) _fixture.DatabaseContext).LoggerFactory);
            Assert.Null(userManager.Login("wrong credential type", "user", "123password"));
            Assert.Equal(UserManagerErrorCode.NoCredentialType, userManager.ErrorCode);

        }

        [Fact]
        public void TestNoIdentifierFound()
        {
            // no need of transaction (no db write)
            UserManager userManager = new UserManager(_fixture.DatabaseContext,
                ((TestContext)_fixture.DatabaseContext).LoggerFactory);
            Assert.Null(userManager.Login(Security.Enums.CredentialType.Email, "no such user", "123password"));
            Assert.Equal(UserManagerErrorCode.NoMatchCredentialTypeAndIdentifier, userManager.ErrorCode);

        }

        [Fact]
        public void TestWrongSecret()
        {
            // no need of transaction (no db write)
            UserManager userManager = new UserManager(_fixture.DatabaseContext,
                ((TestContext)_fixture.DatabaseContext).LoggerFactory);
            Assert.Null(userManager.Login(Security.Enums.CredentialType.Email, "user", "not the password"));
            Assert.Equal(UserManagerErrorCode.SecretVerificationFailed, userManager.ErrorCode);

        }

        [Fact]
        public void TestCorrectCase()
        {
            // no need of transaction (no db write)
            UserManager userManager = new UserManager(_fixture.DatabaseContext,
                ((TestContext)_fixture.DatabaseContext).LoggerFactory);
            User user = userManager.Login(Security.Enums.CredentialType.Email, "user", "123password");
            Assert.NotNull(user);
            Assert.Equal(_fixture.GetRepository<IUserRepository>().WithCredentialIdentifier("user").Id, user.Id);
            Assert.Equal(UserManagerErrorCode.None, userManager.ErrorCode);

        }

        [Fact]
        public void TestGetAllClaims()
        {
            User user = _fixture.GetRepository<IUserRepository>().WithCredentialIdentifier("user");
            Assert.NotNull(user);

            UserManager userManager = new UserManager(_fixture.DatabaseContext,
                ((TestContext)_fixture.DatabaseContext).LoggerFactory);

            IEnumerable<Claim> claims = userManager.GetAllClaims(user);
            Assert.NotEmpty(claims);
            foreach (Claim claim in claims)
            {
                Console.WriteLine(claim.Type + " ### " + claim.Value);
            }
        }

    }
}
