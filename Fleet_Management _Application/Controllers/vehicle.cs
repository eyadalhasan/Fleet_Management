/*using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fleet_Management__Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vehicle : ControllerBase
    {
        // GET: api/<vehicle>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<vehicle>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<vehicle>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<vehicle>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<vehicle>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
*/

//using Fleet_Management__Application.Data;
//using Fleet_Management__Application.Models;
//using FPro;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Data;
//using System.Threading.Tasks;

//[Route("api/[controller]")]
//[ApiController]
//public class VehicleController : ControllerBase
//{
//    private readonly DemoContext2 _context;

//    public VehicleController(DemoContext2 context)
//    {
//        _context = context;
//    }


//    // GET: api/<Vehicle>
//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<vehicle>>> Get()
//    {
//        return await _context.vehicles.ToListAsync();
//    }

//    // GET api/<Vehicle>/5
//    [HttpGet("{id}")]
//    public async Task<ActionResult<vehicle>> Get(int id)
//    {
//        var vehicle = await _context.vehicles.FirstOrDefaultAsync(v => v.vehicleid == id);
//        if (vehicle == null)
//        {
//            return NotFound();
//        }

//        return vehicle;
//    }

//    // POST api/<Vehicle>
//    [HttpPost]
//    public async Task<ActionResult<vehicle>> Post([FromBody] vehicle vehicle)
//    {
//        _context.vehicles.Add(vehicle);
//        await _context.SaveChangesAsync();
//        return CreatedAtAction(nameof(Get), new { id = vehicle.vehicleid }, vehicle);
//    }

//    // PUT api/<Vehicle>/5
//    [HttpPut("{id}")]
//    public async Task<IActionResult> Put(int id, [FromBody] vehicle vehicle)
//    {
//        if (id != vehicle.vehicleid)
//        {
//            return BadRequest();
//        }

//        _context.Entry(vehicle).State = EntityState.Modified;
//        try
//        {
//            await _context.SaveChangesAsync();
//        }
//        catch (DbUpdateConcurrencyException)
//        {
//            if (!_context.vehicles.Any(e => e.vehicleid == id))
//            {
//                return NotFound();
//            }
//            else
//            {
//                throw;
//            }
//        }

//        return NoContent();
//    }

//    // DELETE api/<Vehicle>/5
//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Delete(int id)
//    {
//        var vehicle = await _context.vehicles.FindAsync(id);
//        if (vehicle == null)
//        {
//            return NotFound();
//        }

//        _context.vehicles.Remove(vehicle);
//        await _context.SaveChangesAsync();
//        return NoContent();
//    }
//}




using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using Fleet_Management__Application.Data;
using Fleet_Management__Application.Models;
using System.Collections.Concurrent;
using FPro;
using Microsoft.AspNetCore.JsonPatch;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using System.Security.Policy;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Fleet_Management__Application.Services;



[Route("api/[controller]")]
[ApiController]
public class VehicleController : ControllerBase
{
    private readonly DemoContext2 _context;
    private readonly WebSocketManagerService _webSocketManagerService;

    public VehicleController(DemoContext2 context, WebSocketManagerService webSocketManagerService)
    {
        _webSocketManagerService = webSocketManagerService;

        _context = context;
    }

    [HttpGet]

