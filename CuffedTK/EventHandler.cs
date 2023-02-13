using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.Events.EventArgs;
using System.Collections.Generic;
using Exiled.Events.EventArgs.Player;

namespace CuffedTK
{
    internal sealed class EventHandler
    {
        public CuffedTK plugin;
        public EventHandler(CuffedTK plugin) => this.plugin = plugin;

        private Dictionary<Player, Player> cuffedDict = new Dictionary<Player, Player>();

        public void OnWaitingForPlayers()
        {
            Log.Info(message: "Loaded and waiting for players...");
            cuffedDict.Clear();
        }

        public void OnCuffing(HandcuffingEventArgs ev)
        {
            if (plugin.Config.Debug) { Log.Debug($"{ev.Player.Nickname} is cuffing {ev.Target.Nickname}"); }

            if (ev.IsAllowed && !cuffedDict.ContainsKey(ev.Target))
            {
                cuffedDict.Add(ev.Target, ev.Player);

                if (plugin.Config.Debug)
                {
                    Log.Debug("Current cuffedDict<target, player> is:");
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
                            if (plugin.Config.Debug) { Log.Debug($"{ev.Player.Nickname} is removing cuffs on {ev.Target.Nickname}"); }

                            ev.Player.ShowHint(plugin.Config.UncufferHint, plugin.Config.UncufferHintTime);

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
            if (!ev.Player.IsCuffed || ev.Player == null || ev.Attacker == null || ev.DamageHandler.Type == DamageType.Unknown) return;

            // if a player is cuffed
            if (ev.Player.IsCuffed)
            {
                // allow scp's to attack cuffed players
                if (ev.Attacker.IsScp) return;

                // they cannot be damaged by these damage sources
                if (plugin.Config.DisallowedDamageTypes.Contains(ev.DamageHandler.Type))
                {
                    ev.IsAllowed = false;
                    return;
                }

                // only the cuffer can damage them with the allowed damage types
                if (cuffedDict.ContainsKey(ev.Player) && cuffedDict[ev.Player] == ev.Attacker) return;

                // don't allow anyone else to damage them
                ev.Attacker.ShowHint(plugin.Config.AttackerHint, plugin.Config.AttackerHintTime);
                ev.IsAllowed = false;
                return;
            }
        }
    }
}