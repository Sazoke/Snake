using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;
using System;

namespace Snake
{
    public class CoordinateAndFood
    {
        public CoordinateAndFood(Food food, Point coordinate)
        {
            Food = food;
            Coordinate = coordinate;
        }
        public Food Food;
        public Point Coordinate;
    }

    public class Game
    {
        private SnakePlayer player;
        private Dictionary<Point, Food> elements;
        private Random random;
        private int width;
        private int height;
        private int score;

        public Game(int width, int height)
        {
            random = new Random();
            this.height = height;
            this.width = width;
            player = new SnakePlayer(new Point(width / 2 + 1, height / 2 + 1));
            elements = new Dictionary<Point, Food>();
            score = 0;
        }

        public void NextStep()
        {
            player.Move();
            var head = player.GetBody().First();
            if (elements.ContainsKey(head))
            {
                var element = elements[head];
                if (element.Equals(Food.Apple))
                    score++;
                player.EatFood(element);
                elements.Remove(head);
            }
        }

        public void SpawnFood(Food food)
        {
            var location = new Point(random.Next(0, width - 2), random.Next(0, height - 2));
            if(!elements.ContainsKey(location) && !player.GetBody().Contains(location))
                elements.Add(location, food);
        }

        public IEnumerable<KeyValuePair<Point, Food>> Elements => elements;

        public void KeyDown(Key pressedKey)
        {
            if (pressedKey != Key.Up &&
                pressedKey != Key.Down &&
                pressedKey != Key.Right &&
                pressedKey != Key.Left)
                return;
            Direction newDirection;
            Enum.TryParse<Direction>(pressedKey.ToString(), true, out newDirection);
            player.ChangeDirection(newDirection);
        } 

        public bool IsGameFinish { get { return player.IsDead(width, height); } }

        public IEnumerable<Point> BodyOfPLayer { get { return player.GetBody(); } }

        public int Score { get { return score; } } 

        public int Speed { get { return player.Speed; } }
    }
}