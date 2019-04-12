using System;
using System.Collections.Generic;

namespace BooksLibrary
{
    public class Book
    {
        #region Attributes
        long bookID;
        string name, publisher;
        Author author;
        List<Genre> genres = new List<Genre>();
        DateTime releaseDate;
        #endregion
        #region Properties
        public long BookID {
            get
            {
                return bookID;
            }
            set
            {
                bookID = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (Validation.isAValidName(value)) name = value;
                else throw new System.InvalidOperationException("The provided string is not a valid name!");
            }
        }
        public Author Author {
            get
            {
                return author;
            }
            set
            {
                if (Validation.isAValidName(value.Name)) author = value;
                else throw new System.InvalidOperationException("The provided string is not a valid name!");
            }
        }
        public string Publisher {
            get
            {
                return publisher;
            }
            set
            {
                if (Validation.isAValidString(value) || !Validation.isNotEmpty(value)) publisher = value;
                else throw new System.InvalidOperationException("The provided string is not a valid string!");
            }
        }
        public List<Genre> Genres {
            get
            {
                return genres;
            }
            set
            {
                foreach (Genre genre in value)
                {
                    if (!Validation.isNotEmpty(genre.Name)) throw new System.InvalidOperationException("The provided string is not a valid genre!");
                }
                genres = value;
            }
        }
        public DateTime ReleaseDate {
            get
            {
                return releaseDate;
            }
            set
            {
                releaseDate = value;
            }
        }
        #endregion
        #region Constructor
        public Book(long id, string name, Author author, string publisher, List<Genre> genres, DateTime releaseDate)
        {
            BookID = id;
            Name = name;
            Author = author;
            Publisher = publisher;
            Genres = genres;
            ReleaseDate = releaseDate;
        }
        public Book (string name, Author author, string publisher, List<Genre> genres, DateTime releaseDate)
        {
            Name = name;
            Author = author;
            Publisher = publisher;
            Genres = genres;
            ReleaseDate = releaseDate;
        }
        #endregion
    }
}
