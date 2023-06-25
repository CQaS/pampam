using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaVaras.Models
{
    public class Inmuebles
    {
        [Key]
        [Display(Name = "Codigo")]
        public int id_Inm { get; set; }
        [Required]
        [Display(Name = "Domicilio")]
        public String dom_Inm { get; set; }
        [Required]
        public String uso { get; set; }
        [Required]
        public String tipo { get; set; }
        [Required]
        public int ambientes { get; set; }
        [Required]
        [Display(Name = "Precio")]
        public int precio { get; set; }
        public int disponible { get; set; }

        public String imagen { get; set; }

        [Required]
        [Display(Name = "Elige una Imagen:")]
        public IFormFile imagenFile { get; set; }
        [Display(Name = "Dueño")]
        public int prop_Id { get; set; }
        [ForeignKey(nameof(Inmuebles.prop_Id))]
        public Propietarios Duenio { get; set; }

        public Inquilinos inquilinos { get; set; }
        public Contratos contratos { get; set; }
        public Inmuebles inmuebles { get; set; }
        public BuscarPorFecha buscarPorFecha { get; set; }
    }
}
