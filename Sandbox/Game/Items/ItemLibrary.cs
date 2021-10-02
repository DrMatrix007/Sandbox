using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox.Game.Items {

    public sealed class ItemLibrary : IEnumerable<Item> {
        private readonly List<Item> _items = new();

        public Item[] Items => _items.ToArray();

        public Item GetItem(string id) {
            return _items.FirstOrDefault(e => e.id == id);
        }

        public void AddItemsToLibrary(params Item[] items) {
            foreach (var item in items) {
                AddItemToLibrary(item);
            }
        }

        public void AddItemsToLibrary(IEnumerable<Item> items) {
            foreach (var item in items) {
                AddItemToLibrary(item);
            }
        }

        public ItemLibrary(params Item[] items) {
            AddItemsToLibrary(items);
        }

        public ItemLibrary(IEnumerable<Item> items) {
            AddItemsToLibrary(items);
        }

        public Item this[string id]
        {
            get => GetItem(id);
        }

        public Item AddItemToLibrary(Item item) {
            if (GetItem(item.id) == null) {
                _items.Add(item);
            } else {
                throw new System.Exception($"There already is a block with id of {item.id}");
            }
            return item;
        }

        public IEnumerator<Item> GetEnumerator() {
            foreach (var item in _items) {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}