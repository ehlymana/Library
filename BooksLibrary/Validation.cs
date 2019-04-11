using System;
using System.Collections.Generic;
using System.Text;

namespace BooksLibrary
{
    public class Validation
    {
        // this method only checks whether the string value is empty
        public static bool isNotEmpty (string value)
        {
            return value.Length != 0;
        }
        // this method checks whether the string consists only of letters and separators
        public static bool isAValidName (string value)
        {
            if (value.Length == 0) return false;
            // break the name into words
            string[] words = value.Split(' ');
            foreach (string word in words)
            {
                // check whether the word begins with an uppercase letter
                // only English characters are allowed
                if (word[0] < 'A' || word[0] > 'Z') return false;
                foreach (char character in word.Substring(1))
                {
                    // check whether all the other characters of the word are lowercase letters
                    // only English characters are allowed
                    if (character < 'a' || character > 'z') return false;
                }
            }
            return true;
        }
        // this method only checks whether the string consists of standard inputs, without the uppercase-lowercase restrictions
        public static bool isAValidString (string value)
        {
            if (value.Length == 0) return false;
            List<char> allowedSpecialCharacters = new List<char> { ' ', ',' , '.', '-'};
            foreach (char character in value)
            {
                // first check whether characters are allowed special characters
                bool isASpecialCharacter = false;
                foreach (char special in allowedSpecialCharacters)
                {
                    if (character == special)
                    {
                        isASpecialCharacter = true;
                        break;
                    }
                }
                // character neither an allowed letter, neither an allowed special character - is not valid
                if (!isASpecialCharacter && (character < 'A' || character > 'Z') && (character < 'a' || character > 'z')) return false;
            }
            return true;
        }
    }
}
