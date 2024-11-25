using System;
using System.Linq;
using System.Windows;
using Repositorys.Context;
using Core;

namespace UI
{
    public partial class Window1 : Window
    {
        private readonly FilmstripContext _context;

        public Window1()
        {
            InitializeComponent();
            _context = new FilmstripContext();
            LoadTypes();
            LoadGenres();
        }

        // Завантаження типів
        private void LoadTypes()
        {
            TypesListBox.ItemsSource = _context.Types.ToList();
        }

        // Завантаження жанрів
        private void LoadGenres()
        {
            GenresListBox.ItemsSource = _context.Genres.ToList();
        }

        // Додавання нового типу
        private void AddType_Click(object sender, RoutedEventArgs e)
        {
            string newTypeName = Microsoft.VisualBasic.Interaction.InputBox("Enter new type name:", "Add New Type", "");

            if (!string.IsNullOrWhiteSpace(newTypeName))
            {
                var newType = new Types { name = newTypeName };
                _context.Types.Add(newType);
                _context.SaveChanges();
                LoadTypes();
            }
        }

        // Редагування типу
        private void EditType_Click(object sender, RoutedEventArgs e)
        {
            if (TypesListBox.SelectedItem is Types selectedType)
            {
                string updatedTypeName = Microsoft.VisualBasic.Interaction.InputBox("Edit type name:", "Edit Type", selectedType.name);

                if (!string.IsNullOrWhiteSpace(updatedTypeName))
                {
                    selectedType.name = updatedTypeName;
                    _context.Types.Update(selectedType);
                    _context.SaveChanges();
                    LoadTypes();
                }
            }
        }

        // Видалення типу
        private void DeleteType_Click(object sender, RoutedEventArgs e)
        {
            if (TypesListBox.SelectedItem is Types selectedType)
            {
                _context.Types.Remove(selectedType);
                _context.SaveChanges();
                LoadTypes();
            }
        }

        // Додавання нового жанру
        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            string newGenreName = Microsoft.VisualBasic.Interaction.InputBox("Enter new genre name:", "Add New Genre", "");

            if (!string.IsNullOrWhiteSpace(newGenreName))
            {
                var newGenre = new Genre { name = newGenreName };
                _context.Genres.Add(newGenre);
                _context.SaveChanges();
                LoadGenres();
            }
        }

        // Редагування жанру
        private void EditGenre_Click(object sender, RoutedEventArgs e)
        {
            if (GenresListBox.SelectedItem is Genre selectedGenre)
            {
                string updatedGenreName = Microsoft.VisualBasic.Interaction.InputBox("Edit genre name:", "Edit Genre", selectedGenre.name);

                if (!string.IsNullOrWhiteSpace(updatedGenreName))
                {
                    selectedGenre.name = updatedGenreName;
                    _context.Genres.Update(selectedGenre);
                    _context.SaveChanges();
                    LoadGenres();
                }
            }
        }

        // Видалення жанру
        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            if (GenresListBox.SelectedItem is Genre selectedGenre)
            {
                _context.Genres.Remove(selectedGenre);
                _context.SaveChanges();
                LoadGenres();
            }
        }
    }
}
