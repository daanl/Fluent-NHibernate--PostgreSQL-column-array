using ConsoleApplication.CustomNpgsql;
using FluentNHibernate.Mapping;

namespace ConsoleApplication
{
    public class ProjectMap : ClassMap<Project>
    {
        public ProjectMap()
        {
            Id(x => x.Id);

            Map(x => x.Tags)
                .CustomType<PostgresSqlArrayType>()
                .CustomSqlType("text[]");
        }
    }
}
