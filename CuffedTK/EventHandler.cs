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


            if (ev.IsAllowed)
            {
                cuffedDict.Add(ev.Target, ev.Cuffer);
            }


            if (plugin.Config.Debug)
            {
                Log.Debug("Current cuffedDict<target, cuffer> is:");
                foreach (var key in cuffedDict.Keys)
                {
                    Log.Debug($"{key.Nickname} -> {cuffedDict[key].Nickname}");
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
                            if (plugin.Config.Debug) { Log.Debug($"{ev.Cuffer.Nickname} is removing cuffs on {ev.Target.Nickname}"); }

                            cuffedDict.Remove(player);
                            break;
                        }
                    }
                }
            }
        }


        public void OnDeath(DyingEventArgs ev)
        {
            if (cuffedDict.ContainsKey(ev.Target))
            {
                foreach (Player player in cuffedDict.Keys)
                {
                    if (player == ev.Target)
                    {
                        if (plugin.Config.Debug) { Log.Debug($"{ev.Killer.Nickname} has killed {ev.Target.Nickname} while cuffed, removing from cuffedDict"); }

                        cuffedDict.Remove(player);
                        break;
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


        public void OnForceClass(ChangingRoleEventArgs ev)
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
         
            if (plugin.Config.DisallowedDamageTypes.Contains(ev.Handler.Type) && (ev.Target.Team == Team.CDP || ev.Target.Team == Team.RSC))
            {
                if (plugin.Config.DamageTypesTime > 0)
                    ev.Attacker.ShowHint(plugin.Config.DamageTypesMessage.Replace("%PLAYER%", ev.Target.Nickname).Replace("%DAMAGETYPE%", ev.Handler.Type.ToString()), plugin.Config.DamageTypesTime);
                ev.IsAllowed = false;
            }
            else if ((ev.Target.Team == Team.CDP && plugin.Config.DisallowDamagetodclass.Contains(ev.Attacker.Team)) || (ev.Target.Team == Team.RSC && plugin.Config.DisallowDamagetoScientists.Contains(ev.Attacker.Team)))
            {
                if (plugin.Config.AttackerHintTime > 0)
                    ev.Attacker.ShowHint(plugin.Config.AttackerHint.Replace("%PLAYER%", ev.Target.Nickname), plugin.Config.AttackerHintTime);
                ev.IsAllowed = false;
            }
            else if ((plugin.Config.DisallowDamageToChaos && ev.Target.Team == Team.CHI && plugin.Config.DisallowDamagetodclass.Contains(ev.Attacker.Team)) || (plugin.Config.DisallowDamageToMTF && ev.Target.Team == Team.MTF && plugin.Config.DisallowDamagetoScientists.Contains(ev.Attacker.Team)))
            {
                if (plugin.Config.AttackerHintTime > 0)
                    ev.Attacker.ShowHint(plugin.Config.AttackerHint.Replace("%PLAYER%", ev.Target.Nickname), plugin.Config.AttackerHintTime);
                ev.IsAllowed = false;
            }
        }
    }
}
