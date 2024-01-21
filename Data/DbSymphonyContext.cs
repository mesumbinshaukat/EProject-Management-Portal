using Microsoft.EntityFrameworkCore;
using Symphony_LTD.Models;

namespace Symphony_LTD.Data
{
    public class DbSymphonyContext: DbContext
    {
       public DbSymphonyContext(DbContextOptions<DbSymphonyContext> options): base (options) {}

        public DbSet<Course>? Courses { get; set; }
        public DbSet<Admin>? _Admin { get; set; }
        public DbSet<Contact>? _Contact { get; set; }
        public DbSet<Student>? Students { get; set; }
        public DbSet<FAQ>? FAQS { get; set; }
        public DbSet<Exam>? Exams { get; set; }
        public DbSet<Result>? Results { get; set; }
        public DbSet<Branch>? Branches { get; set; }
        public DbSet<About>? _AboutUs { get; set; }
        public DbSet<Faculty>? _Faculty { get; set; }
        public DbSet<Class>? _Class { get; set; }
        public DbSet<CourseExam>? _CourseExam { get; set; }
        public DbSet<EntranceExam>? _EntranceExam { get; set; }
        public DbSet<EntranceExamResult>? _EntranceExamResult { get; set; }
        public DbSet<CourseExamResult>? _CourseExamResult { get; set; }
        public DbSet<HomeSectionOne>? _HomeSectionOne { get; set; }
        public DbSet<EntranceExamList>? _EntranceExamList {  get; set; }
    }
}
