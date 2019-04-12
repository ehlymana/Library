using System;
using System.Collections.Generic;
using System.Text;

namespace BooksLibrary
{
    public class Genre
    {
        #region Attributes
        long genreID;
        string name;
        #endregion
        #region Properties
        public long GenreID { get => genreID; set => genreID = value; }
        public string Name { get => name; set => name = value; }
        #endregion
        #region Constructors

        public Genre(long id, string name)
        {
            GenreID = id;
            Name = name;
        }
        public Genre (string name)
        {
            Name = name;
        }
        #endregion
    }
}
