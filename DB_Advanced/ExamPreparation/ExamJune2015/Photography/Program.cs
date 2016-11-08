using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Photography
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctx = new PhotographySystemEntities();

            //ListCamerasManufacturerAndModel(ctx);

            //ExportManufacturersAndCamerasToJson(ctx);

            //ExportPhotographsAsXml(ctx);

            //ImportManufacturersAndLensesFromXml(ctx);
        }

        private static void ImportManufacturersAndLensesFromXml(PhotographySystemEntities ctx)
        {
            var document = XDocument.Load("../../imported/manufacturers-and-lenses.xml");
            var manufacturers = document.Root.Elements();

            var manCount = 1;

            foreach (var manufacturer in manufacturers)
            {
                Console.WriteLine($"Processing manufacturer #{manCount} ...");

                var manufacturerName = manufacturer.Element("manufacturer-name").Value;
                Manufacturer newManufacturer = null;

                if (ctx.Manufacturers.FirstOrDefault(m => m.Name == manufacturerName) != null)
                {
                    newManufacturer = ctx.Manufacturers.FirstOrDefault(m => m.Name == manufacturerName);
                    Console.WriteLine($"Existing manufacturer: {manufacturerName}");
                }
                else
                {
                    newManufacturer = new Manufacturer { Name = manufacturerName };
                    Console.WriteLine($"Created manufacturer: {manufacturerName}");
                }

                foreach (var lens in manufacturer.Element("lenses").Elements())
                {
                    var model = lens.Attribute("model").Value;
                    var type = lens.Attribute("type").Value;
                    decimal? price = null;

                    if (lens.Attribute("price") != null)
                    {
                        price = decimal.Parse(lens.Attribute("price").Value);
                    }

                    if (ctx.Lenses.FirstOrDefault(l => l.Model == model) != null)
                    {
                        Console.WriteLine($"Existing lens: {model}");
                    }
                    else
                    {
                        var newLens = new Lens
                        {
                            Model = model,
                            Type = type,
                            Price = price,
                            ManufacturerId = newManufacturer.Id
                        };
                        ctx.Lenses.Add(newLens);
                        Console.WriteLine($"Created lens: {newLens.Model}");
                    }
                }

                if (ctx.Manufacturers.FirstOrDefault(m => m.Name == manufacturerName) == null)
                {
                    ctx.Manufacturers.Add(newManufacturer);
                }
                ctx.SaveChanges();

                manCount++;
            }
        }

        private static void ExportPhotographsAsXml(PhotographySystemEntities ctx)
        {
            var photos = ctx.Photographs.Select(p => new
            {
                title = p.Title,
                category = p.Category.Name,
                link = p.Link,
                camera = p.Equipment.Camera.Manufacturer.Name + " " + p.Equipment.Camera.Model,
                megapixels = p.Equipment.Camera.Megapixels,
                lens = p.Equipment.Lens.Manufacturer.Name + " " + p.Equipment.Lens.Model,
                lensPrice = p.Equipment.Lens.Price
            })
            .OrderBy(p => p.title).ToList();

            var xml = new XDocument();
            var root = new XElement("photographs");
            xml.Add(root);

            photos.ForEach(p =>
            {
                var photoNode = new XElement("photograph");
                photoNode.SetAttributeValue("title", p.title);
                photoNode.SetElementValue("category", p.category);
                photoNode.SetElementValue("link", p.link);

                var equipmentNode = new XElement("equipment");
                var cameraNode = new XElement("camera", p.camera);
                if (p.megapixels != null)
                {
                    cameraNode.SetAttributeValue("megapixels", p.megapixels);
                }
                var lensNode = new XElement("lens", p.lens);
                if (p.lensPrice != null)
                {
                    lensNode.SetAttributeValue("price", p.lensPrice);
                }

                equipmentNode.Add(cameraNode);
                equipmentNode.Add(lensNode);
                photoNode.Add(equipmentNode);

                root.Add(photoNode);
            });

            xml.Save("../../exported/photographs.xml");
        }

        private static void ExportManufacturersAndCamerasToJson(PhotographySystemEntities ctx)
        {
            var manufacturers = ctx.Manufacturers.Select(m => new
            {
                manufacturer = m.Name,
                cameras = m.Cameras.Select(c => new
                {
                    model = c.Model,
                    price = c.Price
                })
                .OrderBy(c => c.model)
            })
            .OrderBy(m => m.manufacturer);

            var manufacturersJson = JsonConvert.SerializeObject(manufacturers, Formatting.Indented);
            File.WriteAllText("../../exported/manufactureres-and-cameras.json", manufacturersJson);
        }

        private static void ListCamerasManufacturerAndModel(PhotographySystemEntities ctx)
        {
            var cameras = ctx.Cameras.Select(c => new
            {
                ManufacturerAndModel = c.Manufacturer.Name + " " + c.Model
            })
            .OrderBy(c => c.ManufacturerAndModel).ToList();

            cameras.ForEach(c =>
            {
                Console.WriteLine(c.ManufacturerAndModel);
            });
        }
    }
}
