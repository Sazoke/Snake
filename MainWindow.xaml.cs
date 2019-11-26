using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var names = GetNamesOfImages();

            var comboBox = new ComboBox();
            comboBox.Margin = new Thickness(300, 190, 300, 200);
            comboBox.ItemsSource = GetItems(names);
            comboBox.SelectedIndex = 0;

            var button = new Button();
            button.Margin = new Thickness(300, 220, 300, 160);
            button.Content = "Start Game";
            button.Click += (sender, e) =>
            {
                var newGame = new WindowGame(comboBox.SelectedItem.ToString().Split(' ').Last());
                newGame.Show();
            };

            Grid.Children.Add(comboBox);
            Grid.Children.Add(button);

            Show();
        }

        private IEnumerable<ComboBoxItem> GetItems(List<string> names)
        {
            foreach (var name in names)
            {
                var item = new ComboBoxItem();
                item.Content = name;
                yield return item;
            }
        }

        private List<string> GetNamesOfImages()
        {
            var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            for (int i = 0; i < 2; i++)
                directory = directory.Parent;
            directory = directory.GetDirectories()[2];
            var files = directory.GetFiles();
            var names = files.Select(n => n.Name.Split('.').First());
            return names.ToList();
        }
    }
}
