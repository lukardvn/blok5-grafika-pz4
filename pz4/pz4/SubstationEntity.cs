using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz4
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class SubstationEntity
    {
        private ulong id;
        private string name;
        private double x;
        private double y;

        public ulong Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
    }
}
