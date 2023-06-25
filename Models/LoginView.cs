using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaVaras.Models
{
	public class LoginView
    {
        [DataType(DataType.EmailAddress)]
        public string usuario { get; set; }
        [DataType(DataType.Password)]
        public string contraseña { get; set; }
        
        public string pregunta { get; set; }
	}	
}
