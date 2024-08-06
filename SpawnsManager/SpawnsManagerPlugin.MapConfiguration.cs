using RestoreMonarchy.SpawnsManager.Models;
using Rocket.Core.Logging;
using SDG.Framework.IO.Deserialization;
using SDG.Framework.IO.Serialization;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RestoreMonarchy.SpawnsManager
{
    public partial class SpawnsManagerPlugin
    {
        private string SpawnAssetsFileName => $"SpawnAssets.{Provider.map}.xml";
        private string SpawnAssetsFilePath => Path.Combine(Directory, SpawnAssetsFileName);

        private void LoadMapConfiguration()
        {
            if (File.Exists(SpawnAssetsFilePath))
            {
                using (StreamReader reader = new(SpawnAssetsFilePath))
                {
                    SpawnAssetsConfiguration = (SpawnAssetsConfiguration)xmlSerializer.Deserialize(reader);
                }
                
                Logger.Log($"Loaded {SpawnAssetsConfiguration.SpawnAssets.Length} spawn assets from {SpawnAssetsFileName}.");
                return;
            }
        }

        private void SaveMapConfiguration()
        {
            if (File.Exists(SpawnAssetsFilePath))
            {
                return;
            }

            SpawnAssetsConfiguration = new()
            {
                Map = Provider.map,
                SpawnAssets = GetSpawnAssets()
            };

            string fileName = $"SpawnAssets.{Provider.map}.xml";
            string filePath = Path.Combine(Directory, fileName);
            using (StreamWriter writer = new(filePath))
            {
                xmlSerializer.Serialize(writer, SpawnAssetsConfiguration);
            }

            Logger.Log($"Generated {fileName} with {SpawnAssetsConfiguration.SpawnAssets.Length} spawn assets.", ConsoleColor.Yellow);
        }

        private SpawnAssetConfig[] GetSpawnAssets()
        {
            List<SpawnAssetInfo> spawnAssetInfos = LevelZombies.tables.Select(x => new SpawnAssetInfo(x.lootID, x.name, "Zombie_")).ToList();
            spawnAssetInfos.AddRange(LevelItems.tables.Select(x => new SpawnAssetInfo(x.tableID, x.name, "Item_")));
            spawnAssetInfos.AddRange(LevelVehicles.tables.Select(x => new SpawnAssetInfo(x.tableID, x.name, "Vehicle_", true)));
            spawnAssetInfos.AddRange(LevelAnimals.tables.Select(x => new SpawnAssetInfo(x.tableID, x.name, "Animal_")));

            for (byte b3 = 0; b3 < Regions.WORLD_SIZE; b3 += 1)
            {
                for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
                {
                    List<ResourceSpawnpoint> resourceSpawnpoints = LevelGround.trees[b3, b4];

                    foreach (ResourceSpawnpoint resourceSpawnpoint in resourceSpawnpoints)
                    {
                        if (resourceSpawnpoint.asset == null)
                        {
                            continue;
                        }

                        if (spawnAssetInfos.Any(x => x.Id == resourceSpawnpoint.asset.rewardID && x.Origin == resourceSpawnpoint.asset.resourceName))
                        {
                            continue;
                        }

                        SpawnAssetInfo spawnAssetInfo = new(resourceSpawnpoint.asset.rewardID, resourceSpawnpoint.asset.resourceName, "Resource_");
                        spawnAssetInfos.Add(spawnAssetInfo);
                    }
                }
            }

            spawnAssetInfos.RemoveAll(x => x.Id == 0);

            List<SpawnAssetConfig> spawnAssetConfigs = new();
            IEnumerable<IGrouping<ushort, SpawnAssetInfo>> groupedSpawnAssetInfos = spawnAssetInfos.GroupBy(x => x.Id);
            foreach (IGrouping<ushort, SpawnAssetInfo> groupedSpawnAssetInfo in groupedSpawnAssetInfos)
            {
                ushort spawnAssetId = groupedSpawnAssetInfo.Key;
                SpawnAssetInfo spawnAssetInfo = groupedSpawnAssetInfo.First();
                SpawnAsset spawnAsset = Assets.find(EAssetType.SPAWN, spawnAssetId) as SpawnAsset;
                if (spawnAsset == null)
                {
                    continue;
                }

                SpawnAssetConfig spawnAssetConfig = new()
                {
                    Id = spawnAsset.id,
                    GUID = spawnAsset.GUID,
                    Name = spawnAsset.name,
                    Description = string.Join(", ", groupedSpawnAssetInfo.Select(x => x.Prefix + x.Origin))
                };

                List<SpawnTableConfig> spawnTableConfigs = new();

                IEnumerable<SpawnItemInfo> spawnItemInfos = GetSpawnItems(spawnAsset, spawnAssetInfo.IsVehicle);
                foreach (SpawnItemInfo spawnItemInfo in spawnItemInfos)
                {
                    SpawnTableConfig spawnTableConfig = new()
                    {
                        AssetId = spawnItemInfo.AssetId,
                        Name = spawnItemInfo.Name,
                        Weight = spawnItemInfo.Weight
                    };
                    spawnTableConfigs.Add(spawnTableConfig);
                }

                spawnAssetConfig.Tables = spawnTableConfigs.ToArray();

                spawnAssetConfigs.Add(spawnAssetConfig);
            }

            return spawnAssetConfigs.ToArray();
        }
    }
}
