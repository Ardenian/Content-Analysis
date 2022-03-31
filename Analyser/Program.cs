using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Analyser
{
    struct TimedPost
    {
        public DateTime time;
        public string tag;

        public TimedPost(DateTime time, string tag)
        {
            this.time = time;
            this.tag = tag;
        }

        public override string ToString()
        {
            return "{" + tag + "," + time.ToString() + "}";
        }
    }

    struct SortedPost
    {
        public string tag;
        public int posts;
        public int year;
        public int month;

        public SortedPost(string tag, int posts, int year, int month)
        {
            this.tag = tag;
            this.posts = posts;
            this.year = year;
            this.month = month;
        }

        public override string ToString()
        {

            return tag+";"+posts+";01." + month +"."+ year+";";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder builder = new StringBuilder();
            XmlDocument document = new XmlDocument();
            string filePath = @"###################################################.xml";
            document.Load(filePath);

            var file = File.Create(@"###################################################.txt");

            List<TimedPost> timedPosts = new List<TimedPost>();
            var posts = document.SelectNodes("Posts/Post");
            builder.AppendLine($"Successfully validated posts: {posts.Count}");

            foreach (XmlNode post in posts)
            {
                DateTime time = Convert.ToDateTime(post.SelectSingleNode("Time").InnerText);
                foreach (XmlNode tag in post.SelectNodes("Targets/Target"))
                {
                    timedPosts.Add(new TimedPost(time, tag.InnerText));
                }
            }

            var sortedList = timedPosts.GroupBy(item => new {item.time.Year, item.time.Month, item.tag}).Select(group => new SortedPost(group.Key.tag, group.Count(), group.Key.Year, group.Key.Month));

            string[] tags = new string[] { "###", "###", "###", "###", "###", "###", "###" };
            foreach (var item in sortedList)
            {
                if (tags.Contains(item.tag))
                {
                    builder.AppendLine(item.ToString());
                }          
            }

            StreamWriter writer = new StreamWriter(file);
            Print(ref document, ref builder, "###", "#########");
            writer.Write(builder.ToString());
            writer.Close();

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static void Print(ref XmlDocument document, ref StringBuilder builder, string target, string keyword)
        {
            var nodes = document.SelectNodes($"Posts/Post[./Targets/Target/text() = '{target}' and ./Tags/Tag/text() = '{keyword}']");
            for (int nodeIndex = 0; nodeIndex < nodes.Count; nodeIndex++)
            {
                var node = nodes[nodeIndex];
                builder.AppendLine(XElement.Parse(node.OuterXml).ToString());
            }
        }
    }
}
