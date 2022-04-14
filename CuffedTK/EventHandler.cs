using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.Events.EventArgs;
using System.Collections.Generic;

namespace CuffedTK
{
    internal sealed class EventHandler
    {
        public CuffedTK plugin;
        public EventHandler(CuffedTK plugin) => this.plugin = plugin;

        private Dictionary<Player, Player> cuffedDict = new Dictionary<Player, Player>();


        public void OnCuffing(HandcuffingEventArgs ev)
        {
            if (plugin.Config.Debug) { Log.Debug($"{ev.Cuffer.Nickname} is cuffing {ev.Target.Nickname}"); }


            if (ev.IsAllowed && !cuffedDict.ContainsKey(ev.Target))
            {
                cuffedDict.Add(ev.Target, ev.Cuffer);

                if (plugin.Config.Debug)
                {
                    Log.Debug("Current cuffedDict<target, cuffer> is:");
                    foreach (var key in cuffedDict.Keys)
                    {
                        Log.Debug($"{key.Nickname} -> {cuffedDict[key].Nickname}");
                    }
                }
            }
        }


        public void OnRemovingCuffs(RemovingHandcuffsEventArgs ev)
        {
            if (ev.IsAllowed)
            {
                if (cuffedDict.ContainsKey(ev.Target))
                {
                    foreach (Player player in cuffedDict.Keys)
                    {
                        if (player == ev.Target)
                        {
                            if (plugin.Config.Debug) { Log.Debug($"{ev.Cuffer} is removing cuffs on {ev.Target.Nickname}"); }

                            ev.Cuffer.ShowHint("Uncuffing to kill will result in a ban", duration: 5);

                            cuffedDict.Remove(player);
                            break;
                        }
                    }
                }
            }
        }


        public void OnEscape(EscapingEventArgs ev)
        {
            if (cuffedDict.ContainsKey(ev.Player))
            {
                foreach (Player player in cuffedDict.Keys)
                {
                    if (player == ev.Player)
                    {
                        if (plugin.Config.Debug) { Log.Debug($"{ev.Player.Nickname} is escaping, removing from cuffedDict"); }

                        cuffedDict.Remove(player);
                        break;
                    }
                }
            }
        }


        public void OnChangeClass(ChangingRoleEventArgs ev)
        {
            if (cuffedDict.ContainsKey(ev.Player))
            {
                foreach (Player player in cuffedDict.Keys)
                {
                    if (player == ev.Player)
                    {
                        if (plugin.Config.Debug) { Log.Debug($"{ev.Player.Nickname} is changing class, removing from cuffedDict"); }

                        cuffedDict.Remove(player);
                        break;
                    }
                }
            }
        }


        public void onHurting(HurtingEventArgs ev)
        {
            if (!ev.Target.IsCuffed || ev.Target == null || ev.Attacker == null || ev.Handler.Type == DamageType.Unknown) return;
            // only the cuffer can damage them
            if (cuffedDict.ContainsKey(ev.Target) && cuffedDict[ev.Target] == ev.Attacker) return;
         
            if (plugin.Config.DisallowedDamageTypes.Contains(ev.Handler.Type) && (ev.Target.Role.Team == Team.CDP || ev.Target.Role.Team == Team.RSC))
            {
                if (plugin.Config.DamageTypesTime > 0)
                    ev.Attacker.ShowHint(plugin.Config.DamageTypesMessage.Replace("%PLAYER%", ev.Target.Nickname).Replace("%DAMAGETYPE%", ev.Handler.Type.ToString()), plugin.Config.DamageTypesTime);
                ev.IsAllowed = false;
            }
            else if ((ev.Target.Role.Team == Team.CDP && plugin.Config.DisallowDamagetodclass.Contains(ev.Attacker.Role.Team)) || (ev.Target.Role.Team == Team.RSC && plugin.Config.DisallowDamagetoScientists.Contains(ev.Attacker.Role.Team)))
            {
                if (plugin.Config.AttackerHintTime > 0)
                    ev.Attacker.ShowHint(plugin.Config.AttackerHint.Replace("%PLAYER%", ev.Target.Nickname), plugin.Config.AttackerHintTime);
                ev.IsAllowed = false;
            }
            else if ((plugin.Config.DisallowDamageToChaos && ev.Target.Role.Team == Team.CHI && plugin.Config.DisallowDamagetodclass.Contains(ev.Attacker.Role.Team)) || (plugin.Config.DisallowDamageToMTF && ev.Target.Role.Team == Team.MTF && plugin.Config.DisallowDamagetoScientists.Contains(ev.Attacker.Role.Team)))
            {
                if (plugin.Config.AttackerHintTime > 0)
                    ev.Attacker.ShowHint(plugin.Config.AttackerHint.Replace("%PLAYER%", ev.Target.Nickname), plugin.Config.AttackerHintTime);
                ev.IsAllowed = false;
            }
        }
    }
}
