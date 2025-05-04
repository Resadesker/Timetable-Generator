namespace Backend_Generator.Model
{
    public class LessonAssignment
    {
        public SchoolClass Class { get; set; }
        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }
        public Room Room { get; set; }
        public TimeSlot TimeSlot { get; set; }
    }
}
