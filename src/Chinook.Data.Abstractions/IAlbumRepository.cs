// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using Chinook.Data.Entities;
using ExtCore.Data.Abstractions;

namespace Chinook.Data.Abstractions
{
    public interface IAlbumRepository: IRepository
    {
        IEnumerable<Album> All();
    }
}
