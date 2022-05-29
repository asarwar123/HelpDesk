using HelpDesk.api.Models;
using HelpDesk.api.Services;
using HelpDesk.api.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace HelpDesk.api.Controllers
{
    /// <summary>
    /// All actions about tickets
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Route("v{version:apiVersion}/tickets")]
    [Authorize]

    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _repository;
        private readonly IMapper _mapper;
        private const int maxPageSize = 10;
        private readonly ILogger<TicketController> _logger;


        public TicketController(ILogger<TicketController> logger, ITicketRepository repository,IMapper mapper)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));

            _repository = repository ?? 
                throw new ArgumentNullException(nameof(repository));

            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get a single tickets by Ticket ID
        /// </summary>
        /// <param name="id">ID of the ticket</param>
        /// <returns>An actionresult of TicketsDTO</returns>
        /// <response code="200">Returns the requested Ticket</response>
        [HttpGet("{id}", Name = "GetTicketByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

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

        /// <summary>
        /// Get all tickets
        /// </summary>
        /// <param name="filterText">Filter by Text</param>
        /// <param name="queryString">Search query string</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="pageNumber">Page number to show</param>
        /// <returns>List of matching tickets</returns>
        /// <response code>Returns List of tickets</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Create a new ticket
        /// </summary>
        /// <param name="newCreationTicket">Object of TicketCreationDTO</param>
        /// <returnsNewly created ticket></returns>
        /// <response code="201">Create new ticket in DB</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TicketsDTO>> CreateTicket(TicketCreationDTO newCreationTicket)
        {
            try
            {
                Ticket newTicket = _mapper.Map<Ticket>(newCreationTicket);
                await _repository.createTicketAsync(newTicket);

                return Ok(newTicket);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception occured while creating new Ticket", ex);
                return StatusCode(500, "A problem occured while handling your requet");
            }
        }

        /// <summary>
        /// Update ticket details
        /// </summary>
        /// <param name="TicketId">Ticket ID to be updated</param>
        /// <param name="updatedTicket">updated ticket</param>
        /// <returns></returns>
        [HttpPut("{TicketId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult UpdateTicket(Guid TicketId, TicketsDTO updatedTicket)
        {
            try
            {
                 return NotFound();
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


        /// <summary>
        /// Delete a ticket
        /// </summary>
        /// <param name="ticketId">ID of ticket tobe deleted</param>
        /// <returns></returns>
        /// <response code="204">Ticket deleted successfuly</response>
        [HttpDelete("{TicketId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


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
