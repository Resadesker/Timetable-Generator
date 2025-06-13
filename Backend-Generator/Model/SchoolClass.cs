namespace Backend_Generator.Model
{
    public class SchoolClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ScheduleEntry> ScheduleEntries { get; set; }
    }
}
