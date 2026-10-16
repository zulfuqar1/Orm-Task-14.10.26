using UniversityManagementSystem.Services;

namespace Orm_Task_14._10._26
{
    internal class Program
    {
        static void Main(string[] args)
        {


      

            while (true)
            {
                Console.Clear();

                Console.WriteLine("===== UNIVERSITY MANAGEMENT SYSTEM =====");
                Console.WriteLine("1. Add Department");
                Console.WriteLine("2. Add Teacher");
                Console.WriteLine("3. Add Course");
                Console.WriteLine("4. Add Student");
                Console.WriteLine("5. Enroll Student in Course");
                Console.WriteLine("6. Show Departments and Teachers");
                Console.WriteLine("7. Show Departments, Courses and Teachers");
                Console.WriteLine("8. Show Students and Courses");
                Console.WriteLine("9. Show Courses and Students");
                Console.WriteLine("10. Update Student");
                Console.WriteLine("11. Delete Student");
                Console.WriteLine("12. Test DeleteBehavior");
                Console.WriteLine("13. Show ChangeTracker and EntityState");
                Console.WriteLine("0. Exit");

                Console.Write("\nChoose: ");
                string? choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            UniversityService.AddDepartment();
                            break;

                        case "2":
                            UniversityService.AddTeacher();
                            break;

                        case "3":
                            UniversityService.AddCourse();
                            break;

                        case "4":
                            UniversityService.AddStudent();
                            break;

                        case "5":
                            UniversityService.EnrollStudent();
                            break;

                        case "6":
                            UniversityService.ShowDepartmentsWithTeachers();
                            break;

                        case "7":
                            UniversityService.ShowDepartmentsWithCoursesAndTeachers();
                            break;

                        case "8":
                            UniversityService.ShowStudentsWithCourses();
                            break;

                        case "9":
                            UniversityService.ShowCoursesWithStudents();
                            break;

                        case "10":
                            UniversityService.UpdateStudent();
                            break;

                        case "11":
                            UniversityService.DeleteStudent();
                            break;

                        case "12":
                            UniversityService.TestDeleteBehavior();
                            break;

                        case "13":
                            UniversityService.ShowChangeTracker();
                            break;

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }


        }
    }
}
