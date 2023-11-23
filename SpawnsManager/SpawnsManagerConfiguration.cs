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
            SpawnAssets =
            [
                new SpawnAssetConfig()
                {
                    Id = 380,
                    Name = "Washington Militia",
                    Tables =
                    [
                        new SpawnTableConfig()
                        {
                            AssetId = 1441,
                            Weight = 100,
                            Name = "Shadowstalker MK 2"
                        },
                        new SpawnTableConfig()
                        {
                            AssetId = 1443,
                            Weight = 200,
                            Name = "Shadowstalker MK 2 Drum"
                        },
                    ]
                }
            ];
        }
    }
}