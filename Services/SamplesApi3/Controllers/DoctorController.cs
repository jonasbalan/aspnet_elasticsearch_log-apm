using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamplesApi3.Infrastructure;
using SamplesApi3.Model;

namespace SamplesApi3.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DoctorController : ControllerBase
    {

        private readonly ILogger<DoctorController> _logger;
        private readonly DoctorScheduleDbContext _dbContext;

        public DoctorController(ILogger<DoctorController> logger,  DoctorScheduleDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Doctor> Get()
        {
            _logger.LogInformation("Get All Doctors");
            return _dbContext.Doctor.Include(x=>x.Adresses).Include(x=>x.Specialties).Include(x=>x.Schedules).Include(x=>x.SchedulePatients);
        }

    }
}
