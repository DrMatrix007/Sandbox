using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Items.Inventories {
    public class Inventory {

        public ItemStack[] itemStacks;

        public Inventory(int itemStackSize) {
            itemStacks = new ItemStack[itemStackSize];
            for (int i = 0; i < itemStacks.Length; i++) {
                itemStacks[i] = new ItemStack();
            }
        }

        public bool Add(Item i) {

            foreach (var stack in itemStacks) {
                if(stack.AvailableFor(i)) {
                    stack.Add(i);
                    return true;
                }
            }
            
            return false;
        }
        public bool Remove(Item i) {
            foreach (var item in itemStacks) {
                if(item.AvailableFor(i)) {
                    item.Remove(i);
                    return true;
                }
            }
            return false;
        }


    }
}
