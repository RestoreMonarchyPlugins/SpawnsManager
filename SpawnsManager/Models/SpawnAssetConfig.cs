using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RestoreMonarchy.SpawnsManager.Models
{
    public class SpawnAssetConfig
    {
        [XmlAttribute]
        public ushort Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Description { get; set; }

        [XmlArrayItem("Table")]
        public SpawnTableConfig[] Tables { get; set; }

        public bool ShouldSerializeDescription() => !string.IsNullOrEmpty(Description);
    }
}
