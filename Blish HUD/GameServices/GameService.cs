﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Blish_HUD.GameServices;

namespace Blish_HUD {
    public abstract class GameService {

        private static readonly GameService[] _allServices = new GameService[] {
            Settings        = new SettingsService(),
            Debug           = new DebugService(),
            Input           = new InputService(),
            Content         = new ContentService(),
            Gw2Mumble       = new Gw2MumbleService(),
            Gw2WebApi       = new Gw2WebApiService(),
            Animation       = new AnimationService(),
            Graphics        = new GraphicsService(),
            Overlay         = new OverlayService(),
            GameIntegration = new GameIntegrationService(),
            ArcDpsV2          = new ArcDpsServiceV2(), // This needs to be initialized bf the V1
            ArcDps          = new ArcDpsService(),
            Contexts        = new ContextsService(),
            Module          = new ModuleService()
        };

        public static IReadOnlyList<GameService> All => _allServices;

        public event EventHandler<EventArgs> FinishedLoading;

        protected virtual void OnFinishedLoading(EventArgs e) {
            this.FinishedLoading?.Invoke(this, e);
        }

        protected abstract void Initialize();
        protected abstract void Load();
        protected abstract void Unload();
        protected abstract void Update(GameTime gameTime);

        private protected BlishHud ActiveBlishHud;

        public bool Loaded { get; private set; }

        private IServiceModule[] _serviceModules = Array.Empty<IServiceModule>();

        internal void SetServiceModules(params IServiceModule[] serviceModules) {
            _serviceModules = serviceModules ?? Array.Empty<IServiceModule>();
        }

        internal void DoInitialize(BlishHud game) {
            ActiveBlishHud = game;

            Initialize();
        }

        internal void DoLoad() {
            Load();

            foreach (var serviceModule in _serviceModules) {
                serviceModule.Load();
            }

            this.Loaded = true;
            OnFinishedLoading(EventArgs.Empty);
        }

        internal void DoUnload() {
            foreach (var serviceModule in _serviceModules) {
                serviceModule.Unload();
            }

            Unload();

            this.Loaded = false;
        }

        internal void DoUpdate(GameTime gameTime) {
            foreach (var serviceModule in _serviceModules) {
                serviceModule.Update(gameTime);
            }

            Update(gameTime);
        }

        #region Static Service References

        public static readonly DebugService           Debug;
        public static readonly SettingsService        Settings;
        public static readonly ContentService         Content;
        public static readonly Gw2MumbleService       Gw2Mumble;
        public static readonly Gw2WebApiService       Gw2WebApi;
        public static readonly AnimationService       Animation;
        public static readonly GraphicsService        Graphics;
        public static readonly OverlayService         Overlay;
        public static readonly InputService           Input;
        public static readonly GameIntegrationService GameIntegration;
        public static readonly ArcDpsService          ArcDps;
        public static readonly ArcDpsServiceV2        ArcDpsV2;
        public static readonly ContextsService        Contexts;
        public static readonly ModuleService          Module;

        #endregion

    }
}
