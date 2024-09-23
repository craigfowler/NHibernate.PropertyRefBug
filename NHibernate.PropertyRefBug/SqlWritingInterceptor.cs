using NHibernate.SqlCommand;

namespace NHibernate.PropertyRefBug;

public class SqlWritingInterceptor : EmptyInterceptor
{
    public override SqlString OnPrepareStatement(SqlString sql)
    {
        Console.Error.WriteLine(sql);
        return base.OnPrepareStatement(sql);
    }
}