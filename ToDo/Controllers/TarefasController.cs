using Desafio.DAL;
using Desafio.Domain.DTO;
using Desafio.Domain.Entity;
using Desafio.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desafio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly TarefasService tarefaService;
        public TarefasController(TarefasService taskService)
        {
            this.tarefaService = taskService;
        }

        [HttpGet]
        public IEnumerable<Tarefa> Get()
        {
            return tarefaService.ListarTodos();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var retorno = tarefaService.PesquisarPorId(id);

            if (retorno.Sucesso)
            {
                return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return NotFound(retorno.Mensagem);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] TarefaCreateRequest postModel)
        {
            if (ModelState.IsValid)
            {
                var retorno = tarefaService.CadastrarNova(postModel);
                if (!retorno.Sucesso)
                    return BadRequest(retorno.Mensagem);
                return Ok(retorno);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("concluido/{id}")]
        public IActionResult PutStatus(int id)
        {

            if (ModelState.IsValid)
            {
                var retorno = tarefaService.Concluir(id);
                if (!retorno.Sucesso)
                    return BadRequest(retorno.Mensagem);
                return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("prioridade/{id}")]
        public IActionResult Put(int id, [FromBody] TarefaPrioridadeRequest putModel)
        {
            if (ModelState.IsValid)
            {
                var retorno = tarefaService.Editar(id, putModel);
                if (!retorno.Sucesso)
                    return BadRequest(retorno.Mensagem);
                return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var retorno = tarefaService.Deletar(id);
            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);
            return Ok();

        }

    }
}
