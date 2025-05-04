using Backend_Generator.Model;

namespace Backend_Generator
{
    public class Scheduler
    {
        private readonly List<SchoolClass> _classes;
        private readonly List<Subject> _subjects;
        private readonly List<Teacher> _teachers;
        private readonly List<Room> _rooms;

        private const int MaxLessonsPerDay = 10;
        private const int MaxConsecutiveLessons = 6;

        public Scheduler(List<SchoolClass> classes, List<Subject> subjects, List<Teacher> teachers, List<Room> rooms)
        {
            _classes = classes;
            _subjects = subjects;
            _teachers = teachers;
            _rooms = rooms;
        }

        public List<LessonAssignment> GenerateSchedule()
        {
            var assignments = new List<LessonAssignment>();

            foreach (var schoolClass in _classes)
            {
                var classSchedule = new Dictionary<DayOfWeek, bool[]>(); // Class timeslot usage
                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                    classSchedule[day] = new bool[MaxLessonsPerDay];

                foreach (var req in schoolClass.SubjectRequirements)
                {
                    var subject = req.Subject;
                    int hoursLeft = req.WeeklyHours;

                    for (int h = 0; h < hoursLeft; h++)
                    {
                        var assigned = TryAssignLesson(schoolClass, subject, assignments, classSchedule);
                        if (!assigned)
                            throw new Exception($"Unable to assign {subject.Name} to {schoolClass.Name}");
                    }
                }
            }

            return assignments;
        }

        private bool TryAssignLesson(SchoolClass schoolClass, Subject subject, List<LessonAssignment> assignments, Dictionary<DayOfWeek, bool[]> classSchedule)
        {
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                for (int period = 0; period < MaxLessonsPerDay; period++)
                {
                    if (classSchedule[day][period]) continue;

                    if (!IsConsecutiveLimitOkay(classSchedule[day], period)) continue;

                    var teacher = _teachers.FirstOrDefault(t =>
                        t.CanTeachSubjects.Contains(subject) &&
                        t.Availability.TryGetValue(day, out var avail) &&
                        avail[period] &&
                        !assignments.Any(a => a.Teacher == t && a.TimeSlot.Day == day && a.TimeSlot.Period == period));

                    if (teacher == null) continue;

                    var room = _rooms.FirstOrDefault(r =>
                        r.RoomType == subject.RequiredRoomType &&
                        r.Availability.TryGetValue(day, out var avail) &&
                        avail[period] &&
                        !assignments.Any(a => a.Room == r && a.TimeSlot.Day == day && a.TimeSlot.Period == period));

                    if (room == null) continue;

                    assignments.Add(new LessonAssignment
                    {
                        Class = schoolClass,
                        Subject = subject,
                        Teacher = teacher,
                        Room = room,
                        TimeSlot = new TimeSlot { Day = day, Period = period }
                    });

                    classSchedule[day][period] = true;
                    teacher.Availability[day][period] = false;
                    room.Availability[day][period] = false;
                    return true;
                }
            }

            return false;
        }

        private bool IsConsecutiveLimitOkay(bool[] daySchedule, int period)
        {
            int count = 0;

            for (int i = Math.Max(0, period - MaxConsecutiveLessons + 1); i <= period; i++)
            {
                int consec = 0;
                for (int j = 0; j < MaxConsecutiveLessons && i + j < MaxLessonsPerDay; j++)
                {
                    if (daySchedule[i + j]) consec++;
                    else break;
                }
                if (consec >= MaxConsecutiveLessons)
                    return false;
            }

            return true;
        }
    }

}
