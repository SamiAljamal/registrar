using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Registrar.Objects
{
  public class Course
  {
    int _id;
    string _teacher;
    string _name;

    public Course(string teacher, string name, int id = 0)
    {
      _id = id;
      _teacher = teacher;
      _name = name;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = this.GetId() == newCourse.GetId();
        bool teacherEquality = this.GetTeacher() == newCourse.GetTeacher();
        bool nameEquality = this.GetName() == newCourse.GetName();
        return (idEquality && teacherEquality && nameEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetTeacher()
    {
      return _teacher;
    }

    public string GetName()
    {
      return _name;
    }

    public void SetTeacher(string newTeacher)
    {
      _teacher = newTeacher;
    }

    public void SetName(string newName)
    {
      _name = newName;
    }

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseTeacher = rdr.GetString(1);
        string courseName = rdr.GetString(2);
        Course newCourse = new Course(courseTeacher, courseName, courseId);
        allCourses.Add(newCourse);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCourses;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO courses (teacher, name) OUTPUT INSERTED.id VALUES (@CourseTeacher, @CourseName);", conn);

      SqlParameter teacherParameter = new SqlParameter();
      teacherParameter.ParameterName = "@CourseTeacher";
      teacherParameter.Value = this.GetTeacher();

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CourseName";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(teacherParameter);
      cmd.Parameters.Add(nameParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
         this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Course Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand ("SELECT * FROM courses WHERE id = @CourseId;", conn);
      SqlParameter CourseIdParameter = new SqlParameter();
      CourseIdParameter.ParameterName = "@CourseId";
      CourseIdParameter.Value = id.ToString();
      cmd.Parameters.Add(CourseIdParameter);
      rdr = cmd.ExecuteReader();

      int foundCourseId = 0;
      string foundCourseName = null;
      string foundCourseTeacher = null;

      while(rdr.Read())
      {
        foundCourseId = rdr.GetInt32(0);
        foundCourseName = rdr.GetString(2);
        foundCourseTeacher = rdr.GetString(1);
      }
      Course foundCourse = new Course(foundCourseTeacher,foundCourseName,foundCourseId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCourse;
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
      cmd.ExecuteNonQuery();
    }


  }
}
