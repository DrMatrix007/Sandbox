using MatrixEngine.Content;
using MatrixEngine.Framework;
using Sandbox.Game.Items;
using Sandbox.Game.Worlds;

namespace Sandbox {

    internal class Program {

        private static void Main(string[] _) {
            var itemLibrary = new ItemLibrary(
                new Item("grass", TextureManager.GetTexture("Assets/Items/grass.png"), ItemType.Block, durability: 0.0f));

            var app = new App("Sandbox", false, new World(itemLibrary));

            app.Run();
        }
    }
}