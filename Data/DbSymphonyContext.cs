using Microsoft.EntityFrameworkCore;
using Symphony_LTD.Models;

namespace Symphony_LTD.Data
{
    public class DbSymphonyContext: DbContext
    {
       public DbSymphonyContext(DbContextOptions<DbSymphonyContext> options): base (options) {}

        public DbSet<Course> Courses { get; set; }
        public DbSet<Admin> _Admin { get; set; }
        public DbSet<Contact> _Contact { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<FAQ> FAQS { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Result> Results { get; set; }
    }
}
