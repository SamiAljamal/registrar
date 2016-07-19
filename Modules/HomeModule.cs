using System.Collections.Generic;
using System;
using Nancy;
using Registrar.Objects;

namespace Registrar
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"]=_=> {
        return View["index.cshtml"];
      };

      Get["courses/all"] =_=> {
        List<Course> allCourses = Course.GetAll();
        return View["courses.cshtml", allCourses];
      };

      Get["/courses/{id}"] = parameters =>{
        Course selectedCourse = Course.Find(parameters.id);
        return View["course.cshtml", selectedCourse];
      };

      Post["/courses/new"] = _ => {
        Course newCourse = new Course(Request.Form["course-teacher"], Request.Form["course-name"]);
        newCourse.Save();
        List<Course> allCourses = Course.GetAll();
        return View["courses.cshtml", allCourses];
      };

      Get["/students/all"] =_=> {
        List<Student> allStudents = Student.GetAll();
        return View["students.cshtml", allStudents];
      };

      Get["/students/{id}"] = parameters => {
        Student selectedStudent = Student.Find(parameters.id);
        return View["student.cshtml", selectedStudent];
      };

      Post["/students/new"] = _ => {
        Student newStudent = new Student(Request.Form["student-name"]);
        newStudent.Save();
        List<Student> allStudents = Student.GetAll();
        return View["students.cshtml", allStudents];
      };
    }
  }
}
