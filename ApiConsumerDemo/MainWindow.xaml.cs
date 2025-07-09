using DemoLibrary;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApiConsumerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int maxNumber;
        private int currentNumber;

        public MainWindow()
        {
            InitializeComponent();
            ApiHelper.InitializeClient();

        }
        private async Task LoadImage(int imageNumber =0)
        {
            var comic = await (new ComicProcessor()).LoadComic(imageNumber);
            if (imageNumber == 0)
            {
                maxNumber = comic.Num;
            }
            currentNumber = comic.Num;
            var uriSource = new Uri(comic.Img, UriKind.Absolute);
            comicImage.Source = new BitmapImage(uriSource);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadImage();
        }
    }
}