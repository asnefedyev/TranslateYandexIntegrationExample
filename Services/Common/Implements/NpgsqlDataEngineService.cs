using System.Data;
using Example.Services.Common.Contracts;
using Example.Services.Common.Implements.Contexts;
using Microsoft.EntityFrameworkCore;
using TranslatorExample.Services.Common.Contracts;

namespace Example.Services.Common.Implements
{
    public class NpgsqlDataEngineService : IDataEngineService
    {
        private readonly IServiceProvider _sp;
        private readonly IDatabaseConnection _Connection;
        private readonly string _dbAlias;

        public NpgsqlDataEngineService(IServiceProvider sp, string dbAlias)
        {
            _sp = sp;
            _Connection = (IDatabaseConnection?)sp.GetService(typeof(IDatabaseConnection));
        }

        public List<TEntity> GetEntityList<TEntity>(Func<IDbConnection, List<TEntity>> processing) where TEntity : class
        {
            List<TEntity> lst;
            using (IDbConnection connection = _Connection.GetDbConnection())
            {
                lst = processing(connection);
            }
            return lst;
        }

        public void ChangeEntityList(Action<IDbConnection> processing)
        {
            using (IDbConnection connection = _Connection.GetDbConnection())
            {
                processing(connection);
            }
        }

        public List<TEntity> GetEntityList<TEntity>(Func<DbContext, List<TEntity>> processing) where TEntity : class
        {
            using (var scope = _sp.CreateScope())
            {
                List<TEntity> prev;
                using (var context = scope.ServiceProvider.GetRequiredService<ExampleDbContext>())
                {
                    var strategy = context.Database.CreateExecutionStrategy();
                    return strategy.Execute(() =>
                    {
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                var result = processing(context);
                                prev = result;
                                transaction.Commit();
                                return prev;
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    });
                }
            }
        }

        public void ChangeEntityList(Action<DbContext> processing)
        {
            using (var scope = _sp.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<ExampleDbContext>())
                {
                    var strategy = context.Database.CreateExecutionStrategy();
                    strategy.Execute(() =>
                    {
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                processing(context);
                                transaction.Commit();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    });
                }
            }
        }

        public bool IsDataBaseAvailability()
        {
            throw new NotImplementedException();
        }

        public string GetDbSessionId()
        {
            throw new NotImplementedException();
        }
    }
}