namespace DataExporting
{
    using MassDefect.Data;
    using System.Linq;
    using Newtonsoft.Json;
    using System.IO;

    class DataExporting
    {
        private const string PlanetsWhichAreNotAnomalyOrigins = "../../../exported/planets.json";
        private const string PeopleWhichHaveNotBeenVictims = "../../../exported/people.json";
        private const string TopAnomaly = "../../../exported/anomaly.json";

        static void Main()
        {
            var ctx = new MassDefectEntities();

            //ExportPlanetsWhichAreNotAnomalyOrigins(ctx);

            //ExportPeopleWhichHaveNotBeenVictims(ctx);

            ExportTopAnomaly(ctx);
        }

        private static void ExportTopAnomaly(MassDefectEntities ctx)
        {
            var anomaly = ctx.Anomalies
                .OrderByDescending(a => a.Victims.Count)
                .Take(1)
                .Select(a => new
                {
                    id = a.Id,
                    originPlanet = new
                    {
                        name = a.OriginPlanet.Name
                    },
                    teleportPlanet = new
                    {
                        name = a.TeleportPlanet.Name
                    },
                    victimsCount = a.Victims.Count
                });

            var jsonAnomaly = JsonConvert.SerializeObject(anomaly, Formatting.Indented);
            File.WriteAllText(TopAnomaly, jsonAnomaly);
        }

        private static void ExportPeopleWhichHaveNotBeenVictims(MassDefectEntities ctx)
        {
            var people = ctx.Persons
                .Where(p => !p.Anomalies.Any())
                .Select(p => new
                {
                    name = p.Name,
                    homePlanet = new
                    {
                        name = p.HomePlanet.Name
                    }
                });

            var peolpeJson = JsonConvert.SerializeObject(people, Formatting.Indented);
            File.WriteAllText(PeopleWhichHaveNotBeenVictims, peolpeJson);
        }

        private static void ExportPlanetsWhichAreNotAnomalyOrigins(MassDefectEntities ctx)
        {
            var planets = ctx.Planets
                .Where(p => !p.OriginPlanetAnomalies.Any())
                .Select(p => new
                {
                    name = p.Name
                });

            var planetsJson = JsonConvert.SerializeObject(planets, Formatting.Indented);
            File.WriteAllText(PlanetsWhichAreNotAnomalyOrigins, planetsJson);
        }
    }
}
