using System.Linq.Expressions;
using System.Text.Json;
using AutoMapper;
using CommandsService.Dtos;

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
                    //TODO
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
    }
    enum EventType{
        PlatformPublished,
        UnDetermined 
    }
}