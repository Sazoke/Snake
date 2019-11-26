using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Snake
{
    public partial class WindowGame : Window
    {
        private Game game;
        private Uri uriOfSkin;
        private System.Windows.Threading.DispatcherTimer timer;
        private int iteration = 0;
        private int size = 20;
        
        public WindowGame(string skin)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            GridGame.Width = Width;
            GridGame.Height = Height;
            uriOfSkin = GetUriOfSkin(skin);
            game = new Game((int)GridGame.Width / size, (int)GridGame.Height / size);
            CreateMap();
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(1000000);
            timer.Tick += (sender, e) => TickAction();
            timer.Start();
        }

        private Uri GetUriOfSkin(string skin)
        {
            var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            for (int i = 0; i < 2; i++)
                directory = directory.Parent;
            return new Uri(Path.Combine(directory.FullName, "Images/" + skin + ".png"));
        }

        private void TickAction()
        {
            switch (iteration)
            {
                case 10:
                    game.SpawnFood(Food.Apple);
                    break;
                case 20:
                    game.SpawnFood(Food.Banana);
                    break;
                case 30:
                    game.SpawnFood(Food.Brick);
                    break;
                case 40:
                    game.SpawnFood(Food.Bomb);
                    iteration = 0;
                    break;
                default:
                    break;
            }
            iteration++;
            if(iteration % (10 / game.Speed) == 0)
                game.NextStep();
            if (game.IsGameFinish)
                GameOver();
            CreateMap();
        }

        private void GameOver()
        {
            MessageBox.Show(game.Score.ToString());
            Close();
            timer.Stop();
        }

        private void CreateMap()
        {
            GridGame.Children.Clear();
            foreach (var item in game.BodyOfPLayer)
                AddImageToGrid(item);
            foreach (var element in game.Elements)
                AddImageToGrid(element.Key, element.Value.ToString());
        }

        private void AddImageToGrid(System.Drawing.Point point, string skin = null)
        {
            var image = new Image();
            if (skin == null)
                image.Source = new BitmapImage(uriOfSkin);
            else
                image.Source = new BitmapImage(new Uri(@"ImagesOfBonus\" + skin + ".png", UriKind.RelativeOrAbsolute));
            image.Margin = new Thickness(
            point.X * size,
            point.Y * size,
            GridGame.Width - (point.X + 1) * size,
            GridGame.Height - (point.Y + 1) * size);
            GridGame.Children.Add(image);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) => game.KeyDown(e.Key);
    }
}
