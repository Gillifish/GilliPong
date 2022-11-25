using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Ball
{
    public Texture2D _texture;
    public Vector2 pos;
    public Color _color;
    public float speed;
    public float xVelocity = 0;
    public float yVelocity = 0;

    public Ball(Vector2 pos, float speed, Color _color)
    {
        this.pos = pos;
        this.speed = speed;
        this._color = _color;
    }

    public void UpdatePosition()
    {
        this.pos.X += xVelocity;
        this.pos.Y += yVelocity;
    }

    public int GetRadius()
    {
        return _texture.Width / 2;
    }

    public void Reset(int centerX, int centerY)
    {
        this.pos.X = centerX;
        this.pos.Y = centerY;
        this.xVelocity = 0;
        this.yVelocity = 0;
    }
}