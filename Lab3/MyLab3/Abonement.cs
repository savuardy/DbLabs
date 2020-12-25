using System;

namespace MyLab3
{
    public class Abonement
    {
        public virtual int abon_id { get; set; }
        public virtual Reader AbonOwnerId { get; set; }
    }

    public class AbonementManipulations
    {
        public static void Insert(int abonOwnerId)
        {
            using (var session = DbHelper.OpenSession())
            {
                var abonEntity = new Abonement()
                {
                    AbonOwnerId = session.Get<Reader>(abonOwnerId)
                };
                session.Save(abonEntity);
                session.Flush();
                session.Close();
            }
        }
        
        public static void Update(int id, int owner)
        {
            using (var session = DbHelper.OpenSession())
            {
                var abonEntity = session.Get<Abonement>(id);
                abonEntity.AbonOwnerId = session.Get<Reader>(owner);
                session.Update(abonEntity);
                session.Flush();
                session.Close();
            }
        }
        
        public static void Delete(int id)
        {
            using (var session = DbHelper.OpenSession())
            {
                session.Delete(session.Get<Abonement>(id));
                session.Flush();
                session.Close();
            }
        }
    }
}