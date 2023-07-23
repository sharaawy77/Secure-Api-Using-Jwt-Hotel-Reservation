using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Secure_Api_Jwt.Models;
using Secure_Api_Jwt.Services;

namespace Secure_Api_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservationsController : ControllerBase
    {
        private readonly IReserveService reserveService;

        public reservationsController(IReserveService reserveService)
        {
            this.reserveService = reserveService;
        }

        // GET: api/reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<reservation>>> Getreservations()
        {
          if (await reserveService.GetReservationsAsync() == null)
          {
              return NotFound();
          }
            return Ok(await reserveService.GetReservationsAsync());
        }

        // GET: api/reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<reservation>> Getreservation(int id)
        {
          if (await reserveService.GetReservationsAsync() == null)
          {
              return NotFound();
          }
            var reservation = await reserveService.GetReservationAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation) ;
        }

        // PUT: api/reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        
        public async Task<IActionResult> Putreservation(int id, reservation model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }
            else
            {
                var res=await reserveService.UpdateReservation(model);
                if (!res.IsNullOrEmpty())
                {
                    return Problem(res);
                }
                return Ok();
            }

            

            
        }

        // POST: api/reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Reserve")]
        public async Task<ActionResult<reservation>> Postreservation(reservation model)
        {
          if (!ModelState.IsValid)
          {
              return BadRequest(model);
          }
            var res = await reserveService.ReserveAsync(model);
            if (!res.IsNullOrEmpty())
            {
                return Problem(res);
            }
            return Ok();
        }

        // DELETE: api/reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletereservation(int id)
        {
            if (await reserveService.GetReservationsAsync() == null)
            {
                return NotFound();
            }
            var reservation = await reserveService.GetReservationAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            var res = await reserveService.DeleteReservation(id);
            if (!res.IsNullOrEmpty()) { return Problem(res); }
            return Ok();
        }

        
    }
}
