using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using Microsoft.EntityFrameworkCore.InMemory;
using DataLayer;
using Microsoft.EntityFrameworkCore;
namespace TestingLayer
{
    [TestFixture]
    internal class TestManager
    {
        internal static AppDbContext dbContext;

        static TestManager()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("TestDb");
            dbContext = new AppDbContext(builder.Options);
        }


        [SetUp]
        public void RandomSetup()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString()); // different name of the inmemory databases
            dbContext = new AppDbContext(builder.Options);
        }

        [TearDown]
        public void RandomDispose()
        {
            dbContext.Dispose();
        }
    }
}
