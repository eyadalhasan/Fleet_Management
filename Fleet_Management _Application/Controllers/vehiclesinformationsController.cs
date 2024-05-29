//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Fleet_Management__Application.Data;
//using Fleet_Management__Application.Models;
//using FPro;
//using System.Data;
//using System.Collections.Concurrent;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Newtonsoft.Json;
//using Fleet_Management__Application.Services;

//namespace Fleet_Management__Application.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class vehiclesinformationsController : ControllerBase
//    {
//        private readonly DemoContext2 _context;
//        private readonly WebSocketManagerService _webSocketManagerService;

//        public vehiclesinformationsController(DemoContext2 context, WebSocketManagerService webSocketManagerService)
//        {
//            _webSocketManagerService = webSocketManagerService;

//            _context = context;

//        }

//        // GET: api/vehiclesinformations
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Vehiclesinformation>>> Getvehiclesinformations()
//        {
//            var vehciles_informations = await _context.vehiclesinformations.ToListAsync();
//            var gvar = new GVAR();
//            var vehiclesinformations_dt = ConvertToDataTable(vehciles_informations);
//            gvar.DicOfDT["VehcileInformations"] = vehiclesinformations_dt;

//            return Ok(new { sts = 1, gvar });
//        }

//        private DataTable ConvertToDataTable(IEnumerable<Vehiclesinformation> vehiclesinformation)
//        {
//            var table = new DataTable("VehcileInormations");
//            table.Columns.Add("VehicleID");
//            table.Columns.Add("DriverID");
//            table.Columns.Add("VehicleMake");
//            table.Columns.Add("VehicleModel");
//            table.Columns.Add("PurchaseDate");
//            table.Columns.Add("id");
//            foreach (var item in vehiclesinformation)
//            {
//                table.Rows.Add(item.Vehicleid, item.Driverid, item.Vehiclemake, item.Vehiclemodel, item.Purchasedate, item.Id);

//            }
//            return table;

//        }

//        // GET: api/vehiclesinformations/5/
//        [HttpGet("{id}")]

//        public async Task<IActionResult> GetVehiclesInformation(long id)
//        {
//            // Use 'FirstOrDefaultAsync' instead of 'FirstAsync' to avoid exceptions if no data is found.
//            var vehiclesInformation = await _context.vehiclesinformations
//                                                    .Include(vi => vi.Vehicle) // Assuming Vehicle relationship is set
//                                                    .Include(vi => vi.Driver)  // Assuming Driver relationship is set
//                                                    .FirstOrDefaultAsync(i => i.Vehicleid == id);

//            // Check if the requested VehiclesInformation exists
//            if (vehiclesInformation == null)
//            {
//                return NotFound($"No vehicle information found for VehicleID {id}.");
//            }

//            // Fetch the most recent route history
//            var history = await _context.routehistories
//                                        .Where(r => r.Vehicleid == id)
//                                        .OrderByDescending(e => e.Epoch)
//                                        .FirstOrDefaultAsync();

//            // Build the details dictionary
//            var vehiclesInformationsDetails = new ConcurrentDictionary<string, string>
//            {
//                ["VehicleNumber"] = vehiclesInformation.Vehicle?.Vehiclenumber?.ToString() ?? "N/A",
//                ["VehicleType"] = vehiclesInformation.Vehicle?.Vehicletype ?? "N/A",
//                ["DriverName"] = vehiclesInformation.Driver?.Drivername ?? "N/A",
//                ["PhoneNumber"] = vehiclesInformation.Driver?.Phonenumber.ToString() ?? "N/A",
//                ["LastPosition"] = history != null ? $"{history.Latitude}, {history.Longitude}" : "N/A",
//                ["VehicleMake"] = vehiclesInformation.Vehiclemake ?? "N/A",
//                ["VehicleModel"] = vehiclesInformation.Vehiclemodel ?? "N/A",
//                ["LastGPSTime"] = history?.Epoch.ToString() ?? "N/A",
//                ["LastGPSSpeed"] = history?.Vehiclespeed ?? "N/A",
//                ["LastAddress"] = history?.Address ?? "N/A",
//                ["ID"] = vehiclesInformation?.Id.ToString() ?? "N/A",


//            };
//            GVAR gvar = new GVAR();

//            gvar.DicOfDic["VehiclesInformations"] = vehiclesInformationsDetails;


//            return Ok(new { sts = 1, gvar });
//        }
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Putvehiclesinformation(long id, GVAR gvar)
//        {
//            if (gvar == null || gvar.DicOfDic == null || !gvar.DicOfDic.ContainsKey("Tags"))
//            {
//                return BadRequest(new { sts = 0 });
//            }

