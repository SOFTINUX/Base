#if DEBUG

namespace Security.Enums.Debug
{

    internal enum UserManagerErrorCode
    {
        None,
        NoCredentialType,
        NoMatchCredentialTypeAndIdentifier,
        SecretVerificationFailed,
        UnknownCredentialType
    }


}
#endif
