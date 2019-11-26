using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Snake
{
    public class SnakePlayer
    {
        private List<Point> body;
        private Direction direction;

        public SnakePlayer(Point startPoint)
        {
            body = new List<Point>() { startPoint };
            direction = Direction.Up;
        }

        public void Move()
        {
            for (int i = body.Count - 1; i >= 1; i--)
                body[i] = body[i - 1];
            body[0] = GetNextPoint(direction, body[0], 1);
        }

        public bool IsDead(int height, int width) =>
            body.Count == 0 ||
            body.Skip(1).Contains(body.First()) || 
            body[0].X < 0 || 
            body[0].X >= width || 
            body[0].Y < 0 || 
            body[0].Y >= height;

        public void EatFood(Food food)
        {
            switch (food)
            {
                case Food.Apple:
                    {
                        var tail = body.Last();
                        if (body.Count == 1)
                        {
                            body.Add(GetNextPoint(direction, tail, -1));
                            return;
                        }
                        var preTail = body[body.Count - 2];
                        body.Add(new Point(
                            2 * tail.X - preTail.X,
                            2 * tail.Y - preTail.Y));
                        break;
                    }
                case Food.Bomb:
                    {
                        body.RemoveAt(0);
                        break;
                    }
                default:
                    break;
            }
        }

        private Point GetNextPoint(Direction direction, Point point, int modifier)
        {
            var result = new Point();
            switch (direction)
            {
                case Direction.Up:
                    result = new Point(point.X, point.Y + (-1) * modifier);
                    break;
                case Direction.Down:
                    result = new Point(point.X, point.Y + modifier);
                    break;
                case Direction.Left:
                    result = new Point(point.X + (-1) * modifier, point.Y);
                    break;
                case Direction.Right:
                    result = new Point(point.X + modifier, point.Y);
                    break;
                default:
                    result = body.First();
                    break;
            }
            return result;
        }

        public void ChangeDirection(Direction newDirection)
        {
            if((int)direction + (int)newDirection != 0)
                direction = newDirection;
        }
        public IEnumerable<Point> GetBody() => body;

        public override bool Equals(object obj)
        {
            if (!(obj is SnakePlayer))
                return false;
            var newSneak = obj as SnakePlayer;
            if(newSneak.direction == direction && newSneak.body.Count == body.Count)
            {
                for (int i = 0; i < body.Count; i++)
                    if (body[i] != newSneak.body[i])
                        return false;
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
