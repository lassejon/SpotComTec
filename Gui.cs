namespace OOPSpotiflixV2
{
    internal class Gui
    {
        private Data data = new Data();
        //private Media media = new Media();
        private string path = @$"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/SpotiflixData.json";
        public Gui()
        {
            //data.MovieList.Add(new Movie() { WWW=@"https:\\netflix.com/rambo3.mp4", Title="Rambo III", Genre ="Action", ReleaseDate=new DateTime(1988,5,25), Length=new DateTime(1,1,1, 1, 42, 0)});
            while (true)
            {
                Menu();
            }
        }
        private void Menu()
        {
            Console.WriteLine("\nMENU\n1 for movies\n2 for series\n3 for music\n4 for save\n5 for load");

            switch (Console.ReadLine())
            {
                case "1":
                    MovieMenu();
                    break;
                case "2":
                    SeriesMenu();
                    break;
                case "3":
                    MusicMenu();
                    break;
                case "4":
                    SaveData();
                    break;
                case "5":
                    LoadData();
                    break;
                default:
                    break;
            }
        }
        #region DataHandling
        private void SaveData()
        {
            string json = System.Text.Json.JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
            Console.WriteLine("File saved succesfully at " + path);
        }

        private void LoadData()
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Creating new file at: " + path);
                SaveData();
            }
            string json = File.ReadAllText(path);
            data = System.Text.Json.JsonSerializer.Deserialize<Data>(json);
            Console.WriteLine("File loaded succesfully from " + path);
        }
        #endregion
        #region Get input
        private DateTime GetLength()
        {
            DateTime time;
            do
            {
                Console.Write("Length (hh:mm:ss): ");
            }
            while (!DateTime.TryParse("0001-01-01 " + Console.ReadLine(), out time));
            return time;
        }

        private DateTime GetReleaseDate()
        {
            //hvis input ="" så sæt default værdi, ellers tryparse
            DateTime date;
            string input = "";
            do
            {
                Console.Write("Release Date (dd/mm/yyyy): ");
                input = Console.ReadLine();
                if (input == "") return DateTime.Today;
            }
            while (!DateTime.TryParse(input, out date));
            return date;
        }

        private string GetString(string type)
        {
            string? input;
            do
            {
                Console.Write(type);
                input = Console.ReadLine();
                if (input == "") return "Unknown";
            }
            while (input == null);
            return input;
        }

        private int GetInt(string request)
        {
            int i;
            do
            {
                Console.Write(request);
            }
            while (!int.TryParse(Console.ReadLine(), out i));
            return i;
        }
        #endregion        
        #region Movies
        private void MovieMenu()
        {
            Console.WriteLine("\nMOVIE MENU\n1 for list of movies\n2 for search movies\n3 for add new movie");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    ShowMovieList();
                    break;
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    SearchMovie();
                    break;
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    AddMovie();
                    break;
                default:
                    break;
            }
        }

        private void AddMovie()
        {
            Movie movie = new Movie();
            movie.Title = GetString("Title: ");
            movie.Length = GetLength();
            movie.Genre = GetString("Genre: ");
            movie.ReleaseDate = GetReleaseDate();
            movie.WWW = GetString("WWW: ");

            ShowMovie(movie);
            Console.WriteLine("Confirm adding to list (Y/N)");
            if (Console.ReadKey().Key == ConsoleKey.Y) data.MovieList.Add(movie);
        }

        //private void Search<T>(List<T> list)
        //{
        //    Console.Write("Search: ");
        //    string? search = Console.ReadLine();
        //    if (typeof(T) == typeof(Movie))
        //    {
        //        foreach (T item in list)
        //        {
        //            if (search != null)
        //            {
        //                if (item.Title.Contains(search) || item.Genre.Contains(search))
        //                    ShowMovie(movie);
        //            }
        //        }
        //    }

        //    //foreach (T item in list)
        //    //{
        //    //    if (search != null)
        //    //    {
        //    //        if (item.Title.Contains(search) || item.Genre.Contains(search))
        //    //            ShowMovie(movie);
        //    //    }
        //    //}

        //}

        private void SearchMovie()
        {
            Console.Write("Search: ");
            string? search = Console.ReadLine().ToLower();
            foreach (Movie movie in data.MovieList)
            {
                if (search != null)
                {
                    //TODO ToLower or ToUpper to match any case
                    if (movie.Title.ToLower().Contains(search) || movie.Genre.ToLower().Contains(search))
                        ShowMovie(movie);
                }
            }
        }
        private void ShowMovie(Movie m)
        {
            Console.WriteLine($"{m.Title} {m.GetLength()} {m.Genre} {m.GetReleaseDate()} {m.WWW}");
        }

        private void ShowMovieList()
        {
            if (data.MovieList == null || data.MovieList.Count == 0) return;
            foreach (Movie m in data.MovieList)
            {
                ShowMovie(m);
            }
        }
        #endregion
        #region Series
        private void SeriesMenu()
        {
            Console.WriteLine("\nMOVIE MENU\n1 for list of series\n2 for search series\n3 for add new series");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    ShowSeriesList();
                    break;
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    SearchSeries();
                    break;
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    AddSeries();
                    break;
                default:
                    break;
            }
        }
        private void AddSeries()
        {
            Series series = new();
            //TODO Ask for input on series
            series.Title = GetString("Title: ");
            //series.Length = GetLength();
            series.Genre = GetString("Genre: ");
            series.ReleaseDate = GetReleaseDate();
            series.WWW = GetString("WWW: ");

            Console.WriteLine("Add series (Y/N)?");
            if (Console.ReadKey().Key == ConsoleKey.Y) data.SerieList.Add(series);

            Console.WriteLine("Add episode?");
            if (Console.ReadKey().Key == ConsoleKey.Y) AddEpisode(series);
        }
        private void AddEpisode(Series series)
        {
            do
            {
                Episode episode = new();
                episode.Title = GetString("Title: ");
                episode.Season = GetInt("Season: ");
                episode.EpisodeNum = GetInt("Episode number: ");
                episode.Length = GetLength();
                episode.ReleaseDate = GetReleaseDate();

                Console.WriteLine("Add episode (Y/N)?");
                if (Console.ReadKey().Key == ConsoleKey.Y) series.Episodes.Add(episode);
                Console.WriteLine("Add another episode?");
            }
            while (Console.ReadKey().Key == ConsoleKey.Y);  
        }
        private void SearchSeries()
        {
            Console.Write("Search: ");
            string? search = Console.ReadLine();
            foreach (Series series in data.SerieList)
            {
                if (search != null)
                {
                    if (series.Title.Contains(search) || series.Genre.Contains(search))
                        ShowSeries(series);
                }
            }
        }
        private void ShowSeries(Series s)
        {
            Console.WriteLine($"{s.Title}  {s.Genre} {s.GetReleaseDate()} {s.WWW}");
            foreach (Episode e in s.Episodes)
            {
                //TODO Show episode
                Console.WriteLine($"{e.Title}");
            }
        }

        private void ShowSeriesList()
        {
            foreach (Series s in data.SerieList)
            {
                ShowSeries(s);
            }
        }
        #endregion
        #region Music
        private void MusicMenu()
        {
            Console.WriteLine("\nMUSIC MENU\n1 for list music\n2 for search music\n3 for add music");
            switch (Console.ReadKey().Key)
            {

                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    ShowMusicList();
                    break;
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    break;
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    AddMusic();
                    break;
 

                default:
                    break;
            }

        }
        private void AddMusic()
        {
            Album album = new Album();
            album.Title = GetString("Album Title: ");
            album.Artist = GetString("Artist: ");
            album.Genre = GetString("Genre: ");
            album.ReleaseDate = GetReleaseDate();
            album.WWW = GetString("WWW: ");

            ShowAlbum(album);
            Console.WriteLine("Add album to list?");
            if (Console.ReadKey(true).Key == ConsoleKey.Y)
                data.MusicList.Add(album);

            Console.WriteLine("Add new song to album?");
            while (Console.ReadKey(true).Key == ConsoleKey.Y)
            {
                AddSong(album);
                Console.WriteLine("Add another song to album? (y/n)");
            };               
        }

        private void AddSong(Album album)
        {
            Song song = new Song();
            song.Title = GetString("Song Title: ");
            song.Artist = GetInputOrParent(album.Artist, "Artist: ");
            song.Genre = GetInputOrParent(album.Genre, "Genre: ");
            song.ReleaseDate = GetReleaseDate();
            song.Length = GetLength();
            Console.WriteLine("Add this song to album? (y/n)");
            if (Console.ReadKey(true).Key == ConsoleKey.Y)
                album.Songs.Add(song);
        }
        private string GetInputOrParent(string parent, string type)
        {
            Console.Write(type);
            string input = Console.ReadLine();
            if (input != "") parent = input;
            return parent;
        }

        private void ShowMusicList()
        {
            foreach (Album album in data.MusicList)
            {
                ShowAlbum(album, true);
            }
        }

        private void ShowAlbum(Album album, bool showSongs = false)
        {
            //TODO Show album details
            Console.WriteLine($"Album title: {album.Title}");
            if (showSongs)
            {
                foreach (Song song in album.Songs)
                {
                    ShowSong(song);
                }
            }
        }
        private void ShowSong(Song song)
        {
            //TODO Show song details
            Console.Write($"\tSong Title: {song.Title}\tLenght: {GetLength()}\t");
            //if (song.Artist != al)
        }

        #endregion
    }
}
