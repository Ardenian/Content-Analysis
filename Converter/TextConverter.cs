using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq;

namespace Converter
{
    class TextConverter
    {
        private const int TITLE_SECTION = 0;
        private const int NAME_SECTION = 1;
        private const int URL_SECTION = 2;
        private const int TIME_SECTION = 3;

        public bool Create(string text, out Post post)
        {
            string[] separatingStrings = new string[] { ":;:" };
            string[] textSections = text.Split(separatingStrings, StringSplitOptions.RemoveEmptyEntries);

            if (textSections.Length == 4 && text.All(c => !char.IsControl(c)))
            {
                string username = textSections[NAME_SECTION];
                string title = textSections[TITLE_SECTION];
                string[] postTypes = FindPostType(textSections[TITLE_SECTION]);
                string[] genderTags = FindGenderTags(textSections[TITLE_SECTION]);
                string[] userTags = FindUserTags(textSections[TITLE_SECTION]);
                string contentURL = textSections[URL_SECTION];
                long timestamp = Convert.ToInt64(textSections[TIME_SECTION]);

                post = new Post(username, title, postTypes, genderTags, userTags, contentURL, timestamp);

                return true;
            } else
            {
                post = default(Post);
                return false;
            }
        }

        private string[] FindPostType(string text)
        {
            List<string> results = new List<string>();
            string[] values = { "###", "###", "###", "###", "###", "###", "###", "###", "###" };
            for (int index = 0; index < values.Length; index++)
            {
                string value = values[index];
                if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(text, value, CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) >= 0)
                {
                    results.Add(value);
                }
            }

            return results.ToArray();
        }

        private string[] FindGenderTags(string text)
        {
            List<string> results = new List<string>();
            Regex regex = new Regex(@"\[([\w\d]+4[\w\d]+)\]");
            MatchCollection matches = regex.Matches(text);
            for (int matchIndex = 0; matchIndex < matches.Count; matchIndex++)
            {
                GroupCollection groups = matches[matchIndex].Groups;
                for (int index = 1; index < groups.Count; index++)
                {
                    Capture capture = groups[index];
                    results.Add(capture.Value.ToLower());
                }
            }


            return results.ToArray();
        }

        private string[] FindUserTags(string text)
        {
            List<string> results = new List<string>();
            Regex regex = new Regex(@"\[([\s\w\d]+)\]");
            MatchCollection matches = regex.Matches(text);
            for (int matchIndex = 0; matchIndex < matches.Count; matchIndex++)
            {
                GroupCollection groups = matches[matchIndex].Groups;
                for (int index = 1; index < groups.Count; index++)
                {
                    Capture capture = groups[index];
                    if (!Regex.IsMatch(capture.Value, @"([\w\d]+4[\w\d]+)"))
                    {
                        results.Add(capture.Value.ToLower());
                    }        
                }
            }


            return results.ToArray();
        }
    }
}
