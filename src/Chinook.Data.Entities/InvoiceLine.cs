// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using ExtCore.Data.Entities.Abstractions;

namespace Chinook.Data.Entities
{
    [DebuggerDisplay("InvoiceLineId = {InvoiceLineId}")]
    public class InvoiceLine : IEntity
    {
        [Key]
        public int InvoiceLineId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int TrackId { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [ForeignKey("TrackId")]
        public Track Track { get; set; }
    }
}
