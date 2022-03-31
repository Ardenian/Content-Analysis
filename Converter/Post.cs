using System;
using System.Text;
using System.Xml.Serialization;

namespace Converter
{
    [Serializable]
    public struct Post
    {
        private string username;
        private string title;
        private string[] postTypes;
        private string[] genderTags;
        private string[] userTags;
        private string contentURL;
        private long timestamp;

        public Post(string username, string title, string[] postType, string[] genderTags, string[] userTags, string contentURL, long timestamp)
        {
            this.username = username;
            this.title = title;
            this.postTypes = postType;
            this.genderTags = genderTags;
            this.userTags = userTags;
            this.contentURL = contentURL;
            this.timestamp = timestamp;
        }

        public string Username { get => username; }
        public string Title { get => title; }
        public string[] PostTypes { get => postTypes; }
        public string[] GenderTags { get => genderTags; }
        public string[] UserTags { get => userTags; }
        public string ContentURL { get => contentURL; }
        public long Timestamp { get => timestamp; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Username: ");
            builder.AppendLine(username);

            builder.Append("Title: ");
            builder.AppendLine(title);

            builder.Append("Post Types: ");
            for (int postTypesIndex = 0; postTypesIndex < postTypes.Length; postTypesIndex++)
            {
                if (postTypesIndex > 0 && postTypesIndex < postTypes.Length) { builder.Append(", "); }
                builder.Append(postTypes[postTypesIndex]);
            }
            builder.AppendLine();

            builder.Append("Gender Tags: ");
            for (int genderTagIndex = 0; genderTagIndex < genderTags.Length; genderTagIndex++)
            {
                if (genderTagIndex > 0 && genderTagIndex < genderTags.Length) { builder.Append(", "); }
                builder.Append(genderTags[genderTagIndex]);
            }
            builder.AppendLine();

            builder.Append("User Tags: ");
            for (int userTagIndex = 0; userTagIndex < userTags.Length; userTagIndex++)
            {
                if (userTagIndex > 0 && userTagIndex < userTags.Length) { builder.Append(", "); }
                builder.Append(userTags[userTagIndex]);
            }
            builder.AppendLine();

            builder.Append("Content URL: ");
            builder.AppendLine(contentURL);

            builder.Append("Time: ");
            builder.AppendLine(DateTimeOffset.FromUnixTimeSeconds(timestamp).ToString());

            return builder.ToString();
        }
    }
}