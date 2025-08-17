using Microsoft.Xna.Framework;
using Monocle;

namespace Example
{
    public class GameScene : Scene
    {
        private Player player;
        private Text scoreText;
        private Text instructionText1;
        private Text instructionText2;
        
        public override void Begin()
        {
            base.Begin();
            
            // Add renderer so entities actually get rendered
            RendererList.Add(new EverythingRenderer());
            
            // Add player
            player = new Player(new Vector2(Engine.Width / 2, Engine.Height / 2));
            Add(player);
            
            // Add initial collectibles
            for (int i = 0; i < 3; i++)
            {
                SpawnCollectible();
            }
            
            // Add some enemies
            for (int i = 0; i < 2; i++)
            {
                SpawnEnemy();
            }
            
            // Add UI text
            var scoreEntity = new Entity(new Vector2(10, 10));
            scoreText = new Text(Draw.DefaultFont, "Score: 0", Vector2.Zero, Color.White, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
            scoreEntity.Add(scoreText);
            Add(scoreEntity);
            
            var instructionEntity1 = new Entity(new Vector2(10, Engine.Height - 30));
            instructionText1 = new Text(Draw.DefaultFont, "WASD/Arrows to move", Vector2.Zero, Color.Gray, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
            instructionEntity1.Add(instructionText1);
            Add(instructionEntity1);
            
            var instructionEntity2 = new Entity(new Vector2(10, Engine.Height - 50));
            instructionText2 = new Text(Draw.DefaultFont, "Collect green dots, avoid red ones!", Vector2.Zero, Color.Gray, Text.HorizontalAlign.Left, Text.VerticalAlign.Top);
            instructionEntity2.Add(instructionText2);
            Add(instructionEntity2);
            
        }
        
        public void SpawnCollectible()
        {
            Vector2 position;
            int attempts = 0;
            
            do
            {
                position = new Vector2(
                    Calc.Random.Range(20, Engine.Width - 20),
                    Calc.Random.Range(20, Engine.Height - 20)
                );
                attempts++;
            }
            while (attempts < 10 && Vector2.Distance(position, player.Position) < 40);
            
            Add(new Collectible(position));
        }
        
        private void SpawnEnemy()
        {
            Vector2 position;
            int attempts = 0;
            
            do
            {
                position = new Vector2(
                    Calc.Random.Range(20, Engine.Width - 20),
                    Calc.Random.Range(20, Engine.Height - 20)
                );
                attempts++;
            }
            while (attempts < 10 && Vector2.Distance(position, player.Position) < 60);
            
            Add(new Enemy(position));
        }

        public override void Update()
        {
            base.Update();
            
            // Update score text
            if (scoreText != null)
            {
                scoreText.DrawText = $"Score: {player.Score}";
            }
            
        }
    }
}