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

        Dictionary<string,object> model = new Dictionary<string,object>();
        Course selectedCourse = Course.Find(parameters.id);
        List<Student> studentsInCourse = selectedCourse.GetStudents();
        model.Add("course", selectedCourse);
        model.Add("students", studentsInCourse);
        return View["course.cshtml", model];
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

      Post["/students/new"] = _ => {
        Student newStudent = new Student(Request.Form["student-name"], new DateTime(2016, 7, 19));
        newStudent.Save();
        List<Student> allStudents = Student.GetAll();
        return View["students.cshtml", allStudents];
      };

      Get["/student/{id}"] = parameters => {
        Student selectedStudent = Student.Find(parameters.id);
        Dictionary<string,object> model = new Dictionary<string,object>();
        List<Course> studentcourse = selectedStudent.GetCourses();
        List<Course> allCourses = Course.GetAll();

        model.Add("student", selectedStudent);
        model.Add("courses",studentcourse);
        model.Add("allCourses", allCourses);


        return View["student.cshtml",model];
      };

      Post["/student/{id}"] = parameters => {
        Course selectedCourse = Course.Find(Request.Form["course_id"]);

        Student selectedStudent = Student.Find(parameters.id);
        selectedCourse.AddStudent(selectedStudent);

        Dictionary<string,object> model = new Dictionary<string,object>();
        List<Course> studentcourse = selectedStudent.GetCourses();
        List<Course> allCourses = Course.GetAll();

        model.Add("student", selectedStudent);
        model.Add("courses",studentcourse);
        model.Add("allCourses", allCourses);
        return View["student.cshtml", model];
      };

      Get["/student/update/{id}"] = parameters => {
        Student selectedStudent = Student.Find(parameters.id);
        return View["student_update.cshtml", selectedStudent];
      };

      Patch["/student/{id}"] = parameters => {
        Student selectedStudent = Student.Find(parameters.id);
        selectedStudent.Update(Request.Form["student_name"], new DateTime(2016, 7, 19));

        Dictionary<string,object> model = new Dictionary<string,object>();
        List<Course> studentcourse = selectedStudent.GetCourses();
        List<Course> allCourses = Course.GetAll();

        model.Add("student", selectedStudent);
        model.Add("courses",studentcourse);
        model.Add("allCourses", allCourses);
        return View["student.cshtml", model];
      };

      Delete["/student/{sid}/drop/{cid}"] = parameters => {
        Student selectedStudent = Student.Find(parameters.sid);
        Course selectedCourse = Course.Find(parameters.cid);
        selectedStudent.DropCourse(selectedCourse);

        Dictionary<string,object> model = new Dictionary<string,object>();
        List<Course> studentcourse = selectedStudent.GetCourses();
        List<Course> allCourses = Course.GetAll();

        model.Add("student", selectedStudent);
        model.Add("courses",studentcourse);
        model.Add("allCourses", allCourses);
        return View["student.cshtml", model];
      };
    }
  }
}
