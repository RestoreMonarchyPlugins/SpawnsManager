using RestoreMonarchy.SpawnsManager.Models;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.SpawnsManager.Commands
{
    public class CheckMapCommand : IRocketCommand
    {
        private SpawnsManagerPlugin pluginInstance => SpawnsManagerPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedChat.Say(caller, $"Spawns of {Provider.map} map", pluginInstance.MessageColor);
            UnturnedChat.Say(caller, $"Zombies ({LevelZombies.tables.Count}) spawns:", pluginInstance.MessageColor);
            foreach (ZombieTable zombieTable in LevelZombies.tables)
            {
                if (zombieTable.lootID == 0)
                {
                    continue;
                }

                SpawnAsset spawnAsset = Assets.find(EAssetType.SPAWN, zombieTable.lootID) as SpawnAsset;
                if (spawnAsset == null)
                {
                    continue;
                }

                string zombie = zombieTable.isMega ? "Mega" : "Normal";
                UnturnedChat.Say(caller, $"- {zombie} zombie {zombieTable.name} has spawn asset {spawnAsset.id} ({spawnAsset.name})", pluginInstance.MessageColor);
            }

            UnturnedChat.Say(caller, $"Items ({LevelItems.tables.Count}) spawns:", pluginInstance.MessageColor);
            foreach (ItemTable itemTable in LevelItems.tables)
            {
                if (itemTable.tableID == 0)
                {
                    continue;
                }

                SpawnAsset spawnAsset = Assets.find(EAssetType.SPAWN, itemTable.tableID) as SpawnAsset;
                if (spawnAsset == null)
                {
                    continue;
                }

                UnturnedChat.Say(caller, $"- Item {itemTable.name} has spawn asset {spawnAsset.id} ({spawnAsset.name})", pluginInstance.MessageColor);
            }

            UnturnedChat.Say(caller, $"Vehicles ({LevelVehicles.tables.Count}) spawns:", pluginInstance.MessageColor);
            foreach (VehicleTable vehicleTable in LevelVehicles.tables)
            {
                if (vehicleTable.tableID == 0)
                {
                    continue;
                }

                SpawnAsset spawnAsset = Assets.find(EAssetType.SPAWN, vehicleTable.tableID) as SpawnAsset;
                if (spawnAsset == null)
                {
                    continue;
                }

                UnturnedChat.Say(caller, $"- Vehicle {vehicleTable.name} has spawn asset {spawnAsset.id} ({spawnAsset.name})", pluginInstance.MessageColor);
            }

            UnturnedChat.Say(caller, $"Animals ({LevelAnimals.tables.Count}) spawns:", pluginInstance.MessageColor);
            foreach (AnimalTable animalTable in LevelAnimals.tables)
            {
                if (animalTable.tableID == 0)
                {
                    continue;
                }

                SpawnAsset spawnAsset = Assets.find(EAssetType.SPAWN, animalTable.tableID) as SpawnAsset;
                if (spawnAsset == null)
                {
                    continue;
                }

                UnturnedChat.Say(caller, $"- Animal {animalTable.name} has spawn asset {spawnAsset.id} ({spawnAsset.name})", pluginInstance.MessageColor);
            }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "checkmap";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new();

        public List<string> Permissions => new();
    }
}
