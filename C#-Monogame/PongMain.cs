using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;

public class PongMain : Game
{
    public const double MAXBOUNCEANGLE = 5 * (Math.PI / 12);

    Paddle p1;
    Paddle p2;
    Ball ball;
    bool start = false;
    int player1Score = 0;
    int player2Score = 0;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont gameFont;
    

    public PongMain()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        p1 = new Paddle(new Vector2(50, (_graphics.PreferredBackBufferHeight / 2) - 40), 5.0f, _graphics.PreferredBackBufferHeight, Color.White);
        p2 = new Paddle(new Vector2( _graphics.PreferredBackBufferWidth - 70, (_graphics.PreferredBackBufferHeight / 2) - 40), 5.0f, _graphics.PreferredBackBufferHeight, Color.White);
        ball = new Ball(new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2), 5.0f, Color.White);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        p1._texture = Content.Load<Texture2D>("paddle");
        p2._texture = Content.Load<Texture2D>("paddle");
        ball._texture = Content.Load<Texture2D>("ball");
        gameFont = Content.Load<SpriteFont>("GameFont");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        var kstate = Keyboard.GetState();

        if(!start && kstate.IsKeyDown(Keys.Space))
        {
            if(player1Score > player2Score)
            {
                ball.xVelocity = ball.speed;
            } else
            {
                ball.xVelocity = ball.speed * -1;
            }

            start = true;
        }

        if(ball.pos.X < 0)
        {
            player2Score++;
            ball.Reset(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            start = false;
        } else if(ball.pos.X > _graphics.PreferredBackBufferWidth)
        {
            player1Score++;
            ball.Reset(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            start = false;
        }

        Player1Input(kstate);
        Player2Input(kstate);

        HandleBallCollision();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _spriteBatch.DrawString(gameFont, $"{player1Score} | {player2Score}", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 40, 0), Color.White);
        _spriteBatch.Draw(p1._texture, p1.pos, p1._color);
        _spriteBatch.Draw(p2._texture, p2.pos, p2._color);
        _spriteBatch.Draw(ball._texture, ball.pos, null, ball._color, 0f, new Vector2(ball._texture.Width / 2, ball._texture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void Player1Input(KeyboardState state)
    {
        if(state.IsKeyDown(Keys.S))
        {
            p1.MoveDown();
        }

        if(state.IsKeyDown(Keys.W))
        {
            p1.MoveUp();
        }
    }

    private void Player2Input(KeyboardState state)
    {
        if(state.IsKeyDown(Keys.Down))
        {
            p2.MoveDown();
        }

        if(state.IsKeyDown(Keys.Up))
        {
            p2.MoveUp();
        }
    }

    private void HandleBallCollision()
    {
        // Ball boundry collision
        if(ball.pos.Y < 0 + ball.GetRadius() )
        {
            ball.pos.Y = 0 + ball.GetRadius();
            ball.yVelocity *= -1;
        } else if(ball.pos.Y > _graphics.PreferredBackBufferHeight - ball.GetRadius())
        {
            ball.pos.Y = _graphics.PreferredBackBufferHeight - ball.GetRadius();
            ball.yVelocity *= -1;
        }

        // Left Paddle Collision

        if((ball.pos.X - ball.GetRadius() > p1.pos.X && ball.pos.X - ball.GetRadius() < p1.pos.X + p1._texture.Width) && (ball.pos.Y > p1.pos.Y && ball.pos.Y < p1.pos.Y + p1._texture.Height))
        {
            ball.xVelocity *= -1;
            ball.yVelocity = (float)(ball.speed * -Math.Sin(GetBounceAngle(p1)));
        }
        
        // Right Paddle Collision

        if((ball.pos.X + ball.GetRadius() > p2.pos.X && ball.pos.X + ball.GetRadius() < p2.pos.X + p2._texture.Width) && (ball.pos.Y > p2.pos.Y && ball.pos.Y < p2.pos.Y + p2._texture.Height))
        {
            ball.xVelocity *= -1;
            ball.yVelocity = (float)(ball.speed * -Math.Sin(GetBounceAngle(p2)));
        }

        ball.UpdatePosition();
    }

    private double GetBounceAngle(Paddle _paddle)
    {
        var relativeYIntersect = (_paddle.pos.Y + (_paddle._texture.Height / 2)) - ball.pos.Y;
        var normalizedRelativeIntersectionY = (relativeYIntersect / (_paddle._texture.Height / 2));
        var bounceAngle = normalizedRelativeIntersectionY * MAXBOUNCEANGLE;

        return bounceAngle;
    }
}