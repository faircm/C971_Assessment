using C971_Assessment.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971_Assessment.Resources
{
    public class SampleData
    {
        public static void populateDB()
        {
            Term sampleTerm = new Term();
            sampleTerm.Id = 1;
            sampleTerm.Title = "Sample Term";
            sampleTerm.StartDate = DateTime.Parse("05/01/2022");
            sampleTerm.EndDate = DateTime.Parse("10/31/2022");

            Course sampleCourse = new Course();
            sampleCourse.Id = 1;
            sampleCourse.termId = 1;
            sampleCourse.Title = "MOBILE APPLICATION DEVELOPMENT USING C#";
            sampleCourse.StartDate = DateTime.Parse("10/01/2022");
            sampleCourse.EndDate = DateTime.Parse("10/17/2022");
            sampleCourse.DueDate = DateTime.Parse("10/16/2022");
            sampleCourse.InstructorName = "Christopher Fairchild";
            sampleCourse.InstructorEmail = "cfair13@wgu.edu";
            sampleCourse.InstructorPhone = "2099851405";
            sampleCourse.Notes = "Cutting it close";
            sampleCourse.NotificationsOn = true;
            sampleCourse.Status = "Started";

            Assessment sampleAssessment_1 = new Assessment();
            sampleAssessment_1.Id = 1;
            sampleAssessment_1.CourseID = 1;
            sampleAssessment_1.StartDate = DateTime.Parse("10/01/2022");
            sampleAssessment_1.EndDate = DateTime.Parse("10/17/2022");
            sampleAssessment_1.DueDate = DateTime.Parse("10/16/2022");
            sampleAssessment_1.Title = "Mobile Application Project";
            sampleAssessment_1.Type = "Performance";
            sampleAssessment_1.NotificationsOn = true;

            Assessment sampleAssessment_2 = new Assessment();
            sampleAssessment_2.Id = 2;
            sampleAssessment_2.CourseID = 1;
            sampleAssessment_2.StartDate = DateTime.Parse("10/16/2022");
            sampleAssessment_2.EndDate = DateTime.Parse("10/17/2022");
            sampleAssessment_2.DueDate = DateTime.Parse("10/16/2022");
            sampleAssessment_2.Title = "Objective Assessment";
            sampleAssessment_2.Type = "Objective";
            sampleAssessment_2.NotificationsOn = true;

            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                conn.CreateTable<Term>();
                conn.InsertOrReplace(sampleTerm);
            }
            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                conn.CreateTable<Course>();
                conn.InsertOrReplace(sampleCourse);
            }
            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                conn.CreateTable<Assessment>();
                conn.InsertOrReplace(sampleAssessment_1);
            }
            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                conn.CreateTable<Assessment>();
                conn.InsertOrReplace(sampleAssessment_2);
            }
        }
    }
}