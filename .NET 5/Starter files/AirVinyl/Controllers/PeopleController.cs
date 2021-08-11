﻿using AirVinyl.API.DbContexts;
using AirVinyl.API.Helpers;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AirVinyl.Controllers
{
    public class PeopleController : ODataController
    {
        private readonly AirVinylDbContext _airVinylDbContext;

        public PeopleController(AirVinylDbContext airVinylDbContext)
        {
            _airVinylDbContext = airVinylDbContext
                ?? throw new ArgumentNullException(nameof(airVinylDbContext));
        }

        public async Task<IActionResult> Get()
        {
            return Ok(await _airVinylDbContext.People.ToListAsync());
        }

        // People(1)
        public async Task<IActionResult> Get(int key)
        {
            var person = await _airVinylDbContext.People
                .FirstOrDefaultAsync(p => p.PersonId == key);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpGet("odata/People({key})/Email")]
        [HttpGet("odata/People({key})/LastName")]
        [HttpGet("odata/People({key})/FirstName")]
        [HttpGet("odata/People({key})/DateOfBirth")]
        [HttpGet("odata/People({key})/Gender")]
        public async Task<IActionResult> GetPersonProperty(int key)
        {
            var person = await _airVinylDbContext.People
                .FirstOrDefaultAsync(p => p.PersonId == key);

            if (person == null)
            {
                return NotFound();
            }

            var url = HttpContext.Request.GetEncodedUrl();
            var propertyToGet = new Uri(url).Segments.Last();

            if (!person.HasProperty(propertyToGet))
            {
                return NotFound();
            }

            var propertyValue = person.GetValue(propertyToGet);

            if (propertyValue == null)
            {
                // null = no content
                return NoContent();
            }

            return Ok(propertyValue);
        }

        [HttpGet("odata/People({key})/Email/$value")]
        [HttpGet("odata/People({key})/LastName/$value")]
        [HttpGet("odata/People({key})/FirstName/$value")]
        [HttpGet("odata/People({key})/DateOfBirth/$value")]
        [HttpGet("odata/People({key})/Gender/$value")]
        public async Task<IActionResult> GetPersonPropertyRawValue(int key)
        {
            var person = await _airVinylDbContext.People
                .FirstOrDefaultAsync(p => p.PersonId == key);

            if (person == null)
            {
                return NotFound();
            }

            var url = HttpContext.Request.GetEncodedUrl();
            var propertyToGet = new Uri(url).Segments[^2].TrimEnd('/');

            if (!person.HasProperty(propertyToGet))
            {
                return NotFound();
            }

            var propertyValue = person.GetValue(propertyToGet);

            if (propertyValue == null)
            {
                // null = no content
                return NoContent();
            }

            return Ok(propertyValue.ToString());
        }

        // odata/People(key)/VinylRecords
        [HttpGet("odata/People({key})/VinylRecords")]
        public async Task<IActionResult> GetPersonCollectionProperty(int key)
        {
            var url = HttpContext.Request.GetEncodedUrl();
            var collectionPropertyToGet = new Uri(url).Segments.Last();

            var person = await _airVinylDbContext.People
                .Include(collectionPropertyToGet)
                .FirstOrDefaultAsync(p => p.PersonId == key);

            if (person == null)
            {
                return NotFound();
            }

            if (!person.HasProperty(collectionPropertyToGet))
            {
                return NotFound();
            }

            return Ok(person.GetValue(collectionPropertyToGet));
        }
    }
}
