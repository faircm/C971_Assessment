using C971_Assessment.Models;
using Plugin.LocalNotifications;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace C971_Assessment.Resources
{
    public class Notifier
    {
        public static void getCourseNotifications()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                SQLiteCommand selectCourse = new SQLiteCommand(conn);
                selectCourse.CommandText = "SELECT * FROM Course WHERE NotificationsOn == 1";

                List<Course> notifyCourses = selectCourse.ExecuteQuery<Course>();
                foreach (Course course in notifyCourses)
                {
                    if (course.StartDate == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Alert", $"Course {course.Title} starts today.", 1);
                    }
                    else if (course.StartDate == DateTime.Today.AddDays(1))
                    {
                        CrossLocalNotifications.Current.Show("Alert", $"Course {course.Title} starts tomorrow.", 2);
                    }

                    if (course.EndDate == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Alert", $"Course {course.Title} ends today.", 3);
                    }
                    else if (course.EndDate == DateTime.Today.AddDays(1))
                    {
                        CrossLocalNotifications.Current.Show("Alert", $"Course {course.Title} ends tomorrow.", 4);
                    }
                }
            }
        }

        public static void getAssessmentNotifications()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App._databaseLocation))
            {
                SQLiteCommand selectAssessment = new SQLiteCommand(conn);
                selectAssessment.CommandText = "SELECT * FROM Assessment WHERE NotificationsOn == 1";

                List<Assessment> notifyAssessments = selectAssessment.ExecuteQuery<Assessment>();
                foreach (Assessment assessment in notifyAssessments)
                {
                    if (assessment.StartDate == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Alert", $"Assessment {assessment.Title} starts today.", 5);
                    }
                    else if (assessment.StartDate == DateTime.Today.AddDays(1))
                    {
                        CrossLocalNotifications.Current.Show("Alert", $"Assessment {assessment.Title} starts tomorrow.", 6);
                    }

                    if (assessment.DueDate == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Alert", $"Assessment {assessment.Title} is due today.", 7);
                    }
                    else if (assessment.DueDate == DateTime.Today.AddDays(1))
                    {
                        CrossLocalNotifications.Current.Show("Alert", $"Assessment {assessment.Title} is due tomorrow.", 8);
                    }
                }
            }
        }
    }
}