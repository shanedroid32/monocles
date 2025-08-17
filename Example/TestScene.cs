using Microsoft.Xna.Framework;
using Monocle;

namespace Example
{
    public class TestScene : Scene
    {
        public override void Begin()
        {
            base.Begin();
            
            // Add a simple test entity
            Add(new TestEntity(new Vector2(160, 90)));
        }
    }
}