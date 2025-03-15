using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Services;
using Application.Validators;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using FluentAssertions;
using FluentValidation;
using Moq;
using System.Linq.Expressions;

namespace Tests
{
    public class UserServiceTests
    {
        private readonly Mock<ITarefaRepository> _tarefaRepositoryMock;
        private readonly TarefasService _tarefaService;
        private readonly TarefaValidator _tarefaValidator;

        public UserServiceTests()
        {
            _tarefaRepositoryMock = new Mock<ITarefaRepository>();
            _tarefaValidator = new TarefaValidator();
            _tarefaService = new TarefasService(_tarefaRepositoryMock.Object, _tarefaValidator);
        }

        [Fact]
        public async Task CriarTarefaAsync_Valido_DeveInserir()
        {
            var request = new CriarTarefaRequest("Nova tarefa", "Tarefa para ser realizada");

            var result = await _tarefaService.CriarTarefaAsync(request);

            _tarefaRepositoryMock.Verify(repo => repo.InserirAsync(It.IsAny<Tarefa>()), Times.Once);
            result.Titulo.Should().Be(request.Titulo);
            result.Descricao.Should().Be(request.Descricao);
        }

        [Fact]
        public void CriarTarefaAsync_TituloVazio_DisparaExcecao()
        {
            var criarTarefaRequest = new CriarTarefaRequest("", "Tarefa para ser realizada");

            var action = () => _tarefaService.CriarTarefaAsync(criarTarefaRequest).Wait();

            action.Should().Throw<BusinessValidationException>()
                .And.Errors.Should().ContainKey("Titulo")
                .WhoseValue.Should().Contain("O título da tarefa não pode ser vazio.");
        }

        [Fact]
        public void AtualizarTarefaAsync_ConcluirTarefaSemDataConclusao_DisparaExcecao()
        {
            var atualizarTarefarequest = new AtualizarTarefaRequest(1, "Nova tarefa", null, null, EnumStatus.Concluida);

            var tarefaMock = new Tarefa("Nova tarefa", null)
            {
                Id = 1,
                DataCriacao = DateTime.Now,
                Status = EnumStatus.Pendente,
            };

            _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync(tarefaMock);

            var action = () => _tarefaService.AtualizarTarefaAsync(atualizarTarefarequest).Wait();
            action.Should().Throw<BusinessValidationException>()
                .And.Errors.Should().ContainKey("DataConclusao")
                .WhoseValue.Should().Contain("A data de conclusão é obrigatória quando a tarefa for concluída");
        }

        [Fact]
        public void AtualizarTarefaAsync_ConcluirTarefaAntesDaCriacao_DisparaExcecao()
        {
            var atualizarTarefarequest = new AtualizarTarefaRequest(1, "Nova tarefa", null, new DateTime(2025, 1, 1), EnumStatus.Concluida);

            var tarefaMock = new Tarefa("Nova tarefa", null)
            {
                Id = 1,
                DataCriacao = new DateTime(2025, 1, 2),
                Status = EnumStatus.Pendente,
            };

            _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync(tarefaMock);

            var action = () => _tarefaService.AtualizarTarefaAsync(atualizarTarefarequest).Wait();
            action.Should().Throw<BusinessValidationException>()
                .And.Errors.Should().ContainKey("DataConclusao")
                .WhoseValue.Should().Contain("A data de conclusão da tarefa não pode ser menor que a data de criação");
        }

        [Fact]
        public void AtualizarTarefaAsync_TarefaNaoConcluidaComDataDeConclusao_DisparaExcecao()
        {
            var atualizarTarefarequest = new AtualizarTarefaRequest(1, "Nova tarefa", null, new DateTime(2025, 1, 2), EnumStatus.Pendente);

            var tarefaMock = new Tarefa("Nova tarefa", null)
            {
                Id = 1,
                DataCriacao = new DateTime(2025, 1, 1),
                Status = EnumStatus.Pendente,
            };

            _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync(tarefaMock);

            var action = () => _tarefaService.AtualizarTarefaAsync(atualizarTarefarequest).Wait();
            action.Should().Throw<BusinessValidationException>()
                .And.Errors.Should().ContainKey("DataConclusao")
                .WhoseValue.Should().Contain("A data de conclusão da tarefa não deve ser informada quando a tarefa não estiver concluída.");
        }

        [Fact]
        public void AtualizarTarefaAsync_TarefaNaoEncontrada_DisparaExcecao()
        {
            var atualizarTarefarequest = new AtualizarTarefaRequest(1, "Nova tarefa", null, null, EnumStatus.Concluida);

            _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync((Tarefa?)null);

            var action = () => _tarefaService.AtualizarTarefaAsync(atualizarTarefarequest).Wait();
            action.Should().Throw<NotFoundException>()
                .WithMessage("Tarefa não encontrada");
        }

