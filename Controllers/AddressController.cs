using System;
using System.Collections.Generic;
using geoDistance.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<Address> GetAddresses()
        {
            //return repo.GetAddresses();
            return _dbContext.addresses;
        }

        [HttpGet("{id}")]
        public ActionResult<Address> GetAddresses(Guid id)
        {
            // var address = repo.GetAddress(id);

            // if (address is null) return NotFound();

            // return address;
            return null;
        }

        [HttpPost]
        public ActionResult<Address> AddAddress([FromBody] Address address)
        {
            _dbContext.Add(address);
            return null;
        }
    }
}