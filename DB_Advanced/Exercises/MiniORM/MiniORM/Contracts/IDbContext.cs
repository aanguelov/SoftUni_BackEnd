using System.Collections.Generic;

namespace MiniORM.Contracts
{
    public interface IDbContext
    {
        bool Persist(object entity);

        T FindById<T>(int id);

        IEnumerable<T> FindAll<T>();

        IEnumerable<T> FindAll<T>(string predicate);

        T FindFirst<T>();

        T FindFirst<T>(string predicate);
    }
}
