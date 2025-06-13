namespace Backend_Generator.Model
{
    public class ScheduleEntry
    {
        public int Id { get; set; }
        public int SchoolClassId { get; set; }
        public SchoolClass SchoolClass { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        // ↓ NEW: duplicate columns to index on
        public int TeacherId { get; set; }
        public int RoomId { get; set; }

        public int DayOfWeek { get; set; }  // 0–4
        public int HourOfDay { get; set; }  // 0–6
    }
}
