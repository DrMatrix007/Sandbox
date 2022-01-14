using SFML.Graphics;
using System;

namespace Sandbox.Items {

    public enum ItemType {
        Tool,
        Block,
        Item,
    }

    public enum ToolType {
        Shovel,
        Pickaxe,
        Axe,
    }

    public sealed class Item {
        public ItemType itemType;

        public ToolType[] toolTypes;

        public readonly string id;

        public readonly float durability;

        public readonly Texture texture;

        public readonly float miningSpeed;

        public readonly int miningLevel;

        public Block ToBlock() {
            if (itemType != ItemType.Block) {
                throw new Exception($"Item {id} is not a block");
            }
            return new Block(this);
        }

        public Item(string id, Texture texture, ItemType itemType = ItemType.Item, ToolType[] toolTypes = null, float durability = float.PositiveInfinity, float miningSpeed = 1, int miningLevel = 0) {
            this.itemType = itemType;
            this.toolTypes = toolTypes ?? Array.Empty<ToolType>();
            this.id = id;
            this.durability = durability;
            this.texture = texture;
            this.miningSpeed = miningSpeed;
            this.miningLevel = miningLevel;
        }
    }
}