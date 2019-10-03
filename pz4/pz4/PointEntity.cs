using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz4
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class PointEntity
    {
        private double x;
        private double y;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
    }
}
