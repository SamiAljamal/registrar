using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Registrar.Objects
{
  public class CourseTest : IDisposable
  {
    public CourseTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Course.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfCoursesAreTheSame()
    {
      Course firstCourse = new Course("Dr. Feelgood", "PSY101");
      Course secondCourse = new Course("Dr. Feelgood", "PSY101");
      Assert.Equal(firstCourse, secondCourse);
    }

    [Fact]
    public void Test_Save_SavesCourseToDatabase()
    {
      Course testCourse = new Course("Dr. Feelgood", "PSY101");
      testCourse.Save();

      List<Course> testCourses = new List<Course>{testCourse};
      List<Course> resultCourses = Course.GetAll();

      Assert.Equal(testCourses, resultCourses);
    }
    public void Dispose()
    {

      Course.DeleteAll();
    }

    [Fact]
    public void Test_Save_AssignsIdToCourse()
    {

      Course testCourse = new Course("Dr. Feelgood", "PSY101");
      testCourse.Save();
      Course savedCourse = Course.GetAll()[0];

      int result = savedCourse.GetId();
      int testId = testCourse.GetId();

      //Assert
      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_Find_FindsCourseInDatabase()
    {
      //Arrange
      Course testCourse =  new Course("Dr. Feelgood", "PSY101");
      testCourse.Save();

      //Act
      Course foundCourse = Course.Find(testCourse.GetId());

      //Assert
      Assert.Equal(testCourse, foundCourse);
    }

  }
}