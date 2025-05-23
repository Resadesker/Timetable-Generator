namespace Backend_Generator.Model
{
    public class Timeslot
    {
        public int Day;
        public int Hour;

        public Timeslot(int day, int hour)
        {
            Day = day;
            Hour = hour;
        }

        public override string ToString()
        {
            return $"Day {Day + 1}, Lesson {Hour + 1}";
        }
    }
}
