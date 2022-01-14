using Sandbox.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Behaviors;

public class GenerationLayer
{

    public readonly MatrixRange Range;
    public readonly string item;
    public readonly float PerlinNoiseStrength;
    public GenerationLayer(MatrixRange range, string item, float perlinNoiseStrength = 0.0f)
    {
        Range = range;
        this.item = item;
        PerlinNoiseStrength = perlinNoiseStrength;

    }
}


public struct GenerationConfig
{
    public readonly int Seed;

    public readonly uint PerlinPartSize;
    public readonly uint Perlincount;

    //public readonly string Ground;
    public readonly GenerationLayer[] GenerationLayers;

    public GenerationConfig(int seed, uint perlinPartSize, uint perlinCount, GenerationLayer[] underGroundLayers)
    {
        Seed = seed;
        PerlinPartSize = perlinPartSize;
        Perlincount = perlinCount;

        GenerationLayers = underGroundLayers;
    }
}

class GenerationBehavior : Behavior
{

    public readonly ItemLibrary items;
    public readonly GenerationConfig config;

    private PlayerBehavior playerBehavior;

    public readonly Random random;

    public GenerationBehavior(ItemLibrary items, GenerationConfig config, PlayerBehavior playerBehavior)
    {
        this.items = items;
        this.config = config;

        random = new Random(config.Seed);

        this.playerBehavior = playerBehavior;

    }

    public override void Dispose()
    {
    }

    protected override void OnStart()
    {
        GetEngine().Operations.AddOperation(new Operation(Generate()));
    }

    public IEnumerator Generate()
    {
        TilemapBehavior tilemap = GetBehaviorOrException<TilemapBehavior>();
        Dictionary<GenerationLayer, PerlinNoise1D> values = new();
        foreach(var layer in config.GenerationLayers)
        {
            values[layer] = new PerlinNoise1D(config.Perlincount, config.PerlinPartSize, layer.Range, random.Next());
            values[layer].Generate();
        }
        var maxHeight = (uint)(values.Select(a => a.Key.Range.max).Aggregate((a, b) => (uint)(int)(a + b)));
        PerlinNoise2D noise = new PerlinNoise2D(new Vector2u((uint)(config.PerlinPartSize * config.Perlincount) / 10, maxHeight),
             config.PerlinPartSize, MatrixRange.ZeroToOne, random.Next()
         );


        Task generate = new Task(() => noise.Generate());

        generate.Start();

        while(!generate.IsCompleted)
        {
            yield return null;
        }
        var maxCount = 100;
        var counter = 0;
        var layers = values.Reverse().ToArray();
        var size = config.PerlinPartSize * config.Perlincount;
        KeyValuePair<GenerationLayer, PerlinNoise1D> currentLayer;
        KeyValuePair<GenerationLayer, PerlinNoise1D> lastLayer;
        KeyValuePair<GenerationLayer, PerlinNoise1D> nextLayer;

        for(int i = 0; i < size; i++)
        {
            if(counter > maxCount)
            {
                counter = 0;
                yield return null;
            }
            counter++;
            var y = 0;

            for(int pos = 0; pos < layers.Count(); pos++)
            {
                lastLayer = layers[Math.Max(pos - 1, 0)];
                currentLayer = layers[pos];
                nextLayer = layers[Math.Min(pos + 1, layers.Count() - 1)];
                for(int j = 0; j <= currentLayer.Value[i]; j++)
                {
                    var lerpa = MatrixMath.CubicLerp(nextLayer.Key.PerlinNoiseStrength, currentLayer.Key.PerlinNoiseStrength, (float)(j) / currentLayer.Value[i]);
                    var lerpb = MatrixMath.CubicLerp(currentLayer.Key.PerlinNoiseStrength, lastLayer.Key.PerlinNoiseStrength, (float)(j) / currentLayer.Value[i]);
                    var lerpab = MatrixMath.CubicLerp(lerpa, lerpb,(float)(j)/ currentLayer.Value[i]);
                    if(noise.floats[i, -y] < 1 - lerpab)
                    {
                        var b = new Block(items.GetItem(currentLayer.Key.item));
                        b.Color = new Color((byte)(255*(lerpb)));
                        tilemap.SetTile(new((int)(i - size / 2), y), b);
                    }
                    y--;
                }
            }
            //tilemap.SetTile(new((int)(i - size / 2), y), new Block(items.GetItem(config.Ground)));


        }
        yield return null;

        var player = new Actor(
          new RectBehavior(new FloatRect(0, -50, 0.9f, 0.9f * 30 / 14)),
          new PlayerBehavior(10),
          new DynamicRigidbodyBehavior(new Vector2f(0, 50), new Vector2f(50, 0)),
          new SpriteBehavior(new Texture("Assets/Sprites/Player.png"), 0)
        );

        GetScene().AddActor(player);
        playerBehavior = player.GetBehavior<PlayerBehavior>();
        playerBehavior.GetBehavior<RectBehavior>().Position = new Vector2f(0, -100);
        playerBehavior.GetBehavior<DynamicRigidbodyBehavior>().Velocity = new Vector2f();
    }

    protected override void OnUpdate()
    {
    }
}
