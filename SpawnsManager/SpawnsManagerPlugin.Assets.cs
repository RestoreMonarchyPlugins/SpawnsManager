using RestoreMonarchy.SpawnsManager.Helpers;
using RestoreMonarchy.SpawnsManager.Models;
using SDG.Unturned;
using System.Collections.Generic;

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
                }
                else if (asset is ItemAsset itemAsset)
                {
                    spawnItems.Add(new SpawnItemInfo()
                    {
                        AssetId = itemAsset.id,
                        Name = itemAsset.itemName,
                        Weight = spawnTable.weight
                    });
                }
                else if (asset is VehicleAsset vehicleAsset)
                {
                    spawnItems.Add(new SpawnItemInfo()
                    {
                        AssetId = vehicleAsset.id,
                        Name = vehicleAsset.vehicleName,
                        Weight = spawnTable.weight
                    });
                }
                else if (asset is VehicleRedirectorAsset vehicleRedirectorAsset)
                {
                    spawnItems.Add(new SpawnItemInfo()
                    {
                        AssetId = vehicleRedirectorAsset.id,
                        Name = vehicleRedirectorAsset.FriendlyName,
                        Weight = spawnTable.weight
                    });
                }
                else
                {
                    LogDebug($"Unknown asset type: {asset.GetType().Name} - {asset.id} - {asset.name}");
                }
            }

            return spawnItems;
        }
    }
}
