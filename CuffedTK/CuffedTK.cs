﻿using Exiled.API.Features;
using System;

namespace CuffedTK
{
    public class CuffedTK : Plugin<Config>
    {
        public override string Name => "CuffedTK";
        public override string Author => "Marco15453";
        public override string Prefix => "CTK";
        public override Version Version => new Version(1, 9, 1);
        public override Version RequiredExiledVersion => new Version(4, 2, 2);

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
            Exiled.Events.Handlers.Player.Dying += eventHandler.OnDeath;
            Exiled.Events.Handlers.Player.Escaping += eventHandler.OnEscape;
        }

        private void unregisterEvents()
        {
            // Player
            Exiled.Events.Handlers.Player.Hurting -= eventHandler.onHurting;
            Exiled.Events.Handlers.Player.Handcuffing -= eventHandler.OnCuffing;
            Exiled.Events.Handlers.Player.RemovingHandcuffs -= eventHandler.OnRemovingCuffs;
            Exiled.Events.Handlers.Player.Dying -= eventHandler.OnDeath;
            Exiled.Events.Handlers.Player.Escaping -= eventHandler.OnEscape;

            eventHandler = null;
        }
    }
}
