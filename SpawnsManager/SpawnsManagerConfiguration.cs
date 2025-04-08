using Rocket.API;

namespace RestoreMonarchy.SpawnsManager
{
    public class SpawnsManagerConfiguration : IRocketPluginConfiguration
    {
        public bool Debug { get; set; }

        public void LoadDefaults()
        {
            Debug = false;
        }
    }
}