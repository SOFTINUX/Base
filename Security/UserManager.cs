// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Security.Enums.Debug;

namespace Security
{
    public class UserManager
    {
        // Note : par rapport à Platformus, la méthode chargeant toutes les claims de l'utiliseur sera à implémenter dans PermissionManager
        // On construit une liste qui contaient les claims "role" et les claims "permission".

        private readonly IRequestHandler _requestHandler;
        private readonly ICredentialTypeRepository _credentialTypeRepository;
        private readonly ICredentialRepository _credentialRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger _logger;
#if DEBUG
        internal UserManagerErrorCode ErrorCode { get; private set; }
#endif

        public UserManager(IRequestHandler requestHandler_, ILoggerFactory loggerFactory_)
        {
            _requestHandler = requestHandler_;
            _credentialTypeRepository = requestHandler_.Storage.GetRepository<ICredentialTypeRepository>();
            _credentialRepository = requestHandler_.Storage.GetRepository<ICredentialRepository>();
            _userRepository = requestHandler_.Storage.GetRepository<IUserRepository>();
            _userRoleRepository = requestHandler_.Storage.GetRepository<IUserRoleRepository>();
            _roleRepository = requestHandler_.Storage.GetRepository<IRoleRepository>();
            _logger = loggerFactory_.CreateLogger(GetType().FullName);
        }

        public User Login(string loginTypeCode_, string identifier_, string secret_)
        {
            CredentialType credentialType = _credentialTypeRepository.WithCode(loginTypeCode_);

            if (credentialType == null)
            {
                _logger.LogDebug("No such credential type: " + loginTypeCode_);
#if DEBUG
                ErrorCode = UserManagerErrorCode.NoCredentialType;
#endif
                return null;
            }

            if (credentialType.Code == Enums.CredentialType.Email)
            {
                Credential credential = _credentialRepository.WithKeys(credentialType.Id, identifier_);
                if (credential == null)
                {
                    _logger.LogDebug("No match for provided credential type and identifier");
#if DEBUG
                    ErrorCode = UserManagerErrorCode.NoMatchCredentialTypeAndIdentifier;
#endif
                    return null;
                }

                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                if (passwordHasher.VerifyHashedPassword(null, credential.Secret, secret_) == PasswordVerificationResult.Success)
                    return _userRepository.WithKey(credential.UserId);

                _logger.LogDebug("Credential secret verification failed");
#if DEBUG
                ErrorCode = UserManagerErrorCode.SecretVerificationFailed;
#endif
                return null;
            }

#if DEBUG
            ErrorCode = UserManagerErrorCode.UnknownCredentialType;
#endif

            throw new NotImplementedException("Credential type '" + credentialType.Code + " ' not handled");
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="user_">User who passed login verification</param>
        /// <param name="isPersistent_">When true, cookie persists across brower closure</param>
        public async void LoadClaims(User user_, bool isPersistent_ = false)
        {
            ClaimsIdentity identity = new ClaimsIdentity(GetAllClaims(user_), CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await _requestHandler.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = isPersistent_ }
            );
        }

        /// <summary>
        /// Gathers all claims applicable to this user.
        /// </summary>
        /// <param name="user_"></param>
        /// <returns></returns>
        internal IEnumerable<Claim> GetAllClaims(User user_)
        {
            List<Claim> claims = new List<Claim>();
            // user
            claims.AddRange(GetUserClaims(user_));
            // roles
            claims.AddRange(GetRoleClaims(user_));
            // permissions
            claims.AddRange(new PermissionManager().GetFinalPermissions(_requestHandler, user_));
            return claims;
        }

        /// <summary>
        /// Gathers claims of type "name*"
        /// </summary>
        /// <param name="user_"></param>
        /// <returns></returns>
        private IEnumerable<Claim> GetUserClaims(User user_)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user_.Id.ToString()),
                new Claim(ClaimTypes.Name, user_.DisplayName)
            };

            return claims;
        }

        /// <summary>
        /// Gathers all claims of type "role".
        /// </summary>
        /// <param name="user_"></param>
        /// <returns></returns>
        private IEnumerable<Claim> GetRoleClaims(User user_)
        {
            List<Claim> claims = new List<Claim>();
            IEnumerable<int> roleIds = _userRoleRepository.FilteredByUserId(user_.Id)?.Select(ur_ => ur_.RoleId).ToList();

            if (roleIds == null)
                return claims;
            // TODO improve code : use a navigation property above to get role's code instead of n queries
            foreach (int roleId in roleIds)
            {
                Role role = _roleRepository.WithKey(roleId);

                claims.Add(new Claim(ClaimTypes.Role, role.Code));
            }

            return claims;
        }

        public async void SignOut()
        {
            await _requestHandler.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public int GetCurrentUserId()
        {
            if (!_requestHandler.HttpContext.User.Identity.IsAuthenticated)
                return -1;

            Claim claim = _requestHandler.HttpContext.User.Claims.FirstOrDefault(c_ => c_.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
                return -1;

            if (!int.TryParse(claim.Value, out int currentUserId))
                return -1;

            return currentUserId;
        }

        public User GetCurrentUser()
        {
            int currentUserId = GetCurrentUserId();

            return currentUserId == -1 ? null : _userRepository.WithKey(currentUserId);
        }
    }
}