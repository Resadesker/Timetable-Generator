using Backend_Generator.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend_Generator.Data
{
    public class AppDbContext : DbContext
    {
        /*public DbSet<SchoolClass> Classes { get; set; }
        public DbSet<Lesson> Subjects { get; set; }
        public DbSet<TimeSlot> Teachers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<LessonAssignment> Lessons { get; set; }*/

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
