using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend_Generator.Data;
using Backend_Generator.Model;
using System.Text.Json;

namespace Backend_Generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SchoolController> _logger;

        public SchoolController(AppDbContext context, ILogger<SchoolController> logger)
        {
            _context = context;
            _logger = logger;
        }

        
    }
}
