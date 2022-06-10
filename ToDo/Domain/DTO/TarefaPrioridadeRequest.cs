using System.ComponentModel.DataAnnotations;

namespace Desafio.Domain.DTO
{
    public class TarefaPrioridadeRequest
    {
        [Required]
        [Range(1, 5, ErrorMessage = "A prioridade deve estar no range 1 a 5")]
        public int Prioridade { get; set; }
    }
}
