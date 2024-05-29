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
using Newtonsoft.Json;
using Fleet_Management__Application.Services;
using Humanizer;


namespace Fleet_Management__Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutehistoriesController : ControllerBase
    {
        private readonly DemoContext2 _context;
        private readonly WebSocketManagerService _webSocketManagerService;



        public RoutehistoriesController(DemoContext2 context, WebSocketManagerService webSocketManagerService)
        {
            _context = context;
            _webSocketManagerService = webSocketManagerService;

        }

        // GET: api/Routehistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Routehistory>>> Getroutehistories()
        {
            return await _context.routehistories.ToListAsync();

        }

        //// GET: api/Routehistories/5
        //[HttpGet("{id}/{startEpoch}/{endEpoch}")]

        //public async Task<ActionResult<Routehistory>> GetRoutehistory(long id)
        //{
        //    var routehistory = await _context.routehistories.Include(vi => vi.Vehicle).Where(e => e.Vehicleid == id).FirstOrDefaultAsync();

        //    if (routehistory == null)
        //    {
        //        return NotFound();
        //    }

        //    return routehistory;
        //}
        [HttpGet("{vehicleId}/{startTimeEpoch}/{endTimeEpoch}")]
        public async Task<ActionResult<Dictionary<string, DataTable>>> GetRouteHistory(long vehicleId, long startTimeEpoch, long endTimeEpoch)
        {
            try
            {
                // Retrieve route history data for the specified Routehistoryid within the specified time range
                var routeHistories = await _context.routehistories
                    .Include(rh => rh.Vehicle)
                    .Where(rh => rh.Vehicleid == vehicleId && rh.Epoch >= startTimeEpoch && rh.Epoch <= endTimeEpoch)
                    .ToListAsync();

                if (routeHistories == null || !routeHistories.Any())
                {
                    return NotFound(new { sts = 0 });
                }
                var routeHistoryData = new Dictionary<string, object>();


                var dataTable = new DataTable("RouteHistory");

                dataTable.Columns.Add("VehicleID");
                dataTable.Columns.Add("VehicleNumber");
                dataTable.Columns.Add("Address");
                dataTable.Columns.Add("Status");
                dataTable.Columns.Add("Latitude");
                dataTable.Columns.Add("Longitude");
                dataTable.Columns.Add("VehicleDirection");
                dataTable.Columns.Add("GPSSpeed");
                dataTable.Columns.Add("GPSTime");
                foreach (var routeHistory in routeHistories)
                {
                    dataTable.Rows.Add(routeHistory.Vehicleid, routeHistory.Vehicle.Vehiclenumber, routeHistory.Address, routeHistory.Status, routeHistory.Latitude, routeHistory.Longitude, routeHistory.Vehicledirection, routeHistory.Vehiclespeed, routeHistory.Epoch);

                }

                var gvar = new GVAR();
                gvar.DicOfDT["RouteHistory"] = dataTable;



                return Ok(new { STS = 1, gvar });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { STS = 0 });
            }
        }


        // PUT: api/Routehistories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoutehistory(long id, GVAR gvar)
        {
            if (gvar == null || !gvar.DicOfDic.ContainsKey("Tags"))
            {
                return BadRequest(new { sts = 0 });
            }
            var tags = gvar.DicOfDic["Tags"];

            var RoutehistoryJson = JsonConvert.SerializeObject(tags);
            var RoutehistoryInfo = JsonConvert.DeserializeObject<Routehistory>(RoutehistoryJson);

            _context.Entry(RoutehistoryInfo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!_context.routehistories.Any(e => e.Vehicleid == id))
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

        // POST: api/Routehistories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Routehistory>> PostRoutehistory(GVAR routehistoryObj)
        {
            try
            {
                var routeHistoryJson = JsonConvert.SerializeObject(routehistoryObj.DicOfDic["Tags"]);
                var routeHistory = JsonConvert.DeserializeObject<Routehistory>(routeHistoryJson);
                _context.routehistories.Add(routeHistory);
                var broadcastMessage = JsonConvert.SerializeObject(new { Status = "Added", RouteHistory = routeHistory });
                await _context.SaveChangesAsync();
                _webSocketManagerService.Broadcast(broadcastMessage);  // Broadcast to all connected clients



                return CreatedAtAction("GetRoutehistory", new { sts = 1 });

            }
            catch
            {
                return BadRequest(new { sts = 0 });
            }




        }

        // DELETE: api/Routehistories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoutehistory(long id)
        {
            var routehistory = await _context.routehistories.FindAsync(id);
            if (routehistory == null)
            {
                return NotFound();
            }

            _context.routehistories.Remove(routehistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoutehistoryExists(long id)
        {
            return _context.routehistories.Any(e => e.Routehistoryid == id);
        }
    }
}
