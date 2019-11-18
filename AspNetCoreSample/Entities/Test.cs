using ST.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AspNetCoreSample
{
    [Table("TEST")]
    public class Test: IEntity
    {
        [Column("ID")]
        public string Id { get; set; }


        [Column("NAME")]
        public string Name { get; set; }


        [Column("AGE")]
        public int Age { get; set; }


        [Column("CREATIONTIME")]
        public DateTime CreationTime { get; set; }


        [Column("REMARK")]
        public string Remark { get; set; }
    }
}
