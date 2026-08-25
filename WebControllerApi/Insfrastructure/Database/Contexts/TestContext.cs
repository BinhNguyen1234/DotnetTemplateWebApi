using Microsoft.EntityFrameworkCore;

namespace WebControllerApi.Insfrastructure.Database.Context
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
        }
    }
}
