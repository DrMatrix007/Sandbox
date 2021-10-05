﻿using MatrixEngine.GameObjects.Components;
using MatrixEngine.GameObjects.Components.RenderComponents;
using MatrixEngine.GameObjects.Components.PhysicsComponents;
using Sandbox.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Sandbox.Game.Components.Maps {

    [RequireComponent(typeof(SpriteRendererComponent))]
    [RequireComponent(typeof(RigidBodyComponent))]
    [RequireComponent(typeof(ColliderComponent))]
    public class ItemDropComponent : Component {
        private Item i;

        private Random random = new Random();

        public ItemDropComponent(Item i)
        {
            this.i = i;
        }

        public override void Start()
        {
            GetComponent<SpriteRendererComponent>().SetTexture(i.texture, (int)i.texture.Size.X * 2);
            SetComponent<RigidBodyComponent>(new RigidBodyComponent(new Vector2f(0, 30), new Vector2f(10 + random.Next(-5, 6), 20 + random.Next(-5, 6))));
            GetComponent<RigidBodyComponent>().Velocity = new Vector2f(random.Next(-1, 2), random.Next(-1, 2)) * 4;
        }

        public override void Update()
        {
            if (RigidBodyComponent.TouchDown) {
                //RigidBodyComponent.isStatic = true;
            }
        }
    }
}