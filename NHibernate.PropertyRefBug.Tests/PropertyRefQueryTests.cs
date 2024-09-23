using System;
using System.Linq;
using NHibernate.PropertyRefBug.Entities;

namespace NHibernate.PropertyRefBug.Tests;

[TestFixture]
public class PropertyRefQueryTests
{
    [Test]
    public void QueryingForSomeDataWhichWasInsertedViaAdoShouldProvideExpectedResults()
    {
        using var sessionFactory = SessionFactoryCreator.GetSessionFactory();
        using var connection = AdoDbInitialiser.GetConnection();
        connection.Open();
        using var session = sessionFactory.WithOptions().Connection(connection).Interceptor(new SqlWritingInterceptor()).OpenSession();
        
        AdoDbInitialiser.CreateSchema(connection);
        AdoDbInitialiser.AddData(connection);

        var validOrders = session.Query<Order>().Where(x => x.CreatedDate > new DateTime(2024, 9, 10));
        var orderCount = session.Query<LineItem>().Count(x => validOrders.Any(y => y == x.Order));
        Assert.That(orderCount, Is.EqualTo(1));
    }
}