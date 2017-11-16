// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

namespace Infrastructure
{
    // TODO to be removed in favor of processing IDatabaseMetadata in a new Security.ServiceConfiguration.DatabaseInitialization class.
    public interface IDatabaseInitializer
    {
        void CheckAndInitialize(IRequestHandler context_);
    }
}
