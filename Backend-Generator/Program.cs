using Microsoft.EntityFrameworkCore;
using System.Text;
using Backend_Generator.Model;
using Backend_Generator.Data;
using Backend_Generator;
using System;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=scheduler.db")
    .Options;

using (var context = new AppDbContext(options))
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    var math = new Subject { Name = "Math", RequiredRoomType = RoomType.Standard };
    var bio = new Subject { Name = "Biology", RequiredRoomType = RoomType.Lab };

    context.Subjects.AddRange(math, bio);
    context.SaveChanges();

    var teacher1 = new Teacher
    {
        Name = "Mr. Smith",
        CanTeachSubjects = new List<Subject> { math, bio },
        Availability = CreateFullWeekAvailability()
    };

    var teacher2 = new Teacher
    {
        Name = "Ms. Johnson",
        CanTeachSubjects = new List<Subject> { bio },
        Availability = CreateFullWeekAvailability()
    };

    context.Teachers.AddRange(teacher1, teacher2);
    context.SaveChanges();

    var room1 = new Room { Name = "Room 101", RoomType = RoomType.Standard, Availability = CreateFullWeekAvailability() };
    var room2 = new Room { Name = "Lab A", RoomType = RoomType.Lab, Availability = CreateFullWeekAvailability() };

    context.Rooms.AddRange(room1, room2);
    context.SaveChanges();

    var class1 = new SchoolClass
    {
        Name = "1A",
        SubjectRequirements = new List<SubjectRequirement>
        {
            new SubjectRequirement { Subject = math, WeeklyHours = 5 },
            new SubjectRequirement { Subject = bio, WeeklyHours = 3 }
        }
    };

    context.Classes.Add(class1);
    context.SaveChanges();

    // Load all data from DB
    var classes = context.Classes
        .Include(c => c.SubjectRequirements)
        .ThenInclude(sr => sr.Subject)
        .ToList();

    var subjects = context.Subjects.ToList();
    var teachers = context.Teachers.Include(t => t.CanTeachSubjects).ToList();
    var rooms = context.Rooms.ToList();

    // Generate timetable
    var scheduler = new Scheduler(classes, subjects, teachers, rooms);
    var timetable = scheduler.GenerateSchedule();

    // Print timetable
    Console.WriteLine("Generated Timetable:");
    foreach (var group in timetable.GroupBy(l => l.Class.Name))
    {
        Console.WriteLine($"\nClass: {group.Key}");
        foreach (var lesson in group.OrderBy(l => l.TimeSlot.Day).ThenBy(l => l.TimeSlot.Period))
        {
            var start = TimeSpan.FromHours(8) + TimeSpan.FromMinutes(lesson.TimeSlot.Period * 50);
            var end = start + TimeSpan.FromMinutes(50);
            Console.WriteLine($"{lesson.TimeSlot.Day} {start:hh\\:mm}-{end:hh\\:mm} | {lesson.Subject.Name} with {lesson.Teacher.Name} in {lesson.Room.Name}");
        }
    }
}

// Helper function for full availability
Dictionary<DayOfWeek, bool[]> CreateFullWeekAvailability()
{
    var avail = new Dictionary<DayOfWeek, bool[]>();
    foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
    {
        avail[day] = Enumerable.Repeat(true, 10).ToArray(); // 10 lessons/day
    }
    return avail;
}
