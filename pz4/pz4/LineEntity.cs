using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz4
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]

    public class LineEntity
    {
        private ulong id;
        private string name;
        private bool isUnderground;
        private decimal r;
        private string conductorMaterial;
        private string lineType;
        private int termalConstantHeat;
        private ulong firstEnd;
        private ulong secondEnd;
        private List<PointEntity> vertices;

        public ulong Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public bool IsUnderground { get => isUnderground; set => isUnderground = value; }
        public decimal R { get => r; set => r = value; }
        public string ConductorMaterial { get => conductorMaterial; set => conductorMaterial = value; }
        public string LineType { get => lineType; set => lineType = value; }
        public int TermalConstantHeat { get => termalConstantHeat; set => termalConstantHeat = value; }
        public ulong FirstEnd { get => firstEnd; set => firstEnd = value; }
        public ulong SecondEnd { get => secondEnd; set => secondEnd = value; }

        [System.Xml.Serialization.XmlArrayItemAttribute("Point", IsNullable = false)]
        public List<PointEntity> Vertices { get => vertices; set => vertices = value; }
    }
}
