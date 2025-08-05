using System.ComponentModel.DataAnnotations;

namespace Sarap.Models
{
    public class NotaCredito
    {
        public int NotaCreditoID { get; set; }

        [Required(ErrorMessage = "La factura es requerida")]
        public int FacturaID { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El motivo es requerido")]
        [StringLength(500, ErrorMessage = "El motivo no puede exceder 500 caracteres")]
        public string Motivo { get; set; }

    }
}