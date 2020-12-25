using System;

namespace MyLab3
{
    public class Book
    {
        public virtual int BookId { get; set; }
        public virtual string BookName { get; set; }
        public virtual DateTime ReturnDate { get; set; }
        public virtual Abonement AbonId { get; set; }
    }
    
    public class BookManipulations
    {
        public static void Insert(string bookName, string returnDate, int assignedAbon)
        {
            using (var session = DbHelper.OpenSession())
            {
                var bookEntity = new Book()
                {
                    BookName = bookName,
                    ReturnDate = DateTime.Parse(returnDate),
                    AbonId = session.Get<Abonement>(assignedAbon),
                };
                session.Save(bookEntity);
                session.Flush();
                session.Close();
            }
        }
        
        public static void Update(int id, string bookName, string returnDate, int assignedAbon)
        {
            using (var session = DbHelper.OpenSession())
            {
                var bookEntity = session.Get<Book>(id);
                bookEntity.BookName = bookName;
                bookEntity.ReturnDate = DateTime.Parse(returnDate);
                bookEntity.AbonId = session.Get<Abonement>(assignedAbon);
                session.Update(bookEntity);
                session.Flush();
                session.Close();
            }
        }
        
        public static void Delete(int id)
        {
            using (var session = DbHelper.OpenSession())
            {
                session.Delete(session.Get<Book>(id));
                session.Flush();
                session.Close();
            }
        }
    }
}