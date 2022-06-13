using Desafio.DAL;
using Desafio.Domain.DTO;
using Desafio.Domain.Entity;
using Desafio.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Tests.Services
{
    public class DesafioServiceTest : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly TarefasService _service;

        public DesafioServiceTest()
        {
            // Criando banco em memória
            var options = new DbContextOptionsBuilder<AppDbContext>()
                // Guid.NewGuid().ToString(): Garantindo a criação de um banco novo
                //  a cada execução de teste, evitando a existência de dados não inseridos durante os testes
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            // Criamos as instâncias que vamos usar nos testes
            _dbContext = new AppDbContext(options);
            _service = new TarefasService(_dbContext);
        }

        [Fact]
        public void Quando_PassadoTarefaValida_DeveCadastrar_E_Retornar()
        {
            // Preparando entrada
            var request = new TarefaCreateRequest()
            {
                Nome = "Terminar as Atividades do Decola",
                Descricao = "Vamos",
                Prioridade =1
            };

            // Executando
            var retorno = _service.CadastrarNova(request);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.Nome, request.Nome);
            Assert.Equal(retorno.ObjetoRetorno.Descricao, request.Descricao);
            Assert.Equal(retorno.ObjetoRetorno.Prioridade, request.Prioridade);
        }

        [Fact]
        public void Quando_PassadaTarefaInvalida_Deve_RetornarErro()
        {
            var mensagemEsperada = "A prioridade deve estar no range 0 a 5 para criação";

            // Preparando entrada
            var request = new TarefaCreateRequest()
            {
                Nome = "limpa a casa",
                Descricao = "Tarefa bala",
                Prioridade=15
               
            };

            // Executando
            var retorno = _service.CadastrarNova(request);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
        }

        [Fact]
        public void Quando_ChamadoListarTodos_Deve_RetornarTodos()
        {
            var lista = ListaAlbunsStub();

            var retorno = _service.ListarTodos();

            // Validando resultados
            Assert.Equal(retorno.Count, lista.Count);
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorId_Com_IdExistente_Deve_RetornarTarefa()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();

            // Executa com id que foi cadastrado no banco
            var retorno = _service.PesquisarPorId(lista[0].IdTask);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.IdTask, lista[0].IdTask);
            Assert.Equal(retorno.ObjetoRetorno.Nome, lista[0].Nome);
            Assert.Equal(retorno.ObjetoRetorno.Descricao, lista[0].Descricao);
            Assert.Equal(retorno.ObjetoRetorno.Prioridade, lista[0].Prioridade);
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorId_Com_IdNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();
            var mensagemEsperada = "Tarefa não encontrada!";

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.PesquisarPorId(lista.Count + 1);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
        }

        [Fact]
        public void Quando_ChamadoConcluir_Com_IdExistente_Deve_RetornarTarefaAtualizada()
        {
            var lista = ListaAlbunsStub();

            var retorno = _service.Concluir(lista[0].IdTask);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.Status, true);
        }

        [Fact]
        public void Quando_ChamadoConcluir_Com_IdNaoExistente_Deve_RetornarErro()
        {

            var lista = ListaAlbunsStub();

            var mensagemEsperada = "Tarefa não encontrada!";

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.Concluir(lista.Count + 1);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
        }

        [Fact]
        public void Quando_ChamadoDeletar_Com_IdExistente_Deve_RetornarAlbumAtualizado()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();

            // Executa com id que foi cadastrado no banco
            var retorno = _service.Deletar(lista[0].IdTask);

            // Validando resultados
            Assert.True(retorno.ObjetoRetorno);
            // Verifica se existe um álbum a menos na base
            Assert.Equal(_dbContext.Tarefas.Count(), lista.Count - 1);
        }

        [Fact]
        public void Quando_ChamadoDeletar_Com_IdNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();
            var mensagemEsperada = "Tarefa não encontrada!";

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.Deletar(lista.Count + 1);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
            // Verifica se existe o mesmo número de álbuns na base
            Assert.Equal(_dbContext.Tarefas.Count(), lista.Count);
        }

        private List<Tarefa> ListaAlbunsStub()
        {
            // Dados para mock
            var lista = new List<Tarefa>()
            {
                new Tarefa()
                {
                    Nome =  "Limpar a casa",
                    Descricao = "Casa ta suja slc",
                    Prioridade = 3
                },
                new Tarefa()
                {
                    Nome =  "Limpar a casa?",
                    Descricao = "Casa ta suja slc"
                }
            };

            // Salvamos os dados no banco
            _dbContext.AddRange(lista);
            _dbContext.SaveChanges();

            // Retornamos para usar nas validações
            return lista;
        }

        public void Dispose()
        {
            // Garante que o banco usado nos testes foi deletado
            _dbContext.Database.EnsureDeleted();
            // Informa pro Garbage Collector que o objeto já foi limpo. Leia mais:
            // - https://docs.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1816
            // - https://stackoverflow.com/a/151244/7467989
            GC.SuppressFinalize(this);
        }
    }
}