//            var vehicleJson = JsonConvert.SerializeObject(gvar.DicOfDic["Tags"]);
//            var vehcileInfo = JsonConvert.DeserializeObject<Vehiclesinformation>(vehicleJson);
//            vehcileInfo ??= new Vehiclesinformation();

//            _context.Entry(vehcileInfo).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!vehiclesinformationExists(id))
//                {
//                    return NotFound(new { sts = 0 });
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return Ok(new { sts = 1 });
//        }



//        // PUT: api/vehiclesinformations/5

//        //[HttpPut("{id}")]
//        //public async Task<IActionResult> Putvehiclesinformation(long id, Vehiclesinformation vehiclesinformation)
//        //{
//        //    if (id != vehiclesinformation.Vehicleid)
//        //    {
//        //        return BadRequest(new { sts = 0 });
//        //    }

//        //    _context.Entry(vehiclesinformation).State = EntityState.Modified;

//        //    try
//        //    {
//        //        await _context.SaveChangesAsync();
//        //    }
//        //    catch (DbUpdateConcurrencyException)
//        //    {
//        //        if (!vehiclesinformationExists(id))
//        //        {
//        //            return NotFound(new { sts = 0 });
//        //        }
//        //        else
//        //        {
//        //            throw;
//        //        }
//        //    }

//        //    return Ok(new { sts = 1 });
//        //}

//        // POST: api/vehiclesinformations
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<Vehiclesinformation>> Postvehiclesinformation(GVAR vehiclesinformation)
//        {

//            try
//            {
//                var vehicleJson = JsonConvert.SerializeObject(vehiclesinformation.DicOfDic["Tags"]);
//                var vehcileInfo = JsonConvert.DeserializeObject<Vehiclesinformation>(vehicleJson);
//                vehcileInfo ??= new Vehiclesinformation();
//                _context.vehiclesinformations.Add(vehcileInfo);
//                await _context.SaveChangesAsync();
//                var gvar = new GVAR();

//                // Construct the URL for the location header
//                var locationUri = Url.Action(nameof(GetVehiclesInformation), new { id = vehcileInfo.Vehicleid });

//                // Return a success response (201 Created) with the location header set to the URI of the new resource

//                _webSocketManagerService.Broadcast("New vehicleinfo added");


//                return Created(locationUri, new { sts = 1 });







//            }
//            catch (Exception ex)
//            {

//                return Ok(new { sts = 0 });

//            }


//            //_context.vehiclesinformations.Add(vehiclesinformation);
//            //try
//            //{
//            //    await _context.SaveChangesAsync();
//            //}
//            //catch (DbUpdateException)
//            //{
//            //    if (vehiclesinformationExists(vehiclesinformation.Vehicleid))
//            //    {
//            //        return Conflict();
//            //    }
//            //    else
//            //    {
//            //        throw;
//            //    }
//            //}

//        }

//        // DELETE: api/vehiclesinformations/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Deletevehiclesinformation(long id)
//        {
//            var vehiclesinformation = await _context.vehiclesinformations.FindAsync(id);
//            if (vehiclesinformation == null)
//            {
//                return NotFound(new { sts = 0 });
//            }

//            _context.vehiclesinformations.Remove(vehiclesinformation);
//            await _context.SaveChangesAsync();

//            return Ok(new { sts = 1 });
//        }

//        private bool vehiclesinformationExists(long id)
//        {
//            return _context.vehiclesinformations.Any(e => e.Driverid == id);
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Fleet_Management__Application.Services;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Fleet_Management__Application.Data;
using Fleet_Management__Application.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data;
using FPro;

