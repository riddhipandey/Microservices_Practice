using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatfromsController : ControllerBase{
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;

        public PlatfromsController(IPlatformRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatfroms(){
            Console.WriteLine("--- Getting Platfroms ---");
            var platfromItems =  _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platfromItems));
        }

        [HttpGet("{Id}", Name = "GetPlatfromById")]
        public ActionResult<Platform> GetPlatfromById(int Id){
            var platfromItem = _repository.GetPlatformById(Id);
            if(platfromItem !=null){
                return _mapper.Map<Platform>(platfromItem);
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto){
            Console.WriteLine("Create Platform 1");
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            Console.WriteLine("Create Platform 2");
            _repository.CreatePlatfrom(platformModel);
            _repository.SaveChanges();
            Console.WriteLine("Create Platform 3");

            var platfromReadDto = _mapper.Map<PlatformReadDto>(platformModel);
            return CreatedAtRoute(nameof(GetPlatfromById),new {Id = platfromReadDto.Id},platfromReadDto);
        }
    }
}