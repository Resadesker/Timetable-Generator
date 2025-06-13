namespace Backend_Generator.Model
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
    }
}
