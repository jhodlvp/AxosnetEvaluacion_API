using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AxosnetEvaluacion_API.DTOs
{
    public class ProveedorGetDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(150, ErrorMessage = "La longitud máxima es de 150 caracteres")]
        public string Nombre { get; set; }
    }

    public class ProveedorPostDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(150, ErrorMessage = "La longitud máxima es de 150 caracteres")]
        public string Nombre { get; set; }
    }

    public class ProveedorUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(150, ErrorMessage = "La longitud máxima es de 150 caracteres")]
        public string Nombre { get; set; }
    }
}
