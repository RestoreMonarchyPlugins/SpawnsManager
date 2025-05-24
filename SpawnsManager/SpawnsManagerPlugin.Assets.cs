using RestoreMonarchy.SpawnsManager.Helpers;
using RestoreMonarchy.SpawnsManager.Models;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestoreMonarchy.SpawnsManager
{
    public partial class SpawnsManagerPlugin
    {
        private void AddAndOverrideSpawnAssets()
        {
            LogDebug("Adding and overriding spawn assets...");
            foreach (SpawnAssetConfig spawnAssetConfig in SpawnAssetsConfiguration.SpawnAssets)
            {
                SpawnAsset asset = new()
                {
                    id = spawnAssetConfig.Id,
                    GUID = spawnAssetConfig.GUID,
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
                    }
                    else if (spawnTableConfig.AssetId != 0)
                    {
                        ReflectionHelper.SpawnTableLegacyAssetIdField.SetValue(spawnTable, spawnTableConfig.AssetId);
                    }
                    else
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
            LogDebug($"{SpawnAssetsConfiguration.SpawnAssets.Length} spawn assets have been added or overriden!");
        }

        public List<SpawnItemInfo> GetSpawnItems(SpawnAsset spawnAsset, EAssetType assetType, List<SpawnItemInfo> spawnItems = null, int depth = 0, decimal chance = 1)
        {
            bool isFirstDepth = depth == 0;
            if (spawnItems == null)
            {
                spawnItems = new List<SpawnItemInfo>();
            }

            if (depth > 32)
            {
                return spawnItems;
            }

            decimal sumWeight = spawnAsset.tables.Sum(x => x.weight);
            foreach (SpawnTable spawnTable in spawnAsset.tables)
            {
                Asset asset = spawnTable.FindAsset(assetType);
                if (asset == null)
                {
                    continue;
                }

                decimal spawnChance = spawnTable.weight / sumWeight * chance;

                if (asset is SpawnAsset spawnAsset2)
                {
                    depth++;
                    GetSpawnItems(spawnAsset2, assetType, spawnItems, depth, spawnChance);
                }
                else if (asset is ItemAsset itemAsset)
                {
                    spawnItems.Add(new SpawnItemInfo()
                    {
                        AssetId = itemAsset.id,
                        Name = itemAsset.itemName,
                        Chance = spawnChance
                    });
                }
                else if (asset is VehicleAsset vehicleAsset)
                {
                    spawnItems.Add(new SpawnItemInfo()
                    {
                        AssetId = vehicleAsset.id,
                        Name = vehicleAsset.vehicleName,
                        Chance = spawnChance
                    });
                }
                else if (asset is VehicleRedirectorAsset vehicleRedirectorAsset)
                {
                    spawnItems.Add(new SpawnItemInfo()
                    {
                        AssetId = vehicleRedirectorAsset.id,
                        Name = vehicleRedirectorAsset.FriendlyName,
                        Chance = spawnChance
                    });
                }
                else if (asset is AnimalAsset animalAsset)
                {
                    spawnItems.Add(new SpawnItemInfo()
                    {
                        AssetId = animalAsset.id,
                        Name = animalAsset.animalName,
                        Chance = spawnChance
                    });
                }
                else
                {
                    LogDebug($"Unknown asset type: {asset.GetType().Name} - {asset.id} - {asset.name}");
                }
            }

            if (isFirstDepth)
            {
                // aggregate spawn items by AssetId
                spawnItems = spawnItems
                    .GroupBy(x => x.AssetId)
                    .Select(g => new SpawnItemInfo
                    {
                        AssetId = g.Key,
                        Name = g.First().Name,
                        Chance = g.Sum(x => x.Chance)
                    })
                    .ToList();

                // calculate weights
                int totalWeight = 10000;
                foreach (SpawnItemInfo spawnItem in spawnItems)
                {
                    spawnItem.Weight = Math.Max(1, (int)(spawnItem.Chance * totalWeight));
                }
                spawnItems = spawnItems.OrderByDescending(x => x.Weight).ToList();
            }

            return spawnItems;
        }
    }
}
