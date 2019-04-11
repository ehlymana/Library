using System;
using System.Collections.Generic;

namespace BooksLibrary
{
    public class Book
    {
        #region Attributes
        long bookID;
        string name, author, publisher;
        List<string> genres = new List<string>();
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
        public string Author {
            get
            {
                return author;
            }
            set
            {
                if (Validation.isAValidName(value)) author = value;
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
                if (Validation.isAValidString(value)) publisher = value;
                else throw new System.InvalidOperationException("The provided string is not a valid string!");
            }
        }
        public List<string> Genres {
            get
            {
                return genres;
            }
            set
            {
                foreach (string genre in value)
                {
                    if (!Validation.isNotEmpty(genre)) throw new System.InvalidOperationException("The provided string is not a valid genre!");
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
        public Book (string name, string author, string publisher, List<string> genres, DateTime releaseDate)
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
