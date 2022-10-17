using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971_Assessment.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Title { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumCourses { get; set; }
    }
}