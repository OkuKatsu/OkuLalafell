using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using static OkuLalafell.Utils.Constant;

namespace OkuLalafell
{
    [Serializable]
    internal class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;
        public Race SelectedRace { get; set; } = Race.LALAFELL;
        public Gender SelectedGender { get; set; } = Gender.KEEP;
        public bool Enabled { get; set; } = false;
        public bool StayOn { get; set; } = false;
        public bool Naked { get; set; } = false;
        public bool NakedWithEmp { get; set; } = false;
        public bool LalaWithGender { get; set; } = false;
        public bool ShowHQ { get; set; } = false;

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private IDalamudPluginInterface? pluginInterface;

        public void Initialize(IDalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public void Save()
        {
            pluginInterface!.SavePluginConfig(this);
        }
    }
}
