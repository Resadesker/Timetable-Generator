using System;
using System.Collections.Generic;
using System.Linq;
using Backend_Generator.Data;
using Backend_Generator.Model;

namespace Backend_Generator
{
    public class TimetableGenerator
    {
        private const int DaysPerWeek = 5;
        private const int HoursPerDay = 7;
        private const int LessonsPerSubject = 2;

        public static void GenerateAndSave()
        {
            using var db = new AppDbContext();

            // Create DB + seed master data only once
            db.Database.EnsureCreated();

            var classes = db.Classes.ToList();
            var lessons = db.Lessons.ToList();
            var rng = new Random();

            foreach (var cls in classes)
            {
                foreach (var lesson in lessons)
                {
                    int assigned = 0;
                    int maxAttempts = 100; // Prevent infinite loop
                    int attempts = 0;

                    while (assigned < LessonsPerSubject && attempts < maxAttempts)
                    {
                        int day = rng.Next(0, DaysPerWeek);
                        int hour = rng.Next(0, HoursPerDay);

                        bool classBusy = db.Schedule.Any(e =>
                            e.SchoolClassId == cls.Id &&
                            e.DayOfWeek == day &&
                            e.HourOfDay == hour);

                        bool teacherBusy = db.Schedule.Any(e =>
                            e.TeacherId == lesson.TeacherId &&
                            e.DayOfWeek == day &&
                            e.HourOfDay == hour);

                        bool roomBusy = db.Schedule.Any(e =>
                            e.RoomId == lesson.RoomId &&
                            e.DayOfWeek == day &&
                            e.HourOfDay == hour);

                        if (!classBusy && !teacherBusy && !roomBusy)
                        {
                            db.Schedule.Add(new ScheduleEntry
                            {
                                SchoolClassId = cls.Id,
                                LessonId = lesson.Id,
                                TeacherId = lesson.TeacherId,
                                RoomId = lesson.RoomId,
                                DayOfWeek = day,
                                HourOfDay = hour
                            });
                            db.SaveChanges();
                            assigned++;
                        }

                        attempts++;
                    }

                    if (assigned < LessonsPerSubject)
                    {
                        Console.WriteLine($"Could not assign {LessonsPerSubject} lessons of {lesson.SubjectId} to {cls.Name}");
                    }

                }
            }
        }
    }
}
