using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegApi.DB;
using StudentRegApi.Models;
using System.Threading.Tasks;

namespace StudentRegApi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class StudentController : Controller
    {
        private readonly StudentDb studentDb;
        public StudentController(StudentDb studentDb)
        {
            this.studentDb = studentDb;
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
        public async Task<IActionResult> AddStudent([FromBody] Student studentreg)
        {
            await studentDb.Student.AddAsync(studentreg);
            await studentDb.SaveChangesAsync();

            return Ok(studentreg);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] string id,Student updatestudent) 
        {
            if (!int.TryParse(id, out int studentId))
            {
                return BadRequest("Invalid student ID");
            }
            var student = await studentDb.Student.FindAsync(studentId);

            if (student==null) {

                return NotFound();
            }
            student.FirstName = updatestudent.FirstName;
            student.LastName = updatestudent.LastName;
            student.NIC = updatestudent.NIC;
            student.Address = updatestudent.Address;
            student.Mobile = updatestudent.Mobile;
            student.Email = updatestudent.Email;
            student.ProfileImageUrl = updatestudent.ProfileImageUrl;
            student.DateOfBirth = updatestudent.DateOfBirth;


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
            var student = await studentDb.Student.FindAsync();

            if(student==null)
            {
                return NotFound();
            }
            studentDb.Student.Remove(student);
            await studentDb.SaveChangesAsync();

            return Ok();
        }


    }
}
