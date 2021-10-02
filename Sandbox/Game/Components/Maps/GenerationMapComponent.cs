using MatrixEngine.GameObjects;
using MatrixEngine.GameObjects.Components;
using MatrixEngine.GameObjects.Components.PhysicsComponents;
using MatrixEngine.GameObjects.Components.RenderComponents;
using MatrixEngine.GameObjects.Components.TilemapComponents;
using MatrixEngine.StateManagment;
using MatrixEngine.Utilities;
using Sandbox.Game.Components.Entities;
using Sandbox.Game.Items;
using SFML.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Game.Components.Maps {

    [RequireComponent(typeof(TilemapComponent))]
    [RequireComponent(typeof(TilemapRendererComponent))]
    public class GenerationMapComponent : Component {
        public PerlinNoise1D perl;

        private TilemapComponent tilemap;

        public ItemLibrary items;

        private readonly Provider<PlayerComponent> playerProv;

        public TilemapComponent Tilemap
        {
            get => tilemap;
        }

        public GenerationMapComponent(ItemLibrary items, Provider<PlayerComponent> prov) {
            this.items = items;
            playerProv = prov;
        }

        public override void Setup() {
            base.Setup();

            SetComponent<RigidBodyComponent>(new RigidBodyComponent(true));
            SetComponent<ColliderComponent>(new ColliderComponent(ColliderComponent.ColliderType.Tilemap));
        }

        public override void Start() {
            tilemap = GetComponent<TilemapComponent>();

            tilemap.pixelsPerUnit = 16;

            perl = new PerlinNoise1D(10000, 10, new MatrixEngine.Utilities.Range(30, 50));
            perl.Generate();

            OperationManager.AddOperation(Generate().ToOperation());
        }

        public IEnumerator Generate() {
            var w = perl.size / 2;

            for (int i = -w; i < w; i++) {
                var v = perl[w + i];
                for (int y = 0; y < v; y++) {
                    tilemap.SetTile(i, -y, items["grass"].ToBlock());
                }
                if (i == 0) {
                    Console.WriteLine('?');

                    var p = playerProv.Get();

                    p.Transform.position = new Vector2f(i, -v - 10);
                    p.GetComponent<RigidBodyComponent>().Velocity = new Vector2f();
                }
            }
            yield return null;
        }

        public override void Update() {
        }

        public void Damage(Vector2i pos, float d) {
            var b = tilemap.GetTileFromTilemapPos<Block>(pos);

            if (b == null) {
                return;
            }

            tilemap.SetTile(pos, b);
            if (b.Damage(d)) {
                var g = new GameObject(tilemap.GetWorldPosFromTilePos(pos), new ItemDropComponent(b.Item));
                this.Scene.AddGameObject(g);
                tilemap.SetTile(pos, null);
            }
        }
    }
}