    public async Task<ActionResult<GVAR>> GetAllVehicles()
    {
        try
        {
            var vehicles = await _context.vehicles.ToListAsync();
            if (vehicles == null || !vehicles.Any())
            {
                return NotFound("No vehicles found.");
            }

            long id;
            List<Routehistory> vehicleRouteHistory = new List<Routehistory>();



            var gvar = new GVAR();
            foreach (var vehicle in vehicles)
            {
                id = vehicle.Vehicleid;
                var lastRouteHistory = _context.routehistories.Where(r => r.Vehicleid == id).OrderByDescending(i => i.Epoch).FirstOrDefault();
                if (lastRouteHistory != null)
                {
                    vehicleRouteHistory.Add(lastRouteHistory);

                }






            }



            DataTable dataTable = ConvertToDataTable(vehicles, vehicleRouteHistory);



            // No need to serialize, just add the DataTable directly
            gvar.DicOfDT.TryAdd("Vehicles", dataTable);

            return Ok(new { sts = 1, gvar });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }


    // GET api/<Vehicle>/5
    //[HttpGet("{id}")]
    //public async Task<ActionResult<GVAR>> GetVehicle(long id)
    //{

    //    var vehicle = await _context.vehicles.FindAsync(id);

    //    if (vehicle == null)
    //    {
    //        return NotFound();
    //    }
    //    var gvar = new GVAR();


    //    var vehicleDetails = new ConcurrentDictionary<string, string>
    //    {
    //        ["VehicleID"] = vehicle.vehicleid.ToString(),
    //        ["VehicleNumber"] = vehicle.vehiclenumber.ToString(),
    //        ["VehicleType"] = vehicle.vehicletype.ToString()
    //    };

    //    gvar.DicOfDic["Vehicle"] = vehicleDetails;


    //    return Ok(gvar.DicOfDic);
    //}
    [HttpGet("{id}")]
    public async Task<ActionResult<GVAR>> GetVehicle(long id)
    {

        var vehicle = await _context.vehicles.FindAsync(id);

        if (vehicle == null)
        {
            return NotFound();
        }
        var gvar = new GVAR();
        //var lastRouteHistory = await _context.routehistories.Where(r => r.vehicleid == id).OrderByDescending(r => r.epoch).FirstOrDefaultAsync();


        //if (lastRouteHistory == null)
        //{
        //    return NotFound("No route history available for this vehicle.");
        //}

        var vehicleDetails = new ConcurrentDictionary<string, string>
        {

            ["VehicleID"] = vehicle.Vehicleid.ToString(),
            ["VehicleNumber"] = vehicle.Vehiclenumber.ToString(),
            ["VehicleType"] = vehicle.Vehicletype.ToString(),
    


        };

        gvar.DicOfDic["Vehicle"] = vehicleDetails;




        return Ok(gvar.DicOfDic);
    }
    private DataTable ConvertToDataTable(IEnumerable<Vehicle> vehicles, List<Routehistory> vehicleRouteHistory)
    {
        DataTable dt = new DataTable("Vehicles");
        dt.Columns.Add("VehicleID", typeof(long));
        dt.Columns.Add("VehicleNumber", typeof(long));
        dt.Columns.Add("VehicleType", typeof(string));
        dt.Columns.Add("LastDirection");
        dt.Columns.Add("LastStatus");
        dt.Columns.Add("LastAddress");
        dt.Columns.Add("LastLatitude");
        dt.Columns.Add("LastLongitude");


        foreach (var vehicle in vehicles)
        {
            var history = vehicleRouteHistory.FirstOrDefault(x => x.Vehicleid == vehicle.Vehicleid);
            if (history != null)
            {
                dt.Rows.Add(vehicle.Vehicleid, vehicle.Vehiclenumber, vehicle.Vehicletype, history.Vehicledirection, history.Status, history.Address, history.Latitude, history.Longitude);

            }
            else
            {
                dt.Rows.Add(vehicle.Vehicleid, vehicle.Vehiclenumber, vehicle.Vehicletype, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value);
            }
        }
        return dt;
    }

    // POST api/<Vehicle>
    //[HttpPost]
    //public async Task<ActionResult<GVAR>> Post([FromBody] vehicle vehicle)
    //{
    //    _context.vehicles.Add(vehicle);
    //    await _context.SaveChangesAsync();
    //    var gvar = new GVAR();
    //    var vehicleDetails = new ConcurrentDictionary<string, string>
    //    {
    //        ["VehicleID"] = vehicle.vehicleid.ToString(),
    //        ["VehicleNumber"] = vehicle.vehiclenumber.ToString(),
    //        ["VehicleType"] = vehicle.vehicletype.ToString()
    //    };
    //    gvar.DicOfDic.TryAdd("Vehicle", vehicleDetails);
    //    return gvar;
    //}

    [HttpPost]

    //public async Task<ActionResult<GVAR>> AddVehicle([FromBody] dynamic jsonData)
    //{
    //    try
    //    {
    //        string jsonString = JsonConvert.SerializeObject(jsonData);
    //        GVAR gvarobj = JsonConvert.DeserializeObject<GVAR>(jsonString);

    //        var vehicleJson = JsonConvert.SerializeObject(gvarobj.DicOfDic["Tags"]);
    //        var vehicle = JsonConvert.DeserializeObject<vehicle>(vehicleJson);

    //        vehicle ??= new vehicle();

    //        //if (vehicle == null)
    //        //{
    //        //    vehicle = new vehicle();
    //        //}
    //        _context.vehicles.Add(vehicle);

    //        // Save changes to the database
    //        await _context.SaveChangesAsync();

    //        // Create a new GVAR object (optional, depending on your use case)
    //        var gvar = new GVAR();

    //        // Construct the URL for the location header
    //        var locationUri = Url.Action(nameof(GetVehicle), new { id = vehicle.vehicleid });

    //        // Return a success response (201 Created) with the location header set to the URI of the new resource
    //        return Created(locationUri, gvar);
    //    }
    //    catch (Exception ex)
    //    {
    //        // Return an error response if an exception occurs during database operation
    //        return StatusCode(500, "An error occurred while adding the vehicle to the database.");
    //    }
    //}
    public async Task<ActionResult<GVAR>> AddVehicle([FromBody] GVAR jsonData)
    {

        try
        {

            var vehicleJson = JsonConvert.SerializeObject(jsonData.DicOfDic["Tags"]);
            var vehcileInfo = JsonConvert.DeserializeObject<Vehicle>(vehicleJson);
            vehcileInfo ??= new Vehicle();



            //if (vehicle == null)
            //{
            //    vehicle = new vehicle();
            //}
            _context.vehicles.Add(vehcileInfo);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Create a new GVAR object (optional, depending on your use case)
            var gvar = new GVAR();

            // Construct the URL for the location header
            var locationUri = Url.Action(nameof(GetVehicle), new { id = vehcileInfo.Vehicleid });

            // Return a success response (201 Created) with the location header set to the URI of the new resource
            _webSocketManagerService.Broadcast("New vehicle added");



            return Created(locationUri, new { sts = 1 });
        }
        catch (Exception ex)
        {
            // Return an error response if an exception occurs during database operation
            return StatusCode(500, new { sts = 0 });
        }
    }



    //public async Task<ActionResult<GVAR>> AddVehicle([FromBody] dynamic jsonData)
    //{
    //    // Extract vehicle details from the JSON data

    //    var tags = jsonData.DicOfDic.Tags;
    //    var vehicleNumber = tags["VehicleNumber"];
    //    var vehicleType = tags["VehicleType"];

    //    // Create a new GVAR object to hold the vehicle details
    //    var gvar = new GVAR();
    //    var vehicleDetails = new ConcurrentDictionary<string, string>
    //    {
    //        ["VehicleNumber"] = vehicleNumber,
    //        ["VehicleType"] = vehicleType
    //    };
    //    gvar.DicOfDic.TryAdd("Vehicle", vehicleDetails);

    //    // Create a new Vehicle entity and populate it with the extracted data
    //    var newVehicle = new vehicle
    //    {
    //        vehiclenumber = vehicleNumber,
    //        vehicletype = vehicleType
    //    };

    //    try
    //    {
    //        // Add the new vehicle to the DbSet
    //        _context.vehicles.Add(newVehicle);

    //        // Save changes to the database
    //        await _context.SaveChangesAsync();

    //        // Retrieve the ID of the newly created vehicle
    //        var newId = newVehicle.vehicleid; // Assuming Id is the property representing the primary key

    //        // Construct the URL for the location header
    //        var locationUri = Url.Action(nameof(GetVehicle), new { id = newId });

    //        // Return a success response (201 Created) with the location header set to the URI of the new resource
    //        return Created(locationUri, gvar);
    //    }
    //    catch (Exception ex)
    //    {
    //        // Return an error response if an exception occurs during database operation
    //        return StatusCode(500, "An error occurred while adding the vehicle to the database.");
    //    }
    //}

    // PUT api/<Vehicle>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, [FromBody] GVAR vehicle)
    {

        if (vehicle == null || !vehicle.DicOfDic.ContainsKey("Tags"))
        {
            return BadRequest(new { sts = 0 });
        }
        var tags = vehicle.DicOfDic["Tags"];

        var vehicleJson = JsonConvert.SerializeObject(tags);
        var vehcileInfo = JsonConvert.DeserializeObject<Vehicle>(vehicleJson);

        _context.Entry(vehcileInfo).State = EntityState.Modified;
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
    // DELETE api/<Vehicle>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var vehicle = await _context.vehicles.FindAsync(id);

        if (vehicle == null)
        {
            return NotFound(new { sts = 0 });
        }
        _context.vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
        return Ok(new { sts = 1 });
    }
    // PATCH: api/Vehicle/5
    [HttpPatch("{id}")]

    public async Task<IActionResult> UpdateVehicle(long id, [FromBody] JsonPatchDocument<Vehicle> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest("Patch document is empty.");
        }

        var vehicle = await _context.vehicles.FirstOrDefaultAsync(v => v.Vehicleid == id);
        if (vehicle == null)
        {
            return NotFound($"No vehicle found with ID {id}.");
        }

        patchDoc.ApplyTo(vehicle, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _context.SaveChangesAsync();
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}

