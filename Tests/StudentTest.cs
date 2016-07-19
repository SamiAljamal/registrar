using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Registrar.Objects
{
  public class StudentTest : IDisposable
  {
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Student.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfStudentsAreSame()
    {
      Student firstStudent = new Student("Fred");
      Student secondStudent = new Student("Fred");

      Assert.Equal(firstStudent, secondStudent);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Student testStudent = new Student("Fred");

      //Act
      testStudent.Save();
      List<Student> result = Student.GetAll();
      List<Student> testStudents = new List<Student>{testStudent};

      //Assert
      Assert.Equal(testStudents, result);
    }

    [Fact]
    public void Test_Find_FindsStudentInDatabase()
    {
      Student testStudent = new Student("Joe");
      testStudent.Save();
      Student foundStudent = Student.Find(testStudent.GetId());
      Assert.Equal(testStudent, foundStudent);
    }

    [Fact]
    public void Test_Update_UpdatesStudentInDatabase()
    {
      Student testStudent = new Student("Nancy");
      testStudent.Save();
      string newName = "Lulu";
      testStudent.Update(newName);
      Assert.Equal(newName, testStudent.GetName());
    }

    [Fact]
    public void Test_AddCourse_AddsCourseToStudent()
    {
      Course newcourse = new Course("Dr. Jerry", "Bio101");
      newcourse.Save();

      Student newstudent = new Student("Tommy JOnes");
      newstudent.Save();

      newstudent.AddCourse(newcourse);
      List<Course> result = newstudent.GetCourses();
      List<Course> testList = new List<Course>{newcourse};

      Assert.Equal(testList, result);
    }

    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }


  }
}
