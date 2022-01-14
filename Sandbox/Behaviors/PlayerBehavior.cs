
namespace Sandbox.Behaviors;

public class PlayerBehavior : Behavior
{
    private const float JUMP_TOLERANCE = 0.01f;
    public float Speed { get; private set; }

    public bool AllowJump { get; private set; } = true;

    public PlayerBehavior(float speed = 5)
    {
        Speed = speed;
    }

    public override void Dispose()
    {
    }

    protected override void OnStart()
    {

    }

    protected override void OnUpdate()
    {
        var inputHandler = GetEngine().InputHandler;
        var rigidbodyBehavior = GetBehaviorOrException<DynamicRigidbodyBehavior>();
        Vector2f vel = new Vector2f();
        var rectBehavior = GetBehaviorOrException<RectBehavior>();
        var renderer = GetScene().GetPlugin<RendererPlugin>();

        if(inputHandler.IsPressed(Keyboard.Key.D))
        {
            vel.X += Speed;
        }
        if(inputHandler.IsPressed(Keyboard.Key.A))
        {
            vel.X -= Speed;
        }
        if(rigidbodyBehavior.Velocity.Y > 0)
        {
            AllowJump = true;
        }
        if(inputHandler.IsPressed(Keyboard.Key.W) && rigidbodyBehavior.IsTouchingDown)
        {
            vel.Y = -20;
            AllowJump = false;
        }
        
        
        rigidbodyBehavior.Velocity.X = vel.X.Abs()>= rigidbodyBehavior.Velocity.X .Abs() || (vel.X.Sign()!=rigidbodyBehavior.Velocity.X.Sign() && vel.X.Sign()!=0)? vel.X: rigidbodyBehavior.Velocity.X;
        rigidbodyBehavior.Velocity.Y = vel.Y != 0 ? vel.Y : rigidbodyBehavior.Velocity.Y;
        renderer?.Camera.SetPosition(rectBehavior.Position);
        renderer.Camera.SetArea(MathF.Pow(2,inputHandler.ScrollY));


    }
}
