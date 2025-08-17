using Microsoft.Xna.Framework;
using Monocle;

namespace Example
{
    public class TestEntity : Entity
    {
        private float timer;
        
        public TestEntity(Vector2 position) : base(position)
        {
            timer = 0f;
        }

        public override void Update()
        {
            base.Update();
            
            timer += Engine.DeltaTime;
            
            // Simple movement
            Position.X = 160 + (float)System.Math.Sin(timer) * 50;
            Position.Y = 90 + (float)System.Math.Cos(timer * 0.7f) * 30;
        }

        public override void Render()
        {
            // Draw a simple rectangle
            Draw.Rect(Position.X - 10, Position.Y - 10, 20, 20, Color.White);
        }
    }
}