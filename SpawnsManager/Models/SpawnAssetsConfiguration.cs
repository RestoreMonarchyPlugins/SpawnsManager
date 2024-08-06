using Rocket.API;
using System.Xml.Serialization;

namespace RestoreMonarchy.SpawnsManager.Models
{
    public class SpawnAssetsConfiguration : IDefaultable
    {
        public string Map { get; set; }
        [XmlArrayItem("SpawnAsset")]
        public SpawnAssetConfig[] SpawnAssets { get; set; }

        public void LoadDefaults()
        {
            SpawnAssets = [];
        }
    }
}
