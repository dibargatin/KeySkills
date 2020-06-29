using System.Collections.Generic;
using System.Threading.Tasks;
using KeySkills.Core.Models;
using KeySkills.Core.Repositories;
using KeySkills.WebApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace KeySkills.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VacanciesController : ControllerBase
    {
        private readonly IVacancyRepository _repository;

        public VacanciesController(IVacancyRepository repository) =>
            _repository = repository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vacancy>>> Get() =>
            Ok(await _repository.GetAllAsync());        
    }
}