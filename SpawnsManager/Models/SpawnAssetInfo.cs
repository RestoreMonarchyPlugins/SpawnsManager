using SDG.Unturned;

namespace RestoreMonarchy.SpawnsManager.Models
{
    public class SpawnAssetInfo
    {
        public SpawnAssetInfo(ushort id, string origin, string prefix, EAssetType assetType = EAssetType.ITEM)
        {
            Id = id;
            Prefix = prefix;
            Origin = origin;
            AssetType = assetType;
        }

        public ushort Id { get; set; }
        public string Origin { get; set; }
        public EAssetType AssetType { get; set; }
        public string Prefix { get; set; }
    }
}
