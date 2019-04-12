using System;
using System.Collections.Generic;
using System.Text;

namespace BooksLibrary
{
    public class Author
    {
        #region Attributes
        long authorID;
        string name;
        #endregion
        #region Properties
        public long AuthorID { get => authorID; set => authorID = value; }
        public string Name { get => name; set => name = value; }
        #endregion
        #region Constructor
        public Author(long id, string name)
        {
            AuthorID = id;
            Name = name;
        }
        public Author(string name)
        {
            Name = name;
        }
        #endregion
    }
}
