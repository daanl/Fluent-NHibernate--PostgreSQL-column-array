using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace ConsoleApplication.CustomNpgsql
{
    public class PostgresSqlArrayType : IUserType
    {
        bool IUserType.Equals(object x, object y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return (x == null) ? 0 : x.GetHashCode();
        }

        public virtual object NullSafeGet(IDataReader resultSet, string[] names, object owner)
        {
            var index = resultSet.GetOrdinal(names[0]);
            
            if (resultSet.IsDBNull(index))
            {
                return null;
            }
           
            var res = resultSet.GetValue(index) as string[];
          
            if (res != null)
            {
                return res.ToList();
            }
            
            throw new NotImplementedException();
        }

        public virtual void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            var parameter = ((IDbDataParameter)cmd.Parameters[index]);
            if (value == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                var list = (List<string>)value;
                parameter.Value = list.ToArray();
            }
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }
        
        public SqlType[] SqlTypes
        {
            get
            {
                var sqlTypes = new SqlType[]
                {
                    new NpgsqlExtendedSqlType(
                        DbType.Object, 
                        NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Text
                    )
                };

                return sqlTypes;
            }
        }

        public virtual Type ReturnedType
        {
            get { return typeof(IList<string>); }
        }
        
        public bool IsMutable { get; private set; }
    }
}