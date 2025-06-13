using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend_Generator.Data;

namespace Backend_Generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            using var db = new AppDbContext();
            var grouped = db.Schedule
                .Include(e => e.Lesson).ThenInclude(l => l.Subject)
                .Include(e => e.Lesson).ThenInclude(l => l.Teacher)
                .Include(e => e.Lesson).ThenInclude(l => l.Room)
                .Include(e => e.SchoolClass)
                .ToList()
                .GroupBy(e => e.SchoolClassId)
                .Select(g => new {
                    ClassId = g.Key,
                    ClassName = g.First().SchoolClass.Name,
                    Entries = g.Select(e => new {
                        e.DayOfWeek,
                        e.HourOfDay,
                        Subject = e.Lesson.Subject.Name,
                        Teacher = e.Lesson.Teacher.Name,
                        Room = e.Lesson.Room.Name
                    })
                });

            return Ok(grouped);
        }
    }
}
