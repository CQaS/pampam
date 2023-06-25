using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace InmobiliariaVaras.Models
{
    public class Inquilinos
        {
    [Key]
        [Display(Name = "Codigo")]
        public int id_Inq { get; set; }
        [Required]
        [Display(Name = "DNI")]
        public int dni_Inq { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public String nombre_Inq { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public String email { get; set; }
        [Required]
        [Display(Name = "Domicilio")]
        public String dom_Inq { get; set; }
        [Required]
        [Display(Name = "Teléfono Inquilino")]
        public int tel_Inq { get; set; }
        [Required]
        [Display(Name = "Domicilio Laboral")]
        public String domicilio_Lab { get; set; }
        [Required]
        [Display(Name = "Nombre Garante")]
        public String nombre_Garante { get; set; }
        [Required]
        [Display(Name = "DNI Garante")]
        public int dni_Garante { get; set; }
        [Required]
        [Display(Name = "Teléfono Garante")]
        public int tel_Garante { get; set; }
    }
}
        
