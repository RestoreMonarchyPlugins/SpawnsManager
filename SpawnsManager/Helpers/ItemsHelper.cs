using SDG.Unturned;

namespace RestoreMonarchy.SpawnsManager.Helpers
{
    internal static class ItemsHelper
    {
        internal static void DespawnItems()
        {
            for (byte x = 0; x < Regions.WORLD_SIZE; x++)
            {
                for (byte y = 0; y < Regions.WORLD_SIZE; y++)
                {
                    ItemRegion region = ItemManager.regions[x, y];
                    foreach (ItemData itemData in region.items)
                    {
                        if (itemData == null)
                        {
                            continue;
                        }
                        if (itemData.isDropped)
                        {
                            continue;
                        }

                        ReflectionHelper.ItemDataLastDroppedField.SetValue(itemData, 0);
                    }
                    region.lastRespawn = 0;
                }
            }
        }
    }
}
