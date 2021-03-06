using SFML.Graphics;

namespace Sandbox.Items {

    public class Block : Tile {
        private float BlockHP;

        private readonly float MaxHP;

        public readonly Item Item;

        public Block(Item item) : base(item.texture) {
            BlockHP = item.durability;
            MaxHP = BlockHP;
            Item = item;
        }

        public bool Damage(float val) {
            BlockHP -= val;
            Color = new Color((byte)(255 * (BlockHP / MaxHP)), (byte)(255 * (BlockHP / MaxHP)), (byte)(255 * (BlockHP / MaxHP)), (byte)(255 * (BlockHP / MaxHP)));
            return BlockHP <= 0;
        }
    }
}