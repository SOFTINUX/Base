using System;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security
{
    public class UserManager
    {
        // Note : par rapport à Platformus, la méthode chargeant toutes les claims de l'utiliseur sera à implémenter dans PermissionManager
        // On construit une liste qui contaient les claims "role" et les claims "permission".

        private IRequestHandler _requestHandler;
        private ICredentialTypeRepository _credentialTypeRepository;
        private ICredentialRepository _credentialRepository;
        private IUserRepository _userRepository;
        private ILogger _logger;

        public UserManager(IRequestHandler requestHandler_, ILoggerFactory loggerFactory_)
        {
            _requestHandler = requestHandler_;
            _credentialTypeRepository = requestHandler_.Storage.GetRepository<ICredentialTypeRepository>();
            _credentialRepository = requestHandler_.Storage.GetRepository<ICredentialRepository>();
            _userRepository = requestHandler_.Storage.GetRepository<IUserRepository>();
            _logger = loggerFactory_.CreateLogger(GetType().FullName);
        }

        public User Login(string loginTypeCode_, string identifier_, string secret_)
        {
            CredentialType credentialType = _credentialTypeRepository.WithCode(loginTypeCode_);

            if (credentialType == null)
            {
                _logger.LogDebug("No such credential type: " + loginTypeCode_);
                return null;
            }

            if (credentialType.Code == Enums.CredentialType.Email)
            {
                Credential credential = _credentialRepository.WithKeys(credentialType.Id, identifier_);
                if (credential == null)
                {
                    _logger.LogDebug("No data for provided credential type and identifier");
                    return null;
                }

                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                if (passwordHasher.VerifyHashedPassword(null, credential.Secret, secret_) == PasswordVerificationResult.Success)
                    return _userRepository.WithKey(credential.UserId);

                _logger.LogDebug("Credential secret verification failed");
                return null;
            }

            throw new NotImplementedException("Credential type '" + credentialType.Code + " ' not handled");

        }

        public async void LoadClaims(User user_, bool isPersistent_ = false)
        {
            // TODO (à mon avis il faut supprimer async, c'est une action synchrone)
            throw new NotImplementedException();
        }

        public async void SignOut()
        {
            // TODO (à mon avis il faut supprimer async, c'est une action synchrone)
            throw new NotImplementedException();
        }

        public int GetCurrentUserId()
        {
            // TODO
            throw new NotImplementedException();
        }

        public int GetCurrentUser()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}