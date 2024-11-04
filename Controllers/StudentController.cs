using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegApi.DB;
using StudentRegApi.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StudentRegApi.Services;

namespace StudentRegApi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class StudentController : Controller
    {
        private readonly StudentDb studentDb;
        private readonly S3Service s3Service;

        public StudentController(StudentDb studentDb, S3Service s3Service)
        {
            this.studentDb = studentDb;
            this.s3Service = s3Service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudent()
        {
            var students = await studentDb.Student.ToListAsync();
            return Ok(students);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] string id)
        {
            if (!int.TryParse(id, out int studentId))
            {
                return BadRequest("Invalid student ID");
            }

            var student = await studentDb.Student.FirstOrDefaultAsync(x => x.StudentId == studentId);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromForm] StudentDto studentDto)
        {
            var imageUrl = await s3Service.UploadFileAsync(studentDto.Image);
            var student = new Student
            {
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Mobile = studentDto.Mobile,
                Email = studentDto.Email,
                NIC = studentDto.NIC,
                DateOfBirth = studentDto.DateOfBirth,
                Address = studentDto.Address,
                ProfileImageUrl = imageUrl
            };
            await studentDb.Student.AddAsync(student);
            await studentDb.SaveChangesAsync();

            return Ok(student);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] string id, [FromForm] StudentDto studentDto)
        {
            if (!int.TryParse(id, out int studentId))
            {
                return BadRequest("Invalid student ID");
            }
            var student = await studentDb.Student.FindAsync(studentId);

            if (student == null)
            {
                return NotFound();
            }

            if (studentDto.Image != null)
            {
                var imageUrl = await s3Service.UploadFileAsync(studentDto.Image);
                student.ProfileImageUrl = imageUrl;
            }

            student.FirstName = studentDto.FirstName;
            student.LastName = studentDto.LastName;
            student.NIC = studentDto.NIC;
            student.Address = studentDto.Address;
            student.Mobile = studentDto.Mobile;
            student.Email = studentDto.Email;
            student.DateOfBirth = studentDto.DateOfBirth;

            await studentDb.SaveChangesAsync();
            return Ok(student);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] string id)
        {
            if (!int.TryParse(id, out int studentId))
            {
                return BadRequest("Invalid student ID");
            }
            var student = await studentDb.Student.FindAsync(studentId);

            if (student == null)
            {
                return NotFound();
            }
            studentDb.Student.Remove(student);
            await studentDb.SaveChangesAsync();

            return Ok();
        }
    }

    public class StudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Mobile { get; set; }
        public string Email { get; set; }
        public long NIC { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public IFormFile Image { get; set; }
    }
}
