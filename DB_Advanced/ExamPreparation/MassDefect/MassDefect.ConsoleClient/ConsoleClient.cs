namespace MassDefect.ConsoleClient
{
    using Data;
    using System;
    using System.Linq;

    class ConsoleClient
    {
        static void Main()
        {
            var ctx = new MassDefectEntities();
            
            //Inititate database
            ctx.SolarSystems.Count();
            Console.WriteLine(ctx.Anomalies.Count());
        }
    }
}
