using MatrixEngine.Content;
using MatrixEngine.Framework;
using MatrixEngine.Scenes.Plugins;
using MatrixEngine.StateManagment;
using MatrixEngine.UI;
using Sandbox.Game.Items.Inventories;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sandbox.Game.Plugins {
    internal class InventoryUIControllerPlugin : Plugin {


        private Provider<Inventory> inventoryProvider;

        private List<TextConsumerUIObject> textsList;

        public InventoryUIControllerPlugin(Provider<Inventory> inventoryProvider) {
            this.inventoryProvider = inventoryProvider;
        }

        public override void LateUpdate() {
        }

        public override void Start() {

            var poslist = new List<Vector2f>();


            var i = inventoryProvider.Get();

            if (i == null) {
                throw new NullReferenceException($"inventory provider refrence to null");
            }

            var maxs = (int)(i.itemStacks.Length.Sqrt() + 1);

            for (int y = 0; y < maxs; y++) {
                for (int x = 0; x < maxs; x++) {
                    poslist.Add(new Vector2f(x, y) * (100.0f / maxs));
                }
            }






            textsList = new List<TextConsumerUIObject>();
            var c = 0;
            foreach (var item in i.itemStacks) {
                var t = new TextConsumerUIObject(new Anchor(poslist[c], new Vector2f(50 / maxs, 50 / maxs)),
                    new FunctionProvider<string>(() => {
                        return item.ToString();
                    }),
                    new UITextStyle(10, Color.White, Color.Black, FontManager.CascadiaCode, isResize: true));
                scene.AddUIObject(t);
                textsList.Add(t);
                c++;
            }

        }

        public void SetActive(bool active) {

        }

        public override void Update() {
            if (InputHandler.IsPressedDown(SFML.Window.Keyboard.Key.Tab)) {
                Task.Run(() => {
                    foreach (var item in textsList.ToArray()) {
                        item.IsActive = !item.IsActive;
                        //Console.WriteLine(item.IsActive);
                    }
                });
            }
        }
    }
}
