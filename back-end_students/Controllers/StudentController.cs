using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_end_students.Datas;
using back_end_students.DTO;
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
    }
}
