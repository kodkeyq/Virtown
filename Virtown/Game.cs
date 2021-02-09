using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VirtownShared.Atlas;
using VirtownShared.Entities;
using VirtownShared.Global;

namespace Virtown
{
    public partial class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Entity en;
        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            EntitiesStorage.Begin(GraphicsDevice, _spriteBatch, Content);
            EntitiesStorage.NewEntityData("cabinet1", 1, 3, 4, EntityTypeEnum.Furniture, "furniture", 16, 64);
            EntitiesStorage.End();
            en = new Entity(EntitiesStorage.GetEntityData("cabinet1"), new Point(10, 10), DirectionEnum.PlusX);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(EntitiesStorage.Atlas2D, new Vector2(0, 0), Color.White);

            for (int i = 0; i < en.IsoDirectionSize.X; i++ )
            {
                for (int j = 0; j < en.IsoDirectionSize.Y; j++)
                {
                        Vector2 position = Isometric.IsoToCart(new Point(i, j)).ToVector2();

                        Rectangle srcRect = en.GetSprite(new Point(i, j));
                    _spriteBatch.Draw(EntitiesStorage.Atlas2D, position + new Vector2(100, 100), srcRect, Color.White);
                    
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
