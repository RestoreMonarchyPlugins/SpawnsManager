namespace RestoreMonarchy.SpawnsManager.Models
{
    public class SpawnAssetInfo
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
}
