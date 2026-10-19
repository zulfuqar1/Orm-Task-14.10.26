using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orm_Task_14._10._26.Services;
using System.Text;
using System.Text;
using UniversityManagementSystem.Data;
using UniversityManagementSystem.Services;

namespace Orm_Task_14._10._26
{
    internal class Program
    {
        static void Main(string[] args)
        {


            Console.OutputEncoding = Encoding.UTF8;

            using var db = new UniversityDbContext();

            db.Database.Migrate();

            var departments = new DepartmentService(db);
            var teachers = new TeacherService(db);
            var courses = new CourseService(db);
            var students = new StudentService(db);
            var enrollments = new EnrollmentService(db);

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("========== UNIVERSITY MANAGEMENT ==========");
                Console.WriteLine(" 1. Department əlavə et");
                Console.WriteLine(" 2. Teacher əlavə et");
                Console.WriteLine(" 3. Course əlavə et");
                Console.WriteLine(" 4. Student əlavə et");
                Console.WriteLine(" 5. Student-i Course-a yaz");
                Console.WriteLine(" 6. Department-ləri əlaqəli məlumatlarla göstər");
                Console.WriteLine(" 7. Student-ləri və onların Course-larını göstər");
                Console.WriteLine(" 8. Course üzrə tələbələri göstər");
                Console.WriteLine(" 9. Student məlumatlarını yenilə");
                Console.WriteLine("10. Student-i sil");
                Console.WriteLine("11. DeleteBehavior ssenarilərini yoxla");
                Console.WriteLine("12. ChangeTracker və EntityState məlumatlarını göstər");
                Console.WriteLine(" 0. Çıxış");
                Console.Write("Seçim: ");
                var choice = Console.ReadLine();

                if (choice == "0") break;

                try
                {
                    switch (choice)
                    {
                        case "1":
                            var dept = departments.Add(ReadString("Ad: "), ReadString("Təsvir: "));
                            Console.WriteLine($"Department əlavə olundu. Id = {dept.Id}");
                            break;

                        case "2":
                            ShowDepartments();
                            var teacher = teachers.Add(ReadString("Ad Soyad: "), ReadString("Email: "), ReadInt("DepartmentId: "));
                            Console.WriteLine($"Teacher əlavə olundu. Id = {teacher.Id}");
                            break;

                        case "3":
                            ShowDepartments();
                            foreach (var t in teachers.GetAll())
                                Console.WriteLine($"  Teacher {t.Id}: {t.FullName} ({t.Department.Name})");
                            var course = courses.Add(ReadString("Kurs adı: "), ReadInt("Credit (1-10): "),
                                                     ReadInt("DepartmentId: "), ReadInt("TeacherId: "));
                            Console.WriteLine($"Course əlavə olundu. Id = {course.Id}");
                            break;

                        case "4":
                            var st = students.Add(ReadString("Ad Soyad: "), ReadString("Email: "),
                                                  ReadDate("Doğum tarixi (yyyy-MM-dd): "));
                            Console.WriteLine($"Student əlavə olundu. Id = {st.Id}");
                            break;

                        case "5":
                            ShowStudents();
                            ShowCourses();
                            var en = enrollments.Enroll(ReadInt("StudentId: "), ReadInt("CourseId: "));
                            Console.WriteLine($"Student kursa yazıldı. Enrollment Id = {en.Id}, tarix = {en.EnrollmentDate:yyyy-MM-dd}");
                            break;

                        case "6":
                            foreach (var d in departments.GetAllRelated())
                            {
                                Console.WriteLine($"\n[{d.Id}] {d.Name} - {d.Description}");
                                Console.WriteLine("  Müəllimlər:");
                                foreach (var t in d.Teachers) Console.WriteLine($"    - {t.FullName} ({t.Email})");
                                Console.WriteLine("  Kurslar:");
                                foreach (var c in d.Courses)
                                    Console.WriteLine($"    - {c.Name} ({c.Credit} kredit), müəllim: {c.Teacher.FullName}");
                            }
                            break;

                        case "7":
                            foreach (var s in students.GetWithEnrollments())
                            {
                                Console.WriteLine($"\n[{s.Id}] {s.FullName} ({s.Email}), doğum: {s.BirthDate:yyyy-MM-dd}");
                                foreach (var e in s.Enrollments)
                                    Console.WriteLine($"    - {e.Course.Name} | {e.EnrollmentDate:yyyy-MM-dd} | Qiymət: {e.Grade?.ToString() ?? "-"}");
                            }
                            Console.WriteLine("\n--- Hesabat: Student | Course | Tarix | Qiymət ---");
                            foreach (var r in students.GetReport())
                                Console.WriteLine($"{r.Student,-20} {r.Course,-20} {r.Date:yyyy-MM-dd}  {r.Grade?.ToString() ?? "-"}");
                            break;

                        case "8":
                            foreach (var c in courses.GetWithStudents())
                            {
                                Console.WriteLine($"\n[{c.Id}] {c.Name}");
                                if (c.Enrollments.Count == 0) Console.WriteLine("    (tələbə yoxdur)");
                                foreach (var e in c.Enrollments)
                                    Console.WriteLine($"    - {e.Student.FullName} ({e.Student.Email})");
                            }
                            break;

                        case "9":
                            ShowStudents();
                            var id = ReadInt("StudentId: ");
                            var ok = students.Update(id, ReadString("Yeni ad soyad: "), ReadString("Yeni email (boş ola bilər): "));
                            Console.WriteLine(ok ? "Student yeniləndi." : "Student tapılmadı.");
                            break;

                        case "10":
                            ShowStudents();
                            var delId = ReadInt("Silinəcək StudentId: ");
                            Console.WriteLine(students.Delete(delId) ? "Student silindi (Enrollment-lar cascade ilə silindi)." : "Student tapılmadı.");
                            break;

                        case "11":
                            DeleteBehaviorService.Run();
                            break;

                        case "12":
                            EntityStateService.Run();
                            break;

                        default:
                            Console.WriteLine("Yanlış seçim, yenidən cəhd edin.");
                            break;
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Verilənlər bazası xətası: {ex.InnerException?.Message ?? ex.Message}");
                    db.ChangeTracker.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Xəta: {ex.Message}");
                    db.ChangeTracker.Clear();
                }
            }

            void ShowDepartments()
            {
                foreach (var d in departments.GetAll()) Console.WriteLine($"  Department {d.Id}: {d.Name}");
            }

            void ShowCourses()
            {
                foreach (var c in courses.GetAll()) Console.WriteLine($"  Course {c.Id}: {c.Name} ({c.Teacher.FullName})");
            }

            void ShowStudents()
            {
                foreach (var s in students.GetAll()) Console.WriteLine($"  Student {s.Id}: {s.FullName}");
            }

            string ReadString(string prompt)
            {
                Console.Write(prompt);
                return Console.ReadLine() ?? "";
            }

            int ReadInt(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    if (int.TryParse(Console.ReadLine(), out var v)) return v;
                    Console.WriteLine("Düzgün rəqəm daxil edin.");
                }
            }

            DateTime ReadDate(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    if (DateTime.TryParse(Console.ReadLine(), out var v)) return v;
                    Console.WriteLine("Düzgün tarix daxil edin.");
                }
            }


        }
    }
}
