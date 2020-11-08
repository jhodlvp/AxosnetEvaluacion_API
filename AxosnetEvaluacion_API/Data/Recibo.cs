using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AxosnetEvaluacion_API.Data
{
    [Table("Recibos")]
    public partial class Recibo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public float Monto { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        public string Comentarios { get; set; }
        [ForeignKey("IdProveedor")]
        public Proveedor Proveedor { get; set; }
        public int IdProveedor { get; set; }
        [ForeignKey("IdMoneda")]
        public Moneda Moneda { get; set; }
        public int IdMoneda { get; set; }
    }
}