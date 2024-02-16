using System.Linq.Expressions;
using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace CommandsService.EventProcessing{
    public class EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper) : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly IMapper _mapper = mapper;

        public void ProcessEvent(string message)
        {
            var eventType = DetermineDevent(message);
            switch(eventType){
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;

            }
        }

        private EventType DetermineDevent(string notificationMessage){
            Console.WriteLine("Determining Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch(eventType.Event){
                case "Platfrom_Publisher":
                    Console.WriteLine("Platform Published event detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("Published event could not be determined");
                    return EventType.UnDetermined;
            }
        }

        private void AddPlatform(string platformPublishedMessage){
            using(var scope = _serviceScopeFactory.CreateScope()){
                var repo = scope.ServiceProvider.GetService<ICommandRepo>();
                var platfromPublishedDto = JsonSerializer.Deserialize<PlatfromPublishedDto>(platformPublishedMessage);
                try{
                    var plat = _mapper.Map<Platform>(platfromPublishedDto);
                    if(!repo.ExternalPlatformExist(plat.ExternalID)){
                        repo.CreatePlatfrom(plat);
                        repo.SaveChanges();
                        Console.WriteLine("Platform added successfully");
                    }else{
                        Console.WriteLine("Platform already exists");
                    }
                }
                catch(Exception ex){
                    Console.WriteLine("Could not add platform to Database" + ex);
                }
            }
        }
    
    }
    enum EventType{
        PlatformPublished,
        UnDetermined 
    }
}