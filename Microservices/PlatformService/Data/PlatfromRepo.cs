using PlatformService.Models;

namespace PlatformService.Data{
    public class PlatfromRepo(AppDbContext context) : IPlatformRepo
    {
        private readonly AppDbContext _context = context;

        public void CreatePlatfrom(Platform platform)
        {
            ArgumentNullException.ThrowIfNull(platform);
            _context.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetPlatformById(int Id)
        {
            return _context.Platforms.FirstOrDefault(p => p.Id == Id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}