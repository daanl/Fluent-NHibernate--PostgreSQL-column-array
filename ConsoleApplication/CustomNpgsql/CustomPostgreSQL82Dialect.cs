using System.Data;
using NHibernate.Dialect;

namespace ConsoleApplication.CustomNpgsql
{
    public class CustomPostgreSQL82Dialect : PostgreSQL82Dialect
    {
        public CustomPostgreSQL82Dialect()
        {
            RegisterColumnType(DbType.Object, "text[]");
        }
    }
}