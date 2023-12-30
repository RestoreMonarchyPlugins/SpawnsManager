using RestoreMonarchy.SpawnsManager.Models;
using Rocket.API;
using System.Management.Instrumentation;
using System.Xml.Serialization;

namespace RestoreMonarchy.SpawnsManager
{
    public class SpawnsManagerConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        [XmlArrayItem("SpawnAsset")]
        public SpawnAssetConfig[] SpawnAssets { get; set; }

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            SpawnAssets = [];
        }
    }
}