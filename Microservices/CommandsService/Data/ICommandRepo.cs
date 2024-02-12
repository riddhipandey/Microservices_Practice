using CommandsService.Models;

namespace CommandsService.Data{
    public interface ICommandRepo{
        bool SaveChanges();
        //This is for platfroms 
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatfrom(Platform platform);
        bool PlatformExists(int platfromId);
        bool ExternalPlatformExist(int externalPlatformId);


        //Commands
        IEnumerable<Command> GetCommandsForPlatfrom(int platfromId);
        Command GetCommand(int platfromId, int commandId);
        void CreateCommand(int platfromId, Command command);
    }
}