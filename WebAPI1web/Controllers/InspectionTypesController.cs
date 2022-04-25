#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI1web.Data;
using WebAPI1web.Models;

namespace WebAPI1web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InspectionTypesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InspectionType>>> GetInspectionTypes()
        {
            return await _context.InspectionTypes.ToListAsync();
        }
    }
}
