/*using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Backend_Generator.Model;

namespace Backend_Generator
{
    public class DatabaseContext : DbContext
    {
            //public DbSet<Student> Students { get; set; }
            //public DbSet<City> Cities { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder options)
            {
                options.UseSqlite("Data Source=school.db");
                //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                /*modelBuilder.Entity<Student>().ToTable("Students");
                modelBuilder.Entity<City>().ToTable("Cities");

                modelBuilder.Entity<City>()
                    .HasMany(c => c.Students)
                    .WithOne(s => s.City)
                    .HasForeignKey(s => s.CityId);*/
            /*}
        }

    public class Timeslot
    {
        public DayOfWeek Day { get; set; }
        public int Hour { get; set; } // e.g. 1 to 8
    }

    public class LessonRequirement
    {
        public SchoolClass Class { get; set; }
        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }
        public int HoursPerWeek { get; set; }
    }

    public class ScheduledLesson
    {
        public Timeslot Timeslot { get; set; }
        public SchoolClass Class { get; set; }
        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }
        public Classroom Room { get; set; }
    }
}*/