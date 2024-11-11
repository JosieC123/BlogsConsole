using System.Xml.Serialization;
using NLog;
using NLog.LayoutRenderers;
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
    Console.WriteLine("Enter q to quit");

    choice = Console.ReadLine();

    if (choice == "1")
    {
        //Display all blogs
        var db = new DataContext();
        var query = db.Blogs.OrderBy(b => b.BlogId).ToList();
        if (query.Count == 0)
        {
            Console.WriteLine("0 Blogs Returned");
        }
        else
        {
            Console.WriteLine($"{query.Count} Blogs Returned");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.BlogId}. {item.Name}");
            }
        }
    }
    else if (choice == "2")
    {
        //Add blog
        // Create and save a new Blog
        Console.Write("Enter a name for a new Blog: ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            logger.Error("Blog name cannot be null");
        }
        else
        {
            var blog = new Blog { Name = name };
            var db = new DataContext();
            db.AddBlog(blog);
            logger.Info("Blog added - {name}", name);
        }
    }
    else if (choice == "3")
    {
        //create post
        //make foreach loop display blog id and name
        var db = new DataContext();
        var query = db.Blogs.OrderBy(b => b.BlogId).ToList();

        Console.WriteLine("Select a blog you would like to post to: ");
        foreach (var item in query)
        {
            Console.WriteLine($"{item.BlogId}) {item.Name}");
        }

        var blogOption = Console.ReadLine();
        if (int.TryParse(blogOption, out int blogId))
        {
            var selectedBlog = query.FirstOrDefault(b => b.BlogId == blogId);

            if (selectedBlog != null)
            {
                //create title
                Console.WriteLine("Enter the Post title: ");
                var postTitle = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(postTitle))
                {
                    logger.Error("Post title cannot be null");
                    return;
                }

                //create content
                Console.WriteLine("Enter the Post content: ");
                var postContent = Console.ReadLine();

                //created AddPost method to match AddBlog
                var post = new Post { Title = postTitle, Content = postContent, BlogId = selectedBlog.BlogId };
                db.AddPost(post);
                logger.Info("Post added - {postTitle}", postTitle);
            }
            else
            {
                //inValid integer entered for blog id
                logger.Error("There are no blogs saved with that Id");
            }
        }
        else
        {
            //Invalid blog id entered - not an integer
            logger.Error("Invalid Blog Id");
        }
    }
    else if (choice == "4")
    {
        //Display Posts
        Console.WriteLine("Select the blog's Posts to display\n");

        //Display all blogs
        var db = new DataContext();
        var query = db.Blogs.OrderBy(b => b.BlogId).ToList();
        foreach (var item in query)
        {
            Console.WriteLine($"{item.BlogId}) {item.Name}");
        }

        var blogOption = Console.ReadLine();
        if (int.TryParse(blogOption, out int blogId))
        {

            if (blogId == 0)
            {
                //display all if entered 0
                var queryPosts = db.Posts.OrderBy(b => b.PostId).ToList();
                Console.WriteLine($"\n{queryPosts.Count} Post(s) Returned");
                foreach (var item in queryPosts)
                {
                    Console.WriteLine($"Blog: {item.Blog.Name}\n\tTitle: {item.Title}\n\tContent: {item.Content}\n");
                }
            }
            else
            {
                var selectedChoice = query.FirstOrDefault(b => b.BlogId == blogId);

                if (selectedChoice != null)
                {
                    //show posts based of blog id
                    var queryPosts = db.Posts.Where(p => p.BlogId == blogId).OrderBy(b => b.PostId).ToList();
                    Console.WriteLine($"\n{queryPosts.Count} Post(s) Returned");
                    foreach (var item in queryPosts)
                    {
                        Console.WriteLine($"Blog: {item.Blog.Name}\n\tTitle: {item.Title}\n\tContent: {item.Content}\n");
                    }
                }
                else
                {
                    //inValid integer entered for blog id
                    logger.Error("There are no blogs saved with that Id");
                }
            }
        }
        else
        {
            //Invalid blog id entered - not an integer
            Console.WriteLine("Invalid Blog Id");
        }
    }
} while (choice == "1" || choice == "2" || choice == "3" || choice == "4");

logger.Info("Program ended");
