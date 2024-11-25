using Repositorys.Repositories;
using Repositorys.Context;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core;
using DocumentFormat.OpenXml.Bibliography;

namespace UI
{
    public partial class SortedFilms : Window
    {
        private readonly FilmstripContext _context;
        private readonly TypesRepository _typesRepository;
        private readonly GenreRepository _genreRepository;
        private bool _isAscending = true; // Напрям сортування для року
        private bool _isAscendingForRating = true; // Напрям сортування для рейтингу
        private bool _isAscendingForVotes = true; // Напрям сортування для голосів
        private readonly ReportGenerator _reportGenerator;

        public SortedFilms()
        {
            InitializeComponent();
            _context = new FilmstripContext();
            _typesRepository = new TypesRepository(_context);
            _genreRepository = new GenreRepository(_context);
            _reportGenerator = new ReportGenerator();

            LoadTypes();
            LoadGenres();
        }

        private void LoadTypes()
        {
            TypeComboBox.ItemsSource = _typesRepository.Get().ToList();
        }

        private void LoadGenres()
        {
            GenreCbox.ItemsSource = _genreRepository.Get().ToList();
        }

        // Сортування за роком
        private void YearB(object sender, RoutedEventArgs e)
        {
            var sortedFilms = _isAscending
                ? _context.Filmstrips.OrderBy(f => f.yearReleaseId).ToList()
                : _context.Filmstrips.OrderByDescending(f => f.yearReleaseId).ToList();

            FilmsListbox.ItemsSource = sortedFilms;

            MessageBox.Show($"Сортування за роком: {(_isAscending ? "Зростання" : "Спадання")}",
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);

            _reportGenerator.AddEntry(
                searchOrSortType: "Сортування за роком",
                criteria: "N/A",
                resultsCount: sortedFilms.Count,
                sortOrder: _isAscending ? "Зростання" : "Спадання"
            );

            _isAscending = !_isAscending; // Змінити напрям сортування
        }

        // Сортування за рейтингом
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var sortedFilms = _isAscendingForRating
                ? _context.Filmstrips.OrderBy(f => f.avarageRatingsId).ToList()
                : _context.Filmstrips.OrderByDescending(f => f.avarageRatingsId).ToList();

            FilmsListbox.ItemsSource = sortedFilms;

            MessageBox.Show($"Сортування за рейтингом: {(_isAscendingForRating ? "Зростання" : "Спадання")}",
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);

            _reportGenerator.AddEntry(
                searchOrSortType: "Сортування за рейтингом",
                criteria: "N/A",
                resultsCount: sortedFilms.Count,
                sortOrder: _isAscendingForRating ? "Зростання" : "Спадання"
            );

            _isAscendingForRating = !_isAscendingForRating;
        }

        // Сортування за голосами
        private void VotesB(object sender, RoutedEventArgs e)
        {
            var sortedFilms = _isAscendingForVotes
                ? _context.Filmstrips.OrderBy(f => f.NumVotes).ToList()
                : _context.Filmstrips.OrderByDescending(f => f.NumVotes).ToList();

            FilmsListbox.ItemsSource = sortedFilms;

            MessageBox.Show($"Сортування за кількістю голосів: {(_isAscendingForVotes ? "Зростання" : "Спадання")}",
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);

            _reportGenerator.AddEntry(
                searchOrSortType: "Сортування за кількістю голосів",
                criteria: "N/A",
                resultsCount: sortedFilms.Count,
                sortOrder: _isAscendingForVotes ? "Зростання" : "Спадання"
            );

            _isAscendingForVotes = !_isAscendingForVotes;
        }

        // Пошук за типом
        private void TypeB(object sender, RoutedEventArgs e)
        {
            if (TypeComboBox.SelectedItem is Types selectedType)
            {
                var results = _context.Filmstrips.Where(f => f.typeId == selectedType.id).ToList();

                FilmsListbox.ItemsSource = results;

                _reportGenerator.AddEntry(
                    searchOrSortType: "Пошук за типом",
                    criteria: selectedType.name,
                    resultsCount: results.Count
                );
            }
        }

