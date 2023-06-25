using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace InmobiliariaVaras.Models
{
    public class Propietarios
    {
        [Key]
        [Display(Name = "Codigo")]
        public int id_Prop { get; set; }
        [Required]
        [Display(Name = "Nombre ")]
        public String nombre { get; set; }
        [Required]
        [Display(Name = "DNI")]
        public int dni { get; set; }
        [Required]
        [Display(Name = "Domicilio")]
        public String dom_Prop { get; set; }
        [Required]
        [Display(Name = "Telefono")]
        public int tel { get; set; }
     
    }   
}
