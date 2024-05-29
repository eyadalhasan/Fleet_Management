using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fleet_Management__Application.Data;
using Fleet_Management__Application.Models;
using FPro;
using System.Data;
using Newtonsoft.Json;

namespace Fleet_Management__Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class driversController : ControllerBase
    {
        private readonly DemoContext2 _context;

        public driversController(DemoContext2 context)
        {
            _context = context;
        }
        public DataTable ConvertToDataTable(IEnumerable<Driver> drivers)
        {
            var table = new DataTable();
            table.Columns.Add("DriverID");
            table.Columns.Add("DriverName");
            table.Columns.Add("PhoneNumber");

            foreach (Driver driver in drivers)
            {
                table.Rows.Add(driver.Driverid, driver.Drivername, driver.Phonenumber);

            }
            return table;

        }

        // GET: api/drivers
        [HttpGet]

        public async Task<ActionResult<IEnumerable<GVAR>>> Getdrivers()
        {
            var drivers = await _context.drivers.ToListAsync();
            var gvar = new GVAR();
            gvar.DicOfDT["drivers"] = ConvertToDataTable(drivers);
            return Ok(new { sts = 1, gvar });
        }



        // GET: api/drivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Driver>> Getdriver(long id)
        {
            var driver = await _context.drivers.FindAsync(id);

            if (driver == null)
            {
                return NotFound();
            }

            return driver;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Putdriver(long id, GVAR gvar)
        {
            if (gvar == null || gvar.DicOfDic == null || !gvar.DicOfDic.ContainsKey("Tags"))
            {
                return BadRequest(new { sts = 0 });
            }
            var driverjson = JsonConvert.SerializeObject(gvar.DicOfDic["Tags"]);
            var driverInfo = JsonConvert.DeserializeObject<Driver>(driverjson);
            driverInfo ??= new Driver();

            _context.Entry(driverInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!driverExists(id))
                {
                    return NotFound(new { sts = 0 });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { sts = 1 });
        }
        // PUT: api/drivers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Putdriver(long id, Driver driver)
        //{
        //    if (id != driver.Driverid)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(driver).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!driverExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/drivers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Driver>> Postdriver(GVAR driverObj)
        {
            try
            {
                var driverJson = JsonConvert.SerializeObject(driverObj.DicOfDic["Tags"]);
                var driver = JsonConvert.DeserializeObject<Driver>(driverJson);
                driver ??= new Driver();
                _context.drivers.Add(driver);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Getdriver", new { id = driver.Driverid }, new { sts = 1 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { sts = 0 });

            }

        }

        // DELETE: api/drivers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletedriver(long id)
        {
            var driver = await _context.drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound(new { sts = 0 });
            }
            _context.drivers.Remove(driver);
            await _context.SaveChangesAsync();
            return Ok(new { sts = 1 });
        }
        private bool driverExists(long id)
        {
            return _context.drivers.Any(e => e.Driverid == id);
        }
    }
}
