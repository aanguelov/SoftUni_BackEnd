namespace ImportDataFromXml
{
    using MassDefect.Data;
    using System.Xml.Linq;
    using System;
    using MassDefect.Models;
    using System.Linq;

    class ImportXml
    {
        private const string NewAnomaliesPath = "../../../datasets/new-anomalies.xml";

        static void Main()
        {
            var xml = XDocument.Load(NewAnomaliesPath);
            var anomalies = xml.Root.Elements();
            var ctx = new MassDefectEntities();

            foreach (var anomaly in anomalies)
            {
                ImportAnomalyAndVictim(anomaly, ctx);
            }
        }

        private static void ImportAnomalyAndVictim(XElement anomaly, MassDefectEntities ctx)
        {
            if (anomaly.Attribute("origin-planet") == null || anomaly.Attribute("teleport-planet") == null)
            {
                Console.WriteLine("Error: Invalid data.");
            }
            else
            {
                var originPlanet = GetPlanetByName(anomaly.Attribute("origin-planet").Value, ctx);
                var teleportPlanet = GetPlanetByName(anomaly.Attribute("teleport-planet").Value, ctx);

                if (originPlanet != null || teleportPlanet != null)
                {
                    var newAnomaly = new Anomaly
                    {
                        OriginPlanet = originPlanet,
                        TeleportPlanet = teleportPlanet
                    };

                    ctx.Anomalies.Add(newAnomaly);
                    Console.WriteLine("Successfully imported anomaly.");

                    var victims = anomaly.Element("victims").Elements();

                    foreach (var victim in victims)
                    {
                        if (victim.Attribute("name") != null)
                        {
                            ImportVictim(victim.Attribute("name").Value, ctx, newAnomaly);
                        }
                        else
                        {
                            Console.WriteLine("Error: Invalid data.");
                        }
                    }

                    ctx.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Error: Invalid data.");
                }
            }
        }

        private static void ImportVictim(string value, MassDefectEntities ctx, Anomaly newAnomaly)
        {
            var personEntity = GetPersonByName(value, ctx);
            if (personEntity != null)
            {
                newAnomaly.Victims.Add(personEntity);
            }
            else
            {
                Console.WriteLine("Error: Invalid data.");
            }
        }

        private static Person GetPersonByName(string value, MassDefectEntities ctx)
        {
            return ctx.Persons.FirstOrDefault(p => p.Name == value);
        }

        private static Planet GetPlanetByName(string value, MassDefectEntities ctx)
        {
            return ctx.Planets.FirstOrDefault(p => p.Name == value);
        }
    }
}
