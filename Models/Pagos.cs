using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaVaras.Models
{
    public class Pagos
    {
        [Key]
        [Display(Name = "Código")]
        public int id_Pagos{ get; set; }
        [Display(Name = "N° Pago")]
        [Required]
        public int num_Pago { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Fecha")]
        public DateTime fecha { get; set; }
        [Required]
        [Display(Name = "Importe")]
        public decimal importe { get; set; }
        
        [Display(Name = "Contrato Nro.")]
        public int contrato_id { get; set; }
        
        [ForeignKey("contrato_Id")]        
        public Contratos Contratos { get; set; }
        
        public Inquilinos inquilinos { get; set; }

        public Inmuebles Inmuebles { get; set; }

    }
}
