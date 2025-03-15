using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Validators.Extension;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using System.Linq.Expressions;

namespace Application.Services
{
    public class TarefasService(ITarefaRepository _tarefaRepository, IValidator<Tarefa> tarefaValidator) : ITarefaService
    {
        public async Task<TarefaResponse> CriarTarefaAsync(CriarTarefaRequest tarefaRequest)
        {
            Tarefa tarefa = new Tarefa(tarefaRequest.Titulo, tarefaRequest.Descricao);

            var validacao = await tarefaValidator.ValidateAsync(tarefa);
            if (!validacao.IsValid)
                throw new BusinessValidationException(validacao.ObterErros());

            await _tarefaRepository.InserirAsync(tarefa);
            return MapearParaDtoResposta(tarefa);
        }

        public async Task ApagarTarefaAsync(int id)
        {
            Tarefa? tarefa = await _tarefaRepository.ObterPorIdAsync(id);

            if (tarefa == null)
                throw new NotFoundException("Tarefa não encontrada");

            await _tarefaRepository.DeletarAsync(tarefa);
        }

        public async Task<TarefaResponse?> ObterTarefaPorId(int id)
        {
            Tarefa? tarefa = await _tarefaRepository.ObterPorIdAsync(id);

            if (tarefa == null)
                throw new NotFoundException("Tarefa não encontrada");

            return MapearParaDtoResposta(tarefa);
        }

        public async Task<IEnumerable<TarefaResponse>> ObterTarefasAsync()
        {
            IEnumerable<Tarefa> tarefas = await _tarefaRepository.ObterTodosAsync();

            return tarefas.ToList().ConvertAll(r => MapearParaDtoResposta(r));
        }

        public async Task AtualizarTarefaAsync(AtualizarTarefaRequest tarefaRequest)
        {
            Tarefa? tarefa = await _tarefaRepository.ObterPorIdAsync(tarefaRequest.Id);

            if (tarefa == null)
                throw new NotFoundException("Tarefa não encontrada");

            tarefa.Titulo = tarefaRequest.Titulo;
            tarefa.Descricao = tarefaRequest.Descricao;
            tarefa.Status = tarefaRequest.Status;
            tarefa.DataConclusao = tarefaRequest.DataConclusao;

            var validacao = await tarefaValidator.ValidateAsync(tarefa);
            if (!validacao.IsValid)
                throw new BusinessValidationException(validacao.ObterErros());

            await _tarefaRepository.AtualizarAsync(tarefa);
        }

        private TarefaResponse MapearParaDtoResposta(Tarefa tarefa)
        {
            return new TarefaResponse(
                tarefa.Id, 
                tarefa.Titulo, 
                tarefa.Descricao, 
                tarefa.DataCriacao, 
                tarefa.DataConclusao, 
                tarefa.Status
            );
        }

        public async Task<IEnumerable<TarefaResponse>> BuscarTarefasAsync(Expression<Func<Tarefa, bool>> filtro)
        {
            var tarefas = await _tarefaRepository.BuscarAsync(filtro);
            return tarefas.ToList().ConvertAll(r => MapearParaDtoResposta(r));
        }
    }
}
