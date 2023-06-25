using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaVaras.Models
{
    
    public class Contratos
    {
        [Key]
        [Display(Name = "Codigo")]        
        public int id_Cont { get; set; }
        [Required]
        [Display(Name = "Fecha Inicio")]
        [DataType(DataType.Date)]
        public DateTime fecha_In{ get; set; }
        [Required]
        [Display(Name = "Fecha Finalizacion")]
        [DataType(DataType.Date)]
        public DateTime fecha_Fin { get; set; }
        [Required]
        public int valor { get; set; }
        [Display(Name = "Inquilinos")]
        public int inq_Id { get; set; }

        [Display(Name = "Inmuebles")]
        public int inm_Id { get; set; }

        public Inquilinos Inquilinos { get; set; }
        public Inmuebles Inmuebles { get; set; }
    }
}