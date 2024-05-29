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
    public class PolygongeofencesController : ControllerBase
    {
        private readonly DemoContext2 _context;

        public PolygongeofencesController(DemoContext2 context)
        {
            _context = context;
        }

        // GET: api/Polygongeofences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Polygongeofence>>> Getpolygongeofences()
        {
            var polygongeofences = await _context.polygongeofences.ToListAsync();
            GVAR gvar = new GVAR();
            gvar.DicOfDT["Polygongeofence"] = ConvertToDataTable(polygongeofences);
            return Ok(new { sts = 1, gvar = gvar });

        }
        public DataTable ConvertToDataTable(IEnumerable<Polygongeofence> geofences)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("GeofenceID");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            dt.Columns.Add("id");



            foreach (var geofence in geofences)
            {
                dt.Rows.Add(geofence.Geofenceid, geofence.Latitude, geofence.Longitude, geofence.Id);
            }

            return dt;

        }

        // GET: api/Polygongeofences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Polygongeofence>> GetPolygongeofence(long id)
        {
            var polygongeofence = await _context.polygongeofences.FindAsync(id);
            var polygongeofenceDic = new ConcurrentDictionary<string, string>
            {
                ["GeofenceID"] = polygongeofence.Geofenceid.ToString(),
                ["Latitude"] = polygongeofence.Latitude.ToString(),
                ["Longitude"] = polygongeofence.Longitude.ToString(),
                ["id"] = polygongeofence.Id.ToString(),


            };
            if (polygongeofence == null)
            {
                return NotFound(new { sts = 0 });
            }
            GVAR gvar = new GVAR();
            gvar.DicOfDic["Polygongeofence"] = polygongeofenceDic;



            return Ok(new { sts = 1, gvar = gvar });
        }


        // PUT: api/Polygongeofences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPolygongeofence(long id, GVAR gvar)
        {
            if (gvar == null || !gvar.DicOfDic.ContainsKey("Tags"))
            {
                return BadRequest(new { sts = 0 });
            }
            var tags = gvar.DicOfDic["Tags"];

            var PolygongeofenceJson = JsonConvert.SerializeObject(tags);
            var Polygongeofencenfo = JsonConvert.DeserializeObject<Polygongeofence>(PolygongeofenceJson);

            _context.Entry(Polygongeofencenfo).State = EntityState.Modified;
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

        // POST: api/Polygongeofences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Polygongeofence>> PostPolygongeofence(GVAR jsonData)
        {
            var geofence = JsonConvert.SerializeObject(jsonData.DicOfDic["Tags"]);
            var geofenceInfo = JsonConvert.DeserializeObject<Polygongeofence>(geofence);
            geofenceInfo ??= new Polygongeofence();
            _context.polygongeofences.Add(geofenceInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeofence", new { sts = 1 });
        }

        // DELETE: api/Polygongeofences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolygongeofence(long id)
        {
            var polygongeofence = await _context.polygongeofences.FindAsync(id);
            if (polygongeofence == null)
            {
                return NotFound(new { sts = 0 });
            }

            _context.polygongeofences.Remove(polygongeofence);
            await _context.SaveChangesAsync();

            return BadRequest(new { sts = 0 });
        }

        private bool PolygongeofenceExists(long id)
        {
            return _context.polygongeofences.Any(e => e.Id == id);
        }
    }
}
