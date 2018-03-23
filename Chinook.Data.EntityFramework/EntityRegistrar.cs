// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Chinook.Data.Entities;

namespace Chinook.Data.EntityFramework
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelBuilder_)
        {
            //Album
            modelBuilder_.Entity<Album>( etb_ =>
                {
                    etb_.HasKey(e_ => e_.AlbumId);
                    etb_.Property(e_ => e_.AlbumId).ValueGeneratedOnAdd();
                }
            );

            //Artist
            modelBuilder_.Entity<Artist>( etb_ =>
                {
                    etb_.HasKey(e_ => e_.ArtistId);
                    etb_.Property(e_ => e_.ArtistId).ValueGeneratedOnAdd();
                }
            );

            //Customer
            modelBuilder_.Entity<Customer>( etb_ =>
                {
                    etb_.HasKey(e_ => e_.CustomerId);
                    etb_.Property(e_ => e_.CustomerId).ValueGeneratedOnAdd();
                }
            );

            //Employee
            modelBuilder_.Entity<Employee>( etb_ =>
                {
                    etb_.HasKey(e_ => e_.EmployeeId);
                    etb_.Property(e_ => e_.EmployeeId).ValueGeneratedOnAdd();
                }
            );

            //Genre
            modelBuilder_.Entity<Genre>( etb_ =>
                {
                    etb_.HasKey(e_ => e_.GenreId);
                    etb_.Property(e_ => e_.GenreId).ValueGeneratedOnAdd();
                }
            );

            //Invoice
            modelBuilder_.Entity<Invoice>( etb_ =>
                {
                    etb_.HasKey(e_ => e_.InvoiceId);
                    etb_.Property(e_ => e_.InvoiceId).ValueGeneratedOnAdd();
                }
            );

            //InvoiceLine
            modelBuilder_.Entity<InvoiceLine>( etb_ =>
                {
                    etb_.HasKey(e_ => e_.InvoiceLineId);
                    etb_.Property(e_ => e_.InvoiceLineId).ValueGeneratedOnAdd();
                }
            );

            //MediaType
            modelBuilder_.Entity<MediaType>( etb_ =>
                {
                    etb_.HasKey(e_ => e_.MediaTypeId);
                    etb_.Property(e_ => e_.MediaTypeId).ValueGeneratedOnAdd();
                }
            );

            //Playlist
            modelBuilder_.Entity<Playlist>( etb_ =>
                {
                    etb_.HasKey(e_ => e_.PlaylistId);
                    etb_.Property(e_ => e_.PlaylistId).ValueGeneratedOnAdd();
                }
            );

            //PlaylistTrack
            modelBuilder_.Entity<PlaylistTrack>( etb_ =>
                {
                    etb_.HasKey(e_ => e_.TrackId);
                    etb_.Property(e_ => e_.TrackId).ValueGeneratedOnAdd();

                    etb_.HasKey(e_ => e_.PlaylistId);
                    etb_.Property(e_ => e_.PlaylistId).ValueGeneratedOnAdd();
                }
            );

            //Track
            modelBuilder_.Entity<Track>( etb_ =>
                {
                    etb_.HasKey(e_ => e_.TrackId);
                    etb_.Property(e_ => e_.TrackId).ValueGeneratedOnAdd();
                }
            );
        }
    }
}
