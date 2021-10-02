using MatrixEngine.GameObjects.Components;
using MatrixEngine.GameObjects.Components.PhysicsComponents;

namespace Sandbox.Game.Components.Entities {

    [RequireComponent(typeof(RigidBodyComponent))]
    [RequireComponent(typeof(ColliderComponent))]
    public abstract class EntityComponent : Component {

        protected new RigidBodyComponent RigidBodyComponent
        {
            set;
            get;
        }

        protected new ColliderComponent ColliderComponent
        {
            set;
            get;
        }

        protected void MoveVerticly(float v) {
            var vel = RigidBodyComponent.Velocity;
            //if (rigidBodyComponent.touchDown || v.Sign() == vel.X.Sign()) {
            vel.X = v;
            RigidBodyComponent.Velocity = vel;
            //}
        }

        protected void Jump(float power) {
            if (RigidBodyComponent.TouchDown) {
                var vel = RigidBodyComponent.Velocity;
                vel.Y = power;
                RigidBodyComponent.Velocity = vel;
            }
        }

        public override void Start() {
            RigidBodyComponent = GetComponent<RigidBodyComponent>();
            ColliderComponent = GetComponent<ColliderComponent>();
        }
    }
}