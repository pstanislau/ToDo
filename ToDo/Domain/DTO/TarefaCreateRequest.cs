using System.ComponentModel.DataAnnotations;

namespace Desafio.Domain.DTO;

public class TarefaCreateRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "O Nome é obrigatório!")]
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public bool Status { get; set; }
    [Range(0, 5, ErrorMessage = "A prioridade deve estar no range 0 a 5 para criação")]
    public int Prioridade { get; set; }
}