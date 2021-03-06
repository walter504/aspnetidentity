﻿// Copyright (c) Microsoft Corporation, Inc. All rights reserved.
// Licensed under the MIT License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Xunit;

namespace Identity.Test
{
    public class IdentityExtensionsTest
    {
        public const string ExternalAuthenticationType = "TestExternalAuth";

        [Fact]
        public void IdentityNullCheckTest()
        {
            IIdentity identity = null;
            ExceptionHelper.ThrowsArgumentNull(() => identity.GetUserId(), "identity");
            ExceptionHelper.ThrowsArgumentNull(() => identity.GetUserName(), "identity");
            ClaimsIdentity claimsIdentity = null;
            ExceptionHelper.ThrowsArgumentNull(() => claimsIdentity.FindFirstValue(null), "identity");
        }

        [Fact]
        public void IdentityNullIfNotClaimsIdentityTest()
        {
            IIdentity identity = new TestIdentity();
            Assert.Null(identity.GetUserId());
            Assert.Null(identity.GetUserName());
        }

        [Fact]
        public void UserNameAndIdTest()
        {
            var id = CreateTestExternalIdentity();
            Assert.Equal("NameIdentifier", id.GetUserId());
            Assert.Equal("Name", id.GetUserName());
        }

        [Fact]
        public void CustomIdTest()
        {
            var id = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "3", null, ExternalAuthenticationType),
                },
                ExternalAuthenticationType);
            Assert.Equal(3, id.GetUserId<int>());
        }

        [Fact]
        public void NoIdReturnsDefaultValue()
        {
            var id = new ClaimsIdentity();
            Assert.Equal(0, id.GetUserId<int>());
        }

        [Fact]
        public void IdentityExtensionsFindFirstValueNullIfUnknownTest()
        {
            var id = CreateTestExternalIdentity();
            Assert.Null(id.FindFirstValue("bogus"));
        }

        private static ClaimsIdentity CreateTestExternalIdentity()
        {
            return new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "NameIdentifier", null, ExternalAuthenticationType),
                    new Claim(ClaimTypes.Name, "Name")
                },
                ExternalAuthenticationType);
        }

        private class TestIdentity : IIdentity
        {
            public string AuthenticationType
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsAuthenticated
            {
                get { throw new NotImplementedException(); }
            }

            public string Name
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}