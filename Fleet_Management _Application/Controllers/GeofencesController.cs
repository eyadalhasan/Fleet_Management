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
    public class GeofencesController : ControllerBase
    {
        private readonly DemoContext2 _context;

        public GeofencesController(DemoContext2 context)
        {
            _context = context;
        }

        // GET: api/Geofences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Geofence>>> Getgeofences()
        {


            var geofences = await _context.geofences.ToListAsync();
            GVAR gvar = new GVAR();
            gvar.DicOfDT["Geofences"] = ConvertToDataTable(geofences);

            return Ok(new { sts = 1, gvar });


        }

        // GET: api/Geofences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Geofence>> GetGeofence(long id)
        {
            var geofence = await _context.geofences.FindAsync(id);
            var geeofenceDic = new ConcurrentDictionary<string, string>
            {
                ["GeofenceID"] = geofence.Geofenceid.ToString(),
                ["GeofenceType"] = geofence.Geofencetype.ToString(),
                ["AddedDate"] = geofence.Addeddate.ToString(),
                ["StrokeColor"] = geofence.Strockcolor.ToString(),
                ["StrokeOpacity"] = geofence.Strockopacity.ToString(),
                ["StrokeWeight"] = geofence.Strockweight.ToString(),
                ["FillColor"] = geofence.Fillcolor.ToString(),
                ["FillOpacity"] = geofence.Fillopacity.ToString()
            };
            GVAR gvar = new GVAR();

            gvar.DicOfDic["Geofence"] = geeofenceDic;


            if (geofence == null)
            {
                return NotFound(new { sts = 0 }); ;
            }

            return Ok(new { sts = 1, Geofence = gvar.DicOfDic["Geofence"] });
        }
        public DataTable ConvertToDataTable(IEnumerable<Geofence> geofences)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("GeofenceID");
            dt.Columns.Add("GeofenceType");
            dt.Columns.Add("AddedDate");
            dt.Columns.Add("StrokeColor");
            dt.Columns.Add("StrokeOpacity");
            dt.Columns.Add("StrokeWeight");
            dt.Columns.Add("FillColor");
            dt.Columns.Add("FillOpacity");

            foreach (var geofence in geofences)
            {
                dt.Rows.Add(geofence.Geofenceid, geofence.Geofencetype, geofence.Addeddate.ToString(), geofence.Strockcolor, geofence.Strockopacity, geofence.Strockweight, geofence.Fillcolor, geofence.Fillopacity);
            }

            return dt;

        }

        // PUT: api/Geofences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeofence(long id, GVAR gvar)
        {
            {
                if (gvar == null || !gvar.DicOfDic.ContainsKey("Tags"))
                {
                    return BadRequest(new { sts = 0 });
                }
                var tags = gvar.DicOfDic["Tags"];

                var geofenceJson = JsonConvert.SerializeObject(tags);
                var geofenceInfo = JsonConvert.DeserializeObject<Geofence>(geofenceJson);

                _context.Entry(geofenceInfo).State = EntityState.Modified;
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

        }
        // POST: api/Geofences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Geofence>> PostGeofence(GVAR jsonData)
        {
            var geofence = JsonConvert.SerializeObject(jsonData.DicOfDic["Tags"]);
            var geofenceInfo = JsonConvert.DeserializeObject<Geofence>(geofence);
            geofenceInfo ??= new Geofence();
            _context.geofences.Add(geofenceInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeofence", new { sts = 1 });
        }

        // DELETE: api/Geofences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeofence(long id)
        {
            var geofence = await _context.geofences.FindAsync(id);
            if (geofence == null)
            {
                return NotFound();
            }

            _context.geofences.Remove(geofence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GeofenceExists(long id)
        {
            return _context.geofences.Any(e => e.Geofenceid == id);
        }
    }
}
