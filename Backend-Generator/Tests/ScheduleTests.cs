using NUnit.Framework;
using Backend_Generator;
using Backend_Generator.Data;
using Backend_Generator.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Backend_Generator.Tests
{
    public class ScheduleTests
    {
        private AppDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestTimetable")
                .Options;

            _context = new AppDbContext(true);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        public void GenerateAndSave_AssignsTwoLessonsPerClassAndSubject()
        {

            // Act
            TimetableGenerator.GenerateAndSave();

            var schedule = _context.Schedule.ToList();
            var classes = _context.Classes.ToList();
            var lessons = _context.Lessons.ToList();

            foreach (var cls in classes)
            {
                foreach (var lesson in lessons)
                {
                    var assignedCount = schedule.Count(e =>
                        e.SchoolClassId == cls.Id &&
                        e.LessonId == lesson.Id);
                    Assert.LessOrEqual(assignedCount, 2, $"Class {cls.Name} has too many {lesson.SubjectId} lessons.");
                }
            }
        }

        [Test]
        public void GenerateAndSave_DoesNotDoubleBookClassTeacherOrRoom()
        {
            // Act
            TimetableGenerator.GenerateAndSave();

            var schedule = _context.Schedule.ToList();

            // Check for duplicates per (class, day, hour)
            var classConflicts = schedule
                .GroupBy(e => new { e.SchoolClassId, e.DayOfWeek, e.HourOfDay })
                .Where(g => g.Count() > 1)
                .ToList();
            Assert.IsEmpty(classConflicts, "Some classes are double-booked.");

            // Check for duplicates per (teacher, day, hour)
            var teacherConflicts = schedule
                .GroupBy(e => new { e.TeacherId, e.DayOfWeek, e.HourOfDay })
                .Where(g => g.Count() > 1)
                .ToList();
            Assert.IsEmpty(teacherConflicts, "Some teachers are double-booked.");

            // Check for duplicates per (room, day, hour)
            var roomConflicts = schedule
                .GroupBy(e => new { e.RoomId, e.DayOfWeek, e.HourOfDay })
                .Where(g => g.Count() > 1)
                .ToList();
            Assert.IsEmpty(roomConflicts, "Some rooms are double-booked.");
        }
    }
}
