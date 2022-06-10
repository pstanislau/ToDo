using System.ComponentModel.DataAnnotations;

namespace Desafio.Domain.DTO;

public class TarefaCreateRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "O Nome é obrigatório!")]
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public bool Status { get; set; }
    public int Prioridade { get; set; }
}