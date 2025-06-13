namespace Backend_Generator.Model
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<ScheduleEntry> ScheduleEntries { get; set; }
    }
}
