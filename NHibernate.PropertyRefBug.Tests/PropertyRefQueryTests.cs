using System;
using System.Linq;
using NHibernate.PropertyRefBug.Entities;

namespace NHibernate.PropertyRefBug.Tests;

[TestFixture]
public class PropertyRefQueryTests
{
    [Test]
    public void QueryForDataWhichWasInsertedViaAdoUsingASubqueryShouldProvideExpectedResults()
    {
        using var sessionFactory = SessionFactoryCreator.GetSessionFactory();
        using var connection = AdoDbInitialiser.GetConnection();
        connection.Open();
        using var session = sessionFactory.WithOptions().Connection(connection).Interceptor(new SqlWritingInterceptor()).OpenSession();
        
        AdoDbInitialiser.CreateSchema(connection);
        AdoDbInitialiser.AddData(connection);

        // This form of query is how we first discovered the issue.  This is a simplified reproduction of the
        // sort of Linq that we were using in our app.  It seems to occur when we force an EXISTS( ... ) subquery.
        var validOrders = session.Query<Order>().Where(x => x.CreatedDate > new DateTime(2024, 9, 10));
        var orderCount = session.Query<LineItem>().Count(x => validOrders.Any(y => y == x.Order));
        
        Assert.That(orderCount, Is.EqualTo(1));
    }
    
    [Test]
    public void SimpleQueryForDataWhichWasInsertedViaAdoShouldProvideExpectedResults()
    {
        using var sessionFactory = SessionFactoryCreator.GetSessionFactory();
        using var connection = AdoDbInitialiser.GetConnection();
        connection.Open();
        using var session = sessionFactory.WithOptions().Connection(connection).Interceptor(new SqlWritingInterceptor()).OpenSession();
        
        AdoDbInitialiser.CreateSchema(connection);
        AdoDbInitialiser.AddData(connection);

        // This style of equivalent query does not exhibit the problem.  This test passes no matter which NH version.
        var lineItem = session.Query<LineItem>().FirstOrDefault(x => x.Order.CreatedDate > new DateTime(2024, 9, 10));
        Assert.That(lineItem, Is.Not.Null);
    }
}