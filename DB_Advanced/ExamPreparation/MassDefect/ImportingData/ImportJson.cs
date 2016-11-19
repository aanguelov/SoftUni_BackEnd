namespace ImportingData
{
    using System;
    using MassDefect.Data;
    using System.IO;
    using Newtonsoft.Json;
    using MassDefect.Models.JsonModels;
    using MassDefect.Models;
    using System.Linq;

    class ImportJson
    {
        private const string SolarSystemsPath = "../../../datasets/solar-systems.json";
        private const string StarsPath = "../../../datasets/stars.json";
        private const string PlanetsPath = "../../../datasets/planets.json";
        private const string PersonsPath = "../../../datasets/persons.json";
        private const string AnomaliesPath = "../../../datasets/anomalies.json";
        private const string AnomalyVictimsPath = "../../../datasets/anomaly-victims.json";

        static void Main()
        {
            var ctx = new MassDefectEntities();

            ImportSolarSystems(ctx);
            ImportStars(ctx);
            ImportPlanets(ctx);
            ImportPeople(ctx);
            ImportAnomalies(ctx);
            ImportAnomalyVictims(ctx);
        }

        private static void ImportAnomalyVictims(MassDefectEntities ctx)
        {
            var anomalyVictimsFile = File.ReadAllText(AnomalyVictimsPath);
            var jsonAnomalyVictims = JsonConvert.DeserializeObject<JsonAnomalyVictim[]>(anomalyVictimsFile);

            foreach (var anomalyVictim in jsonAnomalyVictims)
            {
                if (anomalyVictim.Id == null || anomalyVictim.Person == null)
                {
                    Console.WriteLine("Error: Invalid data.");
                }
                else
                {
                    var anomalyEntity = GetAnomalyById(anomalyVictim.Id, ctx);
                    var personEntity = GetPersonByName(anomalyVictim.Person, ctx);

                    if (anomalyEntity != null || personEntity != null)
                    {
                        anomalyEntity.Victims.Add(personEntity);
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid data.");
                    }
                }
            }

            ctx.SaveChanges();
        }

        private static Person GetPersonByName(string person, MassDefectEntities ctx)
        {
            return ctx.Persons.FirstOrDefault(p => p.Name == person);
        }

        private static Anomaly GetAnomalyById(int? id, MassDefectEntities ctx)
        {
            return ctx.Anomalies.Find(id);
        }

        private static void ImportAnomalies(MassDefectEntities ctx)
        {
            var anomaliesFile = File.ReadAllText(AnomaliesPath);
            var jsonAnomalies = JsonConvert.DeserializeObject<JsonAnomaly[]>(anomaliesFile);

            foreach (var anomaly in jsonAnomalies)
            {
                if (anomaly.OriginPlanet == null || 
                    anomaly.TeleportPlanet == null || 
                    GetPlanetByName(anomaly.OriginPlanet, ctx) == null ||
                    GetPlanetByName(anomaly.TeleportPlanet, ctx) == null)
                {
                    Console.WriteLine("Error: Invalid data.");
                }
                else
                {
                    var newAnomaly = new Anomaly
                    {
                        OriginPlanet = GetPlanetByName(anomaly.OriginPlanet, ctx),
                        TeleportPlanet = GetPlanetByName(anomaly.TeleportPlanet, ctx)
                    };

                    ctx.Anomalies.Add(newAnomaly);
                    Console.WriteLine($"Successfully imported anomaly.");
                }
            }

            ctx.SaveChanges();
        }

        private static void ImportPeople(MassDefectEntities ctx)
        {
            var peopleFile = File.ReadAllText(PersonsPath);
            var jsonPeople = JsonConvert.DeserializeObject<JsonPerson[]>(peopleFile);

            foreach (var person in jsonPeople)
            {
                if (person.Name == null || 
                    person.HomePlanet == null || 
                    GetPlanetByName(person.HomePlanet, ctx) == null)
                {
                    Console.WriteLine("Error: Invalid data.");
                }
                else
                {
                    var newPerson = new Person
                    {
                        Name = person.Name,
                        HomePlanet = GetPlanetByName(person.HomePlanet, ctx)
                    };

                    ctx.Persons.Add(newPerson);
                    Console.WriteLine($"Successfully imported Person {newPerson.Name}.");
                }
            }
            ctx.SaveChanges();
        }

        private static Planet GetPlanetByName(string homePlanet, MassDefectEntities ctx)
        {
            return ctx.Planets.FirstOrDefault(p => p.Name == homePlanet);
        }

        private static void ImportPlanets(MassDefectEntities ctx)
        {
            var planetsFile = File.ReadAllText(PlanetsPath);
            var planetsJson = JsonConvert.DeserializeObject<JsonPlanet[]>(planetsFile);

            foreach (var planet in planetsJson)
            {
                if (planet.Name == null || 
                    planet.Sun == null || 
                    planet.SolarSystem == null ||
                    GetStarByName(planet.Sun, ctx) == null ||
                    GetSolarSystemByName(planet.SolarSystem, ctx) == null)
                {
                    Console.WriteLine("Error: Invalid data.");
                }
                else
                {
                    var newPlanet = new Planet
                    {
                        Name = planet.Name,
                        SolarSystem = GetSolarSystemByName(planet.SolarSystem, ctx),
                        Sun = GetStarByName(planet.Sun, ctx)
                    };

                    ctx.Planets.Add(newPlanet);
                    Console.WriteLine($"Successfully imported Planet {newPlanet.Name}.");
                }
            }

            ctx.SaveChanges();
        }

        private static Star GetStarByName(string sun, MassDefectEntities ctx)
        {
            return ctx.Stars.FirstOrDefault(s => s.Name == sun);
        }

        private static void ImportStars(MassDefectEntities ctx)
        {
            var starsFile = File.ReadAllText(StarsPath);
            var jsonStars = JsonConvert.DeserializeObject<JsonStar[]>(starsFile);

            foreach (var star in jsonStars)
            {
                if (star.Name == null || 
                    star.SolarSystem == null || 
                    GetSolarSystemByName(star.SolarSystem, ctx) == null)
                {
                    Console.WriteLine("Error: Invalid data.");
                }
                else
                {
                    var newStar = new Star
                    {
                        Name = star.Name,
                        SolarSystem = GetSolarSystemByName(star.SolarSystem, ctx)
                    };

                    ctx.Stars.Add(newStar);
                    Console.WriteLine($"Successfully imported Star {newStar.Name}.");
                }                
            }

            ctx.SaveChanges();
        }

        private static SolarSystem GetSolarSystemByName(string solarSystem, MassDefectEntities ctx)
        {
            return ctx.SolarSystems.FirstOrDefault(ss => ss.Name == solarSystem);
        }

        private static void ImportSolarSystems(MassDefectEntities ctx)
        {
            var solarSystemsFile = File.ReadAllText(SolarSystemsPath);
            var jsonSolarSystems = JsonConvert.DeserializeObject<JsonSolarSystem[]>(solarSystemsFile);

            foreach (var system in jsonSolarSystems)
            {
                if (system.Name == null)
                {
                    Console.WriteLine("Error: Invalid data.");
                }
                else
                {
                    var newSolarSystem = new SolarSystem
                    {
                        Name = system.Name
                    };

                    ctx.SolarSystems.Add(newSolarSystem);
                    Console.WriteLine($"Successfully imported Solar System {system.Name}.");
                }               
            }

            ctx.SaveChanges();
        }
    }
}
