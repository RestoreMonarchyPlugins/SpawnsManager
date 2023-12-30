using RestoreMonarchy.SpawnsManager.Models;
using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.SpawnsManager.Commands
{
    public class LoadMapSpawnsCommand : IRocketCommand
    {
        private SpawnsManagerPlugin pluginInstance => SpawnsManagerPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (pluginInstance.Configuration.Instance.SpawnAssets.Length > 0)
            {
                UnturnedChat.Say(caller, "Can't load the map spawns if there are more than 0 in the configuration already.", pluginInstance.MessageColor);
                return;
            }

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
                    Name = spawnAsset.name,
                    Description = string.Join(", ", groupedSpawnAssetInfo.Select(x => x.Prefix + x.Origin))
                };

                List<SpawnTableConfig> spawnTableConfigs = new();

                IEnumerable<SpawnItemInfo> spawnItemInfos = pluginInstance.GetSpawnItems(spawnAsset, spawnAssetInfo.IsVehicle);
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

            pluginInstance.Configuration.Instance.SpawnAssets = spawnAssetConfigs.ToArray();
            pluginInstance.Configuration.Save();
            UnturnedChat.Say(caller, $"Loaded {spawnAssetConfigs.Count()} spawn assets from the map.", pluginInstance.MessageColor);
        }

        private class SpawnAssetInfo
        {
            public SpawnAssetInfo(ushort id, string origin, string prefix, bool isVehicle = false)
            {
                Id = id;
                Prefix = prefix;
                Origin = origin;
                IsVehicle = isVehicle;
            }

            public ushort Id { get; set; }
            public string Origin { get; set; }
            public bool IsVehicle { get; set; }
            public string Prefix { get; set; }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Console;

        public string Name => "loadmapspawns";

        public string Help => "Loads all the spawn assets from the map";

        public string Syntax => "";

        public List<string> Aliases => new();

        public List<string> Permissions => new();
    }
}
