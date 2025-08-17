using Microsoft.Xna.Framework;
using Monocle;

namespace Example
{
    [Tracked]
    public class Collectible : Entity
    {
        private float floatAmount = 0f;
        private float baseY;
        
        public Collectible(Vector2 position) : base(position)
        {
            Collider = new Hitbox(12, 12, -6, -6);
            baseY = position.Y;
        }

        public override void Update()
        {
            base.Update();
            
            floatAmount += Engine.DeltaTime * 3f;
            Position.Y = baseY + (float)System.Math.Sin(floatAmount) * 2f;
        }

        public void Collect()
        {
            // Spawn a new collectible at random position
            var scene = Scene as GameScene;
            if (scene != null)
            {
                scene.SpawnCollectible();
            }
            
            RemoveSelf();
        }

        public override void Render()
        {
            // Draw a simple green square
            Draw.Rect(Position.X - 6, Position.Y - 6, 12, 12, Color.Green);
        }
    }
}