        // Пошук за жанром
        private void Button_Click_Genre(object sender, RoutedEventArgs e)
        {
            if (GenreCbox.SelectedItem is Genre selectedGenre)
            {
                var results = _context.FilmstripGenres
                    .Where(fg => fg.GenreId == selectedGenre.id)
                    .Select(fg => fg.Filmstrip)
                    .ToList();

                FilmsListbox.ItemsSource = results;

                _reportGenerator.AddEntry(
                    searchOrSortType: "Пошук за жанром",
                    criteria: selectedGenre.name,
                    resultsCount: results.Count
                );
            }
        }

        // Пошук за роком
        private void Button_Click_Year(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Search_Year_Copy.Text) && int.TryParse(Search_Year_Copy.Text, out int year))
            {
                var results = _context.Filmstrips.Where(f => f.yearRelease.year == year).ToList();

                FilmsListbox.ItemsSource = results;

                _reportGenerator.AddEntry(
                    searchOrSortType: "Пошук за роком",
                    criteria: year.ToString(),
                    resultsCount: results.Count
                );
            }
        }

        // Пошук за назвою
        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            string searchName = Search_Name.Text;

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                var results = _context.Filmstrips
                    .Where(f => f.name.ToLower().Contains(searchName.ToLower()))
                    .ToList();

                FilmsListbox.ItemsSource = results;

                _reportGenerator.AddEntry(
                    searchOrSortType: "Пошук за назвою",
                    criteria: searchName,
                    resultsCount: results.Count
                );
            }
        }

        private void FIELD_Click(object sender, RoutedEventArgs e)
        {
            var query = _context.Filmstrips.AsQueryable();

            // Фільтрація за типом
            if (TypeComboBox.SelectedItem is Types selectedType)
                query = query.Where(f => f.typeId == selectedType.id);

            // Фільтрація за жанром
            if (GenreCbox.SelectedItem is Genre selectedGenre)
            {
                var filmIdsByGenre = _context.FilmstripGenres
                    .Where(fg => fg.GenreId == selectedGenre.id)
                    .Select(fg => fg.FilmstripId)
                    .ToList();

                query = query.Where(f => filmIdsByGenre.Contains(f.id));
            }

            // Фільтрація за роком
            if (!string.IsNullOrWhiteSpace(Search_Year_Copy.Text) && int.TryParse(Search_Year_Copy.Text, out int year))
                query = query.Where(f => f.yearRelease.year == year);

            // Фільтрація за назвою
            string searchName = Search_Name.Text;
            if (!string.IsNullOrWhiteSpace(searchName))
                query = query.Where(f => f.name.ToLower().Contains(searchName.ToLower()));

            var results = query.ToList();
            FilmsListbox.ItemsSource = results;

            _reportGenerator.AddEntry(
                searchOrSortType: "Пошук за всіма полями",
                criteria: "Фільтр активовано",
                resultsCount: results.Count
            );
        }

        private void ReportB(object sender, RoutedEventArgs e)
        {
            try
            {
                // Використання діалогового вікна для вибору шляху збереження
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Title = "Зберегти звіт",
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = "FilmSearchReport.xlsx", // Стандартна назва файлу
                    DefaultExt = ".xlsx"
                };

                // Показати діалогове вікно
                bool? result = saveFileDialog.ShowDialog();

                if (result == true)
                {
                    // Отримати обраний шлях
                    string filePath = saveFileDialog.FileName;

                    // Перевірка на валідність шляху
                    if (string.IsNullOrWhiteSpace(filePath) || filePath.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0)
                    {
                        throw new ArgumentException("Некоректний шлях для збереження файлу.");
                    }

                    // Генерація звіту
                    _reportGenerator.GenerateReport(filePath);

                    // Повідомлення про успіх
                    MessageBox.Show($"Звіт успішно збережено: {filePath}", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Повідомлення, якщо користувач скасував збереження
                    MessageBox.Show("Збереження звіту скасовано.", "Скасування", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                // Обробка помилок під час генерації або збереження звіту
                MessageBox.Show($"Помилка збереження звіту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
