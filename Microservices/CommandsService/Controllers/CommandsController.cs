using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController(ICommandRepo commandRepo, IMapper mapper) : ControllerBase{
            private readonly ICommandRepo _commandRepo = commandRepo;
            private readonly IMapper _mapper = mapper;

            [HttpGet]
            public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId){
                Console.WriteLine($"Get Commands for Platform: {platformId}");
                if(!_commandRepo.PlatformExists(platformId)){
                    return NotFound();
                }
                var commands = _commandRepo.GetCommandsForPlatfrom(platformId);
                return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
            }

            [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
            public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId){
                Console.WriteLine("Get Commands for Platform:"+ platformId + " and comand : " +commandId);
                Console.WriteLine("Get Commands for Platform:" + commandId);
                if(!_commandRepo.PlatformExists(platformId)){
                    return NotFound();
                }
                var command = _commandRepo.GetCommand(platformId,commandId);
                Console.WriteLine("Temp " + command);
                if(command == null){
                    return NotFound();
                }
                return Ok(_mapper.Map<CommandReadDto>(command));
            }

            [HttpPost]
            public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto){
                Console.WriteLine($"CreateCommandForPlatform : {platformId}");
                if(_commandRepo.PlatformExists(platformId)){
                    Console.WriteLine($"Platform does not exist : {platformId}");
                    return NotFound();
                }
                Console.WriteLine($"Platform exist : {platformId}");
                var command = _mapper.Map<Command>(commandCreateDto);
                _commandRepo.CreateCommand(platformId,command);
                _commandRepo.SaveChanges();
                var commandReadDto = _mapper.Map<CommandReadDto>(command);
                 Console.WriteLine($"Command for : {platformId}");
                return CreatedAtRoute(nameof(GetCommandForPlatform),new { platformId, commandId = commandReadDto.Id},commandReadDto);
            }

            


        }
    
}