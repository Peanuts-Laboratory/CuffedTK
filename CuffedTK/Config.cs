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

        [Description("For how long should the Hint be shown to the Attacker? (-1 = Disabled)")]
        public ushort AttackerHintTime { get; set; } = 3;

        [Description("What hint should the Attacker be displayed when trying to damage a Cuffed D-Class? %PLAYER% will be replaced with the Target Username")]
        public string AttackerHint { get; set; } = "<color=red>That player is cuffed!</color>";

        [Description("What Team should not be allowed to damage an Cuffed D-Class! CDP = Class-D, CHI = Chaos, MTF = Nine-Tailed Fox, RSC = Scientists, TUT = Tutorial")]
        public HashSet<Team> DisallowDamagetodclass { get; set; } = new HashSet<Team>
        {
            Team.MTF,
            Team.RSC
        };
        [Description("Should Chaos be considered with the settings above (they will also take no dmg while cuffed)?")]
        public bool DisallowDamageToChaos { get; set; } = true;
        [Description("What Team should not be allowed to damage an Cuffed Scientists! CDP = Class-D, CHI = Chaos, MTF = Nine-Tailed Fox, RSC = Scientists, TUT = Tutorial")]
        public HashSet<Team> DisallowDamagetoScientists { get; set; } = new HashSet<Team>
        {
            Team.CDP,
            Team.CHI
        };
        [Description("Should MTF be considered with the settings above (they will also take no dmg while cuffed)?")]
        public bool DisallowDamageToMTF { get; set; } = true;
        [Description("For how long should the hint be shown to the Attacker? (-1 = Disabled)")]
        public int DamageTypesTime { get; set; } = 3;

        [Description("What hint should be displayed when trying to damage a Cuffed D-Class with a Disallowed DamageType? %PLAYER% will be replaced with the Target Username and %DAMAGETYPE% will be replaced with the DamageType")]
        public string DamageTypesMessage { get; set; } = "<color=red>That player is cuffed!</color>";

        [Description("What DamageType should not be allowed to damage a Cuffed D-Class or Cuffed Scientist? Check https://exiled-team.github.io/EXILED/api/Exiled.API.Enums.DamageType.html for all DamageTypes")]
        public HashSet<DamageType> DisallowedDamageTypes { get; set; } = new HashSet<DamageType>
        {
            DamageType.Explosion,
            DamageType.FriendlyFireDetector
        };
        public bool Debug { get; set; } = false;
    }
}
