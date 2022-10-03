﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971_Assessment.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public string InstructorPhone { get; set; }

        [MaxLength(250)]
        public string Notes { get; set; }

        public bool NotificationsOn { get; set; }
    }
}