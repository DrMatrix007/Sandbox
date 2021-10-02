using MatrixEngine.GameObjects.Components;
using MatrixEngine.GameObjects.Components.RenderComponents;
using MatrixEngine.GameObjects.Components.TilemapComponents;
using MatrixEngine.StateManagment;
using Sandbox.Game.Components.Entities;
using SFML.System;
using MatrixEngine.Framework;
using SFML.Graphics;

namespace Sandbox.Game.Components.Maps {

    [RequireComponent(typeof(TilemapComponent))]
    [RequireComponent(typeof(TilemapRendererComponent))]
    public class PointerMapComponent : Component {
        private TilemapComponent tilemap;

        public readonly Tile Pointer;

        private readonly Provider<PlayerComponent> playerP;

        public Vector2i PointerPosition { get; private set; }

        public PointerMapComponent(Tile tile, Provider<PlayerComponent> provider) {
            Pointer = tile;
            playerP = provider;
        }

        public override void Start() {
            tilemap = GetComponent<TilemapComponent>();
            GetComponent<TilemapRendererComponent>().layer = 1000;
        }

        public override void Update() {
            tilemap.Clear();

            var p = playerP.Get();

            if (p == null) {
                return;
            }

            var poi = InputHandler.GetMouseWorldPos();
            //poi.X = -poi.X;
            var ppos = p.Transform.fullRect.center;
            //ppos.X = -ppos.X;
            var pos = PhysicsEngine.LineCast(new(ppos, poi));

            var line = new VertexArray(PrimitiveType.Lines);
            line.Append(new Vertex(ppos) { Color = Color.Black });
            line.Append(new Vertex(poi) { Color = Color.Black });

            App.Window.Draw(line);

            line.Dispose();

            line = new VertexArray(PrimitiveType.Lines);
            line.Append(new Vertex(ppos) { Color = Color.Red });
            line.Append(new Vertex(pos) { Color = Color.Red });

            App.Window.Draw(line);

            line.Dispose();

            PointerPosition = tilemap.GetPosOfTileFromWorldPos(pos - (ppos - pos).Normalize() / 1000);

            tilemap.SetTile(PointerPosition, Pointer);
        }
    }
}