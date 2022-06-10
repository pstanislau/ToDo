using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Desafio.Domain.Entity
{
    [Table("ToDo")]
    public class Tarefa
    {
        [Key]
        public int IdTask { get; set; }
        [Required]
        [StringLength(255)]
        public string Nome { get; set; }
        [StringLength(255)]
        public string Descricao { get; set; }
        public bool Status { get; set; }
        public int Prioridade { get; set; }
    }
}