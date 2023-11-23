using System.Xml.Serialization;

namespace RestoreMonarchy.SpawnsManager.Models
{
    public class SpawnTableConfig
    {
        [XmlAttribute]
        public ushort SpawnId { get; set; }
        [XmlAttribute]
        public ushort AssetId { get; set; }
        [XmlAttribute]
        public int Weight { get; set; }
        [XmlAttribute]
        public string Name { get; set; }

        public bool ShouldSerializeSpawnId() => SpawnId != 0;
        public bool ShouldSerializeAssetId() => AssetId != 0;
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}
