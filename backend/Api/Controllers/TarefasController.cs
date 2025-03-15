using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefasController(ITarefaService _tarefaService) : ControllerBase
    {
        [HttpGet("{id}")]
        [EndpointSummary("Obtem uma tarefa pelo seu id.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TarefaResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterTarefa(int id)
        {
            var tarefa = await _tarefaService.ObterTarefaPorId(id);
            if (tarefa is null)
                return NotFound("Tarefa não encontrada");

            return Ok(tarefa);
        }

        [HttpGet()]
        [EndpointSummary("Obtem todas as tarefas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TarefaResponse>))]
        public async Task<IActionResult> ObterTarefas([FromQuery] EnumStatus? status)
        {
            IEnumerable<TarefaResponse> tarefas;

            if (status != null)
                tarefas = await _tarefaService.BuscarTarefasAsync(r => r.Status == status);
            else
                tarefas = await _tarefaService.ObterTarefasAsync();

            return Ok(tarefas);
        }

        [HttpPost]
        [EndpointSummary("Cria uma tarefa")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TarefaResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarTarefaAsync(CriarTarefaRequest tarefaRequest)
        {
            TarefaResponse tarefaCriada = await _tarefaService.CriarTarefaAsync(tarefaRequest);

            return CreatedAtAction(nameof(ObterTarefa), new { id = tarefaCriada.Id }, tarefaCriada);
        }

        [HttpPut("{id}")]
        [EndpointSummary("Atualizar uma tarefa")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AtualizarTarefaAsync(int id, AtualizarTarefaRequest atualizarTarefaRequest)
        {
            if (id != atualizarTarefaRequest.Id)
                return Problem("O Id da rota é diferente do Id da tarefa", statusCode: StatusCodes.Status422UnprocessableEntity);

            await _tarefaService.AtualizarTarefaAsync(atualizarTarefaRequest);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Remover uma tarefa")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoverTarefaAsync(int id) 
        {
            await _tarefaService.ApagarTarefaAsync(id);
            return NoContent();
        }
    }
}
