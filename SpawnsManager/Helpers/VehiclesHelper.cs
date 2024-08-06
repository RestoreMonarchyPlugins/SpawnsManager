using SDG.Unturned;
using System.Linq;

namespace RestoreMonarchy.SpawnsManager.Helpers
{
    internal static class VehiclesHelper
    {
        internal static void DespawnVehicles()
        {
            foreach (InteractableVehicle vehicle in VehicleManager.vehicles.ToList())
            {
                if (vehicle == null)
                {
                    continue;
                }

                if (vehicle.passengers.Any(x => x.player != null))
                {
                    continue;
                }

                if (vehicle.isLocked)
                {
                    continue;
                }

                VehicleManager.askVehicleDestroy(vehicle);
            }
        }
    }
}
