using MatrixEngine.Content;
using MatrixEngine.GameObjects.Components.PhysicsComponents;
using MatrixEngine.GameObjects.Components.RenderComponents;
using MatrixEngine.GameObjects.Components.TilemapComponents;
using MatrixEngine.StateManagment;
using Sandbox.Game.Components.Maps;
using Sandbox.Game.Items.Inventories;
using SFML.Window;

namespace Sandbox.Game.Components.Entities {

    public class PlayerComponent : EntityComponent {
        public readonly float speed = 8f;

        public StateProvider<Inventory> InventoryProvider
        {
            get;
            private set;
        }

        public readonly float JumpPower = -20;

        private SpriteRendererComponent spriteRendererComponent;

        private Provider<PointerMapComponent> pointerProv;

        private Provider<GenerationMapComponent> mapProv;

        public PlayerComponent(Provider<PointerMapComponent> pointerProv, Provider<GenerationMapComponent> mapProv, StateProvider<Inventory> invenProv)
        {
            this.pointerProv = pointerProv;
            this.mapProv = mapProv;
            this.InventoryProvider = invenProv;
        }

        public override void Start()
        {
            spriteRendererComponent = SetComponent<SpriteRendererComponent>();
            spriteRendererComponent.SetTexture("Assets/Sprites/Player.png", 16);
            RigidBodyComponent = SetComponent(new RigidBodyComponent(new SFML.System.Vector2f(0, 60), new SFML.System.Vector2f(50, 0)));
            ColliderComponent = SetComponent(new ColliderComponent(ColliderComponent.ColliderType.Rect));
        }

        public override void Update()
        {
            if (InputHandler.IsPressed(Keyboard.Key.Add)) {
                App.Camera.zoom += 1 * App.DeltaTimeAsSeconds;
            }
            if (InputHandler.IsPressed(Keyboard.Key.Subtract)) {
                App.Camera.zoom -= 1 * App.DeltaTimeAsSeconds;
            }

            App.Camera.position = Transform.fullRect.center;
            if (InputHandler.IsPressed(SFML.Window.Keyboard.Key.D)) {
                MoveVerticly(speed);
            }
            if (InputHandler.IsPressed(SFML.Window.Keyboard.Key.A)) {
                MoveVerticly(-speed);
            }
            if (InputHandler.IsPressed(Keyboard.Key.W, Keyboard.Key.Space)) {
                Jump(JumpPower);
            }
            if (InputHandler.IsPressed(Mouse.Button.Left)) {
                var g = mapProv.Get();
                var p = pointerProv.Get();
                if (p == null || g == null) {
                }
                else {
                    g.Damage(p.PointerPosition, 1 * App.DeltaTimeAsSeconds);

                    //g.Tilemap.SetTile(p.PointerPosition, null);
                }
            }
            if (InputHandler.IsPressed(Mouse.Button.Right)) {
                var g = mapProv.Get();
                var p = pointerProv.Get();
                if (p == null || g == null) {
                }
                else {
                    g.Tilemap.SetTile(p.PointerPosition, g.items["grass"].ToBlock());
                }
            }
        }
    }
}