namespace Backend_Generator.Model
{
    public class Lesson
    {
        public string Subject;
        public string Teacher;
        public string Room;

        public override string ToString()
        {
            return $"{Subject} with {Teacher} in {Room}";
        }
    }
}
