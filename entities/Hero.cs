using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Hero
{
    private readonly AnimatedSprite _hero;
    private const float GRAVITY = 1600f;
    private const float JUMP_FORCE = -600f;
    private const float GROUND_Y = 500f;
    private const float MOVEMENT_SPEED = 400f;
    private Vector2 _position;
    private float _verticalVelocity;
    private float _movementDirection;
    private bool _isJumping;
    private bool _isAttacking;
    private Animation _currentAnimation;
    private Animation _standAnimation;
    private Animation _jumpAnimation;
    private Animation _walkAnimation;
    private Animation _attackAnimation;
    private Animation _deathAnimation;


    public Hero(TextureAtlas atlas)
    {
        LoadAnimationsFromXML(atlas);

        // default animation
        _hero = new AnimatedSprite(_standAnimation);
        _hero.Scale = new Vector2(4f, 4f);

        _position = new Vector2(100f, GROUND_Y);
    }

    public void Update(GameTime gameTime, KeyboardInfo keyboard, MouseInfo mouse)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        MovePlayerInput(keyboard, deltaTime);
        UpdatePlayerJump(gameTime);
        MousePlayerInput(mouse);
        _hero.Update(gameTime);

        if (_isAttacking && _hero.IsFinished)
        {
            _isAttacking = false;
            _hero.IsLooping = true;
            SetAnimation(GetMovementAnimation());
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _hero.Draw(spriteBatch, _position);
    }

    private void UpdatePlayerJump(GameTime gameTime)
    {
        if (!_isJumping) return;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _verticalVelocity += GRAVITY * deltaTime;

        _position.Y += _verticalVelocity * deltaTime;

        if (_position.Y >= GROUND_Y && _verticalVelocity > 0f)
        {
            _position.Y = GROUND_Y;
            _verticalVelocity = 0f;
            _isJumping = false;
            if (!_isAttacking)
                SetAnimation(GetMovementAnimation());
        }
    }

    private void MovePlayerInput(KeyboardInfo keyboard, float deltaTime)
    {
        if (keyboard.WasKeyJustPressed(Keys.Space) && !_isJumping)
        {
            _isJumping = true;
            _verticalVelocity = JUMP_FORCE;
            if (!_isAttacking)
                SetAnimation(_jumpAnimation);
        }

        bool left = keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left);
        bool right = keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right);

        _movementDirection = (right ? 1f : 0f) - (left ? 1f : 0f);

        _position.X += _movementDirection * MOVEMENT_SPEED * deltaTime;
        if (!_isAttacking && !_isJumping)
            SetAnimation(GetMovementAnimation());

        if (_movementDirection < 0f)
            _hero.Effects = SpriteEffects.FlipHorizontally;
        else if (_movementDirection > 0f)
            _hero.Effects = SpriteEffects.None;
    }

    private void MousePlayerInput(MouseInfo mouse)
    {
        if (!_isAttacking && mouse.WasButtonJustPressed(MouseButton.Left))
        {
            _isAttacking = true;
            _hero.IsLooping = false;
            SetAnimation(_attackAnimation);
        }
    }

    private Animation GetMovementAnimation()
    {
        if (_isJumping)
            return _jumpAnimation;

        return _movementDirection != 0f ? _walkAnimation : _standAnimation;
    }

    private void SetAnimation(Animation animation)
    {
        if (ReferenceEquals(_currentAnimation, animation)) return;

        _currentAnimation = animation;
        _hero.Animation = animation;
    }

    private void LoadAnimationsFromXML(TextureAtlas atlas)
    {
        _standAnimation = atlas.GetAnimation("hero-stand-animation");
        _jumpAnimation = atlas.GetAnimation("hero-jump-animation");
        _walkAnimation = atlas.GetAnimation("hero-walk-animation");
        _attackAnimation = atlas.GetAnimation("hero-attack-animation");
        _deathAnimation = atlas.GetAnimation("hero-death-animation");
    }
}