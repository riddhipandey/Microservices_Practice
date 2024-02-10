using CommandsService.Models;

namespace CommandsService.Data{
    public class CommandRepo(AppDbContext context) : ICommandRepo
    {
        private readonly AppDbContext _context = context;

        public void CreateCommand(int platfromId, Command command)
        {

            ArgumentNullException.ThrowIfNull(command);
            command.PlatfromId = platfromId;
            _context.Commands.Add(command);
        }

        public void CreatePlatfrom(Platform platform)
        {
            ArgumentNullException.ThrowIfNull(platform);
            _context.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return [.. _context.Platforms];
        }

        public Command GetCommand(int platfromId, int commandId)
        {
            return _context.Commands.Where(c => c.PlatfromId == platfromId && c.Id == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatfrom(int platfromId)
        {
            return _context.Commands.Where(c => c.PlatfromId == platfromId).OrderBy(p=> p.Platform.Name);
        }

        public bool PlatformExists(int platfromId)
        {
            return _context.Platforms.Any(p => p.Id == platfromId);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >=0;
        }
    }
}