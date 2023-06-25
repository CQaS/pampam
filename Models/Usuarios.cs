using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaVaras.Models
{

	public enum enRoles
	{
		Administrador = 1,
		Empleado = 2,
		
	}
	public class Usuarios
    {
		[Key]
		[Display(Name = "Código")]
		public int id_Us { get; set; }
		[Required]
		[Display(Name = "Nombre")]
		public string nombre { get; set; }
		[Required]
		[Display(Name = "Apellido")]
		public string apellido { get; set; }
		[Display(Name = "Avatar")]
        public string avatar { get; set; }

        public IFormFile AvatarFile{ get; set; }
		
		[Required, DataType(DataType.EmailAddress)]
		[Display(Name = "E-mail")]
		public string email { get; set; }
		[Required, DataType(DataType.Password)]
		[Display(Name = "Contraseña")]
		public string contraseña { get; set; }

		[Display(Name = "Pregunta")]
		public string pregunta { get; set; }
		[Display(Name = "Rol")]
		public int rol { get; set; }

		[NotMapped]
		 public string rolNombre => rol > 0 ? ((enRoles)rol).ToString() : "";
		public static IDictionary<int, string> ObtenerRoles()
		{
			SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
			Type tipoEnumRol = typeof(enRoles);
			foreach (var valor in Enum.GetValues(tipoEnumRol))
			{
				roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
			}
			return roles;
		}


	}
}
