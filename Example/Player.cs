using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;

namespace Example
{
    [Tracked]
    public class Player : Entity
    {
        private float speed = 120f;
        private int score = 0;
        
        public int Score { get { return score; } }
        
        public Player(Vector2 position) : base(position)
        {
            Collider = new Hitbox(16, 16, -8, -8);
        }

        public override void Update()
        {
            base.Update();
            
            Vector2 movement = Vector2.Zero;
            
            // Keyboard controls
            if (MInput.Keyboard.Check(Keys.Left) || MInput.Keyboard.Check(Keys.A))
                movement.X -= 1;
            if (MInput.Keyboard.Check(Keys.Right) || MInput.Keyboard.Check(Keys.D))
                movement.X += 1;
            if (MInput.Keyboard.Check(Keys.Up) || MInput.Keyboard.Check(Keys.W))
                movement.Y -= 1;
            if (MInput.Keyboard.Check(Keys.Down) || MInput.Keyboard.Check(Keys.S))
                movement.Y += 1;
            
            // Normalize diagonal movement
            if (movement != Vector2.Zero)
                movement.Normalize();
            
            Position += movement * speed * Engine.DeltaTime;
            
            // Keep player on screen
            Position.X = Calc.Clamp(Position.X, 8, Engine.Width - 8);
            Position.Y = Calc.Clamp(Position.Y, 8, Engine.Height - 8);
            
            // Check for collectibles
            var collectible = CollideFirst<Collectible>();
            if (collectible != null)
            {
                score += 10;
                collectible.Collect();
            }
            
            // Check for enemies
            if (CollideCheck<Enemy>())
            {
                // Game over - restart
                score = 0;
                Position = new Vector2(Engine.Width / 2, Engine.Height / 2);
            }
        }

        public override void Render()
        {
            // Draw a simple blue square
            Draw.Rect(Position.X - 8, Position.Y - 8, 16, 16, Color.Blue);
        }
    }
}