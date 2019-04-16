using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EcoloWeb.Data.Entity.Identity
{
    public class Compromiso: EntityBase
    {
        [Column(TypeName = "char(600)")]
        [Required]
        public string Notas { get; set; }


    }
}
