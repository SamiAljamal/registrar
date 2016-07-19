using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Registrar.Objects
{
  public class Student
  {
    int _id;
    DateTime _admission;
    string _name;

    public Student (string name, DateTime admission, int id = 0)
    {
      _id = id;
      _admission = admission;
      _name = name;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = this.GetId() == newStudent.GetId();
        bool nameEquality = this.GetName() == newStudent.GetName();
        bool admissionEquality = this.GetAdmission() == newStudent.GetAdmission();
        return (idEquality && nameEquality && admissionEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public DateTime GetAdmission()
    {
      return _admission;
    }

    public void SetName(string newName)
    {
      _name = newName;
    }

    public void SetAdmission(DateTime newAdmission)
    {
      _admission = newAdmission;
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        DateTime studentAdmission = rdr.GetDateTime(2);
        Student newStudent = new Student(studentName, studentAdmission, studentId);
        allStudents.Add(newStudent);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allStudents;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students (name, admission) OUTPUT INSERTED.id VALUES (@StudentName, @studentAdmission);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@StudentName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);

      SqlParameter admissionParameter = new SqlParameter();
      admissionParameter.ParameterName = "@StudentAdmission";
      admissionParameter.Value = this.GetAdmission();
      cmd.Parameters.Add(admissionParameter);

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

    public static Student Find(int Id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId;", conn);
      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = Id.ToString();
      cmd.Parameters.Add(studentIdParameter);
      rdr = cmd.ExecuteReader();

      int foundStudentId = 0;
      string foundStudentName = null;
      DateTime foundStudentAdmission = new DateTime();

      while(rdr.Read())
      {
        foundStudentId = rdr.GetInt32(0);
        foundStudentName = rdr.GetString(1);
        foundStudentAdmission = rdr.GetDateTime(2);
      }
      Student foundStudent = new Student(foundStudentName, foundStudentAdmission, foundStudentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStudent;
    }
    public void AddCourse(Course newCourse)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (student_id, course_id) VALUES (@StudentId, @CourseId);", conn);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();
      cmd.Parameters.Add(studentIdParameter);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = newCourse.GetId();
      cmd.Parameters.Add(courseIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void DropCourse(Course dropCourse)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM students_courses WHERE student_id = @StudentId AND course_id = @CourseId", conn);
      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = dropCourse.GetId();

      cmd.Parameters.Add(studentIdParameter);
      cmd.Parameters.Add(courseIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Course> GetCourses()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT courses.* FROM students JOIN students_courses ON (students.id = students_courses.student_id) JOIN courses ON (students_courses.course_id = courses.id) where students.id = @StudentId", conn);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();
      cmd.Parameters.Add(studentIdParameter);

      rdr = cmd.ExecuteReader();

      List<Course> courses = new List<Course> {};

      while (rdr.Read())
      {
        int thisCourseId = rdr.GetInt32(0);
        string TeacherName = rdr.GetString(1);
        string CourseName = rdr.GetString(2);
        Course foundCourse = new Course(TeacherName,CourseName,thisCourseId);
        courses.Add(foundCourse);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return courses;
    }
    public void Update(string newName, DateTime newAdmission)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE students SET name = @NewName, admission = @NewAdmission OUTPUT INSERTED.name, INSERTED.admission WHERE id = @StudentId;", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter newAdmissionParameter = new SqlParameter();
      newAdmissionParameter.ParameterName = "@NewAdmission";
      newAdmissionParameter.Value = newAdmission;
      cmd.Parameters.Add(newAdmissionParameter);

      SqlParameter StudentIdParameter = new SqlParameter();
      StudentIdParameter.ParameterName = "@StudentId";
      StudentIdParameter.Value = this.GetId();
      cmd.Parameters.Add(StudentIdParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._name = rdr.GetString(0);
        this._admission = rdr.GetDateTime(1);
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

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
