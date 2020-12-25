namespace MyLab3
{
    public class Author
    {
        public virtual int author_id { get; set; }
        public virtual string AuthorName { get; set; }
    }

    public class AuthorManipulations
    {
        public static void Insert(string authorName)
            {
                using (var session = DbHelper.OpenSession())
                {
                    var authorEntity = new Author()
                    {
                        AuthorName = authorName
                    };
                    session.Save(authorEntity);
                    session.Flush();
                    session.Close();
                }
            }
        
            public static void Update(int id, string newAuthorName)
            {
                using (var session = DbHelper.OpenSession())
                {
                    var authorEntity = session.Get<Author>(id);
                    authorEntity.AuthorName = newAuthorName;
                    session.Update(authorEntity);
                    session.Flush();
                    session.Close();
                }
            }
        
            public static void Delete(int id)
            {
                using (var session = DbHelper.OpenSession())
                {
                    session.Delete(session.Get<Author>(id));
                    session.Flush();
                    session.Close();
                }
            }
    }
}