using MatrixEngine.GameObjects.Components.TilemapComponents;
using SFML.Graphics;

namespace Sandbox.Game.Items {

    public class Block : Tile {
        private float BlockHP;

        private readonly float MaxHP;

        public Block(Item item) : base(item.texture) {
            BlockHP = item.durability;
            MaxHP = BlockHP;
        }

        public bool Damage(float val) {
            BlockHP -= val;
            color = new Color((byte)(255 * (BlockHP / MaxHP)), (byte)(255 * (BlockHP / MaxHP)), (byte)(255 * (BlockHP / MaxHP)), (byte)(255 * (BlockHP / MaxHP)));
            return BlockHP <= 0;
        }
    }
}