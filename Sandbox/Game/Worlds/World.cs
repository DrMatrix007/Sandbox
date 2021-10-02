using MatrixEngine.Content;
using MatrixEngine.GameObjects;
using MatrixEngine.GameObjects.Components.RenderComponents;
using MatrixEngine.GameObjects.Components.TilemapComponents;
using MatrixEngine.StateManagment;
using Sandbox.Game.Components;
using Sandbox.Game.Components.Entities;
using Sandbox.Game.Components.Maps;
using Sandbox.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixEngine.UI;
using SFML.Graphics;
using MatrixEngine.Scenes;
using MatrixEngine.Scenes.Plugins;
using Sandbox.Game.Plugins;
using Sandbox.Game.Items.Inventories;

namespace Sandbox.Game.Worlds {

    public class World : Scene {
        private readonly ItemLibrary items;

        public World(ItemLibrary items) {
            this.items = items;
        }

        public override void Start() {
            base.Start();

            var invenProv = new StateProvider<Inventory>(new Inventory(32));

            var playerProvider = new LockValueProvider<PlayerComponent>();
            var mapProvider = new LockValueProvider<GenerationMapComponent>();
            var pointerProvider = new LockValueProvider<PointerMapComponent>();

            playerProvider.SetValue(new PlayerComponent(pointerProvider, mapProvider, invenProv));
            mapProvider.SetValue(new GenerationMapComponent(items, playerProvider));
            pointerProvider.SetValue(new PointerMapComponent(new Tile(TextureManager.GetTexture("Assets/Items/Pointer.png")), playerProvider));

            playerProvider.Lock();
            mapProvider.Lock();
            pointerProvider.Lock();

            AddPlugin(new InventoryUIControllerPlugin(invenProv));

            var g = new GameObject();
            g.SetComponent<PlayerComponent>(playerProvider.Get());
            AddGameObject(g);

            var tilemap = new GameObject(mapProvider.Get());

            AddGameObject(tilemap);

            var pointer = new GameObject(pointerProvider.Get());
            AddGameObject(pointer);

            var fpsprov = new FunctionProvider<string>(() => {
                return $"fps is: {(1 / app.DeltaTimeAsSeconds).ToString("00000.00")}";
            });

            var fpsui = new TextConsumerUIObject(new Anchor(new(80, 0), new(20, 10)), fpsprov, new UITextStyle(
                10, Color.White, Color.Black, FontManager.CascadiaCode, 10, true
                ));
            AddUIObject(fpsui);

            var gameobjectCountUI = new TextConsumerUIObject(new Anchor(new(80, 10), new(20, 10)), new FunctionProvider<string>(() => {
                return $"GameObjects: {this.Count()}";
            }), new UITextStyle(200, Color.Black, Color.White, FontManager.CascadiaCode, isResize: true));
            AddUIObject(gameobjectCountUI);
        }
    }
}