using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz4
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = false)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class NetworkModel
    {
        private List<SubstationEntity> substations;
        private List<NodeEntity> nodes;
        private List<SwitchEntity> switches;
        private List<LineEntity> lines;

        [System.Xml.Serialization.XmlArrayItemAttribute("SubstationEntity", IsNullable = false)]
        public List<SubstationEntity> Substations { get => substations; set => substations = value; }

        [System.Xml.Serialization.XmlArrayItemAttribute("NodeEntity", IsNullable = false)]
        public List<NodeEntity> Nodes { get => nodes; set => nodes = value; }

        [System.Xml.Serialization.XmlArrayItemAttribute("SwitchEntity", IsNullable = false)]
        public List<SwitchEntity> Switches { get => switches; set => switches = value; }

        [System.Xml.Serialization.XmlArrayItemAttribute("LineEntity", IsNullable = false)]
        public List<LineEntity> Lines { get => lines; set => lines = value; }
    }
}
