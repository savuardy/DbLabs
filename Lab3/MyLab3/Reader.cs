using System;

namespace MyLab3
{
    public class Reader
    {
        public virtual int reader_id { get; set; }
        public virtual string ReaderName { get; set; }
        public virtual string ReaderAddress { get; set; }
        public virtual DateTime ReaderDob { get; set; }
    }
    
    public class ReaderManipulations
    {
        public static void Insert(string readerName, string address, string readerDob)
        {
            using (var session = DbHelper.OpenSession())
            {
                var readerEntity = new Reader()
                {
                    ReaderName = readerName,
                    ReaderAddress = address,
                    ReaderDob = DateTime.Parse(readerDob),
                };
                session.Save(readerEntity);
                session.Flush();
                session.Close();
            }
        }
        
        public static void Update(int id, string newReaderName , string newAddress, string newDob)
        {
            using (var session = DbHelper.OpenSession())
            {
                var readerEntity = session.Get<Reader>(id);
                readerEntity.ReaderName = newReaderName;
                readerEntity.ReaderAddress = newAddress;
                readerEntity.ReaderDob = DateTime.Parse(newDob);
                session.Update(readerEntity);
                session.Flush();
                session.Close();
            }
        }
        
        public static void Delete(int id)
        {
            using (var session = DbHelper.OpenSession())
            {
                session.Delete(session.Get<Reader>(id));
                session.Flush();
                session.Close();
            }
        }
    }
}