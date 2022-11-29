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
    public class SubjectsController : ControllerBase
    {

        IUnitOfWork unitOfWork;
        IGenericRepo<Subject> repo;
        public SubjectsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.repo = this.unitOfWork.GetRepo<Subject>();
        }

        // GET: api/Subjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
        {
            var data = await this.repo.GetAllAsync();
            return data.ToList();
        }
        [HttpGet("VM")]
        public async Task<ActionResult<IEnumerable<SubjectViewModel>>> GetSubjectViewModels()
        {
            var data = await this.repo.GetAllAsync(x => x.Include(c => c.Teachers));
            return data.Select(s => new SubjectViewModel
            {
                SubjectId = s.SubjectId,
                SubjectName = s.SubjectName,
                Title = s.Title,
                TotalHour = s.TotalHour,
                CanDelete = s.Teachers.Count == 0
            }).ToList();
        }
        /// <summary>
        /// to get all subjects with teacher entries
        /////////////////////////////////////////////
        [HttpGet("WithTeachers")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectWithTeachers()
        {
            var data = await this.repo.GetAllAsync(x => x.Include(c => c.Teachers));
            return data.ToList();
        }
        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetSubject(int id)
        {
            var subject = await this.repo.GetAsync(c => c.SubjectId == id);

            if (subject == null)
            {
                return NotFound();
            }

            return subject;
        }
        /// <summary>
        /// to get single subjects with teacher entries
        /////////////////////////////////////////////
        [HttpGet("{id}/WithTeachers")]
        public async Task<ActionResult<Subject>> GetSubjectWithTeachers(int id)
        {
            var subject = await this.repo.GetAsync(c => c.SubjectId == id, x => x.Include(c => c.Teachers));

            if (subject == null)
            {
                return NotFound();
            }

            return subject;
        }
        // PUT: api/Subjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubject(int id, Subject subject)
        {
            if (id != subject.SubjectId)
            {
                return BadRequest();
            }

            await this.repo.UpdateAsync(subject);

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

        // POST: api/Subjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subject>> PostSubject(Subject subject)
        {
            await this.repo.AddAsync(subject);
            await unitOfWork.CompleteAsync();

            return subject;
        }

        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subject = await repo.GetAsync(c => c.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }

            await this.repo.DeleteAsync(subject);
            await unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}

