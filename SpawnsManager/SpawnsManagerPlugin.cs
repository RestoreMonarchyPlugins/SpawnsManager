using RestoreMonarchy.SpawnsManager.Helpers;
using RestoreMonarchy.SpawnsManager.Models;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestoreMonarchy.SpawnsManager
{
    public class SpawnsManagerPlugin : RocketPlugin<SpawnsManagerConfiguration>
    {
        public static SpawnsManagerPlugin Instance { get; private set; }
        public UnityEngine.Color MessageColor { get; private set; }

        internal ReflectionHelper ReflectionHelper { get; set; }

        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, UnityEngine.Color.yellow);

            ReflectionHelper = new();

            Level.onPreLevelLoaded += OnPreLevelLoaded;

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
        }

        protected override void Unload()
        {
            Level.onPreLevelLoaded -= OnPreLevelLoaded;

            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        public List<SpawnItemInfo> GetSpawnItems(SpawnAsset spawnAsset, bool isVehicle = false, int num = 0)
        {
            List<SpawnItemInfo> spawnItems = new();

            if (num++ > 32)
            {
                return spawnItems;
            }

            foreach (SpawnTable spawnTable in spawnAsset.tables)
            {
                EAssetType assetType = isVehicle ? EAssetType.VEHICLE : EAssetType.ITEM;
                Asset asset = spawnTable.FindAsset(assetType);

                if (asset == null)
                {
                    return [];
                }

                if (asset is SpawnAsset spawnAsset2)
                {
                    spawnItems.AddRange(GetSpawnItems(spawnAsset2, isVehicle, num));
                } else if (asset is ItemAsset itemAsset)
                {
                    spawnItems.Add(new SpawnItemInfo()
                    {
                        AssetId = itemAsset.id,
                        Name = itemAsset.itemName,
                        Weight = spawnTable.weight
                    });
                } else if (asset is VehicleAsset vehicleAsset)
                {
                    spawnItems.Add(new SpawnItemInfo()
                    {
                        AssetId = vehicleAsset.id,
                        Name = vehicleAsset.vehicleName,
                        Weight = spawnTable.weight
                    });
                }
            }

            return spawnItems;
        }

        private void OnPreLevelLoaded(int level)
        {
            Logger.Log("Loading spawn assets...", ConsoleColor.Yellow);
            foreach (SpawnAssetConfig spawnAssetConfig in Configuration.Instance.SpawnAssets)
            {
                SpawnAsset asset = new()
                {
                    GUID = Guid.NewGuid(),
                    id = spawnAssetConfig.Id,
                    name = spawnAssetConfig.Name
                };

                ReflectionHelper.SpawnsAssetInsertRootsProperty.SetValue(asset, new List<SpawnTable>());
                ReflectionHelper.SpawnAssetRootsField.SetValue(asset, new List<SpawnTable>());
                ReflectionHelper.SpawnAssetTablesField.SetValue(asset, new List<SpawnTable>());

                foreach (SpawnTableConfig spawnTableConfig in spawnAssetConfig.Tables)
                {
                    SpawnTable spawnTable = new()
                    {
                        weight = spawnTableConfig.Weight,
                    };

                    if (spawnTableConfig.SpawnId != 0)
                    {
                        ReflectionHelper.SpawnTableLegacySpawnIdField.SetValue(spawnTable, spawnTableConfig.SpawnId);
                    } else if (spawnTableConfig.AssetId != 0)
                    {
                        ReflectionHelper.SpawnTableLegacyAssetIdField.SetValue(spawnTable, spawnTableConfig.AssetId);
                    } else
                    {
                        continue;
                    }                    
                    
                    asset.tables.Add(spawnTable);
                }

                asset.markTablesDirty();
                object assetMapping = ReflectionHelper.CurrentAssetMappingField.GetValue(null);
                ReflectionHelper.AddToMappingMethod.Invoke(null, [asset, true, assetMapping]);
            }

            Assets.linkSpawns();
            Logger.Log($"{Configuration.Instance.SpawnAssets.Length} spawn assets have been loaded!", ConsoleColor.Yellow);
        }
    }
}
