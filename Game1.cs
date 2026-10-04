using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalDungeon;

public class Game1 : Core
{
    private Hero _hero;
    public Game1() : base("Final Dungeon", 1280, 720, false)
    {
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
    }

    protected override void LoadContent()
    {
        TextureAtlas heroAtlas = TextureAtlas.FromFile(Content, "images/hero.xml");
        _hero = new Hero(heroAtlas);
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _hero.Update(gameTime, Input.Keyboard, Input.Mouse);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _hero.Draw(SpriteBatch);

        SpriteBatch.End();
        base.Draw(gameTime);
    }
}
