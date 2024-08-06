using SDG.Unturned;
using System.Reflection;

namespace RestoreMonarchy.SpawnsManager.Helpers
{
    internal static class ReflectionHelper
    {
        public static FieldInfo SpawnAssetRootsField { get; } = typeof(SpawnAsset).GetField("_roots", BindingFlags.NonPublic | BindingFlags.Instance);
        public static FieldInfo SpawnAssetTablesField { get; } = typeof(SpawnAsset).GetField("_tables", BindingFlags.NonPublic | BindingFlags.Instance);
        public static PropertyInfo SpawnAssetAreTablesDirtyProperty { get; } = typeof(SpawnAsset).GetProperty("areTablesDirty", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        public static PropertyInfo SpawnsAssetInsertRootsProperty { get; } = typeof(SpawnAsset).GetProperty("insertRoots", BindingFlags.Instance | BindingFlags.Public);
        public static MethodInfo AddToMappingMethod { get; } = typeof(Assets).GetMethod("AddToMapping", BindingFlags.NonPublic | BindingFlags.Static);
        public static FieldInfo CurrentAssetMappingField { get; } = typeof(Assets).GetField("currentAssetMapping", BindingFlags.NonPublic | BindingFlags.Static);
        public static FieldInfo SpawnTableLegacyAssetIdField { get; } = typeof(SpawnTable).GetField("legacyAssetId", BindingFlags.NonPublic | BindingFlags.Instance);
        public static FieldInfo SpawnTableLegacySpawnIdField { get; } = typeof(SpawnTable).GetField("legacySpawnId", BindingFlags.NonPublic | BindingFlags.Instance);
        public static FieldInfo ItemDataLastDroppedField { get; } = typeof(ItemData).GetField("_lastDropped", BindingFlags.NonPublic | BindingFlags.Instance);
    }
}
