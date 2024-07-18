
using Microsoft.EntityFrameworkCore;
using Oriem.Constants;
using Oriem.Contexts;
using Oriem.Entities;
using Oriem.Extensions;
namespace Oriem.Services;
public static class TeacherService
{
    private static readonly AppDbContext _context;
    static TeacherService()
    {
        _context=new AppDbContext();
    }

    public static void GetAllTeachers()
    {
        foreach(var teacherin in _context.Teachers.ToList())
        {
            Console.WriteLine($"Id:{teacherin.Id}. Name: {teacherin.Name}, Surname: {teacherin.Surname}.");
        }
    }

    public static void AddTeacher()
    {

    TeacherNameInput: Messages.InputMessage("Teacher Name");
        string name=Console.ReadLine();
        if (string.IsNullOrEmpty(name)) {
            Messages.InvalidInputMessage("teacher name");
            goto TeacherNameInput;
        }
    TeacherSurnameInput: Messages.InputMessage("Teacher Surname");
        string surname = Console.ReadLine();
        if (string.IsNullOrEmpty(surname))
        {
            Messages.InvalidInputMessage("teacher surname");
            goto TeacherSurnameInput;
        }

        Teacher teacher = new Teacher { Name = name, Surname = surname };

        _context.Teachers.Add(teacher);

        try
        {
            _context.SaveChanges();
        }
        catch (Exception)
        {
            Messages.ErrorOccuredMessage();
            
        }
        Messages.SuccessMessage("Teacher", "added");
    }

    public static void UpdateTeacher() {

        GetAllTeachers();
        idInputMessage: Console.WriteLine("Please choose Teacher ID for Update");
        string inputAnswer = Console.ReadLine();
        int teacherId;
        bool isTrue = int.TryParse(inputAnswer, out teacherId);

        if (!isTrue)
        {
            Messages.InvalidInputMessage("Teacher Id");
            goto idInputMessage;
        }
        var teacher = _context.Teachers.Find(teacherId);

        if (teacher is null)
        {
            Messages.NotFoundMessage("Teacher");
            goto idInputMessage;
        }

        NameInputMessage:Messages.WantToChangeMessage("name");
        inputAnswer = Console.ReadLine();
        char value;
        isTrue = char.TryParse(inputAnswer, out value);
        if (!isTrue || !value.isValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto NameInputMessage;
        }
        string newName=string.Empty;
        if (value == 'y')
        {
            newNameInput: Console.WriteLine("Yeni adi daxil edin");
             newName= Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newName))
            {
                Messages.InvalidInputMessage("New Teacher Name");
                goto newNameInput;
            }

        }
    SurnameInputMessage: Messages.WantToChangeMessage("Surname");
        inputAnswer = Console.ReadLine();
         isTrue = char.TryParse(inputAnswer, out value);
        if (!isTrue || !value.isValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto SurnameInputMessage;
        }
        string newSurname=string.Empty;
        if (value == 'y')
        {
        newSurnameInput: Console.WriteLine("Yeni Soyadi daxil edin");
             newSurname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newSurname))
            {
                Messages.InvalidInputMessage("New Teacher Surname");
                goto newSurnameInput;
            }
        }
        if (newName != string.Empty)
        {
            teacher.Name= newName;
        }
        if (newSurname != string.Empty)
        {
            teacher.Surname = newSurname;
        }
        Messages.SuccessMessage("Teacher", "Updated");
    }
    public static void RemoveTeacher() {
        GetAllTeachers();
        Console.WriteLine("Please, Choose Teacher ID from list for Deleting....");
        string inputAnswer = Console.ReadLine();
        int teacherId;
        bool isTrue = int.TryParse(inputAnswer, out teacherId);

        if (!isTrue)
        {
            Messages.InvalidInputMessage("Teacher Id");
        }
        var teacher = _context.Teachers.Find(teacherId);

        if (teacher is null)
        {
            Messages.NotFoundMessage("Teacher");
        }

        teacher.IsDeleted= true;
        Messages.SuccessMessage("Teacher", "Deleted");
        


    }
    public static void GetDetailsOfTeacher()
    {
        GetAllTeachers();
        Console.WriteLine("Please, Choose Teacher ID from list for Getting Detailed Information....");
        string inputAnswer = Console.ReadLine();
        int teacherId;
        bool isTrue = int.TryParse(inputAnswer, out teacherId);

        if (!isTrue)
        {
            Messages.InvalidInputMessage("Teacher Id");
        }
        var teacher = _context.Teachers.Include(x=>x.Groups).FirstOrDefault(x=>x.Id==teacherId);

        if (teacher is null)
        {
            Messages.NotFoundMessage("Teacher");
        }
        Console.WriteLine($"{teacher.Name},{teacher.Surname}.");
        Console.Write($"Groups:");
        foreach (Group group in teacher.Groups) {
            Console.WriteLine($"{group.Name},{group.Limit}");
        }
        

    }
}