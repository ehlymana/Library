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
            createAuthorsTable();
            createGenresTable();
            createBooksTable();
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
            int id = 0;
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                // first part - insert the book into the table
                cmd.CommandText = "INSERT INTO BOOKS " +
                    "(NAME, AUTHOR, PUBLISHER, RELEASEDATE) "
                    + "VALUES ('" + b.Name + "', '" + b.Author.AuthorID + "', '"
                    + b.Publisher + "', '" + b.ReleaseDate.Year + "-"
                    + b.ReleaseDate.Month + "-" + b.ReleaseDate.Day + "') RETURNING ID;";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                id = (int)dr[0];
                dr.Close();
            }
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                // second part - insert all of the corellations between genres and the book
                if (b.Genres.Count == 0) return;
                cmd.CommandText = "INSERT INTO GENRES_TO_BOOKS " +
                        "(BOOK_ID, GENRE_ID) "
                        + "VALUES ";
                foreach (Genre g in b.Genres)
                {
                    cmd.CommandText += "(" + id + ", " + g.GenreID + "),";
                }
                cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 1) + ";";
                cmd.ExecuteNonQuery();
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
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT ID FROM GENRES WHERE NAME = '" + g.Name + "';";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                return (int)dr[0];
            }
        }

        public int addAuthor(Author a)
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO AUTHORS " +
                    "(NAME) "
                    + "VALUES ('" + a.Name + "');";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT ID FROM AUTHORS WHERE NAME = '" + a.Name + "';";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                return (int)dr[0];
            }
        }

        public void editBook(Book book)
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "UPDATE BOOKS " +
                    "SET NAME = '" + book.Name + "', AUTHOR = '" + book.Author.AuthorID
                    + "', PUBLISHER = '" + book.Publisher
                    + "', RELEASEDATE = '" + book.ReleaseDate.Year + "-"
                    + book.ReleaseDate.Month + "-" + book.ReleaseDate.Day + "'"
                    + " WHERE ID = " + book.BookID + ";";
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
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM GENRES_TO_BOOKS " +
                    "WHERE BOOK_ID = " + ID + ";";
                cmd.ExecuteNonQuery();
            }
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
                cmd.CommandText = "DELETE FROM GENRES_TO_BOOKS;";
                cmd.ExecuteNonQuery();
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
                    genres.Add(new Genre((int)dr[0], (string)dr[1]));
                }
                dr.Close();
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
                    authors.Add(new Author((int)dr[0], (string)dr[1]));
                }
                dr.Close();
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
                    Genre g = new Genre((int)dr[0], (string)dr[1]);
                    dr.Close();
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
                    Author a = new Author((int)dr[0], (string)dr[1]);
                    dr.Close();
                    return a;
                }
                catch
                {
                    return null;
                }
            }
        }

        public Author findAuthorByID(string ID)
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM AUTHORS" +
                    " WHERE ID = '" + ID + "';";
                NpgsqlDataReader dr = cmd.ExecuteReader();
                try
                {
                    dr.Read();
                    Author a = new Author((int)dr[0], (string)dr[1]);
                    dr.Close();
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
                        if (!ids.Contains((int)dr[0])) {
                            ids.Add((int)dr[0]);
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
