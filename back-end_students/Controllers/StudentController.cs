using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_end_students.Datas;
using back_end_students.DTO;
using back_end_students.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace back_end_students.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly Context _context;

        public StudentController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            var student = from students in _context.students
                       join students_descriptions in _context.students_descriptions on students.id equals students_descriptions.student_id
                       select new StudentDTO
                       {
                           Student_id = students.id,
                           Grade = students.grade,
                           Age = students_descriptions.age,
                           First_name = students_descriptions.first_name,
                           Last_name = students_descriptions.last_name,
                           Adress = students_descriptions.adress,
                           Country = students_descriptions.country
                       };

            return await student.ToListAsync();
        }

        [HttpGet("{id}")]
        public ActionResult<StudentDTO> GetStudents_byId(int id)
        {
            var student = from students in _context.students
                       join students_descriptions in _context.students_descriptions on students.id equals students_descriptions.student_id
                       select new StudentDTO
                       {
                           Student_id = students.id,
                           Grade = students.grade,
                           Age = students_descriptions.age,
                           First_name = students_descriptions.first_name,
                           Last_name = students_descriptions.last_name,
                           Adress = students_descriptions.adress,
                           Country = students_descriptions.country
                       };

            var student_by_id = student.ToList().Find(x => x.Student_id == id);

            if (student_by_id == null)
            {
                return NotFound();
            }
            return student_by_id;
        }

        [HttpPost]
        public async Task<ActionResult<StudentDTO>> Add_Students(AddStudent studentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = new Students()
            {
                grade = studentDTO.Grade
            };
            await _context.students.AddAsync(student);
            await _context.SaveChangesAsync();

            var student_description = new Students_description()
            {
                student_id = studentDTO.Student_id,
                age = studentDTO.Age,
                first_name = studentDTO.First_name,
                last_name = studentDTO.Last_name,
                adress = studentDTO.Adress,
                country = studentDTO.Country
            };
            await _context.AddAsync(student_description);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudents", new { id = student.id }, studentDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Students>> Delete_Student(int id)
        {
            var student = _context.students.Find(id);
            var student_description = _context.students_descriptions.SingleOrDefault(x => x.student_id == id);

            if (student == null)
            {
                return NotFound();
            }
            else
            {
                _context.Remove(student);
                _context.Remove(student_description);
                await _context.SaveChangesAsync();
                return student;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update_Students(int id,StudentDTO studentDTO)
        {
            if (id != studentDTO.Student_id || !StudentExists(id))
            {
                return BadRequest();
            }
            else
            {
                var students = _context.students.SingleOrDefault(x => x.id == id);
                var students_description = _context.students_descriptions.SingleOrDefault(x => x.student_id == id);

                students.grade = studentDTO.Grade;
                students_description.student_id = studentDTO.Student_id;
                students_description.age = studentDTO.Age;
                students_description.first_name = studentDTO.First_name;
                students_description.last_name = studentDTO.Last_name;
                students_description.adress = studentDTO.Adress;
                students_description.country = studentDTO.Country;

                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        private bool StudentExists(int id)
        {
            return _context.students.Any(x => x.id == id);
        }
    }
}
