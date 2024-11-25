using System.IO;
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
using UI.Parsers;
using Repositorys.Context;
using Azure;


namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        } 

        private void Button_Click_WORK(object sender, RoutedEventArgs e)
        {
            SortedFilms sortedFilms = new SortedFilms();
            sortedFilms.ShowDialog();
        }
        private void Button_Click_EDIT_T(object sender, RoutedEventArgs e)        { }
        private void Button_Click_EDIT_G(object sender, RoutedEventArgs e)        { }
        private void Button_Click_EDIT_F(object sender, RoutedEventArgs e)
        {
            Operations operationsWithFilms = new Operations();
            operationsWithFilms.ShowDialog();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Шлях до єдиного CSV-файлу
                string filePath = $@"{PathTb.Text}";

                // Перевірка на існування файлу
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Файл не знайдено. Перевірте шлях!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var context = new FilmstripContext())
                {
                    // Парсинг YearRelease
                    var yearReleases = YearParser.Parse(filePath);
                    YearParser.SaveToDatabase(context, yearReleases);

                    // Парсинг Types
                    var types = TypeParser.Parse(filePath); // Передаємо context, якщо потрібно
                    TypeParser.SaveToDatabase(context, types);

                    // Парсинг Genres
                    var genres = GenreParser.Parse(filePath);
                    GenreParser.SaveToDatabase(context, genres);

                    // Парсинг AvarageRating
                    var avgRatings = RatingParser.Parse(filePath);
                    RatingParser.SaveToDatabase(context, avgRatings);

                    // Парсинг Filmstrip
                    var filmstrips = FilmstripParser.Parse(filePath, context);
                    FilmstripParser.SaveToDatabase(context, filmstrips);

                    // Повідомлення про успішний парсинг
                    MessageBox.Show("Парсинг завершено успішно!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                // Виводимо повідомлення про помилку
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}