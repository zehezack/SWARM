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
        [Route("GetCourse")]
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
        [Route("GetCourse/{KeyValue}/")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Course itmCourse = await _context.Courses.Where(x => x.CourseNo == KeyValue).FirstOrDefaultAsync();
            //Course itmCourse = await _context.Courses.Where(x => x.CourseNo == pCourseNo).FirstOrDefaultAsync();
            return Ok(itmCourse);
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
                var _Crse = await _context.Courses.Where(x => x.CourseNo == _Course.CourseNo).FirstOrDefaultAsync();

                if (_Crse == null)
                {
                    await Post(_Course);
                    return Ok();
                }

                _Crse.CourseNo = _Course.CourseNo;
                _Crse.Cost = _Course.Cost;
                _Crse.Description = _Course.Description;
                _Crse.Prerequisite = _Course.Prerequisite;
                _Crse.PrerequisiteSchoolId = _Course.PrerequisiteSchoolId;
                _Crse.SchoolId = _Course.SchoolId;
                _context.Update(_Crse);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok();
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
                var _Crse = await _context.Courses.Where(x => x.CourseNo == _Course.CourseNo).FirstOrDefaultAsync();

                if (_Crse != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                _Crse = new Course();
                _Crse.Cost = _Course.Cost;
                _Crse.Description = _Course.Description;
                _Crse.Prerequisite = _Course.Prerequisite;
                _Crse.PrerequisiteSchoolId = _Course.PrerequisiteSchoolId;
                _Crse.SchoolId = _Course.SchoolId;

                
                _context.Courses.Add(_Crse);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Crse);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
