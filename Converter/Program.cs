using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] filePaths = Directory.GetFiles(@"###################################################", "xa*");
            Console.WriteLine($"Found {filePaths.Length} files for conversion.");

            TextConverter converter = new TextConverter();
            FileStream file = null;

            XDocument document = new XDocument(new XElement("Posts"));

            try
            {
                file = File.Create(@"###################################################.xml");

                for (int fileIndex = 0; fileIndex < filePaths.Length; fileIndex++)
                {
                    TextReader textReader = null;
                    try
                    {
                        textReader = new StreamReader(filePaths[fileIndex]);

                        Post post;
                        string line;
                        while ((line = textReader.ReadLine()) != null)
                        {
                            if (converter.Create(line, out post))
                            {
                                XElement node = new XElement("Post");
                                node.Add(new XElement("Username", post.Username));
                                node.Add(new XElement("Title", post.Title));

                                var postTypeNode = new XElement("Types");
                                var postTypes = post.PostTypes;
                                for (int postTypeIndex = 0; postTypeIndex < postTypes.Length; postTypeIndex++)
                                {
                                    var type = postTypes[postTypeIndex];
                                    if (type.Equals("###"))
                                    {
                                        postTypeNode.Add(new XElement("Type", "###"));
                                    } else if (type.Equals("Improv"))
                                    {
                                        postTypeNode.Add(new XElement("Type", "###"));
                                    } else
                                    {
                                        postTypeNode.Add(new XElement("Type", type));
                                    }
                                }
                                node.Add(postTypeNode);

                                var genderTagNode = new XElement("Targets");
                                var genderTags = post.GenderTags;
                                for (int genderTagIndex = 0; genderTagIndex < genderTags.Length; genderTagIndex++)
                                {
                                    genderTagNode.Add(new XElement("Target", genderTags[genderTagIndex]));
                                }
                                node.Add(genderTagNode);

                                var userTagNode = new XElement("Tags");
                                var userTags = post.UserTags;
                                for (int userTagIndex = 0; userTagIndex < userTags.Length; userTagIndex++)
                                {
                                    userTagNode.Add(new XElement("Tag",userTags[userTagIndex]));
                                }
                                node.Add(userTagNode);

                                node.Add(new XElement("Link", post.ContentURL));

                                node.Add(new XElement("Time", DateTimeOffset.FromUnixTimeSeconds(post.Timestamp)));

                                document.Root.Add(node); 
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unexpected exception...");
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.InnerException.Message);
                        Console.WriteLine(e.StackTrace);
                    }
                    finally
                    {
                        textReader?.Close();
                    }

                    Console.WriteLine($"Finished processing file {filePaths[fileIndex]}.");
                }
                document.Save(file);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception...");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                file?.Close();
            }
          
            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}