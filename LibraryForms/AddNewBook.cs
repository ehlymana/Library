﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BooksLibrary;

namespace LibraryForms
{
    public partial class AddNewBook : Form
    {
        List<bool> validated = new List<bool> { false, false, false, false };
        PostgreSQL database;
        BooksDashboard initialForm;
        public AddNewBook(PostgreSQL existingDatabase, BooksDashboard form)
        {
            initialForm = form;
            database = existingDatabase;
            InitializeComponent();
        }
        #region ButtonClicks
        // when user enters a new genre it is added to the list 
        private void buttonAddGenre_Click(object sender, EventArgs e)
        {
            listBoxGenres.Items.Add(textBoxGenre.Text);
            listBoxGenres.Refresh();
        }
        // user properly entered book data - the book can be added
        private void buttonAddBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Validation.isNotEmpty(textBoxPublisher.Text)) validated[2] = true;
                // first we need to check whether everything is valid
                if (validated.Contains(false)) return;
                database.connect();
                Author a = database.findAuthorByName(textBoxAuthor.Text);
                database.disconnect();
                if (a == null)
                {
                    database.connect();
                    long id = database.addAuthor(new Author(textBoxAuthor.Text));
                    database.disconnect();
                    a = new Author(id, textBoxAuthor.Text);
                }
                List<Genre> genres = new List<Genre>();
                foreach (string item in listBoxGenres.Items)
                {
                    database.connect();
                    Genre genre = database.findGenreByName(item);
                    database.disconnect();
                    if (genre == null)
                    {
                        database.connect();
                        long id = database.addGenre(new Genre(item));
                        database.disconnect();
                        genres.Add(new Genre(id, item));
                    }
                    else genres.Add(genre);
                }
                Book newBook = new Book(textBoxName.Text, a, textBoxPublisher.Text, genres, dateTimePicker.Value);
                database.connect();
                database.addBook(newBook);
                database.disconnect();
                MessageBox.Show("Book successfully added!", "Message from Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
                initialForm.RefreshData();
                initialForm.Show();
                this.Close();
            }
            catch
            {
                statusLabel.Text = "Error while adding book!";
            }
        }
        // refresh all data on form (delete all input)
        private void buttonReset_Click(object sender, EventArgs e)
        {
            // delete all inputs
            textBoxName.Text = "";
            textBoxAuthor.Text = "";
            textBoxPublisher.Text = "";
            dateTimePicker.Value = DateTime.Now;
            textBoxGenre.Text = "";
            listBoxGenres.Items.Clear();
            // if validation was incorrect, return everything to normal
            errorProvider1.Clear();
            statusLabel.Text = "";
            textBoxName.BackColor = Color.White;
            textBoxAuthor.BackColor = Color.White;
            textBoxPublisher.BackColor = Color.White;
            textBoxGenre.BackColor = Color.White;
            validated = new List<bool> { false, false, false, false };
        }
        // go back to dashboard
        private void buttonExit_Click(object sender, EventArgs e)
        {
            AutoValidate = AutoValidate.Disable;
            this.Close();
            initialForm.RefreshData();
            initialForm.Show();
        }
        #endregion
        #region Validation
        private void textBoxName_Validating(object sender, CancelEventArgs e)
        {
            string message = "";
            if (!Validation.isNotEmpty(textBoxName.Text))
            {
                message = "The provided name must not be empty!";
            }
            else if (!Validation.isAValidName(textBoxName.Text))
            {
                message = "The provided name should consist only of words beginning with uppercase letters!";
            }
            if (message.Length > 0)
            {
                e.Cancel = true;
                errorProvider1.SetError(textBoxName, message);
                statusLabel.Text = "Error in validation of book name!";
                textBoxName.BackColor = Color.LightCoral;
            }
        }
        private void textBoxAuthor_Validating(object sender, CancelEventArgs e)
        {
            string message = "";
            if (!Validation.isNotEmpty(textBoxAuthor.Text))
            {
                message = "The provided author name must not be empty!";
            }
            else if (!Validation.isAValidName(textBoxAuthor.Text))
            {
                message = "The provided author name should consist only of words beginning with uppercase letters!";
            }
            if (message.Length > 0)
            {
                e.Cancel = true;
                errorProvider1.SetError(textBoxAuthor, message);
                statusLabel.Text = "Error in validation of book author!";
                textBoxAuthor.BackColor = Color.LightCoral;
            }
        }

        private void textBoxPublisher_Validating(object sender, CancelEventArgs e)
        {
            string message = "";
            if (!Validation.isNotEmpty(textBoxPublisher.Text))
            {
                validated[2] = true;
            }
            else if (!Validation.isAValidString(textBoxPublisher.Text))
            {
                message = "The provided name should consist only of letters and one of these special characters:  ,.-!";
            }
            if (message.Length > 0)
            {
                e.Cancel = true;
                errorProvider1.SetError(textBoxPublisher, message);
                statusLabel.Text = "Error in validation of book publisher!";
                textBoxPublisher.BackColor = Color.LightCoral;
            }
        }
        private void textBoxGenre_Validating(object sender, CancelEventArgs e)
        {
            string message = "";
            if (!Validation.isNotEmpty(textBoxGenre.Text))
            {
                message = "The provided genre must not be empty!";
            }
            if (message.Length > 0)
            {
                e.Cancel = true;
                errorProvider1.SetError(textBoxGenre, message);
                statusLabel.Text = "Error in validation of book genres!";
                textBoxGenre.BackColor = Color.LightCoral;
            }
        }
        private void textBoxName_Validated(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            textBoxName.BackColor = Color.White;
            statusLabel.Text = "";
            validated[0] = true;
        }

        private void textBoxAuthor_Validated(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            textBoxAuthor.BackColor = Color.White;
            statusLabel.Text = "";
            validated[1] = true;
        }

        private void textBoxPublisher_Validated(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            textBoxPublisher.BackColor = Color.White;
            statusLabel.Text = "";
            validated[2] = true;
        }
        private void textBoxGenre_Validated(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            textBoxGenre.BackColor = Color.White;
            statusLabel.Text = "";
            validated[3] = true;
        }
        #endregion
    }
}
