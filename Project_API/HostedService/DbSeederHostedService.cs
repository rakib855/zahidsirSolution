using Project_API.Models;

namespace Project_API.HostedService
{
    public class DbSeederHostedService : IHostedService
    {

        IServiceProvider serviceProvider;
        public DbSeederHostedService(
            IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {

                var db = scope.ServiceProvider.GetRequiredService<TeacherDbContext>();

                await SeedDbAsync(db);

            }
        }
        public async Task SeedDbAsync(TeacherDbContext db)
        {
            await db.Database.EnsureCreatedAsync();
            if (!db.Subjects.Any())
            {
                var sb1 = new Subject { SubjectName = "C#", Title = "Pro Angular", TotalHour = 100 };
                var sb2 = new Subject { SubjectName = "GAVE", Title = "LoGo Mordarnism", TotalHour = 160 };
                var sb3 = new Subject { SubjectName = "NT", Title = "Network Programmability and Automation", TotalHour = 90 };
                await db.Subjects.AddAsync(sb1);
                await db.Subjects.AddAsync(sb2);
                await db.Subjects.AddAsync(sb3);

                var st1 = new Student { StudentName = "Anika Arabi", AnnualCost = 200000.00M, Picture = "1.jpg", Continuing = true };
                var st2 = new Student { StudentName = "Nuzat Tabassum", AnnualCost = 200000.00M, Picture = "2.jpg", Continuing = true };
                var st3 = new Student { StudentName = "Mridul Khan", AnnualCost = 200000.00M, Picture = "3.jpg", Continuing = true };
                await db.Students.AddAsync(st1);
                await db.Students.AddAsync(st2);
                await db.Students.AddAsync(st3);

                var t1 = new Teacher { TeacherName = "Habibul Haq", DateOfBirth = DateTime.Today.AddDays(-365 * 50), TeacherType = TeacherType.Senior, Subject = sb1 };
                t1.TeacherStudents.Add(new TeacherStudent { Teacher = t1, Student = st1, Fee = 30000 });
                await db.Teachers.AddAsync(t1);

                var t2 = new Teacher { TeacherName = "Shahedur Rahman", DateOfBirth = DateTime.Today.AddDays(-365 * 50), TeacherType = TeacherType.Senior, Subject = sb2 };
                t2.TeacherStudents.Add(new TeacherStudent { Teacher = t2, Student = st2, Fee = 30000 });
                t2.TeacherStudents.Add(new TeacherStudent { Teacher = t2, Student = st1, Fee = 30000 });
                await db.Teachers.AddAsync(t2);

                var t3 = new Teacher { TeacherName = "Mr. Rakibul", DateOfBirth = DateTime.Today.AddDays(-365 * 35), TeacherType = TeacherType.Junior, Subject = sb3 };
                t3.TeacherStudents.Add(new TeacherStudent { Teacher = t3, Student = st3, Fee = 30000 });
                t3.TeacherStudents.Add(new TeacherStudent { Teacher = t3, Student = st1, Fee = 30000 });
                await db.Teachers.AddAsync(t3);
                await db.SaveChangesAsync();

            }

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        
    }
}
