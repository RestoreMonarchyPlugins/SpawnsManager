using RestoreMonarchy.SpawnsManager.Models;
using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestoreMonarchy.SpawnsManager.Commands
{
    public class CheckSpawnCommand : IRocketCommand
    {
        private SpawnsManagerPlugin pluginInstance => SpawnsManagerPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 1)
            {
                UnturnedChat.Say(caller, "You must specify spawn id.", pluginInstance.MessageColor);
                return;
            }

            if (!ushort.TryParse(command[0], out ushort spawnId))
            {
                UnturnedChat.Say(caller, "Spawn id is invalid.", pluginInstance.MessageColor);
                return;
            }

            bool isVehicle = false;

            string arg2 = command.ElementAtOrDefault(1);
            if (string.IsNullOrEmpty(arg2))
            {
                bool.TryParse(arg2, out isVehicle);
            }

            SpawnAsset spawnAsset = Assets.find(EAssetType.SPAWN, spawnId) as SpawnAsset;
            if (spawnAsset == null)
            {
                UnturnedChat.Say(caller, "Spawn asset not found.", pluginInstance.MessageColor);
                return;
            }
            
            List<SpawnItemInfo> spawnItems = pluginInstance.GetSpawnItems(spawnAsset, isVehicle);

            int sumWeight = spawnItems.Sum(x => x.Weight);

            UnturnedChat.Say(caller, $"Spawn {spawnAsset.id} ({spawnAsset.name}) has {spawnItems.Count} items:", pluginInstance.MessageColor);
            foreach (SpawnItemInfo spawnItem in spawnItems)
            {
                decimal chance = Math.Round((decimal)spawnItem.Weight / sumWeight * 100, 4);
                UnturnedChat.Say(caller, $"- {spawnItem.Name} ({spawnItem.AssetId}) has {chance}% ({spawnItem.Weight}) chance", pluginInstance.MessageColor);
            }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "checkspawn";

        public string Help => "Checks information of the spawn";

        public string Syntax => "<spawnId>";

        public List<string> Aliases => new();

        public List<string> Permissions => new();
    }
}
