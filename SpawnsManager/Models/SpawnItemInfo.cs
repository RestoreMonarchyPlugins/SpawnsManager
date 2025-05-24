using System;
using System.Xml.Serialization;

namespace RestoreMonarchy.SpawnsManager.Models
{
    public class SpawnItemInfo
    {
        public ushort AssetId { get; set; }   
        public string Name { get; set; }
        public int Weight { get; set; }

        [XmlIgnore]
        public decimal Chance { get; set; }
    }
}
