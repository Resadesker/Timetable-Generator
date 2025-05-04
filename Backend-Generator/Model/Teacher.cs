using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Generator.Model
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Subject> CanTeachSubjects { get; set; } = new();
        public string AvailabilityJson { get; set; }

        [NotMapped]
        public Dictionary<DayOfWeek, bool[]> Availability
        {
            get => string.IsNullOrEmpty(AvailabilityJson)
                ? CreateEmptyAvailability()
                : System.Text.Json.JsonSerializer.Deserialize<Dictionary<DayOfWeek, bool[]>>(AvailabilityJson);
            set => AvailabilityJson = System.Text.Json.JsonSerializer.Serialize(value);
        }

        private Dictionary<DayOfWeek, bool[]> CreateEmptyAvailability()
        {
            var dict = new Dictionary<DayOfWeek, bool[]>();
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                dict[day] = new bool[10]; // 10 lesson slots
            return dict;
        }

    }
}
