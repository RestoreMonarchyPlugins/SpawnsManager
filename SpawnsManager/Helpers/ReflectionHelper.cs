using SDG.Unturned;
using System.Reflection;

namespace RestoreMonarchy.SpawnsManager.Helpers
{
    internal class ReflectionHelper
    {
        public ReflectionHelper()
        {
            SpawnAssetRootsField = typeof(SpawnAsset).GetField("_roots", BindingFlags.NonPublic | BindingFlags.Instance);
            SpawnAssetTablesField = typeof(SpawnAsset).GetField("_tables", BindingFlags.NonPublic | BindingFlags.Instance);
            SpawnAssetAreTablesDirtyProperty = typeof(SpawnAsset).GetProperty("areTablesDirty", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            SpawnsAssetInsertRootsProperty = typeof(SpawnAsset).GetProperty("insertRoots", BindingFlags.Instance | BindingFlags.Public);
            AddToMappingMethod = typeof(Assets).GetMethod("AddToMapping", BindingFlags.NonPublic | BindingFlags.Static);
            CurrentAssetMappingField = typeof(Assets).GetField("currentAssetMapping", BindingFlags.NonPublic | BindingFlags.Static);
            SpawnTableLegacyAssetIdField = typeof(SpawnTable).GetField("legacyAssetId", BindingFlags.NonPublic | BindingFlags.Instance);
            SpawnTableLegacySpawnIdField = typeof(SpawnTable).GetField("legacySpawnId", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public FieldInfo SpawnAssetRootsField { get; }
        public FieldInfo SpawnAssetTablesField { get; }
        public PropertyInfo SpawnAssetAreTablesDirtyProperty { get; }
        public PropertyInfo SpawnsAssetInsertRootsProperty { get; }
        public MethodInfo AddToMappingMethod { get; }
        public FieldInfo CurrentAssetMappingField { get; }
        public FieldInfo SpawnTableLegacyAssetIdField { get; }
        public FieldInfo SpawnTableLegacySpawnIdField { get; }
    }
}
