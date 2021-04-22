using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tests.WordsServiceTests
{
    [SetUpFixture]
    [Parallelizable(ParallelScope.None)]
    public class GlobalTransactionScopeTestSetUp
    {
        private const string DatabaseName = "Promit2_IntegrationTests";

        public static WordsContext Context { get; private set; }

        [OneTimeSetUp]
        public void TestOneTimeSetUp()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<WordsContext>();

            builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database={DatabaseName};Trusted_Connection=True;MultipleActiveResultSets=true")
                .UseInternalServiceProvider(serviceProvider);

            Context = new WordsContext(builder.Options);
            Context.Database.EnsureDeleted();
            Context.Database.Migrate();
        }

        [OneTimeTearDown]
        public void TestOneTimeTearDown()
        {
            Context.Database.EnsureDeleted();

            Context.Dispose();
        }
    }

    public abstract class TransactionScopeTestsBase
    {
        private IDbContextTransaction _transaction;

        [SetUp]
        public void TestSetUp()
        {
            DetachEntries(GlobalTransactionScopeTestSetUp.Context.Words.Local);

            _transaction = GlobalTransactionScopeTestSetUp.Context.Database
                .BeginTransaction(System.Data.IsolationLevel.Serializable);
        }

        private void DetachEntries<T>(LocalView<T> localView)
            where T : class
        {
            foreach (var entry in localView)
            {
                GlobalTransactionScopeTestSetUp.Context.Entry(entry).State = EntityState.Detached;
            }
        }

        [TearDown]
        public void TestTearDown()
        {
            _transaction.Dispose();
        }
    }
}
