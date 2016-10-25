using MiniORM.Contracts;
using MiniORM.Entities;
using System;
using System.Reflection;

namespace MiniORM
{
    class Program
    {
        static void Main()
        {
            EntityManager manager = new EntityManager("asd", true);
            User pesho = new User("peshi", "123456", 16, DateTime.Now);

            FieldInfo id = manager.GetId(pesho.GetType());
            Console.WriteLine(pesho.GetType().Name);
        }
    }
}
