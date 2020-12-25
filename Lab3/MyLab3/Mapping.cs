using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;

using Npgsql;
namespace MyLab3
{
    class ReaderMap:ClassMap<Reader>
    {
        public ReaderMap()
        {
            Schema("public");
            Table("\"reader\"");
            Id(x => x.reader_id).GeneratedBy.Custom("trigger-identity");
            Map(x => x.ReaderName, "reader_name");
            Map(x => x.ReaderAddress, "reader_address");
            Map(x => x.ReaderDob, "reader_dob").CustomType("Date");
        }
    }

    class AuthorMap:ClassMap<Author>
    {
        public AuthorMap()
        {
            Schema("public");
            Table("\"author\"");
            Id(x => x.author_id).GeneratedBy.Custom("trigger-identity");
            Map(x => x.AuthorName, "author_name");
        }
    }

    class AbonementMap : ClassMap<Abonement>
    {
        public AbonementMap()
        {
            Schema("public");
            Table("\"Abonement\"");
            Id(x => x.abon_id).GeneratedBy.Custom("trigger-identity");
            References(x => x.AbonOwnerId).Column("\"reader_id\"");
        }
    }

    class BookMap : ClassMap<Book>
    {
        public BookMap()
        {
            Schema("public");
            Table("\"Book\"");
            Id(x => x.BookId).GeneratedBy.Custom("trigger-identity").Unique();
            Map(x => x.BookName, "book_name");
            Map(x => x.ReturnDate, "return_date").CustomType("Date");
            References(x => x.AbonId).Column("abon_id");
        }
    }

    class AuthoredByMap : ClassMap<AuthoredBy>
    {
        public AuthoredByMap()
        {
            Schema("public");
            Table("\"authored_by\"");
            Id(x=>x.id).GeneratedBy.Custom("trigger_identity");
            References(x => x.author_id).Column("author_id");
            References(x => x.book_id).Column("book_id");
        }
    }
}
