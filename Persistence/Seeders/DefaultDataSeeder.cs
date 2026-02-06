namespace Persistence.Seeders
{
    public class DefaultDataSeeder
    {
        private readonly IEnumerable<IDataSeeder> _seeders;

        public DefaultDataSeeder(IEnumerable<IDataSeeder> seeders)
        {
            _seeders = seeders;
        }

        public async Task SeedAsync()
        {
            foreach (var seeder in _seeders)
            {
                await seeder.SeedAsync();
            }
        }
    }
}
