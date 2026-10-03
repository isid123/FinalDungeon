using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace FinalDungeon;

public class Game1 : Core
{
    private const float GRAVITY = 1600f;
    private const float JUMP_FORCE = -600f;
    private const float GROUND_Y = 500f;

    private AnimatedSprite _hero;
    private Vector2 _heroPosition = new Vector2(100f, GROUND_Y);
    private const float MOVEMENT_SPEED = 5.0f;
    private float _verticalVelocity;
    private bool _isJumping;

    private Animation _standAnimation;
    private Animation _jumpAnimation;

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
        _standAnimation = heroAtlas.GetAnimation("hero-stand-animation");
        _jumpAnimation = heroAtlas.GetAnimation("hero-jump-animation");

        _hero = new AnimatedSprite(_standAnimation);
        _hero.Scale = new Vector2(4.0f, 4.0f);
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _hero.Update(gameTime);

        CheckKeyboardInput();
        UpdateJump(gameTime);
    }

    private void UpdateJump(GameTime gameTime)
    {
        if (!_isJumping) return;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _verticalVelocity += GRAVITY * deltaTime;

        _heroPosition.Y += _verticalVelocity * deltaTime;

        if (_heroPosition.Y >= GROUND_Y && _verticalVelocity > 0f)
        {
            _heroPosition.Y = GROUND_Y;
            _verticalVelocity = 0f;
            _isJumping = false;
            _hero.Animation = _standAnimation;
        }
    }

    private void CheckKeyboardInput()
    {
        float speed = MOVEMENT_SPEED;
        if (Input.Keyboard.WasKeyJustPressed(Keys.Space) && !_isJumping)
        {
            _isJumping = true;
            _verticalVelocity = JUMP_FORCE;
            _hero.Animation = _jumpAnimation;
        }

        if (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
        {
            _heroPosition.X -= speed;
            _hero.Effects = SpriteEffects.FlipHorizontally;

        }

        if (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
        {
            _heroPosition.X += speed;
            _hero.Effects = SpriteEffects.None;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _hero.Draw(SpriteBatch, _heroPosition);

        SpriteBatch.End();
        base.Draw(gameTime);
    }
}
