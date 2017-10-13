using System;
using Infrastructure;
using Security.Data.Entities;

namespace Security
{
    public class UserManager
    {
        // Note : par rapport à Platformus, la méthode chargeant toutes les claims de l'utiliseur sera à implémenter dans PermissionManager
        // On construit une liste qui contaient les claims "role" et les claims "permission".
        
        private IRequestHandler _requestHandler;

        public UserManager(IRequestHandler requestHandler_)
        {
            _requestHandler = requestHandler_;
        }

        public User Validate(string loginTypeCode_, string identifier_, string secret_)
        {
            // TODO
            throw new NotImplementedException();
            
        }

        public async void SignIn(User user_, bool isPersistent_ = false)
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