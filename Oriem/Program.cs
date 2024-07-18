
using Oriem.Constants;
using Oriem.Contexts;
using Oriem.Services;
namespace Oriem;
public static class Program
{
    public static void Main()
    {

        while (true)
        {
            ShowMenu();
            ShowOperations();         
           
        }
    }
    public static void ShowMenu()
    {
        Console.WriteLine("---MENU---");
        Console.WriteLine(" ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("1.All Teachers");
        Console.WriteLine("2.Add Teacher");
        Console.WriteLine( "3.Update Teacher");
        Console.WriteLine( "4.Remove Teacher");
        Console.WriteLine("5.Details of Teacher");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" ");
        Console.WriteLine("6.All Groups");
        Console.WriteLine("7.Create Group");
        Console.WriteLine("8.Update Group");
        Console.WriteLine("9.Remove Group");
        Console.WriteLine("10.Details of Group");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(" ");
        Console.WriteLine("11.All Students");
        Console.WriteLine("12.Add Student");
        Console.WriteLine("13.Update Student");
        Console.WriteLine("14.Remove Student");
        Console.WriteLine("15.Details of Student");
        Console.WriteLine(" ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("0.Exit");
        Console.ResetColor();
        Console.WriteLine(" ");
        Console.WriteLine("Zehmet olmasa menyudan seçim edin (0-15):");
    }
    public static void ShowOperations()
    {
        string choiceInput = Console.ReadLine();
        int choice;
        bool isSucceeded = int.TryParse(choiceInput, out choice);
        if (isSucceeded)
        {
            switch ((Operations)choice)
            {
                case Operations.AllTeachers:
                    TeacherService.GetAllTeachers();
                    break;
                case Operations.CreateTeacher:
                    TeacherService.AddTeacher();
                    break;

                case Operations.UpdateTeacher:
                    TeacherService.UpdateTeacher();
                    break;
                case Operations.RemoveTeacher:
                    TeacherService.RemoveTeacher();
                    break;
                case Operations.DetailsOfTeacher:
                    TeacherService.GetDetailsOfTeacher();
                    break;
                case Operations.AllGroups:
                    GroupService.GetAllGroups();
                    break;
                case Operations.CreateGroup:
                    GroupService.CreateGroup();
                    break;
                case Operations.UpdateGroup:
                    GroupService.UpdateGroup();
                    break;
                case Operations.RemoveGroup:
                    GroupService.RemoveGroup();
                    break;
                case Operations.DetailsOfGroup:
                    GroupService.GetDetailsOfGroup();
                    break;
                case Operations.AllStudents:
                    StudentService.AllStudents();
                    break;
                case Operations.AddStudent:
                    StudentService.AddStudent();
                    break;
                case Operations.UpdateStudent:
                    StudentService.UpdateStudent();
                    break;
                case Operations.RemoveStudent:
                    StudentService.RemoveStudent();
                    break;
                case Operations.DetailsOfStudent:
                    StudentService.GetDetailsOfStudent();
                    break;
                case Operations.Exit:
                    Console.WriteLine("Çıxış edildi...");
                    return;

                default:
                    Messages.InvalidInputMessage("choice");
                    break;


            }
        }
    }
}