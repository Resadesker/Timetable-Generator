namespace Backend_Generator.Model
{
    public class TimeSlot
    {
        public DayOfWeek Day { get; set; }
        public int Period { get; set; } // 0-9 for 10 lessons
    }
}
