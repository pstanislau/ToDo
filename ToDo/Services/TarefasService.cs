using Microsoft.EntityFrameworkCore;
using Desafio.DAL;
using Desafio.Domain.DTO;
using Desafio.Domain.Entity;
using Desafio.Services.Base;

namespace Desafio.Services
{
    public class TarefasService
    {
        private readonly AppDbContext _dbContext;
        public TarefasService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ServiceResponse<Tarefa> CadastrarNova(TarefaCreateRequest model)
        {
            if (model.Prioridade>5 || model.Prioridade<0)
            {
                return new ServiceResponse<Tarefa>("A prioridade deve estar no range 0 a 5 para criação");
            }

            if (model.Prioridade == 0)
            {
                model.Prioridade = 5;
            }

            var novaTarefa = new Tarefa()
            {
                Nome = model.Nome,
                Descricao = model.Descricao,
                Status = false,
                Prioridade = model.Prioridade
            };

            _dbContext.Add(novaTarefa);
            _dbContext.SaveChanges();

            return new ServiceResponse<Tarefa>(novaTarefa);
        }

        public List<Tarefa> ListarTodos()
        {
            return _dbContext.Tarefas.ToList();
        }

        public ServiceResponse<Tarefa> PesquisarPorId(int id)
        {
            var resultado = _dbContext.Tarefas.FirstOrDefault(x => x.IdTask == id);
            if (resultado == null)
                return new ServiceResponse<Tarefa>("Tarefa não encontrada!");

                return new ServiceResponse<Tarefa>(resultado);

        }

        public ServiceResponse<Tarefa> Concluir(int id)
        {
            var resultado = _dbContext.Tarefas.FirstOrDefault(x => x.IdTask == id);

            if (resultado == null)
                return new ServiceResponse<Tarefa>("Tarefa não encontrada!");

            resultado.Status = true;
            _dbContext.Tarefas.Add(resultado).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return new ServiceResponse<Tarefa>(resultado);
        }

        public ServiceResponse<Tarefa> Editar(int id, TarefaPrioridadeRequest model)
        {
            var resultado = _dbContext.Tarefas.FirstOrDefault(x => x.IdTask == id);

            if (resultado == null)
                return new ServiceResponse<Tarefa>("Tarefa não encontrada!");

            resultado.Prioridade = model.Prioridade;
            _dbContext.Tarefas.Add(resultado).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return new ServiceResponse<Tarefa>(resultado);
        }

        public ServiceResponse<bool> Deletar(int id)
        {
            var resultado = _dbContext.Tarefas.FirstOrDefault(x => x.IdTask == id);

            if (resultado == null)
                return new ServiceResponse<bool>("Tarefa não encontrada!");

            _dbContext.Tarefas.Remove(resultado);
            _dbContext.SaveChanges();

            return new ServiceResponse<bool>(true);
        }
    }
}
