using HelpDesk.api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.api.Controllers
{
    [ApiController]
    [Route("v1/tickets")]
    public class TicketController : ControllerBase
    {
        public ILogger<TicketController> _logger { get; }

        public TicketController(ILogger<TicketController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult ListTickets()
        {
            //dummydata.FillData();
            return Ok(dummydata.Tickets);
        }

        [HttpGet("{id}", Name = "GetTicketByID")]
        public ActionResult<TicketsDTO> GetTicket(Guid id)
        {
            try
            {
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
        public ActionResult<TicketsDTO> CreateTicket(TicketsDTO newTicket)
        {
            try
            {
                newTicket.id = Guid.NewGuid();
                dummydata.InsertTicket(newTicket);

                //return CreatedAtRoute("GetTicketByID", newTicket.id );
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

        [HttpPatch("{TicketId}")]
        public ActionResult TicketUpdateMetaData(Guid TicketId, JsonPatchDocument<TicketsDTO> patchDocument)
        {
            try
            {
                TicketsDTO? orignalTicket = dummydata.Tickets.Find(t => t.id == TicketId);

                if (orignalTicket == null)
                    return NotFound();
                else
                {
                    var patchedDoc = new TicketsDTO()
                    {
                        UpdatedBy = orignalTicket.UpdatedBy,
                        // UpdatedAt = UpdatedAt
                    };

                    patchDocument.ApplyTo(patchedDoc, ModelState);

                    if (!ModelState.IsValid)
                    {
                        return BadRequest();
                    }

                    orignalTicket.UpdatedBy = patchedDoc.UpdatedBy;
                    // orignalTicket.UpdatedAt = patchedDoc.UpdatedAt;

                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while Patching a Ticket ID: {TicketId}", ex);
                return StatusCode(500, "A problem occured while handling your requet");
            }
        }

        [HttpDelete("{TicketId}")]
        public IActionResult DeleteTicket(Guid ticketId)
        {
            try
            {
                TicketsDTO? orignalTicket = dummydata.Tickets.Find(t => t.id == ticketId);

                if (orignalTicket == null)
                    return NotFound();
                else
                {
                    if (dummydata.DeleteTicket(ticketId))
                    {
                        return NoContent();
                    }
                    else
                    {
                        return NotFound();
                    }
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
