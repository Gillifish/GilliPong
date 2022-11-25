using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong;

public class Paddle
{
    public Texture2D _texture;
    public Vector2 pos;
    public Color _color;
    public float speed;
    public int lowerBound;

    public Paddle(Vector2 pos, float speed, int lowerBound, Color _color)
    {
        this.pos = pos;
        this.speed = speed;
        this.lowerBound = lowerBound;
        this._color = _color;
    }

    public void MoveUp()
    {
        if(this.pos.Y - this.speed < 0)
        {
            this.pos.Y = 0;
        } else
        {
            this.pos.Y -= this.speed;
        }
    }

    public void MoveDown()
    {
        if(this.pos.Y + this.speed > lowerBound - _texture.Height)
        {
            this.pos.Y = lowerBound - _texture.Height;
        } else 
        {
            this.pos.Y += this.speed;
        }
    }
}