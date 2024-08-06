using RestoreMonarchy.SpawnsManager.Helpers;
using RestoreMonarchy.SpawnsManager.Models;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using SDG.Unturned;
using System;
using System.Xml.Serialization;

namespace RestoreMonarchy.SpawnsManager
{
    public partial class SpawnsManagerPlugin : RocketPlugin<SpawnsManagerConfiguration>
    {
        public static SpawnsManagerPlugin Instance { get; private set; }

        public SpawnAssetsConfiguration SpawnAssetsConfiguration { get; set; }
        private XmlSerializer xmlSerializer = new(typeof(SpawnAssetsConfiguration));

        protected override void Load()
        {
            Instance = this;

            LoadMapConfiguration();

            if (Level.isLoaded)
            {
                SaveMapConfiguration();
                AddAndOverrideSpawnAssets();

                ItemsHelper.DespawnItems();
                VehiclesHelper.DespawnVehicles();
            }            

            Level.onPreLevelLoaded += OnPreLevelLoaded;

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
            Logger.Log("Check out more Unturned plugins at restoremonarchy.com");
        }

        protected override void Unload()
        {
            Level.onPreLevelLoaded -= OnPreLevelLoaded;

            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        private void OnPreLevelLoaded(int level)
        {
            SaveMapConfiguration();
            AddAndOverrideSpawnAssets();
        }

        private void LogDebug(string message)
        {
            if (Configuration.Instance.Debug)
            {
                Logger.Log($"Debug >> {message}", ConsoleColor.Gray);
            }
        }
    }
}
