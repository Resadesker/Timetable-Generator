namespace Backend_Generator.Model
{
    public class Lesson
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public ICollection<ScheduleEntry> ScheduleEntries { get; set; }
    }
}
