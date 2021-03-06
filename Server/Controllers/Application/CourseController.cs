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
    public class CourseController : BaseController<Course>, IBaseController<Course>
    {

        public CourseController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Course> lstCourses = await _context.Courses.OrderBy(x => x.CourseNo).ToListAsync();
                return Ok(lstCourses);
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
                Course itmCourse = await _context.Courses.Where(x => x.CourseNo == KeyValue).FirstOrDefaultAsync();
                return Ok(itmCourse);
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
                Course itmCourse = await _context.Courses.Where(x => x.CourseNo == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmCourse);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Course _Course) //overwrite
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Courses.Where(x => x.CourseNo == _Course.CourseNo).FirstOrDefaultAsync();

                if (context == null)
                {
                    await Post(_Course);
                    return Ok();
                }

                context.CourseNo = _Course.CourseNo;
                context.Cost = _Course.Cost;
                context.Description = _Course.Description;
                context.Prerequisite = _Course.Prerequisite;
                context.PrerequisiteSchoolId = _Course.PrerequisiteSchoolId;
                context.SchoolId = _Course.SchoolId;


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
        public async Task<IActionResult> Post([FromBody] Course _Course) //insert
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Courses.Where(x => x.CourseNo == _Course.CourseNo).FirstOrDefaultAsync();

                if (context != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                context = new Course();
                context.Cost = _Course.Cost;
                context.Description = _Course.Description;
                context.Prerequisite = _Course.Prerequisite;
                context.PrerequisiteSchoolId = _Course.PrerequisiteSchoolId;
                context.SchoolId = _Course.SchoolId;

                
                _context.Courses.Add(context);

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
