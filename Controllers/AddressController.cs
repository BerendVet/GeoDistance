using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using geoDistance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace geoDistance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AddressController : ControllerBase
    {

        private readonly AddressContext _dbContext;

        public AddressController(AddressContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Address> GetAddresses([FromQuery] string? search, [FromQuery] string? orderBy)
        {
            var query =
                from m in _dbContext.addresses
                select m;

            if (search != null)
            {
                query = (IOrderedQueryable<Address>)query.Where(address =>
                    address.Street.ToLower().Contains(search.ToLower()) ||
                    address.Number.ToLower().Contains(search.ToLower()) ||
                    address.City.ToLower().Contains(search.ToLower()) ||
                    address.Country.ToLower().Contains(search.ToLower()) ||
                    address.PostalCode.ToLower().Contains(search.ToLower()));
            }

            var list = query.ToList();

            if (orderBy != null)
            {
                var propertyInfo = typeof(Address).GetProperty(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(orderBy));
                if (propertyInfo != null)
                    return list.OrderBy(x => propertyInfo.GetValue(x, null));
            }

            return list;
        }

        [HttpGet("{id}")]
        public ActionResult<Address> GetAddress(Guid id)
        {
            var address = _dbContext.addresses.Where(address => address.Id == id).SingleOrDefault();
            if (address == null)
            {
                return NotFound();
            }
            return address;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddAddress([FromBody] Address address)
        {
            // regenerate the guid because swagger auto generates the same guid which causes duplicate id
            // in a real api the address model should have a api interface without the id which would be mapped to the database model
            address.Id = new Guid();

            _dbContext.Add(address);

            var changes = await _dbContext.SaveChangesAsync();

            return changes > 0;
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateAddress([FromBody] Address newAddress)
        {
            _dbContext.Update(newAddress);

            var changes = await _dbContext.SaveChangesAsync();

            return changes > 0;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> RemoveAddres(Guid id)
        {
            var address = _dbContext.addresses.Where(address => address.Id == id).SingleOrDefault();
            if (address == null)
            {
                return NotFound();
            }

            _dbContext.Remove(address);

            var changes = await _dbContext.SaveChangesAsync();

            return changes > 0;
        }


    }
}