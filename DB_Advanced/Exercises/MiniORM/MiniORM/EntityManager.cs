using System;
using System.Collections.Generic;
using MiniORM.Contracts;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using MiniORM.Attributes;

namespace MiniORM
{
    public class EntityManager : IDbContext
    {
        private SqlConnection connection;
        private string connectionString;
        private bool isCodeFirst;

        public EntityManager(string connectionString, bool isCodeFirst)
        {
            this.connectionString = connectionString;
            this.isCodeFirst = isCodeFirst;
        }

        public IEnumerable<T> FindAll<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindAll<T>(string predicate)
        {
            throw new NotImplementedException();
        }

        public T FindById<T>(int id)
        {
            throw new NotImplementedException();
        }

        public T FindFirst<T>()
        {
            throw new NotImplementedException();
        }

        public T FindFirst<T>(string predicate)
        {
            throw new NotImplementedException();
        }

        public bool Persist(object entity)
        {
            throw new NotImplementedException();
        }

        public FieldInfo GetId(Type entity)
        {
            FieldInfo id = entity.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                            .FirstOrDefault(x => x.IsDefined(typeof(IdAttribute)));

            if(id == null)
            {
                throw new InvalidOperationException("Entity must have a primary key.");
            }

            return id;
        }

        private string GetTableName(Type entity)
        {
            EntityAttribute attribute = (EntityAttribute)Attribute.GetCustomAttribute(entity, typeof(EntityAttribute));

            if (attribute == null)
            {
                throw new ArgumentNullException("Attribute not found!");
            }

            if (attribute.TableName == null)
            {
                return entity.Name;
            }
            else
            {
                return attribute.TableName;
            }
        }

        private string GetFieldName(FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("No such column.");
            }

            if (!field.IsDefined(typeof(ColumnAttribute)))
            {
                throw new ArgumentException("Field does not have such attribute defined.");
            }

            string columnName = field.GetCustomAttribute<ColumnAttribute>().Name;

            if (columnName == null)
            {
                return field.Name;
            }

            return columnName;
        }
    }
}
