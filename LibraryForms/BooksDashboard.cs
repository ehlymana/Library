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
using BooksLibrary;
using System.IO;

namespace LibraryForms
{
    public partial class BooksDashboard : Form
    {
        PostgreSQL database;
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
        List<string[]> editData = new List<string[]>();
        public BooksDashboard(PostgreSQL db)
        {
            database = db;
            InitializeComponent();
            // initial connection to show all current rows
            this.RefreshData();
        }
        // handling refresh events
        public void RefreshData()
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
        #region Button Clicks
        // adding new book - takes us to a new form
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddNewBook addForm = new AddNewBook(database, this);
            addForm.Show();
            this.Hide();
        }
        // all edits saved in the list will be executed
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                database.connect();
                foreach (string[] edit in editData)
                {
                    database.editBookByColumn(edit[0], edit[1], edit[2]);
                }
                database.disconnect();
                this.RefreshData();
                statusLabel.Text = "Book successfully edited!";
            }
            catch
            {
                statusLabel.Text = "Error while editing book!";
            }
        }
        // selected book will be deleted
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                database.connect();
                database.deleteBook(dataGridView.SelectedRows[0].Cells["ID"].Value.ToString());
                database.disconnect();
                this.RefreshData();
                statusLabel.Text = "Book successfully deleted!";
            }
            catch
            {
                statusLabel.Text = "Error while deleting book!";
            }
        }
        // exporting all data as JSON
        private void buttonExport_Click(object sender, EventArgs e)
        {
            try
            {
                string JSON = "";
                JSON += "[";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JSON += "{";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        JSON += "\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString() + "\",";
                    }
                    JSON = JSON.Substring(0, JSON.Length - 1);
                    JSON += "},";
                }
                JSON = JSON.Substring(0, JSON.Length - 1);
                JSON += "]";
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        File.WriteAllText(fbd.SelectedPath + "\\exportedData.json", JSON);
                        statusLabel.Text = "Data successfully exported to " + fbd.SelectedPath + "\\exportedData.json!";
                    }
                }
                this.RefreshData();
            }
            catch
            {
                statusLabel.Text = "Error while exporting data!";
            }
        }
        // importing all data from JSON
        private void buttonImport_Click(object sender, EventArgs e)
        {
            try
            {
                using (var fbd = new OpenFileDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.FileName))
                    {
                        string JSON = File.ReadAllText(fbd.FileName);
                        string[] objects = JSON.Split('{');
                        database.connect();
                        database.deleteAllBooks();
                        database.disconnect();
                        foreach (string book in objects)
                        {
                            if (book.Length < 20) continue;
                            string id = "", name = "", author = "", publisher = "", releaseDate = "";
                            int i = 0;
                            while (book.Substring(i, 2) != "id") i++;
                            i += 5;
                            while (book.Substring(i, 1) != "\"")
                            {
                                id += book[i];
                                i++;
                            }
                            i += 10;
                            while (book.Substring(i, 1) != "\"")
                            {
                                name += book[i];
                                i++;
                            }
                            i += 12;
                            while (book.Substring(i, 1) != "\"")
                            {
                                author += book[i];
                                i++;
                            }
                            i += 15;
                            while (book.Substring(i, 1) != "\"")
                            {
                                publisher += book[i];
                                i++;
                            }
                            i += 17;
                            while (book.Substring(i, 1) != "\"")
                            {
                                releaseDate += book[i];
                                i++;
                            }
                            database.connect();
                            Author a = database.findAuthorByID(author);
                            if (a == null)
                            {
                                long authorID = database.addAuthor(new Author(author));
                                a = new Author(authorID, author);
                            }
                            database.addBook(new Book(name, a, publisher, new List<Genre>(), DateTime.Parse(releaseDate)));
                            database.disconnect();
                        }
                        statusLabel.Text = "Data successfully imported from " + fbd.FileName + "!";
                    }
                }
                this.RefreshData();
            }
            catch
            {
                statusLabel.Text = "Error while importing data!";
            }
        }
        // filtering books - takes us to a new form
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            SearchBooks searchForm = new SearchBooks(database, this);
            searchForm.Show();
            this.Hide();
        }
        // exiting the dashboard form means exiting the application
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
        #region Events
        // when data is edited, we need to add the edited data into a list so that we can make the change happen
        // upon button click or double click
        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string column = dataGridView.Columns[e.ColumnIndex].Name;
            string value = dataGridView.Rows[e.RowIndex].Cells[column].Value.ToString();
            string ID = dataGridView.Rows[e.RowIndex].Cells["ID"].Value.ToString();
            editData.Add(new string[] { column, value, ID });
        }
        // database change happening upon double click on edited row
        private void dataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string ID = dataGridView.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                string name = dataGridView.Rows[e.RowIndex].Cells["NAME"].Value.ToString();
                string author = dataGridView.Rows[e.RowIndex].Cells["AUTHOR"].Value.ToString();
                string publisher = dataGridView.Rows[e.RowIndex].Cells["PUBLISHER"].Value.ToString();
                string releaseDate = dataGridView.Rows[e.RowIndex].Cells["RELEASEDATE"].Value.ToString();
                database.connect();
                database.editBook(new Book(Int32.Parse(ID), name, new Author(Int32.Parse(author), "Name"), publisher, new List<Genre>(), DateTime.Parse(releaseDate)));
                database.disconnect();
                statusLabel.Text = "Book successfully edited!";
            }
            catch
            {
                statusLabel.Text = "Error while editing book!";
            }
        }
        #endregion
    }
}
