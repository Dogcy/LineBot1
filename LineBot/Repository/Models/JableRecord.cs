using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace LineBot.Repository.Models
{
    public partial class JableRecord
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public int Uid { get; set; }
        [StringLength(50)]
        public string SerchWord { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
    }
}
