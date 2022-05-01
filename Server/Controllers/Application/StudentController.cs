using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers;
using SWARM.Server.Models;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Application
{
    public class StudentController : BaseController<Student>, IBaseController<Student>
    {

        public StudentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Student> lstStudents = await _context.Students.OrderBy(x => x.StudentId).ToListAsync();
                return Ok(lstStudents);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{KeyValue}/")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            try
            {
                Student itmStudents = await _context.Students.Where(x => x.StudentId == KeyValue).FirstOrDefaultAsync();
                return Ok(itmStudents);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            try
            {
                Student itmStudents = await _context.Students.Where(x => x.StudentId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmStudents);
                await _context.SaveChangesAsync();
                return Ok(itmStudents);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Student _Student) //overwrite
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Students.Where(x => x.StudentId == _Student.StudentId).FirstOrDefaultAsync();

                if (context == null)
                {
                    await Post(_Student);
                    return Ok();
                }

                context.StudentId = _Student.StudentId;
                context.Salutation = _Student.Salutation;
                context.FirstName = _Student.FirstName;
                context.StreetAddress = _Student.StreetAddress;
                context.Zip = _Student.Zip;
                context.Phone = _Student.Phone;
                context.Employer = _Student.Employer;
                context.RegistrationDate = _Student.RegistrationDate;


                _context.Update(context);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(context);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student _Student) //insert
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Students.Where(x => x.StudentId == _Student.StudentId).FirstOrDefaultAsync();

                if (context != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                context = new Student();
                context.StudentId = _Student.StudentId;
                context.Salutation = _Student.Salutation;
                context.FirstName = _Student.FirstName;
                context.StreetAddress = _Student.StreetAddress;
                context.Zip = _Student.Zip;
                context.Phone = _Student.Phone;
                context.Employer = _Student.Employer;
                context.RegistrationDate = _Student.RegistrationDate;


                _context.Students.Add(context);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(context);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
