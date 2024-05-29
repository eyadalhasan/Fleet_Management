using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fleet_Management__Application.Data;
using Fleet_Management__Application.Models;
using System.Data;
using FPro;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace Fleet_Management__Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CirclegeofencesController : ControllerBase
    {
        private readonly DemoContext2 _context;

        public CirclegeofencesController(DemoContext2 context)
        {
            _context = context;
        }

        // GET: api/Circlegeofences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Circlegeofence>>> Getcirclegeofences()
        {


            var geofences = await _context.circlegeofences.ToListAsync();
            GVAR gvar = new GVAR();
            gvar.DicOfDT["Geofences"] = ConvertToDataTable(geofences);

            return Ok(new { sts = 1, gvar });

        }
        public DataTable ConvertToDataTable(IEnumerable<Circlegeofence> geofences)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("GeofenceID");
            dt.Columns.Add("Radius");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            dt.Columns.Add("id");



            foreach (var geofence in geofences)
            {
                dt.Rows.Add(geofence.Geofenceid, geofence.Radius, geofence.Latitude, geofence.Longitude, geofence.Id);
            }

            return dt;

        }

        // GET: api/Circlegeofences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Circlegeofence>> GetCirclegeofence(long id)
        {
            var circlegeofence = await _context.circlegeofences.FindAsync(id);
            var circlegeofenceDic = new ConcurrentDictionary<string, string>
            {
                ["GeofenceID"] = circlegeofence.Geofenceid.ToString(),
                ["Radius"] = circlegeofence.Radius.ToString(),
                ["Latitude"] = circlegeofence.Latitude.ToString(),
                ["Longitude"] = circlegeofence.Longitude.ToString(),
                ["id"] = circlegeofence.Id.ToString(),


            };
            GVAR gvar = new GVAR();
            gvar.DicOfDic["Circlegeofence"] = circlegeofenceDic;


            if (circlegeofence == null)
            {
                return NotFound(new { sts = 0 });
            }

            return Ok(new { sts = 1, gvar });
        }

        // PUT: api/Circlegeofences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCirclegeofence(long id, GVAR gvar)
        {
            if (gvar == null || !gvar.DicOfDic.ContainsKey("Tags"))
            {
                return BadRequest(new { sts = 0 });
            }
            var tags = gvar.DicOfDic["Tags"];

            var CirclegeofenceeJson = JsonConvert.SerializeObject(tags);
            var Circlegeofenceinfo = JsonConvert.DeserializeObject<Circlegeofence>(CirclegeofenceeJson);

            _context.Entry(Circlegeofenceinfo).State = EntityState.Modified;
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

        // POST: api/Circlegeofences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Circlegeofence>> PostCirclegeofence(GVAR jsonData)
        {
            var geofence = JsonConvert.SerializeObject(jsonData.DicOfDic["Tags"]);
            var geofenceInfo = JsonConvert.DeserializeObject<Circlegeofence>(geofence);
            geofenceInfo ??= new Circlegeofence();
            _context.circlegeofences.Add(geofenceInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeofence", new { sts = 1 });
        }

        // DELETE: api/Circlegeofences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCirclegeofence(long id)
        {
            var circlegeofence = await _context.circlegeofences.FindAsync(id);
            if (circlegeofence == null)
            {
                return NotFound(new { sts = 0 });
            }

            _context.circlegeofences.Remove(circlegeofence);
            await _context.SaveChangesAsync();

            return BadRequest(new { sts = 0 });
        }

        private bool CirclegeofenceExists(long id)
        {
            return _context.circlegeofences.Any(e => e.Id == id);
        }
    }
}
