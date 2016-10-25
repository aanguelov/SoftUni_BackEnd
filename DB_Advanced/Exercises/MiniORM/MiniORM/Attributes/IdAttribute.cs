using System;

namespace MiniORM.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class IdAttribute : Attribute
    {
        public IdAttribute()
        {

        }
    }
}
