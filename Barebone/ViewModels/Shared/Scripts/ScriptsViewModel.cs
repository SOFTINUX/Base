// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using Barebone.ViewModels.Shared.Script;

namespace Barebone.ViewModels.Shared.Scripts
{
    public class ScriptsViewModel
    {
        public IEnumerable<ScriptViewModel> Scripts { get; set; }
    }
}