namespace MyLab3
{
    public class AuthoredBy
    {
        public virtual int id { get; set; }
        public virtual Book book_id { get; set; }
        public virtual Author author_id { get; set; }
        
    }
    
    public class AuthoredByManipulations
    {
        public static void Insert(int bookId, int authorId)
        {
            using (var session = DbHelper.OpenSession())
            {
                var authoredByEntity = new AuthoredBy()
                {
                    book_id = session.Get<Book>(bookId),
                    author_id = session.Get<Author>(authorId)
                };
                session.Save(authoredByEntity);
                session.Flush();
                session.Close();
            }
        }
        
        public static void Update(int id, int bookId, int authorId)
        {
            using (var session = DbHelper.OpenSession())
            {
                var authoredByEntity = session.Get<AuthoredBy>(id);
                authoredByEntity.book_id = session.Get<Book>(bookId);
                authoredByEntity.author_id = session.Get<Author>(authorId);
                session.Update(authoredByEntity);
                session.Flush();
                session.Close();
            }
        }
        
        public static void Delete(int id)
        {
            using (var session = DbHelper.OpenSession())
            {
                session.Delete(session.Get<AuthoredBy>(id));
                session.Flush();
                session.Close();
            }
        }
    }
}