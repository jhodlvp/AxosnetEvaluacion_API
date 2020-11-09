using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AxosnetEvaluacion_API.DTOs
{
    public class MonedaGetDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50,ErrorMessage = "La longitud máxima es de 50 caracteres")]
        public string Nombre { get; set; }
    }

    public class MonedaPostDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "La longitud máxima es de 50 caracteres")]
        public string Nombre { get; set; }
    }

    public class MonedaUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "La longitud máxima es de 50 caracteres")]
        public string Nombre { get; set; }
    }
}