        [Fact]
        public void AtualizarTarefaAsync_ConcluirTarefaValida_DeveAtualizar()
        {
            var atualizarTarefarequest = new AtualizarTarefaRequest(1, "Nova tarefa Concluida", null, new DateTime(2025, 1, 2), EnumStatus.Concluida);

            var tarefaMock = new Tarefa("Nova tarefa", null)
            {
                Id = 1,
                DataCriacao = new DateTime(2025, 1, 1),
                Status = EnumStatus.Pendente,
            };

            _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync(tarefaMock);

            var action = () => _tarefaService.AtualizarTarefaAsync(atualizarTarefarequest).Wait();
            action.Should().NotThrow();
            _tarefaRepositoryMock.Verify(repo => repo.AtualizarAsync(It.Is<Tarefa>(r => r.Id == 1 && r.Titulo == "Nova tarefa Concluida" && r.Status == EnumStatus.Concluida)), Times.Once);
        }

        [Fact]
        public void ObterTarefaPorId_TarefaNaoEncontrada_DisparaExcecao()
        {
            _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync((Tarefa?)null);

            var action = () => _tarefaService.ObterTarefaPorId(1).Wait();

            action.Should().Throw<NotFoundException>()
                .WithMessage("Tarefa não encontrada");
        }

        [Fact]
        public async Task ObterTarefaPorId_TarefaEncontrada_DeveRetornar()
        {
            DateTime dataCriacao = DateTime.Now;
            var tarefaMock = new Tarefa("Nova tarefa", null)
            {
                Id = 1,
                DataCriacao = dataCriacao,
                Status = EnumStatus.Pendente,
            };

            _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync(tarefaMock);

            var result = await _tarefaService.ObterTarefaPorId(1);
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Titulo.Should().Be("Nova tarefa");
            result.DataCriacao.Should().Be(dataCriacao);
            result.Status.Should().Be(EnumStatus.Pendente);
            result.Descricao.Should().BeNull();
        }

        [Fact]
        public async Task ObterTarefasAsync_TarefasEncontradas_DeveRetornar()
        {
            var tarefasMock = new List<Tarefa>
            {
                new Tarefa("Tarefa 1", null)
                {
                    Id = 1,
                    DataCriacao = DateTime.Now,
                    Status = EnumStatus.Pendente,
                },
                new Tarefa("Tarefa 2", null)
                {
                    Id = 2,
                    DataCriacao = DateTime.Now,
                    Status = EnumStatus.Pendente,
                }
            };
            _tarefaRepositoryMock.Setup(r => r.ObterTodosAsync())
                .ReturnsAsync(tarefasMock);
            var result = await _tarefaService.ObterTarefasAsync();
            result.Should().HaveCount(2);
            result.Should().ContainSingle(r => r.Id == 1 && r.Titulo == "Tarefa 1");
            result.Should().ContainSingle(r => r.Id == 2 && r.Titulo == "Tarefa 2");
        }

        [Fact]
        public async Task BuscarTarefasAsync_FiltroPendente_DeveRetornar()
        {
            var tarefasMock = new List<Tarefa>
            {
                new Tarefa("Tarefa 1", null)
                {
                    Id = 1,
                    DataCriacao = DateTime.Now,
                    Status = EnumStatus.Pendente,
                },
                new Tarefa("Tarefa 2", null)
                {
                    Id = 2,
                    DataCriacao = DateTime.Now,
                    Status = EnumStatus.Concluida,
                }
            };

            _tarefaRepositoryMock.Setup(repo => repo.BuscarAsync(It.IsAny<Expression<Func<Tarefa, bool>>>()))
                .ReturnsAsync((Expression<Func<Tarefa, bool>> filtro) => tarefasMock.Where(filtro.Compile()).ToList());

            var result = await _tarefaService.BuscarTarefasAsync(r => r.Status == EnumStatus.Pendente);
            result.Should().HaveCount(1);
            result.Should().ContainSingle(r => r.Id == 1 && r.Titulo == "Tarefa 1");
        }

        [Fact]
        public void ApagarTarefaAsync_TarefaNaoEncontrada_DeveDispararExcecao()
        {
            _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync((Tarefa?)null);
            
            var action = () => _tarefaService.ApagarTarefaAsync(1).Wait();
            
            action.Should().Throw<NotFoundException>()
                .WithMessage("Tarefa não encontrada");
        }

        [Fact]
        public void ApagarTarefaAsync_TarefaEncontrada_DeveApagar()
        {
            var tarefaMock = new Tarefa("Tarefa 1", null)
            {
                Id = 1,
                DataCriacao = DateTime.Now,
                Status = EnumStatus.Pendente,
            };

            _tarefaRepositoryMock.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync(tarefaMock);

            var action = () => _tarefaService.ApagarTarefaAsync(1).Wait();

            action.Should().NotThrow();
            _tarefaRepositoryMock.Verify(r => r.DeletarAsync(tarefaMock), Times.Once);
        }
    }
}
