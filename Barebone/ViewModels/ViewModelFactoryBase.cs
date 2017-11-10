// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Infrastructure;

namespace Barebone.ViewModels
{
    public class ViewModelFactoryBase
    {
         protected IRequestHandler RequestHandler { get; set; }

        public ViewModelFactoryBase(IRequestHandler requestHandler)
        {
            this.RequestHandler = requestHandler;
        }
    }
}