using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ST.DataAccess.Test
{
    [Table("BLOG")]
    public class Blog : IEntity
    {
        [Column("ID")]
        public int Id { get; set; }


        [Column("Title")]
        public string Title { get; set; }


        [Column("CONTENT")]
        public string Content { get; set; }


        [Column("CREATIONTIME")]
        public DateTime CreationTime { get; set; }

    }
}
