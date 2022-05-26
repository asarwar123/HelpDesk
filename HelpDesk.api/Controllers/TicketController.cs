using HelpDesk.api.Models;
using HelpDesk.api.Services;
using HelpDesk.api.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Text.Json;

namespace HelpDesk.api.Controllers
{
    [ApiController]
    [Route("v1/tickets")]

    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _repository;
        private readonly IMapper _mapper;
        private const int maxPageSize = 10;

        public ILogger<TicketController> _logger { get; }

        public TicketController(ILogger<TicketController> logger, ITicketRepository repository,IMapper mapper)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));

            _repository = repository ?? 
                throw new ArgumentNullException(nameof(repository));

            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}", Name = "GetTicketByID")]
        public async Task<ActionResult<TicketsDTO>> GetTickets(Guid id)
        {
            try
            {
                Ticket? allTickets = await _repository.getTicketAsync(id);

                TicketsDTO ticketsDto = _mapper.Map<TicketsDTO>(allTickets);

                if (ticketsDto == null)
                {
                    _logger.LogInformation($"Ticket with id '{id}' wasn't find");
                    return NotFound();
                }

                return Ok(ticketsDto);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting Ticket ID: {id}", ex);
                return StatusCode(500, "A problem occured while handling your requet");
            }
        }

        [HttpGet]
        public async Task<ActionResult<TicketsDTO>> GetAllTickets(string? filterText,string? queryString,int pageSize=10,int pageNumber=1)
        {
            try
            {
                if (pageSize > maxPageSize)
                    pageSize = maxPageSize;

                //IEnumerable<Ticket> allTickets = await _repository.getTicketsAsync(filterText,queryString,pageSize,pageNumber);

                var (allTickets,paginationmetadata) = await _repository.getTicketsAsync(filterText, queryString, pageSize, pageNumber);


                IEnumerable<TicketsDTO> ticketsDto = _mapper.Map<IEnumerable<TicketsDTO>>(allTickets);

                if (ticketsDto.Count() == 0)
                {
                    _logger.LogInformation($"No data found");
                    return NotFound();
                }

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationmetadata));

                return Ok(ticketsDto);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while getting All Tickets", ex);
                return StatusCode(500, "A problem occured while handling your requet");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TicketsDTO>> CreateTicket(TicketCreationDTO newCreationTicket)
        {
            try
            {
                Ticket newTicket = _mapper.Map<Ticket>(newCreationTicket);

                await _repository.createTicketAsync(newTicket);

                //TicketsDTO displayTicket = _mapper.Map<TicketsDTO>(newTicket);
                //return CreatedAtRoute("GetTicketByID",
                //    new
                //    {
                //        id = newTicket.id
                //    });

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
                 return NotFound();

                //TicketsDTO? orignalTicket = dummydata.Tickets.Find(t => t.id == TicketId);

                //if (orignalTicket == null)
                //    return NotFound();
                //else
                //{
                //    if (dummydata.UpdateTicket(TicketId, updatedTicket))
                //        return NoContent();
                //    else
                //        return NotFound();
                //  }
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
