using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BooksLibrary
{
    public class PostgreSQL
    {
        #region Attributes
        string username, password, server, port;
        NpgsqlConnection connection;
        #endregion
        #region Properties
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Server { get => server; set => server = value; }
        public string Port { get => port; set => port = value; }
        public NpgsqlConnection Connection { get => connection; set => connection = value; }
        #endregion
        #region Constructor
        public PostgreSQL (string username, string password, string server, string port)
        {
            Username = username;
            Password = password;
            Server = server;
            Port = port;
        }
        #endregion
        #region Methods
        public void connect()
        {
            String constring = "Server=" + Server + ";port=" + Port + ";username='" + Username + "';password='" + Password + "'";
            Connection = new NpgsqlConnection(constring);
            Connection.Open();
        }

        public void disconnect()
        {
            Connection.Close();
        }
        public void createAllTables()
        {
            createBooksTable();
            createGenresTable();
            createAuthorsTable();
            createGenresToBooksTable();
        }
        public void createBooksTable()
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS BOOKS(" +
                    "ID SERIAL PRIMARY KEY," +
                    "NAME TEXT NOT NULL," +
                    "AUTHOR INT REFERENCES AUTHORS(ID) NOT NULL," +
                    "PUBLISHER TEXT," +
                    "RELEASEDATE DATE" +
                    ");";
                cmd.ExecuteNonQuery();
            }
        }

        public void createGenresTable()
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS GENRES(" +
                    "ID SERIAL PRIMARY KEY," +
                    "NAME TEXT NOT NULL" +
                    ");";
                cmd.ExecuteNonQuery();
            }
        }

        public void createAuthorsTable()
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS AUTHORS(" +
                    "ID SERIAL PRIMARY KEY," +
                    "NAME TEXT NOT NULL" +
                    ");";
                cmd.ExecuteNonQuery();
            }
        }

        public void createGenresToBooksTable()
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS GENRES_TO_BOOKS(" +
                    "BOOK_ID INT REFERENCES BOOKS(ID) NOT NULL," +
                    "GENRE_ID INT REFERENCES GENRES(ID) NOT NULL" +
                    ");";
                cmd.ExecuteNonQuery();
            }
        }
        public void addBook(Book b)
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                // first part - insert the book into the table
                cmd.CommandText = "INSERT INTO BOOKS " +
                    "(NAME, AUTHOR, PUBLISHER, RELEASEDATE) "
                    + "VALUES ('" + b.Name + "', '" + b.Author.AuthorID + "', '"
                    + b.Publisher + "', '" + b.ReleaseDate.Year + "-"
                    + b.ReleaseDate.Month + "-" + b.ReleaseDate.Day + "')" +
                    "  RETURNING ID;";
                long id = cmd.ExecuteNonQuery();
                // second part - insert all of the corellations between genres and the book
                foreach (Genre g in b.Genres) {
                    cmd.CommandText = "INSERT INTO GENRES_TO_BOOKS " +
                        "(BOOK_ID, GENRE_ID) "
                        + "VALUES (" + id + ", " + g.GenreID + ");";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int addGenre(Genre g)
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO GENRES " +
                    "(NAME) "
                    + "VALUES ('" + g.Name+ "')" +
                    "  RETURNING ID;";
                return cmd.ExecuteNonQuery();
            }
        }

        public int addAuthor(Author a)
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO AUTHORS " +
                    "(NAME) "
                    + "VALUES ('" + a.Name + "')" +
                    "  RETURNING ID;";
                return cmd.ExecuteNonQuery();
            }
        }

        public void editBook(Book oldBook, Book newBook)
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                string allGenres = "";
                foreach (Genre genre in newBook.Genres)
                {
                    allGenres += genre + ",";
                }
                cmd.CommandText = "UPDATE BOOKS " +
                    "SET NAME = '" + newBook.Name + "', AUTHOR = '" + newBook.Author.AuthorID
                    + "', PUBLISHER = '" + newBook.Publisher
                    + "', RELEASEDATE = '" + newBook.ReleaseDate.Year + "-"
                    + newBook.ReleaseDate.Month + "-" + newBook.ReleaseDate.Day + "'"
                    + " WHERE ID = " + oldBook.BookID + ";";
                cmd.ExecuteNonQuery();
            }
        }
        public void editBookByColumn(string column, string newValue, string ID)
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "UPDATE BOOKS "
                    + "SET " + column + "= '" + newValue
                    + "' WHERE ID = " + ID + ";";
                cmd.ExecuteNonQuery();
            }

        }
        public bool deleteBook(string ID)
        {
            int rowsAffected = 0;
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM BOOKS " +
                    "WHERE ID = " + ID + ";";
                rowsAffected = cmd.ExecuteNonQuery();
            }
            return rowsAffected > 0;
        }

        public void deleteAllBooks()
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM BOOKS;";
                cmd.ExecuteNonQuery();
            }
        }

        public List<Genre> findAllGenres()
        {
            List<Genre> genres = new List<Genre>();
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM GENRES;";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    genres.Add(new Genre((long)dr[0], (string)dr[1]));
                }
            }
            return genres;
        }

        public List<Author> findAllAuthors()
        {
            List<Author> authors = new List<Author>();
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM AUTHORS;";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    authors.Add(new Author((long)dr[0], (string)dr[1]));
                }
            }
            return authors;
        }

        public Genre findGenreByName(string name)
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM GENRES" +
                    " WHERE NAME = '" + name + "';";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                try
                {
                    dr.Read();
                    Genre g = new Genre((long)dr[0], (string)dr[1]);
                    return g;
                }
                catch
                {
                    return null;
                }
            }
        }

        public Author findAuthorByName(string name)
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM AUTHORS" +
                    " WHERE NAME = '" + name + "';";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                try
                {
                    dr.Read();
                    Author a = new Author((long)dr[0], (string)dr[1]);
                    return a;
                }
                catch
                {
                    return null;
                }
            }
        }

        public List<long> findBooksByGenre(Genre g)
        {
            List<long> ids = new List<long>();
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM GENRES_TO_BOOKS" +
                    " WHERE GENRE_ID = '" + g.GenreID + "';";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        if (!ids.Contains((long)dr[0])) {
                            ids.Add((long)dr[0]);
                        }
                    }
                    return ids;
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion
    }
}
