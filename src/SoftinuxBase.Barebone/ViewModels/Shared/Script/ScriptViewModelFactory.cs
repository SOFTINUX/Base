// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace SoftinuxBase.Barebone.ViewModels.Shared.Script
{
    public class ScriptViewModelFactory
    {
        public ScriptViewModel Create(Infrastructure.Script script_)
        {
            return new ScriptViewModel()
            {
                Url = script_.Url
            };
        }
    }
}