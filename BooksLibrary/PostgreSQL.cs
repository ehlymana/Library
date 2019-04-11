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

        public void createBooksTable()
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS BOOKS(" +
                    "ID SERIAL PRIMARY KEY," +
                    "NAME TEXT NOT NULL," +
                    "AUTHOR TEXT NOT NULL," +
                    "GENRES TEXT NOT NULL," +
                    "PUBLISHER TEXT," +
                    "RELEASEDATE DATE" +
                    ");";
                cmd.ExecuteNonQuery();
            }
        }
        public void addBook(Book b)
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                string allGenres = "";
                foreach (string genre in b.Genres)
                {
                    allGenres += genre + ",";
                }
                allGenres = allGenres.Substring(0, allGenres.Length - 1);
                cmd.CommandText = "INSERT INTO BOOKS " +
                    "(NAME, AUTHOR, GENRES, PUBLISHER, RELEASEDATE) "
                    + "VALUES ('" + b.Name + "', '" + b.Author + "', '"
                    + allGenres + "', '" + b.Publisher + "', '" + b.ReleaseDate.Year + "-"
                    + b.ReleaseDate.Month + "-" + b.ReleaseDate.Day + "');";
                cmd.ExecuteNonQuery();
            }
        }

        public void editBook(Book oldBook, Book newBook)
        {
            using (NpgsqlCommand cmd = Connection.CreateCommand())
            {
                string allGenres = "";
                foreach (string genre in newBook.Genres)
                {
                    allGenres += genre + ",";
                }
                cmd.CommandText = "UPDATE BOOKS " +
                    "SET NAME = '" + newBook.Name + "', AUTHOR = '" + newBook.Author
                    + "', GENRES = '" + allGenres + "', PUBLISHER = '" + newBook.Publisher
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
        #endregion
    }
}
