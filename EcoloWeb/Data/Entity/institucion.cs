using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EcoloWeb.Data.Entity.Identity
{
    public class institucion:EntityBase
    {
        //Nombre, Dirección, teléfono, contacto

        //[Key]
        //public int IDParticipante { get; set; }

        [Column(TypeName = "char(75)")]
        [Required]
        public string Nombre { get; set; }

        [Required]
        [Column(TypeName = "char(150)")]
        public string Direccion { get; set; }

        [Column(TypeName = "char(200)")]
        public string Contacto { get; set; }

        [Column(TypeName = "char(100)")]
        public string Correo { get; set; }

        [Column(TypeName = "char(13)")]
        public string Celular { get; set; }
    }
}
