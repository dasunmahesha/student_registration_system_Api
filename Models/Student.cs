using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StudentRegApi.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Mobile { get; set; }
        public string Email { get; set; }
        public long NIC { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string ProfileImageUrl { get; set; }

        public static implicit operator DbSet<object>(Student v)
        {
            throw new NotImplementedException();
        }
    }
}
