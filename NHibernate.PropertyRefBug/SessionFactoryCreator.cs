using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace NHibernate.PropertyRefBug;

public static class SessionFactoryCreator
{
    public static ISessionFactory GetSessionFactory()
    {
        var config = GetCommonConfiguration();
        config.AddAssembly(Assembly.GetExecutingAssembly());
        return config.BuildSessionFactory();
    }

    static Configuration GetCommonConfiguration()
    {
        var config = new Configuration();
        config.DataBaseIntegration(x =>
        {
            x.Dialect<SQLiteDialect>();
            x.ConnectionString = "Data Source=:memory:";
        });
        return config;
    }
}