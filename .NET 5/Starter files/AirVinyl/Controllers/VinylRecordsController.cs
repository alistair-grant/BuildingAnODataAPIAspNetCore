using AirVinyl.API.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AirVinyl.Controllers
{
    public class VinylRecordsController : ODataApiController
    {
        private const string EntitySetTemplate = "VinylRecords";
        private const string EntityTypeTemplate = EntitySetTemplate + "({key})";

        private readonly AirVinylDbContext _dbContext;

        public VinylRecordsController(AirVinylDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet(EntitySetTemplate)]
        public async Task<IActionResult> GetVinylRecords()
        {
            return Ok(await _dbContext.VinylRecords.ToListAsync());
        }

        [HttpGet(EntityTypeTemplate)]
        public async Task<IActionResult> GetVinylRecord(int key)
        {
            var vinylRecord = await _dbContext.VinylRecords
                .FirstOrDefaultAsync(r => r.VinylRecordId == key);

            if (vinylRecord == null)
            {
                return NotFound();
            }

            return Ok(vinylRecord);
        }
    }
}
