using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Repositorys.Context;
using Core;
using Core.OtherTables;

namespace UI
{
    /// <summary>
    /// Interaction logic for Operations.xaml
    /// </summary>
    public class SelectableGenre
    {
        public Genre Genre { get; set; } // Сама модель Genre
        public bool IsChecked { get; set; } // Стан CheckBox
    }

    public partial class Operations : Window
    {
        private readonly FilmstripContext _context;

        public Operations()
        {
            InitializeComponent();
            _context = new FilmstripContext();
            LoadComboBoxes();
            LoadListBox();
        }

        public void LoadComboBoxes()
        {
            TypeComboBoxCRUD.ItemsSource = _context.Types.ToList();

            // Завантаження жанрів із станом
            GenreCheckedListBox.ItemsSource = _context.Genres
                .Select(g => new SelectableGenre { Genre = g, IsChecked = false })
                .ToList();
        }
        private void Click(object sender, RoutedEventArgs e)
        {
            LoadComboBoxes();
            LoadListBox();
        }

        private void LoadListBox()
        {
            try
            {
                // Завантаження фільмів у ListBox
                var sortedFilms = _context.Filmstrips
                    .OrderBy(f => f.avarageRatingsId)
                    .ToList();
                FilmsListbox.ItemsSource = sortedFilms;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Open_Click
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.ShowDialog();
        }

        private void AddFilmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Отримуємо введені дані
                string filmName = FilmNameTextBox.Text;
                int votes = int.Parse(VotesTextBox.Text);
                int releaseYear = int.Parse(YearTextBox.Text);
                double averageRating = double.Parse(RatingTextBox.Text);
                var selectedType = (Types)TypeComboBoxCRUD.SelectedItem;
                var selectedGenres = GetSelectedGenres();

                // Створюємо новий запис фільму
                var yearRelease = _context.YearReleases.FirstOrDefault(y => y.year == releaseYear)
                    ?? new YearRelease { year = releaseYear };

                var avgRating = _context.AvarageRatings.FirstOrDefault(a => a.avarageRating == averageRating)
                    ?? new AvarageRating { avarageRating = averageRating };

                if (yearRelease.id == 0) _context.YearReleases.Add(yearRelease);
                if (avgRating.id == 0) _context.AvarageRatings.Add(avgRating);

                _context.SaveChanges();

                var newFilm = new Filmstrip
                {
                    id = Guid.NewGuid().ToString(),
                    name = filmName,
                    NumVotes = votes,
                    yearReleaseId = yearRelease.id,
                    avarageRatingsId = avgRating.id,
                    typeId = selectedType.id
                };

                _context.Filmstrips.Add(newFilm);

                // Додаємо вибрані жанри
                foreach (var genre in selectedGenres)
                {
                    var filmGenre = new FilmstripGenre
                    {
                        Filmstrip = newFilm,
                        Genre = genre
                    };
                    _context.FilmstripGenres.Add(filmGenre);
                }

                _context.SaveChanges();
                LoadListBox(); // Оновлення списку
                MessageBox.Show("Фільм успішно додано!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}");
            }
        }


        private void UpdateFilmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Перевіряємо вибраний елемент
                if (FilmsListbox.SelectedItem is not Filmstrip selectedFilm)
                {
                    MessageBox.Show("Будь ласка, виберіть фільм для редагування!");
                    return;
                }

                // Оновлюємо основну інформацію про фільм
                selectedFilm.name = FilmNameTextBox.Text;
                selectedFilm.NumVotes = int.Parse(VotesTextBox.Text);

                // Перевіряємо введення року та рейтингу
                var releaseYear = int.Parse(YearTextBox.Text);
                var avgRating = double.Parse(RatingTextBox.Text);

                // Оновлюємо зв'язки з YearRelease і AvarageRating
                var yearRelease = _context.YearReleases.FirstOrDefault(y => y.year == releaseYear)
                                  ?? new YearRelease { year = releaseYear };

                var avgRatingEntity = _context.AvarageRatings.FirstOrDefault(a => a.avarageRating == avgRating)
                                      ?? new AvarageRating { avarageRating = avgRating };

                if (yearRelease.id == 0) _context.YearReleases.Add(yearRelease);
                if (avgRatingEntity.id == 0) _context.AvarageRatings.Add(avgRatingEntity);

                _context.SaveChanges();

                selectedFilm.yearReleaseId = yearRelease.id;
                selectedFilm.avarageRatingsId = avgRatingEntity.id;

                // Оновлюємо зв'язок із типом
                selectedFilm.typeId = (TypeComboBoxCRUD.SelectedItem as Types)?.id ?? 0;

                // Оновлюємо жанри
                var selectedGenres = GetSelectedGenres();

                // Видаляємо старі зв'язки
                var oldGenres = _context.FilmstripGenres.Where(fg => fg.FilmstripId == selectedFilm.id).ToList();
                _context.FilmstripGenres.RemoveRange(oldGenres);

                // Додаємо нові зв'язки
                foreach (var genre in selectedGenres)
                {
                    var newFilmGenre = new FilmstripGenre
                    {
                        FilmstripId = selectedFilm.id,
                        GenreId = genre.id
                    };
                    _context.FilmstripGenres.Add(newFilmGenre);
                }

                // Оновлюємо фільм
                _context.Filmstrips.Update(selectedFilm);
                _context.SaveChanges();

                // Оновлюємо список
                LoadListBox();

                MessageBox.Show("Фільм успішно оновлено!");
            }
            catch (FormatException)
            {
                MessageBox.Show("Будь ласка, введіть коректні значення для року та рейтингу!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<Genre> GetSelectedGenres()
        {
            return GenreCheckedListBox.Items.OfType<SelectableGenre>()
                .Where(sg => sg.IsChecked)
                .Select(sg => sg.Genre)
                .ToList();
        }


        private void DeleteFilmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Перевіряємо вибраний елемент
                if (FilmsListbox.SelectedItem is not Filmstrip selectedFilm)
                {
                    MessageBox.Show("Будь ласка, виберіть фільм для видалення!");
                    return;
                }

                // Видаляємо зв’язки з жанрами
                var relatedGenres = _context.FilmstripGenres.Where(fg => fg.FilmstripId == selectedFilm.id).ToList();
                _context.FilmstripGenres.RemoveRange(relatedGenres);

                // Видаляємо фільм
                _context.Filmstrips.Remove(selectedFilm);
                _context.SaveChanges();

                LoadListBox(); // Оновлення списку
                MessageBox.Show("Фільм успішно видалено!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}");
            }
        }
    }
}
