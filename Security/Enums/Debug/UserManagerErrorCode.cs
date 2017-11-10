﻿// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
