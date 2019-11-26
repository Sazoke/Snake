using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Input;

namespace Snake.Tests
{
    [TestFixture]
    class TestsOfSnake
    {
        [TestCase(5, 5, 5, 4, Direction.Up)]
        [TestCase(5, 5, 4, 5, Direction.Left)]
        [TestCase(5, 5, 6, 5, Direction.Right)]

        public void CorrectMove(int pointX, int pointY, int expectedPointX, int expectedPointY, Direction direction)
        {
            var snake = new SnakePlayer(new Point(pointX, pointY));
            var expectedSnake = new SnakePlayer(new Point(expectedPointX, expectedPointY));
            expectedSnake.ChangeDirection(direction);
            snake.ChangeDirection(direction);
            snake.Move();
            Assert.AreEqual(snake, expectedSnake);
        }

        [TestCase]
        public void DontMoveBack()
        {
            var snake = new SnakePlayer(new Point(0, 0));
            var expectedSnake = new SnakePlayer(new Point(0, -1));
            snake.ChangeDirection(Direction.Down);
            snake.Move();
            Assert.AreEqual(snake, expectedSnake);
        }

        [TestCase(4, 0, 5, 5, Direction.Up)]
        [TestCase(4, 0, 5, 5, Direction.Down)]
        [TestCase(0, 4, 5, 5, Direction.Left)]
        [TestCase(4, 4, 5, 5, Direction.Right)]
        public void CorrectDeadInWall(int pointX, int pointY, int width, int height, Direction direction)
        {
            var snake = new SnakePlayer(new Point(pointX, pointY));
            snake.ChangeDirection(direction);
            snake.Move();
            Assert.IsTrue(snake.IsDead(width, height));
        }

        [TestCase(0, Food.Bomb)]
        [TestCase(2, Food.Apple)]
        public void CorrectEat(int expectedCount, Food food)
        {
            var snake = new SnakePlayer(new Point(1, 1));
            snake.EatFood(food);
            Assert.AreEqual(expectedCount, snake.GetBody().Count());
        }

        [TestCase]
        public void CorrectMoveAndEat()
        {
            var snake = new SnakePlayer(new Point(0, 0));
            for (int i = 0; i < 3; i++)
                snake.EatFood(Food.Apple);
            snake.ChangeDirection(Direction.Right);
            for (int i = 0; i < 2; i++)
                snake.Move();
            Assert.AreEqual(snake.GetBody().ToList(), new List<Point>() { new Point(2, 0),  new Point(1, 0), 
                                                                          new Point(0, 0), new Point(0, 1) });
        }
    }
}
