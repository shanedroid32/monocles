using Microsoft.Xna.Framework;
using Monocle;

namespace Example
{
    [Tracked]
    public class Enemy : Entity
    {
        private float speed;
        private Vector2 direction;
        private float changeDirectionTimer;
        
        public Enemy(Vector2 position) : base(position)
        {
            Collider = new Hitbox(14, 14, -7, -7);
            speed = Calc.Random.Range(30f, 60f);
            direction = Calc.AngleToVector(Calc.Random.NextFloat() * MathHelper.TwoPi, 1f);
            changeDirectionTimer = Calc.Random.Range(1f, 3f);
        }

        public override void Update()
        {
            base.Update();
            
            changeDirectionTimer -= Engine.DeltaTime;
            if (changeDirectionTimer <= 0)
            {
                // Change direction randomly
                direction = Calc.AngleToVector(Calc.Random.NextFloat() * MathHelper.TwoPi, 1f);
                changeDirectionTimer = Calc.Random.Range(1f, 3f);
            }
            
            Position += direction * speed * Engine.DeltaTime;
            
            // Bounce off screen edges
            if (Position.X <= 7 || Position.X >= Engine.Width - 7)
            {
                direction.X = -direction.X;
                Position.X = Calc.Clamp(Position.X, 7, Engine.Width - 7);
            }
            if (Position.Y <= 7 || Position.Y >= Engine.Height - 7)
            {
                direction.Y = -direction.Y;
                Position.Y = Calc.Clamp(Position.Y, 7, Engine.Height - 7);
            }
        }

        public override void Render()
        {
            // Draw a simple red square
            Draw.Rect(Position.X - 7, Position.Y - 7, 14, 14, Color.Red);
        }
    }
}