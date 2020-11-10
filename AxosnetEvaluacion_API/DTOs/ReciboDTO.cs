using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AxosnetEvaluacion_API.DTOs
{
    public class ReciboGetDTO
    {
        public int Id { get; set; }
        public float Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentarios { get; set; }
        public int IdProveedor { get; set; }
        public int IdMoneda { get; set; }
    }
    public class ReciboPostDTO
    {
        public int Id { get; set; }
        [Required]
        public float Monto { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [MaxLength(260, ErrorMessage = "La longitud máxima es de 260 caracteres.")]
        public string Comentarios { get; set; }
        [Required]
        public int IdProveedor { get; set; }
        [Required]
        public int IdMoneda { get; set; }
    }
    public class ReciboUpdateDTO
    {
        public int Id { get; set; }
        [MaxLength(260, ErrorMessage = "La longitud máxima es de 260 caracteres.")]
        public string Comentarios { get; set; }
    }
}
