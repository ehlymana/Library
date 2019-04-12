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
            database.connect();
            foreach (string [] edit in editData)
            {
                database.editBookByColumn(edit[0], edit[1], edit[2]);
            }
            database.disconnect();
            this.RefreshData();
        }
        // selected book will be deleted
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            database.connect();
            database.deleteBook(dataGridView.SelectedRows[0].Cells["ID"].Value.ToString());
            database.disconnect();
            this.RefreshData();
        }
        // exporting all data as JSON
        private void buttonExport_Click(object sender, EventArgs e)
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
                    MessageBox.Show("Data successfully exported to " + fbd.SelectedPath + "\\exportedData.json!", "Message from Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            this.RefreshData();
        }
        // importing all data from JSON
        private void buttonImport_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string JSON = File.ReadAllText(fbd.SelectedPath);
                    string[] objects = JSON.Split('{');
                    database.connect();
                    database.deleteAllBooks();
                    database.disconnect();
                    foreach (string book in objects)
                    {
                        string id = "", name = "", author = "", publisher = "", releaseDate = "";
                        int i = 0;
                        while (book.Substring(i, i + 2) != "id") i++;
                        i += 5;
                        while (book.Substring(i, i + 1) != "\"")
                        {
                            id += book[i];
                            i++;
                        }
                        i += 10;
                        while (book.Substring(i, i + 1) != "\"")
                        {
                            name += book[i];
                            i++;
                        }
                        i += 12;
                        while (book.Substring(i, i + 1) != "\"")
                        {
                            author += book[i];
                            i++;
                        }
                        i += 15;
                        while (book.Substring(i, i + 1) != "\"")
                        {
                            publisher += book[i];
                            i++;
                        }
                        i += 17;
                        while (book.Substring(i, i + 1) != "\"")
                        {
                            releaseDate += book[i];
                            i++;
                        }
                        database.connect();
                        Author a = database.findAuthorByName(author);
                        if (a == null)
                        {
                            long authorID = database.addAuthor(new Author(author));
                            a = new Author(authorID, author);
                        }
                        database.deleteAllBooks();
                        database.addBook(new Book(name, a, publisher, null, DateTime.Parse(releaseDate)));
                        database.disconnect();
                    }
                    MessageBox.Show("Data successfully imported from " + fbd.SelectedPath + "!", "Message from Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            this.RefreshData();
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
        // database change happening upon double click on edited content
        private void dataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string column = dataGridView.Columns[e.ColumnIndex].Name;
            string value = dataGridView.Rows[e.RowIndex].Cells[column].Value.ToString();
            string ID = dataGridView.Rows[e.RowIndex].Cells["ID"].Value.ToString();
            database.connect();
            database.editBookByColumn(column, value, ID);
            database.disconnect();
        }
        #endregion
    }
}
