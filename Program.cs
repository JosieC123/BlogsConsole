using System.Xml.Serialization;
using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

string? choice;
do
{
    Console.WriteLine("\n1) Display all blogs");
    Console.WriteLine("2) Add Blog");
    Console.WriteLine("3) Create Post");
    Console.WriteLine("4) Display Posts");

    choice = Console.ReadLine();

    if (choice == "1")
    {
        //Display all blogs
        var db = new DataContext();
        var query = db.Blogs.OrderBy(b => b.Name).ToList();
        if (query.Count == 0)
        {
            Console.WriteLine("0 Blogs Returned");
        }
        else
        {
            Console.WriteLine($"\n{query.Count} Blogs Returned");
            foreach (var item in query)
            {
                Console.WriteLine(item.Name);
            }
        }
    }
    else if (choice == "2")
    {
        //Add blog
        // Create and save a new Blog
        Console.Write("Enter a name for a new Blog: ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name)){
            logger.Error("Blog name cannot be null");
        }else{
        var blog = new Blog { Name = name };
        var db = new DataContext();
        db.AddBlog(blog);
        logger.Info("Blog added - {name}", name);
        }
    }
    else if (choice == "3")
    {
        //create post
    }
    else if (choice == "4")
    {
        //Display Posts
    }
} while (choice == "1" || choice == "2" || choice == "3" || choice == "4");

logger.Info("Program ended");
