namespace Backend_Generator
{
    public class TimetableEngine
    {
        /*public List<ScheduledLesson> GenerateGreedyTimetable(
            List<LessonRequirement> lessonRequirements,
            List<Timeslot> availableTimeslots,
            List<Classroom> rooms)
        {
            var timetable = new List<ScheduledLesson>();
            var used = new HashSet<string>(); // format: $"{day}-{hour}-{classId|teacherId|roomId}"

            foreach (var requirement in lessonRequirements.OrderByDescending(r => r.HoursPerWeek))
            {
                int scheduled = 0;

                foreach (var slot in availableTimeslots)
                {
                    foreach (var room in rooms)
                    {
                        string classKey = $"{slot.Day}-{slot.Hour}-CLASS-{requirement.Class.Id}";
                        string teacherKey = $"{slot.Day}-{slot.Hour}-TEACHER-{requirement.Teacher.Id}";
                        string roomKey = $"{slot.Day}-{slot.Hour}-ROOM-{room.Id}";

                        if (used.Contains(classKey) || used.Contains(teacherKey) || used.Contains(roomKey))
                            continue;

                        // Schedule it
                        timetable.Add(new ScheduledLesson
                        {
                            Timeslot = slot,
                            Class = requirement.Class,
                            Subject = requirement.Subject,
                            Teacher = requirement.Teacher,
                            Room = room
                        });

                        used.Add(classKey);
                        used.Add(teacherKey);
                        used.Add(roomKey);
                        scheduled++;

                        if (scheduled == requirement.HoursPerWeek)
                            break;
                    }
                    if (scheduled == requirement.HoursPerWeek)
                        break;
                }

                // ❌ Not enough slots found (optional logging)
                if (scheduled < requirement.HoursPerWeek)
                {
                    Console.WriteLine($"Warning: Could not schedule all {requirement.HoursPerWeek} hours for {requirement.Class.Name} in {requirement.Subject.Name}");
                }
            }

            return timetable;
        }*/
    }

}
