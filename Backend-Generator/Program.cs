using Microsoft.EntityFrameworkCore;
using System.Text;
using Backend_Generator.Model;
using Backend_Generator.Data;
using Backend_Generator;
using System;

int DaysPerWeek = 5;
int HoursPerDay = 7;

using var db = new AppDbContext();
db.Database.ExecuteSqlRaw("delete from Schedule");
// 1) Generate + persist timetable
Console.WriteLine("Started generating...\n");
TimetableGenerator.GenerateAndSave();
Console.WriteLine("Timetable generated and saved.\n");

// 2) Fetch + print
var schedule = db.Schedule
    .Include(e => e.Lesson).ThenInclude(l => l.Subject)
    .Include(e => e.Lesson).ThenInclude(l => l.Teacher)
    .Include(e => e.Lesson).ThenInclude(l => l.Room)
    .ToList();

foreach (var cls in db.Classes)
{
    Console.WriteLine($"Timetable for {cls.Name}:");
    for (int d = 0; d < DaysPerWeek; d++)
        for (int h = 0; h < HoursPerDay; h++)
        {
            var entry = schedule.FirstOrDefault(e =>
                e.SchoolClassId == cls.Id &&
                e.DayOfWeek == d &&
                e.HourOfDay == h);

            if (entry != null)
            {
                var les = entry.Lesson;
                Console.WriteLine($" Day {d + 1}, Lesson {h + 1}: " +
                    $"{les.Subject.Name} with {les.Teacher.Name} in {les.Room.Name}");
            }
            else
            {
                Console.WriteLine($" Day {d + 1}, Lesson {h + 1}: Free");
            }
        }
    Console.WriteLine();
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // Allow any origin
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add DbContext
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlite("Data Source=app.db"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS before other middleware
app.UseCors();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();