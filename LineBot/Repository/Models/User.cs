using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace LineBot.Repository.Models
{
    [Table("User")]
    public partial class User
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [Column("LineID")]
        [StringLength(50)]
        public string LineId { get; set; }
        [StringLength(10)]
        public string Name { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
    }
}
