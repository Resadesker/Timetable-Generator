using Backend_Generator.Model;

namespace Backend_Generator
{
    public class TimetableGenerator
    {
        private static List<Lesson> Lessons = new List<Lesson>
    {
        new Lesson { Subject = "Math", Teacher = "Teacher X", Room = "Room 101" },
        new Lesson { Subject = "English", Teacher = "Teacher X", Room = "Room 401" },
        new Lesson { Subject = "Physics", Teacher = "Teacher X", Room = "Room 501" },
        new Lesson { Subject = "History", Teacher = "Teacher X", Room = "Room 301" },
        new Lesson { Subject = "Science", Teacher = "Teacher X", Room = "Room 201" },
        new Lesson { Subject = "Chemistry", Teacher = "Teacher Y", Room = "Lab 1" },
        new Lesson { Subject = "Biology", Teacher = "Teacher Y", Room = "Lab 3" },
        new Lesson { Subject = "Geography", Teacher = "Teacher Y", Room = "Room 601" },
        new Lesson { Subject = "Music", Teacher = "Teacher Y", Room = "Room 801" },
        new Lesson { Subject = "Art", Teacher = "Teacher Y", Room = "Room 701" },
        new Lesson { Subject = "Computer Science", Teacher = "Teacher Z", Room = "Room 901" },
        new Lesson { Subject = "Literature", Teacher = "Teacher Z", Room = "Room 1001" },
        new Lesson { Subject = "Economics", Teacher = "Teacher Z", Room = "Room 1101" },
        new Lesson { Subject = "Philosophy", Teacher = "Teacher Z", Room = "Room 1201" },
        new Lesson { Subject = "Physical Education", Teacher = "Teacher Z", Room = "Gym" }
    };

        private static List<string> Classes = new List<string> { "Class A", "Class B", "Class C" };
        private const int DaysPerWeek = 5;
        private const int HoursPerDay = 7;

        private static Dictionary<string, Dictionary<(int, int), Lesson>> classSchedules = new();

        private static Dictionary<(int, int), HashSet<string>> teacherUsage = new();
        private static Dictionary<(int, int), HashSet<string>> roomUsage = new();

        public static void GenerateTimetable()
        {
            Random rng = new();
            foreach (string cls in Classes)
            {
                classSchedules[cls] = new Dictionary<(int, int), Lesson>();
            }

            foreach (string cls in Classes)
            {
                foreach (Lesson lesson in Lessons)
                {
                    int count = 0;
                    while (count < 2)
                    {
                        var timeslot = FindAvailableTimeslot(cls, lesson, rng);
                        if (timeslot == null)
                        {
                            throw new Exception($"No available timeslot for {lesson.Subject} in {cls}");
                        }

                        classSchedules[cls][(timeslot.Day, timeslot.Hour)] = lesson;
                        if (!teacherUsage.ContainsKey((timeslot.Day, timeslot.Hour)))
                            teacherUsage[(timeslot.Day, timeslot.Hour)] = new HashSet<string>();
                        if (!roomUsage.ContainsKey((timeslot.Day, timeslot.Hour)))
                            roomUsage[(timeslot.Day, timeslot.Hour)] = new HashSet<string>();

                        teacherUsage[(timeslot.Day, timeslot.Hour)].Add(lesson.Teacher);
                        roomUsage[(timeslot.Day, timeslot.Hour)].Add(lesson.Room);

                        count++;
                    }
                }
            }

            // Print schedule
            foreach (var cls in Classes)
            {
                Console.WriteLine($"Timetable for {cls}:");
                for (int day = 0; day < DaysPerWeek; day++)
                {
                    for (int hour = 0; hour < HoursPerDay; hour++)
                    {
                        var key = (day, hour);
                        if (classSchedules[cls].TryGetValue(key, out var lesson))
                        {
                            Console.WriteLine($"Day {day + 1}, Lesson {hour + 1}: {lesson}");
                        }
                        else
                        {
                            Console.WriteLine($"Day {day + 1}, Lesson {hour + 1}: Free");
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        private static Timeslot FindAvailableTimeslot(string cls, Lesson lesson, Random rng)
        {
            var shuffled = Enumerable.Range(0, DaysPerWeek)
                .SelectMany(d => Enumerable.Range(0, HoursPerDay).Select(h => new Timeslot(d, h)))
                .OrderBy(_ => rng.Next()).ToList();

            foreach (var timeslot in shuffled)
            {
                var key = (timeslot.Day, timeslot.Hour);
                if (classSchedules[cls].ContainsKey(key)) continue;
                if (teacherUsage.ContainsKey(key) && teacherUsage[key].Contains(lesson.Teacher)) continue;
                if (roomUsage.ContainsKey(key) && roomUsage[key].Contains(lesson.Room)) continue;
                return timeslot;
            }

            return null;
        }
    }
}
