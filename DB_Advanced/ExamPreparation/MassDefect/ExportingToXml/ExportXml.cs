namespace ExportingToXml
{
    using MassDefect.Data;
    using System.Linq;
    using System.Xml.Linq;

    class ExportXml
    {
        private const string Anomalies = "../../../exported/anomalies.xml";

        static void Main()
        {
            var ctx = new MassDefectEntities();

            var anomalies = ctx.Anomalies
                .Select(a => new
                {
                    id = a.Id,
                    originPlanetName = a.OriginPlanet.Name,
                    teleportPlanetName = a.TeleportPlanet.Name,
                    victims = a.Victims.Select(v => v.Name)
                })
                .OrderBy(a => a.id);

            var xmlDocument = new XElement("anomalies");

            foreach (var anomaly in anomalies)
            {
                var anomalyNode = new XElement("anomaly");
                anomalyNode.Add(new XAttribute("id", anomaly.id));
                anomalyNode.Add(new XAttribute("origin-planet", anomaly.originPlanetName));
                anomalyNode.Add(new XAttribute("teleport-planet", anomaly.teleportPlanetName));

                var victims = new XElement("victims");

                foreach (var victim in anomaly.victims)
                {
                    var victimNode = new XElement("victim");
                    victimNode.Add(new XAttribute("name", victim));
                    victims.Add(victimNode);
                }

                anomalyNode.Add(victims);
                xmlDocument.Add(anomalyNode);
            }

            xmlDocument.Save(Anomalies);
        }
    }
}
