using AirVinyl.API.DbContexts;
using AirVinyl.API.Helpers;
using AirVinyl.Entities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AirVinyl.Controllers
{
    public class PeopleController : ODataApiController
    {
        private const string EntitySetTemplate = "People";
        private const string EntityTypeTemplate = EntitySetTemplate + "({key})";

        private readonly AirVinylDbContext _dbContext;

        public PeopleController(AirVinylDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet(EntitySetTemplate)]
        public async Task<IActionResult> GetPeople()
        {
            return Ok(await _dbContext.People.ToListAsync());
        }

        [HttpGet(EntityTypeTemplate)]
        public async Task<IActionResult> GetPerson(int key)
        {
            var person = await _dbContext.People
                .FirstOrDefaultAsync(p => p.PersonId == key);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpGet(EntityTypeTemplate + "/Email")]
        [HttpGet(EntityTypeTemplate + "/LastName")]
        [HttpGet(EntityTypeTemplate + "/FirstName")]
        [HttpGet(EntityTypeTemplate + "/DateOfBirth")]
        [HttpGet(EntityTypeTemplate + "/Gender")]
        public async Task<IActionResult> GetPersonProperty(int key)
        {
            var person = await _dbContext.People
                .FirstOrDefaultAsync(p => p.PersonId == key);

            if (person == null)
            {
                return NotFound();
            }

            var propertyToGet = GetRequestUri().Segments.Last();

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

        [HttpGet(EntityTypeTemplate + "/Email/$value")]
        [HttpGet(EntityTypeTemplate + "/LastName/$value")]
        [HttpGet(EntityTypeTemplate + "/FirstName/$value")]
        [HttpGet(EntityTypeTemplate + "/DateOfBirth/$value")]
        [HttpGet(EntityTypeTemplate + "/Gender/$value")]
        public async Task<IActionResult> GetPersonPropertyRawValue(int key)
        {
            var person = await _dbContext.People
                .FirstOrDefaultAsync(p => p.PersonId == key);

            if (person == null)
            {
                return NotFound();
            }

            var propertyToGet = GetRequestUri().Segments[^2].TrimEnd('/');

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

        [HttpGet(EntityTypeTemplate + "/VinylRecords")]
        public async Task<IActionResult> GetPersonCollectionProperty(int key)
        {
            var collectionPropertyToGet = GetRequestUri().Segments.Last();

            var person = await _dbContext.People
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

        [HttpPost(EntitySetTemplate)]
        public async Task<IActionResult> CreatePerson([FromBody] Person person)
        {
            // add the person to the People collection
            _dbContext.People.Add(person);
            await _dbContext.SaveChangesAsync();

            // return the created person
            return Created(person);
        }
    }
}
