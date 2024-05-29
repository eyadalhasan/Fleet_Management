using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fleet_Management__Application.Data;
using Fleet_Management__Application.Models;
using System.Collections.Concurrent;
using FPro;
using System.Data;
using Newtonsoft.Json;

namespace Fleet_Management__Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RectanglegeofencesController : ControllerBase
    {
        private readonly DemoContext2 _context;

        public RectanglegeofencesController(DemoContext2 context)
        {
            _context = context;
        }

        // GET: api/Rectanglegeofences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rectanglegeofence>>> Getrectanglegeofences()
        {
            var rectanglegeofences = await _context.rectanglegeofences.ToListAsync();
            GVAR gvar = new GVAR();
            gvar.DicOfDT["Rectanglegeofence"] = ConvertToDataTable(rectanglegeofences);
            return Ok(new { sts = 1, gvar });
        }
        public DataTable ConvertToDataTable(IEnumerable<Rectanglegeofence> geofences)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("GeofenceID");
            dt.Columns.Add("North");
            dt.Columns.Add("East");
            dt.Columns.Add("West");
            dt.Columns.Add("South");
            dt.Columns.Add("id");



            foreach (var geofence in geofences)
            {
                dt.Rows.Add(geofence.Geofenceid, geofence.North, geofence.East, geofence.West, geofence.South,geofence.Id);
            }

            return dt;

        }
        // GET: api/Rectanglegeofences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rectanglegeofence>> GetRectanglegeofence(long id)
        {
            var rectanglegeofence = await _context.rectanglegeofences.FindAsync(id);

            var rectanglegeofenceDic = new ConcurrentDictionary<string, string>
            {
                ["GeofenceID"] = rectanglegeofence.Geofenceid.ToString(),
                ["North"] = rectanglegeofence.North.ToString(),
                ["Easte"] = rectanglegeofence.East.ToString(),
                ["West"] = rectanglegeofence.West.ToString(),
                ["South"] = rectanglegeofence.South.ToString(),
                ["id"] = rectanglegeofence.Id.ToString(),

            };
            GVAR gvar = new GVAR();
            gvar.DicOfDic["Rectanglegeofence"] = rectanglegeofenceDic;

            if (rectanglegeofence == null)
            {
                return NotFound(new { sts = 0 });
            }

            return Ok(new { sts = 1, gvar = gvar });
        }

        // PUT: api/Rectanglegeofences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRectanglegeofence(long id, GVAR gvar)
        {
            if (gvar == null || !gvar.DicOfDic.ContainsKey("Tags"))
            {
                return BadRequest(new { sts = 0 });
            }
            var tags = gvar.DicOfDic["Tags"];

            var RectanglegeofenceJson = JsonConvert.SerializeObject(tags);
            var RectanglegeofenceInfo = JsonConvert.DeserializeObject<Rectanglegeofence>(RectanglegeofenceJson);

            _context.Entry(RectanglegeofenceInfo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!_context.vehicles.Any(e => e.Vehicleid == id))
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

        // POST: api/Rectanglegeofences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rectanglegeofence>> PostRectanglegeofence(GVAR jsonData)
        {
            var geofence = JsonConvert.SerializeObject(jsonData.DicOfDic["Tags"]);
            var geofenceInfo = JsonConvert.DeserializeObject<Rectanglegeofence>(geofence);
            geofenceInfo ??= new Rectanglegeofence();
            _context.rectanglegeofences.Add(geofenceInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeofence", new
            {
                sts = 1,

            });
        }

        // DELETE: api/Rectanglegeofences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRectanglegeofence(long id)
        {
            var rectanglegeofence = await _context.rectanglegeofences.FindAsync(id);
            if (rectanglegeofence == null)
            {
                return NotFound(new { sts = 0 });
            }

            _context.rectanglegeofences.Remove(rectanglegeofence);
            await _context.SaveChangesAsync();

            return BadRequest(new { sts = 0 });
        }

        private bool RectanglegeofenceExists(long id)
        {
            return _context.rectanglegeofences.Any(e => e.Id == id);
        }
    }
}
