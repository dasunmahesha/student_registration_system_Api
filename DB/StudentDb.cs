using Microsoft.EntityFrameworkCore;
using StudentRegApi.Models;

namespace StudentRegApi.DB
{
    public class StudentDb : DbContext
    {
        public StudentDb(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Student> Student { get; set; }
    }
}
