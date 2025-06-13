using Backend_Generator.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend_Generator.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<SchoolClass> Classes { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<ScheduleEntry> Schedule { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder opts)
            => opts.UseSqlite("Data Source=Timetable.db");

        protected override void OnModelCreating(ModelBuilder mb)
        {
            // --- UNIQUE CONSTRAINTS to enforce no double-booking ---
            mb.Entity<ScheduleEntry>()
              .HasIndex(e => new { e.SchoolClassId, e.DayOfWeek, e.HourOfDay })
              .IsUnique();

            // one slot per teacher
            mb.Entity<ScheduleEntry>()
              .HasIndex(e => new { e.TeacherId, e.DayOfWeek, e.HourOfDay })
              .IsUnique();

            // one slot per room
            mb.Entity<ScheduleEntry>()
              .HasIndex(e => new { e.RoomId, e.DayOfWeek, e.HourOfDay })
              .IsUnique();

            // --- SEED MASTER DATA once on migration/EnsureCreated ---
            mb.Entity<SchoolClass>().HasData(
                new SchoolClass { Id = 1, Name = "Class A" },
                new SchoolClass { Id = 2, Name = "Class B" },
                new SchoolClass { Id = 3, Name = "Class C" }
            );

            mb.Entity<Teacher>().HasData(
                new Teacher { Id = 1, Name = "Teacher X" },
                new Teacher { Id = 2, Name = "Teacher Y" },
                new Teacher { Id = 3, Name = "Teacher Z" }
            );

            mb.Entity<Room>().HasData(
                new Room { Id = 1, Name = "Room 101" },
                new Room { Id = 2, Name = "Room 401" },
                new Room { Id = 3, Name = "Room 501" },
                new Room { Id = 4, Name = "Room 301" },
                new Room { Id = 5, Name = "Room 201" },
                new Room { Id = 6, Name = "Lab 1" },
                new Room { Id = 7, Name = "Lab 3" },
                new Room { Id = 8, Name = "Room 601" },
                new Room { Id = 9, Name = "Room 801" },
                new Room { Id = 10, Name = "Room 701" },
                new Room { Id = 11, Name = "Room 901" },
                new Room { Id = 12, Name = "Room 1001" },
                new Room { Id = 13, Name = "Room 1101" },
                new Room { Id = 14, Name = "Room 1201" },
                new Room { Id = 15, Name = "Gym" }
            );

            mb.Entity<Subject>().HasData(
                new Subject { Id = 1, Name = "Math" },
                new Subject { Id = 2, Name = "English" },
                new Subject { Id = 3, Name = "Physics" },
                new Subject { Id = 4, Name = "History" },
                new Subject { Id = 5, Name = "Science" },
                new Subject { Id = 6, Name = "Chemistry" },
                new Subject { Id = 7, Name = "Biology" },
                new Subject { Id = 8, Name = "Geography" },
                new Subject { Id = 9, Name = "Music" },
                new Subject { Id = 10, Name = "Art" },
                new Subject { Id = 11, Name = "Computer Science" },
                new Subject { Id = 12, Name = "Literature" },
                new Subject { Id = 13, Name = "Economics" },
                new Subject { Id = 14, Name = "Philosophy" },
                new Subject { Id = 15, Name = "Physical Education" }
            );

            mb.Entity<Lesson>().HasData(
                new Lesson { Id = 1, SubjectId = 1, TeacherId = 1, RoomId = 1 },
                new Lesson { Id = 2, SubjectId = 2, TeacherId = 1, RoomId = 2 },
                new Lesson { Id = 3, SubjectId = 3, TeacherId = 1, RoomId = 3 },
                new Lesson { Id = 4, SubjectId = 4, TeacherId = 1, RoomId = 4 },
                new Lesson { Id = 5, SubjectId = 5, TeacherId = 1, RoomId = 5 },
                new Lesson { Id = 6, SubjectId = 6, TeacherId = 2, RoomId = 6 },
                new Lesson { Id = 7, SubjectId = 7, TeacherId = 2, RoomId = 7 },
                new Lesson { Id = 8, SubjectId = 8, TeacherId = 2, RoomId = 8 },
                new Lesson { Id = 9, SubjectId = 9, TeacherId = 2, RoomId = 9 },
                new Lesson { Id = 10, SubjectId = 10, TeacherId = 2, RoomId = 10 },
                new Lesson { Id = 11, SubjectId = 11, TeacherId = 3, RoomId = 11 },
                new Lesson { Id = 12, SubjectId = 12, TeacherId = 3, RoomId = 12 },
                new Lesson { Id = 13, SubjectId = 13, TeacherId = 3, RoomId = 13 },
                new Lesson { Id = 14, SubjectId = 14, TeacherId = 3, RoomId = 14 },
                new Lesson { Id = 15, SubjectId = 15, TeacherId = 3, RoomId = 15 }
            );
        }
    }
}
