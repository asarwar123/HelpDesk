using HelpDesk.api.Models;
using HelpDesk.api.Services;
using HelpDesk.api.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.api.Controllers
{
    [ApiController]
    [Route("v1/tickets")]
    public class TicketController : ControllerBase
    {
        private readonly TicketRepository _repository;

        public ILogger<TicketController> _logger { get; }

        public TicketController(ILogger<TicketController> logger, TicketRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public IActionResult ListTickets()
        {
            //dummydata.FillData();
            return Ok(dummydata.Tickets);
        }

        [HttpGet("{id}", Name = "GetTicketByID")]
        public async Task<ActionResult<TicketsDTO>> GetTickets(Guid id)
        {
            try
            {
                IEnumerable<Ticket> allTickets = await _repository.getTicketsAsync();

                /////////////// AUTOMAPPER

                TicketsDTO? ticketDto = dummydata.Tickets.FirstOrDefault(d => d.id == id);

                if (ticketDto == null)
                {
                    _logger.LogInformation($"Ticket with id '{id}' wasn't find");
                    return NotFound();
                }

                return Ok(ticketDto);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting Ticket ID: {id}", ex);
                return StatusCode(500, "A problem occured while handling your requet");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TicketsDTO>> CreateTicket(TicketCreationDTO newCreationTicket)
        {
            try
            {
                ///////////// Auto Mapper ticketCreationDTO -> Entities.Ticket
                Ticket newTicket = new Ticket();
                

                //newTicket.id = Guid.NewGuid();
                //dummydata.InsertTicket(newTicket);

                //return CreatedAtRoute("GetTicketByID", newTicket.id );

                await _repository.createTicketAsync(newTicket);

                return Ok(newTicket);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while creating new Ticket", ex);
                return StatusCode(500, "A problem occured while handling your requet");
            }
        }

        [HttpPut("{TicketId}")]
        public IActionResult UpdateTicket(Guid TicketId, TicketsDTO updatedTicket)
        {
            try
            {
                TicketsDTO? orignalTicket = dummydata.Tickets.Find(t => t.id == TicketId);

                if (orignalTicket == null)
                    return NotFound();
                else
                {
                    if (dummydata.UpdateTicket(TicketId, updatedTicket))
                        return NoContent();
                    else
                        return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while updating a Ticket ID: {TicketId}", ex);
                return StatusCode(500, "A problem occured while handling your requet");
            }
        }

        //[HttpPatch("{TicketId}")]
        //public ActionResult TicketUpdateMetaData(Guid TicketId, JsonPatchDocument<TicketsDTO> patchDocument)
        //{
        //    try
        //    {
        //        TicketsDTO? orignalTicket = dummydata.Tickets.Find(t => t.id == TicketId);

        //        if (orignalTicket == null)
        //            return NotFound();
        //        else
        //        {
        //            var patchedDoc = new TicketsDTO()
        //            {
        //                UpdatedBy = orignalTicket.UpdatedBy,
        //                // UpdatedAt = UpdatedAt
        //            };

        //            patchDocument.ApplyTo(patchedDoc, ModelState);

        //            if (!ModelState.IsValid)
        //            {
        //                return BadRequest();
        //            }

        //            orignalTicket.UpdatedBy = patchedDoc.UpdatedBy;
        //            // orignalTicket.UpdatedAt = patchedDoc.UpdatedAt;

        //            return NoContent();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogCritical($"Exception occured while Patching a Ticket ID: {TicketId}", ex);
        //        return StatusCode(500, "A problem occured while handling your requet");
        //    }
        //}

        [HttpDelete("{TicketId}")]
        public async Task<IActionResult> DeleteTicket(Guid ticketId)
        {
            try
            {
                bool isDeleted = await _repository.deleteTicketAysnc(ticketId);

                if (isDeleted)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while deleting a Ticket ID: {ticketId}", ex);
                return StatusCode(500, "A problem occured while handling your requet");
            }
        }
    }
}
