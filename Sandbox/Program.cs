global using MatrixEngine;
global using MatrixEngine.Plugins;
global using MatrixEngine.Behaviors;
global using MatrixEngine.Behaviors.PhysicsBehaviors;
global using MatrixEngine.Behaviors.RendererBehaviors;
global using MatrixEngine.Utils;
global using MatrixEngine.MatrixMath;
global using SFML.System;
global using SFML.Window;
global using SFML.Graphics;
global using Sandbox.Behaviors;
global using System;
using Sandbox.Items;

WindowSettings window = new WindowSettings()
{
    Name = "Sandbox",
    Size = new(1000, 500)
};

var itemLibrary = new ItemLibrary(
    new Item("Grass", new Texture("Assets/Items/grass.png"), ItemType.Block, durability: 1),
    new Item("Stone", new Texture("Assets/Items/stone.png"), ItemType.Block, durability: 2),
    new Item("Dirt", new Texture("Assets/Items/dirt.png"), ItemType.Item, durability: 1)
);

var scene = new Scene();

scene.AddPlugin(new RendererPlugin(new Camera(MathF.Pow(2,24)), new Color(0x69696969)));

scene.AddPlugin(new PhysicsPlugin());







var map = new Actor(
    new TilemapBehavior(),
    new TilemapRendererBehavior(10, 16),
    new GenerationBehavior(itemLibrary, new GenerationConfig(0x54d5, 20,200, new GenerationLayer[] {
        new GenerationLayer(new(1,1),"Grass",0.0f),
        new GenerationLayer(new(10,15),"Dirt",0.05f),
        new GenerationLayer(new(20,40),"Stone",0.25f),
        new GenerationLayer(new(20,40),"Stone",0.5f),
        new GenerationLayer(new(20,40),"Stone",0.10f),
        new GenerationLayer(new(20,40),"Stone",0.05f),

    }),null),
    new TilemapStaticRigidbodyBehavior(new Vector2f(40,0))
);

scene.AddActor(map);



var engine = new Engine(window, scene);




engine.Run();