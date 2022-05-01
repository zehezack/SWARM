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
    public class SectionController : BaseController<Section>, IBaseController<Section>
    {

        public SectionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Section> lstSections = await _context.Sections.OrderBy(x => x.SectionId).ToListAsync();
                return Ok(lstSections);
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
                Section itmSections = await _context.Sections.Where(x => x.SectionId == KeyValue).FirstOrDefaultAsync();
                return Ok(itmSections);
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
                Section itmSections = await _context.Sections.Where(x => x.SectionId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmSections);
                await _context.SaveChangesAsync();
                return Ok(itmSections);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Section _Section) //overwrite
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Sections.Where(x => x.SectionId == _Section.SectionId).FirstOrDefaultAsync();

                if (context == null)
                {
                    await Post(_Section);
                    return Ok();
                }

                context.CourseNo = _Section.CourseNo;
                context.SectionNo = _Section.SectionNo;
                context.StartDateTime = _Section.StartDateTime;
                context.Location = _Section.Location;
                context.InstructorId = _Section.InstructorId;
                context.Capacity = _Section.Capacity;
                context.SchoolId = _Section.SchoolId;
                context.Course = _Section.Course;
                context.Instructor = _Section.Instructor;
                context.School = _Section.School;
                context.Enrollments = _Section.Enrollments;
                context.GradeTypeWeights = _Section.GradeTypeWeights;

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
        public async Task<IActionResult> Post([FromBody] Section _Section) //insert
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Sections.Where(x => x.SectionId == _Section.SectionId).FirstOrDefaultAsync();

                if (context != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }


                context = new Section();
                context.CourseNo = _Section.CourseNo;
                context.SectionNo = _Section.SectionNo;
                context.StartDateTime = _Section.StartDateTime;
                context.Location = _Section.Location;
                context.InstructorId = _Section.InstructorId;
                context.Capacity = _Section.Capacity;
                context.SchoolId = _Section.SchoolId;
                context.Course = _Section.Course;
                context.Instructor = _Section.Instructor;
                context.School = _Section.School;
                context.Enrollments = _Section.Enrollments;
                context.GradeTypeWeights = _Section.GradeTypeWeights;


                _context.Sections.Add(context);

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
