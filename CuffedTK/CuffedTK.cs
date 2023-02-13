using Exiled.API.Features;
using Exiled.API.Enums;
using System;

namespace CuffedTK
{
    public class CuffedTK : Plugin<Config>
    {
        public override string Name => "CuffedTK";
        public override string Author => "Marco15453 -> Drew";
        public override string Prefix => "CTK";
        public override Version Version => new Version(2, 0, 0);
        public override Version RequiredExiledVersion => new Version(6, 0, 0);
        public override PluginPriority Priority { get; } = PluginPriority.Highest;

        private EventHandler eventHandler;


        public override void OnEnabled()
        {
            registerEvents();
            base.OnEnabled();
        }


        public override void OnDisabled()
        {
            unregisterEvents();
            base.OnDisabled();
        }


        private void registerEvents()
        {
            eventHandler = new EventHandler(this);

            // Player
            Exiled.Events.Handlers.Player.Hurting += eventHandler.onHurting;
            Exiled.Events.Handlers.Player.Handcuffing += eventHandler.OnCuffing;
            Exiled.Events.Handlers.Player.RemovingHandcuffs += eventHandler.OnRemovingCuffs;
            Exiled.Events.Handlers.Player.Escaping += eventHandler.OnEscape;
            Exiled.Events.Handlers.Player.ChangingRole += eventHandler.OnChangeClass;

            // Server
            Exiled.Events.Handlers.Server.WaitingForPlayers += eventHandler.OnWaitingForPlayers;
        }


        private void unregisterEvents()
        {
            // Player
            Exiled.Events.Handlers.Player.Hurting -= eventHandler.onHurting;
            Exiled.Events.Handlers.Player.Handcuffing -= eventHandler.OnCuffing;
            Exiled.Events.Handlers.Player.RemovingHandcuffs -= eventHandler.OnRemovingCuffs;
            Exiled.Events.Handlers.Player.Escaping -= eventHandler.OnEscape;
            Exiled.Events.Handlers.Player.ChangingRole -= eventHandler.OnChangeClass;

            // Server
            Exiled.Events.Handlers.Server.WaitingForPlayers -= eventHandler.OnWaitingForPlayers;

            eventHandler = null;
        }
    }
}
