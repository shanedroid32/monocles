using Microsoft.Xna.Framework;
using Monocle;

namespace Example
{
    public class ExampleGame : Engine
    {
        public ExampleGame() : base(640, 360, 1280, 720, "Monocle Example", false)
        {
            ClearColor = Color.Black;
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            // Start with the game scene
            Scene = new GameScene();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
            // Draw system initialized successfully
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}