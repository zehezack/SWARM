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
    public class InstructorController : BaseController<Instructor>, IBaseController<Instructor>
    {

        public InstructorController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Instructor> lstInstructors = await _context.Instructors.OrderBy(x => x.InstructorId).ToListAsync();
                return Ok(lstInstructors);
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
                Instructor itmInstructors = await _context.Instructors.Where(x => x.InstructorId == KeyValue).FirstOrDefaultAsync();
                return Ok(itmInstructors);
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
                Instructor itmInstructors = await _context.Instructors.Where(x => x.InstructorId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmInstructors);
                await _context.SaveChangesAsync();
                return Ok(itmInstructors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Instructor _Instructor) //overwrite
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Instructors.Where(x => x.InstructorId == _Instructor.InstructorId).FirstOrDefaultAsync();

                if (context == null)
                {
                    await Post(_Instructor);
                    return Ok();
                }

                context.SchoolId = _Instructor.SchoolId;
                context.InstructorId = _Instructor.InstructorId;
                context.Salutation = _Instructor.Salutation;
                context.FirstName = _Instructor.FirstName;
                context.LastName = _Instructor.LastName;
                context.StreetAddress = _Instructor.StreetAddress;
                context.Zip = _Instructor.Zip;
                context.Phone = _Instructor.Phone;
                context.StreetAddress = _Instructor.StreetAddress;
                context.StreetAddress = _Instructor.StreetAddress;


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
        public async Task<IActionResult> Post([FromBody] Instructor _Instructor) //insert
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Instructors.Where(x => x.InstructorId == _Instructor.InstructorId).FirstOrDefaultAsync();

                if (context != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                context = new Instructor();
                context.SchoolId = _Instructor.SchoolId;
                context.InstructorId = _Instructor.InstructorId;
                context.Salutation = _Instructor.Salutation;
                context.FirstName = _Instructor.FirstName;
                context.LastName = _Instructor.LastName;
                context.StreetAddress = _Instructor.StreetAddress;
                context.Zip = _Instructor.Zip;
                context.Phone = _Instructor.Phone;
                context.StreetAddress = _Instructor.StreetAddress;
                context.StreetAddress = _Instructor.StreetAddress;


                _context.Instructors.Add(context);

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
