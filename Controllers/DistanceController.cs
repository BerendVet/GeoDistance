using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using geoDistance.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GeoDistance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistanceController : ControllerBase
    {
        private readonly AddressContext _dbContext;

        public DistanceController(AddressContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetDistanceBetweenAddresses")]
        public async Task<ActionResult<string>> GetDistance([FromQuery] Guid from, [FromQuery] Guid to)
        {
            if (from.Equals(to)) return "0.00 Km";

            var fromAddress = _dbContext.addresses.Where(address => address.Id == from).SingleOrDefault();
            var toAddress = _dbContext.addresses.Where(address => address.Id == to).SingleOrDefault();

            if (fromAddress == null || toAddress == null)
            {
                return NotFound();
            }

            using (var client = new HttpClient())
            {
                double latFrom, lngFrom, latTo, lngTo;

                client.BaseAddress = new Uri("https://api.opencagedata.com/geocode/v1/");

                // api call for first address
                var responseTask = client.GetAsync($"json?q={fromAddress.City},{fromAddress.Street},{fromAddress.Number}&countrycode={fromAddress.Country}&key=c85702a9bbb04edd8ab882ea12f7f32a");
                responseTask.Wait();

                var result = responseTask.Result;

                if (!result.IsSuccessStatusCode) return NotFound();

                string responseBody = await result.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(responseBody);

                if (json.results.Count < 1) return NotFound("the first address had no matches.");

                latFrom = json.results[0].geometry.lat.Value;
                lngFrom = json.results[0].geometry.lng.Value;

                // api call for second address
                responseTask = client.GetAsync($"json?q={toAddress.City},{toAddress.Street},{toAddress.Number}&countrycode={toAddress.Country}&key=c85702a9bbb04edd8ab882ea12f7f32a");
                responseTask.Wait();

                result = responseTask.Result;
                if (!result.IsSuccessStatusCode) return NotFound();

                responseBody = await result.Content.ReadAsStringAsync();

                json = JsonConvert.DeserializeObject(responseBody);

                if (json.results.Count < 1) return NotFound("the second address had no matches.");

                latTo = json.results[0].geometry.lat.Value;
                lngTo = json.results[0].geometry.lng.Value;

                // calculate distance
                double distance = Address.CalculateDistance(latFrom, lngFrom, latTo, lngTo);

                return $"{(distance / 1000).ToString("#.##")} Km";
            }
        }
    }
}