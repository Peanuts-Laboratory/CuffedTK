using Exiled.API.Enums;
using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace CuffedTK
{
    public sealed class Config : IConfig
    {
        [Description("Should the plugin be enabled?")]
        public bool IsEnabled { get; set; } = true;
        [Description("What hint should the Attacker be displayed when trying to damage a Cuffed player?")]
        public string AttackerHint { get; set; } = "<color=red>That player is cuffed!</color>";
        [Description("For how long should the Hint be shown to the Attacker? (-1 = Disabled)")]
        public ushort AttackerHintTime { get; set; } = 3;
        [Description("What hint should any uncuffer be displayed when trying to uncuff a player?")]
        public string UncufferHint { get; set; } = "<color=red>Uncuffing to kill will result in a ban</color>";
        [Description("For how long should the Hint be shown to the Uncuffer? (-1 = Disabled)")]
        public ushort UncufferHintTime { get; set; } = 3;
        [Description("What DamageType should not be allowed to damage a cuffed player?")]
        public HashSet<DamageType> DisallowedDamageTypes { get; set; } = new HashSet<DamageType>
        {
            DamageType.Explosion,
            DamageType.FriendlyFireDetector
        };
        [Description("Whether or not MTF/CI can 'escape' while disarmed to switch teams.")]
        public bool disarm_can_switch_teams { get; set; } = true;

        public bool Debug { get; set; } = false;
    }
}
