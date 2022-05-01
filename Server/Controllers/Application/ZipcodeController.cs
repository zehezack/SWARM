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
    public class ZipcodeController : BaseController<Zipcode>, IBaseController<Zipcode>
    {

        public ZipcodeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Zipcode> lstZipcodes = await _context.Zipcodes.OrderBy(x => x.Zip).ToListAsync();
                return Ok(lstZipcodes);
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
                Zipcode itmZipcodes = await _context.Zipcodes.Where(x => x.Zip == KeyValue.ToString()).FirstOrDefaultAsync();
                return Ok(itmZipcodes);
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
                Zipcode itmZipcodes = await _context.Zipcodes.Where(x => x.Zip == KeyValue.ToString()).FirstOrDefaultAsync();
                _context.Remove(itmZipcodes);
                await _context.SaveChangesAsync();
                return Ok(itmZipcodes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Zipcode _Zipcode) //overwrite
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Zipcodes.Where(x => x.Zip == _Zipcode.Zip).FirstOrDefaultAsync();

                if (context == null)
                {
                    await Post(_Zipcode);
                    return Ok();
                }

                context.Zip = _Zipcode.Zip;
                context.City = _Zipcode.City;
                context.State = _Zipcode.State;



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
        public async Task<IActionResult> Post([FromBody] Zipcode _Zipcode) //insert
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Zipcodes.Where(x => x.Zip == _Zipcode.Zip).FirstOrDefaultAsync();

                if (context != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                context = new Zipcode();

                context.Zip = _Zipcode.Zip;
                context.City = _Zipcode.City;
                context.State = _Zipcode.State;


                _context.Zipcodes.Add(context);

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
