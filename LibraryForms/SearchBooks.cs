using BooksLibrary;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryForms
{
    public partial class SearchBooks : Form
    {
        PostgreSQL database;
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
        BooksDashboard initialForm;
        string genreName = "";
        string authorName = "";
        Author author;
        Genre genre;
        List<long> ids;
        public SearchBooks(PostgreSQL existingDatabase, BooksDashboard form)
        {
            initialForm = form;
            database = existingDatabase;
            database.connect();
            List<Genre> genres = database.findAllGenres();
            List<Author> authors = database.findAllAuthors();
            database.disconnect();
            foreach (Genre g in genres)
            {
                listBoxGenre.Items.Add(g.Name);
            }
            foreach (Author a in authors)
            {
                listBoxAuthor.Items.Add(a.Name);
            }
            RefreshAllData();
            InitializeComponent();
        }
        // shows all data in the books table
        public void RefreshAllData()
        {
            database.connect();
            database.createAllTables();
            string query = "SELECT * FROM BOOKS";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, database.Connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dataGridView.DataSource = dt;
            database.disconnect();
        }
        // filters data depending on the selected author and genre
        public void FilterData()
        {
            database.connect();
            string query = "SELECT * FROM BOOKS WHERE AUTHOR = " + author.AuthorID
                + "AND ID = ";
            foreach (long id in ids)
            {
                query += id + " OR ID = ";
            }
            query = query.Substring(0, query.Length - 9) + ";";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, database.Connection);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dataGridView.DataSource = dt;
            database.disconnect();
        }

        // go back to dashboard
        private void buttonDashboard_Click(object sender, EventArgs e)
        {
            initialForm.RefreshData();
            initialForm.Show();
            this.Close();
        }
        // filter books if both author and genre are selected
        private void buttonFilter_Click(object sender, EventArgs e)
        {
            authorName = listBoxAuthor.SelectedItem.ToString();
            genreName = listBoxGenre.SelectedItem.ToString();
            // both author and genre selected - show filtered data
            if (authorName.Length > 0 && genreName.Length > 0)
            {
                database.connect();
                genre = database.findGenreByName(genreName);
                author = database.findAuthorByName(authorName);
                ids = database.findBooksByGenre(genre);
                database.disconnect();
                FilterData();
            }
            // not both are selected - show all data
            else RefreshAllData();
        }
    }
}
