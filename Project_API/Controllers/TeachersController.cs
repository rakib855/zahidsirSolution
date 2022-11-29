using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.Models;
using Project_API.Repositories.Interfaces;
using Project_API.ViewModels;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepo<Teacher> repo;
        private readonly IGenericRepo<TeacherStudent> itemRepo;
        public TeachersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.repo = this.unitOfWork.GetRepo<Teacher>();
            this.itemRepo = this.unitOfWork.GetRepo<TeacherStudent>();
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            var data = await this.repo.GetAllAsync();
            return data.ToList();
        }
        [HttpGet("VM")]
        public async Task<ActionResult<IEnumerable<TeacherViewModel>>> GetTeacherVMs()
        {

            var data = await this.repo.GetAllAsync(x => x.Include(o => o.TeacherStudents).ThenInclude(oi => oi.Student)
                                                        .Include(o => o.Subject));
            return data.Select(o => new TeacherViewModel
            {
                TeacherId = o.TeacherId,
                SubjectId = o.SubjectId,
                TeacherName = o.TeacherName,
                DateOfBirth = o.DateOfBirth,
                TeacherType = o.TeacherType,
                SubjectName = o.Subject.SubjectName,
                
            })
            .ToList();
        }
        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            var teacher = await this.repo.GetAsync(o => o.TeacherId == id);

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }
	[HttpGet("{id}/OI")]
        public async Task<ActionResult<Teacher>> GetTeacherWithSubjectItems(int id)
        {
            var teacher = await this.repo.GetAsync(o => o.TeacherId == id, x=> x.Include(o=> o.TeacherStudents).ThenInclude(oi=> oi.Student)
                                                                            .Include(o=> o.Subject));

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }

        // PUT: api/Teachers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return BadRequest();
            }

            await this.repo.UpdateAsync(teacher);

            try
            {
                await this.unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

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
            await this.repo.UpdateAsync(teacher);

            try
            {
                await this.unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }

            return NoContent();
        }

        // POST: api/Teachers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
        {
            await this.repo.AddAsync(teacher);
            await this.unitOfWork.CompleteAsync();

            return teacher;
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await this.repo.GetAsync(o => o.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            await this.repo.DeleteAsync(teacher);
            await this.unitOfWork.CompleteAsync();

            return NoContent();
        }


    }
}
