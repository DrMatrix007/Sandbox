using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Items.Inventories {
    public class ItemStack {
        public ItemStack() {  
        }

        public override string ToString() {
            return $"ID:{ItemID}, Count:{Count}";
        }
        public bool AvailableFor(String otherID) {
            return _itemID == "" || _itemID == otherID;
        }

        public void Add(Item item) {
            if (!AvailableFor(item)) {
                throw new ArgumentException($"{item.id} is not the same as {_itemID}");
            }
            if (_itemID == "") {
                this.item = item;
                _count = 0;
            }
            _count++;
        }
        public void Remove() {
            _count--;
            if (_count <= 0) {
                item = null;
                _count = 0;
            }
        }
        public void Remove(Item item) {
            if (!AvailableFor(item)) {
                throw new ArgumentException($"{item.id} is not the same as {_itemID}");
            }
            Remove();
        }

        public bool AvailableFor(Item i) {
            return AvailableFor(i.id);
        }

        private Item item;

        private string _itemID =>item?.id??"";

        public string ItemID => _itemID;
        


        private int _count = 0;

        public int Count => _count;

    }
}
