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
    public class SchoolController : BaseController<School>, IBaseController<School>
    {

        public SchoolController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<School> lstSchool = await _context.Schools.OrderBy(x => x.SchoolId).ToListAsync();
                return Ok(lstSchool);
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
                School lstSchool = await _context.Schools.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
                return Ok(lstSchool);
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
                School lstSchool = await _context.Schools.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(lstSchool);
                await _context.SaveChangesAsync();
                return Ok(lstSchool);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] School _School) //overwrite
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Schools.Where(x => x.SchoolId == _School.SchoolId).FirstOrDefaultAsync();

                if (context == null)
                {
                    await Post(_School);
                    return Ok();
                }

                context.SchoolId = _School.SchoolId;
                context.SchoolName = _School.SchoolName;


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
        public async Task<IActionResult> Post([FromBody] School _School) //insert
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Schools.Where(x => x.SchoolId == _School.SchoolId).FirstOrDefaultAsync();

                if (context != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                context = new School();
                context.SchoolId = _School.SchoolId;
                context.SchoolName = _School.SchoolName;


                _context.Schools.Add(context);

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