namespace Fleet_Management__Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vehiclesinformationsController : ControllerBase
    {
        private readonly DemoContext2 _context;
        private readonly WebSocketManagerService _webSocketManagerService;

        public vehiclesinformationsController(DemoContext2 context, WebSocketManagerService webSocketManagerService)
        {
            _context = context;
            _webSocketManagerService = webSocketManagerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehiclesinformation>>> GetVehiclesInformations()
        {
            var vehiclesInformations = await _context.vehiclesinformations.ToListAsync();
            var gvar = new GVAR();
            var vehiclesInformationsDt = ConvertToDataTable(vehiclesInformations);
            gvar.DicOfDT["VehcileInformations"] = vehiclesInformationsDt;

            return Ok(new { sts = 1, gvar });
        }

        private DataTable ConvertToDataTable(IEnumerable<Vehiclesinformation> vehiclesInformation)
        {
            var table = new DataTable("VehcileInformations");
            table.Columns.Add("VehicleID");
            table.Columns.Add("DriverID");
            table.Columns.Add("VehicleMake");
            table.Columns.Add("VehicleModel");
            table.Columns.Add("PurchaseDate");
            table.Columns.Add("id");
            foreach (var item in vehiclesInformation)
            {
                table.Rows.Add(item.Vehicleid, item.Driverid, item.Vehiclemake, item.Vehiclemodel, item.Purchasedate, item.Id);
            }
            return table;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehiclesInformation(long id)
        {
            var vehiclesInformation = await _context.vehiclesinformations
                                                    .Include(vi => vi.Vehicle)
                                                    .Include(vi => vi.Driver)
                                                    .FirstOrDefaultAsync(i => i.Vehicleid == id);

            if (vehiclesInformation == null)
            {
                return NotFound($"No vehicle information found for VehicleID {id}.");
            }

            var history = await _context.routehistories
                                        .Where(r => r.Vehicleid == id)
                                        .OrderByDescending(e => e.Epoch)
                                        .FirstOrDefaultAsync();

            var vehiclesInformationsDetails = new ConcurrentDictionary<string, string>
            {
                ["VehicleNumber"] = vehiclesInformation.Vehicle?.Vehiclenumber?.ToString() ?? "N/A",
                ["VehicleType"] = vehiclesInformation.Vehicle?.Vehicletype ?? "N/A",
                ["DriverName"] = vehiclesInformation.Driver?.Drivername ?? "N/A",
                ["PhoneNumber"] = vehiclesInformation.Driver?.Phonenumber.ToString() ?? "N/A",
                ["LastPosition"] = history != null ? $"{history.Latitude}, {history.Longitude}" : "N/A",
                ["VehicleMake"] = vehiclesInformation.Vehiclemake ?? "N/A",
                ["VehicleModel"] = vehiclesInformation.Vehiclemodel ?? "N/A",
                ["LastGPSTime"] = history?.Epoch.ToString() ?? "N/A",
                ["LastGPSSpeed"] = history?.Vehiclespeed ?? "N/A",
                ["LastAddress"] = history?.Address ?? "N/A",
                ["ID"] = vehiclesInformation?.Id.ToString() ?? "N/A",
            };
            GVAR gvar = new GVAR();
            gvar.DicOfDic["VehiclesInformations"] = vehiclesInformationsDetails;

            return Ok(new { sts = 1, gvar });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehiclesInformation(long id, GVAR gvar)
        {
            if (gvar == null || gvar.DicOfDic == null || !gvar.DicOfDic.ContainsKey("Tags"))
            {
                return BadRequest(new { sts = 0 });
            }

            var vehicleJson = JsonConvert.SerializeObject(gvar.DicOfDic["Tags"]);
            var vehicleInfo = JsonConvert.DeserializeObject<Vehiclesinformation>(vehicleJson);
            vehicleInfo ??= new Vehiclesinformation();

            _context.Entry(vehicleInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiclesInformationExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Vehiclesinformation>> PostVehiclesInformation(GVAR vehiclesInformation)
        {
            try
            {
                var vehicleJson = JsonConvert.SerializeObject(vehiclesInformation.DicOfDic["Tags"]);
                var vehicleInfo = JsonConvert.DeserializeObject<Vehiclesinformation>(vehicleJson);
                vehicleInfo ??= new Vehiclesinformation();
                _context.vehiclesinformations.Add(vehicleInfo);
                await _context.SaveChangesAsync();

                var locationUri = Url.Action(nameof(GetVehiclesInformation), new { id = vehicleInfo.Vehicleid });


                _webSocketManagerService.Broadcast("New vehicleinfo added");

                return Created(locationUri, new { sts = 1 });
            }
            catch (Exception)
            {
                return Ok(new { sts = 0 });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehiclesInformation(long id)
        {
            var vehiclesInformation = await _context.vehiclesinformations.FindAsync(id);
            if (vehiclesInformation == null)
            {
                return NotFound(new { sts = 0 });
            }

            _context.vehiclesinformations.Remove(vehiclesInformation);
            await _context.SaveChangesAsync();

            return Ok(new { sts = 1 });
        }

        private bool VehiclesInformationExists(long id)
        {
            return _context.vehiclesinformations.Any(e => e.Driverid == id);
        }
    }
}

