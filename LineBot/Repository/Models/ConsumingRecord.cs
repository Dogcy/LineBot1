using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace LineBot.Repository.Models
{
    public partial class ConsumingRecord
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("UID")]
        public int Uid { get; set; }
        public int Price { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
    }
}
