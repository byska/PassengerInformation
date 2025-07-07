using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PassengerInformation.Application.UseCases.PassengersUseCases.Commands;
using PassengerInformation.Application.UseCases.PassengersUseCases.Queries;

namespace PassengerInformation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PassengerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePassengerCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<UpdatePassengerCommandResponse> Update(Guid id, [FromBody] UpdatePassengerCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<DeletePassengerCommandResponse> Delete(Guid id)
        {
            return await _mediator.Send(new DeletePassengerCommand(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPassengersQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetPassengerByIdQueryRequest(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("by-flight/{flightNumber}")]
        public async Task<IActionResult> GetByFlight(string flightNumber)
        {
            var result = await _mediator.Send(new GetPassengersByFlightQueryRequest(flightNumber));
            return Ok(result);
        }

        [HttpPatch("{id}/assign-seat")]
        public async Task<IActionResult> AssignSeat(Guid id, [FromBody] AssignSeatCommand command)
        {
            if (id != command.PassengerId) return BadRequest("Id mismatch");
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
