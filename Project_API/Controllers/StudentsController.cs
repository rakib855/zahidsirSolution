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
using Project_API.ViewModels.Input;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private IWebHostEnvironment env;
        IUnitOfWork unitOfWork;
        IGenericRepo<Student> repo;
        public StudentsController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            this.unitOfWork = unitOfWork;
            this.repo = this.unitOfWork.GetRepo<Student>();
            this.env = env;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var data = await this.repo.GetAllAsync();
            return data.ToList();
        }
        [HttpGet("VM")]
        public async Task<ActionResult<IEnumerable<StudentViewModel>>> GetStudentViewModels()
        {
            var data = await this.repo.GetAllAsync(p => p.Include(x => x.TeacherStudents));
            return data.ToList().Select(p => new StudentViewModel
            {
                StudentId = p.StudentId,
                StudentName = p.StudentName,
                AnnualCost = p.AnnualCost,
                Continuing = p.Continuing,
                CanDelete = !p.TeacherStudents.Any(),
                Picture = p.Picture

            }).ToList();
        }
        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await this.repo.GetAsync(x => x.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            await this.repo.UpdateAsync(student);

            try
            {
                await this.unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }

            return NoContent();
        }[HttpPut("{id}/VM")]
        public async Task<IActionResult> PutStudentViewModel(int id, StudentInputModel student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            var existing = await this.repo.GetAsync(p=> p.StudentId== id);
            if (existing != null)
            {
                existing.StudentName= student.StudentName;
                existing.AnnualCost = student.AnnualCost ;
                existing.Continuing= student.Continuing;
                await this.repo.UpdateAsync(existing);
            }

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

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            await this.repo.AddAsync(student);
            await this.unitOfWork.CompleteAsync();

            return student;
        }
        /// <summary>
        // to insert student without image
        ///
        [HttpPost("VM")]
        public async Task<ActionResult<Student>> PostStudentInput(StudentInputModel student)
        {
            var newStudent = new Student
            {
                StudentName = student.StudentName,
                AnnualCost = student.AnnualCost,
                Continuing = student.Continuing,
                Picture = "no-product-image-400x400.png"
            };
            await this.repo.AddAsync(newStudent);
            await this.unitOfWork.CompleteAsync();

            return newStudent;
        }
        [HttpPost("Upload/{id}")]
        public async Task<ImagePathResponse> UploadPicture(int id, IFormFile picture)
        {
            var product = await this.repo.GetAsync(p => p.StudentId == id);
            var ext = Path.GetExtension(picture.FileName);
            string fileName = Guid.NewGuid() + ext;
            string savePath = Path.Combine(this.env.WebRootPath, "Pictures", fileName);
            FileStream fs = new FileStream(savePath, FileMode.Create);
            picture.CopyTo(fs);
            fs.Close();
            product.Picture = fileName;
            await this.repo.UpdateAsync(product);
            await this.unitOfWork.CompleteAsync();
            return new ImagePathResponse { PictureName = fileName };
        }
        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await this.repo.GetAsync(p => p.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            await this.repo.DeleteAsync(student);
            await this.unitOfWork.CompleteAsync();

            return NoContent();
        }


    }
}
