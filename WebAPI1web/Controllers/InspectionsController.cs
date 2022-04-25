#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI1web.Data;
using WebAPI1web.Models;
using System.Security.Claims;
using WebAPI1web.DTOs;

namespace WebAPI1web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InspectionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetInspections()
        {
            return await _context.Inspections.Select(x => new {Inspection=x,InspectionType=x.InspectionType.Name,User=x.User.Name}).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Inspection>> GetInspection(int id)
        {
            var inspection = await _context.Inspections.FindAsync(id);
            if (inspection == null) return NotFound();
            return inspection;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInspection(int id, InspectionDTO inspectionDTO)
        {
            try
            {
                if (id != inspectionDTO.Id) return BadRequest();
                var inspection = await _context.Inspections.SingleOrDefaultAsync(x => x.Id == inspectionDTO.Id);
                if (inspection == null) return BadRequest();
                var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == inspection.UserId);
                if (user == null) return BadRequest();
                var username = GetCurrentUsername();
                if (user.Name != username) return Unauthorized("You are not authorized to modify this inspection!");
                inspection.Status = inspectionDTO.Status;
                inspection.Comment = inspectionDTO.Comment;
                inspection.InspectionTypeId = inspectionDTO.InspectionTypeId;
                inspection.UserId = inspectionDTO.UserId;
                _context.Entry(inspection).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }
            return NoContent();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Inspection>> PostInspection(InspectionDTO inspectionDTO)
        {
            try 
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == inspectionDTO.UserId);
                if(user==null) return BadRequest();
                var username = GetCurrentUsername();
                if (user.Name != username) return Unauthorized("You are not authorized to add this inspection!");
                var inspection = new Inspection
                {
                    Status = inspectionDTO.Status,
                    Comment = inspectionDTO.Comment,
                    InspectionTypeId = inspectionDTO.InspectionTypeId,
                    UserId = inspectionDTO.UserId
                };
                _context.Inspections.Add(inspection);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetInspection", new { id = inspection.Id }, inspection);
            }
            catch {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspection(int id)
        {
            try
            {
                var inspection = await _context.Inspections.SingleOrDefaultAsync(x=>x.Id==id);
                if (inspection == null) return NotFound();
                var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == inspection.UserId);
                if (user == null) return BadRequest();
                var username = GetCurrentUsername();
                if (user.Name != username) return Unauthorized("You are not authorized to delete this inspection!");
                _context.Inspections.Remove(inspection);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        private string GetCurrentUsername()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity != null)
            {
                var userClaims = identity.Claims;
                return userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            }
            return null;
        }
    }
}