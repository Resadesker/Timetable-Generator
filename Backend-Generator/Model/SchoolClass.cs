namespace Backend_Generator.Model
{
    public class SchoolClass
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<SubjectRequirement> SubjectRequirements { get; set; } = new();
    }

    public class SubjectRequirement
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int WeeklyHours { get; set; }
    }
}
