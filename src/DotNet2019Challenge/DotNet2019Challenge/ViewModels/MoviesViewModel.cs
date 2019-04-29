using DotNet2019Challenge.Models;
using DotNet2019Challenge.Services.Movies;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DotNet2019Challenge.ViewModels
{
    public class MoviesViewModel : BindableObject
    {
        private bool _isBusy;
        private Movie _highlight;
        private ObservableCollection<Movie> _topRatedMovies;
        private ObservableCollection<Movie> _popularMovies;
        private ObservableCollection<TVShow> _topRatedTvShows;
        private ObservableCollection<TVShow> _popularTvShows;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public Movie Highlight
        {
            get => _highlight;
            set
            {
                _highlight = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Movie> TopRatedMovies
        {
            get => _topRatedMovies;
            set
            {
                _topRatedMovies = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Movie> PopularMovies
        {
            get => _popularMovies;
            set
            {
                _popularMovies = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TVShow> TopRatedTvShows
        {
            get => _topRatedTvShows;
            set
            {
                _topRatedTvShows = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TVShow> PopularTvShows
        {
            get => _popularTvShows;
            set
            {
                _popularTvShows = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadDataAsync()
        {
            IsBusy = true;

            await LoadTopRatedMoviesAync();
            await LoadPopularMoviesAync();
            await LoadTopRatedTvShowsAync();
            await LoadPopularTvShowsAync();

            IsBusy = false;
        }

        private async Task LoadTopRatedMoviesAync()
        {
            var result = await MoviesService.Instance.GetTopRatedMoviesAsync();

            TopRatedMovies = new ObservableCollection<Movie>(result.Results);
            Highlight = TopRatedMovies.FirstOrDefault();
        }

        private async Task LoadPopularMoviesAync()
        {
            var result = await MoviesService.Instance.GetPopularMoviesAsync();

            PopularMovies = new ObservableCollection<Movie>(result.Results);
        }

        private async Task LoadTopRatedTvShowsAync()
        {
            var result = await MoviesService.Instance.GetTopRatedShowsAsync();

            TopRatedTvShows = new ObservableCollection<TVShow>(result.Results);
        }

        private async Task LoadPopularTvShowsAync()
        {
            var result = await MoviesService.Instance.GetPopularShowsAsync();

            PopularTvShows = new ObservableCollection<TVShow>(result.Results);
        }
    }
}