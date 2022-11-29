using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.Models;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersContextController : ControllerBase
    {
        private readonly TeacherDbContext _context;

        public TeachersContextController(TeacherDbContext context)
        {
            _context = context;
        }

        // GET: api/TeachersContext
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetOrders()
        {
            return await _context.Teachers.ToListAsync();
        }

        // GET: api/TeachersContext/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetOrder(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }

        // PUT: api/TeachersContext/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return BadRequest();
            }

            _context.Entry(teacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPut("VM/{id}")]
        public async Task<IActionResult> PutTeacherWithStudentItem(int id, Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return BadRequest();
            }
            var existing = await _context.Teachers.Include(x => x.TeacherStudents).FirstAsync(o => o.TeacherId == id);
            _context.TeacherStudents.RemoveRange(existing.TeacherStudents);
            foreach (var item in teacher.TeacherStudents)
            {
                _context.TeacherStudents.Add(new TeacherStudent { TeacherId = teacher.TeacherId, StudentId = item.StudentId, Fee = item.Fee });
            }
            _context.Entry(existing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex.InnerException;

            }

            return NoContent();
        }
        // POST: api/TeachersContext
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostOrder(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = teacher.TeacherId }, teacher);
        }

        // DELETE: api/TeachersContext/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.TeacherId == id);
        }
    }
}
