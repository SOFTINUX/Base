// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Chinook.Data.Abstractions;
using Chinook.Data.Entities;
using ExtCore.Data.EntityFramework;
using Album = Chinook.Data.Entities.Album;

namespace Chinook.Data.EntityFramework
{
    public class AlbumRepository : RepositoryBase<Album>, IAlbumRepository
    {
        public IEnumerable<Album> All()
        {
            return dbSet.ToList();
        }
    }
}
