using Oriem.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Oriem.Constants;
using Oriem.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Oriem.Extensions;


namespace Oriem.Services;

public static class StudentService
{
    private static readonly AppDbContext _context;
    static StudentService()
    {
        _context = new AppDbContext();
    }
    public static void AllStudents()

    {
        foreach (var student in _context.Students.ToList())
        {
            Console.WriteLine($"Id:{student.Id}. Name: {student.Name}, Surname: {student.Surname},Birtdate: {student.BirthDate},Email: {student.Email}");
        }
    }
    public static void AddStudent()
    {
    StudentNameInput: Messages.InputMessage("Student Name");
        string name = Console.ReadLine();
        if (string.IsNullOrEmpty(name))
        {
            Messages.InvalidInputMessage("student name");
            goto StudentNameInput;
        }

    StudentSurnameInput: Messages.InputMessage("Student Surname");
        string surname = Console.ReadLine();
        if (string.IsNullOrEmpty(surname))
        {
            Messages.InvalidInputMessage("student surname");
            goto StudentSurnameInput;
        }

    StudentEmailInput: Messages.InputMessage("Student Email");
        string email = Console.ReadLine();
        if (string.IsNullOrEmpty(email))
        {
            Messages.InvalidInputMessage("student email");
            goto StudentEmailInput;
        }

    StudentBirthdateInput: Messages.InputMessage("Student Birthdate (YYYY-MM-DD)");
        string birthdateInput = Console.ReadLine();
        DateOnly birthdate;
        bool isTrue = DateOnly.TryParseExact(birthdateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthdate);

        if (!isTrue)
        {
            Messages.InvalidInputMessage("student birthdate");
            goto StudentBirthdateInput;
        }

        try
        {
            GroupService.GetAllGroups();
        }
        catch (Exception ex)
        {
            Messages.ErrorOccuredMessage();
            Console.WriteLine(ex.Message); 
            return;
        }

    idInputMessage: Messages.InputMessage("Group Id for Student");
        string inputAnswer = Console.ReadLine();
        int groupId;
        isTrue = int.TryParse(inputAnswer, out groupId);
        if (!isTrue)
        {
            Messages.InvalidInputMessage("Group Id");
            goto idInputMessage;
        }
        var group = _context.Groups.Include(g=>g.Students).FirstOrDefault(g=>g.Id == groupId);

        if (group is null)
        {
            Messages.NotFoundMessage("Group");
            goto idInputMessage;
        }

        if (group.Limit < group.Students.Count + 1)
        {
            Console.WriteLine($"Təəssüf ki, Grupda maksimum sayda şagird var.Grup limiti: {group.Limit}");
            return;
        }


        Student student = new Student { Name = name, Surname = surname, Email = email, BirthDate = birthdate, GroupId = group.Id };

        _context.Students.Add(student);

        try
        {
            _context.SaveChanges();
        }
        catch (Exception)
        {
            Messages.ErrorOccuredMessage();
        }

        Messages.SuccessMessage("Student", "added");

    }
    public static void UpdateStudent()
    {
        AllStudents();

    
    studentIdInputMessage: Console.WriteLine("Please choose Student ID for Update");
        string inputAnswer = Console.ReadLine();
        int studentId;
        bool isTrue = int.TryParse(inputAnswer, out studentId);

        if (!isTrue)
        {
            Messages.InvalidInputMessage("Student Id");
            goto studentIdInputMessage;
        }

        var student = _context.Students.Find(studentId);

        if (student == null)
        {
            Messages.NotFoundMessage("Student");
            goto studentIdInputMessage;
        }

    NameInputMessage: Messages.WantToChangeMessage("name");
        inputAnswer = Console.ReadLine();
        char value;
        isTrue = char.TryParse(inputAnswer, out value);
        if (!isTrue || !value.isValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto NameInputMessage;
        }
        string newName = string.Empty;
        if (value == 'y')
        {
        newNameInput: Console.WriteLine("Yeni adi daxil edin");
            newName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newName))
            {
                Messages.InvalidInputMessage("New Student Name");
                goto newNameInput;
            }
        }

    SurnameInputMessage: Messages.WantToChangeMessage("surname");
        inputAnswer = Console.ReadLine();
        isTrue = char.TryParse(inputAnswer, out value);
        if (!isTrue || !value.isValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto SurnameInputMessage;
        }
        string newSurname = string.Empty;
        if (value == 'y')
        {
        newSurnameInput: Console.WriteLine("Yeni soyadi daxil edin");
            newSurname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newSurname))
            {
                Messages.InvalidInputMessage("New Student Surname");
                goto newSurnameInput;
            }
        }

    EmailInputMessage: Messages.WantToChangeMessage("email");
        inputAnswer = Console.ReadLine();
        isTrue = char.TryParse(inputAnswer, out value);
        if (!isTrue || !value.isValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto EmailInputMessage;
        }
        string newEmail = string.Empty;
        if (value == 'y')
        {
        newEmailInput: Console.WriteLine("Yeni e-poçt ünvanını daxil edin");
            newEmail = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newEmail))
            {
                Messages.InvalidInputMessage("New Student Email");
                goto newEmailInput;
            }
        }

    BirthdateInputMessage: Messages.WantToChangeMessage("birthdate");
        inputAnswer = Console.ReadLine();
        isTrue = char.TryParse(inputAnswer, out value);
        if (!isTrue || !value.isValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto BirthdateInputMessage;
        }
        DateOnly newBirthdate = default;
        if (value == 'y')
        {
        newBirthdateInput: Messages.InputMessage("New Student Birthdate (YYYY-MM-DD)");
            string birthdateInput = Console.ReadLine();
            isTrue = DateOnly.TryParseExact(birthdateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out newBirthdate);
            if (!isTrue)
            {
                Messages.InvalidInputMessage("New Student Birthdate");
                goto newBirthdateInput;
            }
        }

    GroupInputMessage: Messages.WantToChangeMessage("group");
        inputAnswer = Console.ReadLine();
        isTrue = char.TryParse(inputAnswer, out value);
        if (!isTrue || !value.isValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto GroupInputMessage;
        }
        int newGroupId = 0;
        if (value == 'y')
        {
            try
            {
                GroupService.GetAllGroups();
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
                return;
            } 

        idInputMessage: Messages.InputMessage("New Group Id for Student");
            inputAnswer = Console.ReadLine();
            isTrue = int.TryParse(inputAnswer, out newGroupId);
            if (!isTrue)
            {
                Messages.InvalidInputMessage("Group Id");
                goto idInputMessage;
            }
            var group = _context.Groups.Include(g => g.Students).FirstOrDefault(g => g.Id == newGroupId);

            if (group == null)
            {
                Messages.NotFoundMessage("Group");
                goto idInputMessage;
            }
        }

        if (newName!=string.Empty)
        {
            student.Name = newName;
        }
        if (newSurname != string.Empty)
        {
            student.Surname = newSurname;
        }
        if (newEmail != string.Empty)
        {
            student.Email = newEmail;
        }
        if (newBirthdate != default)
        {
            student.BirthDate = newBirthdate;
        }
        if (newGroupId != 0)
        {
            student.GroupId = newGroupId;
        }

        try
        {
            _context.SaveChanges();
        }
        catch (Exception)
        {
            Messages.ErrorOccuredMessage();
            return;
        }

        Messages.SuccessMessage("Student", "updated");
    }

    public static void RemoveStudent()
    {
        AllStudents();
        Console.WriteLine("Please, choose Student ID for deleting....");
        string inputAnswer = Console.ReadLine();
        int studentId;
        bool isTrue = int.TryParse(inputAnswer, out studentId);

        if (!isTrue)
        {
            Messages.InvalidInputMessage("Student Id");
            return;
        }
        var student = _context.Students.Find(studentId);

        if (student == null)
        {
            Messages.NotFoundMessage("Student");
            return;
        }

        student.IsDeleted = true;

        try
        {
            _context.SaveChanges();
         
        }
        catch (Exception)
        {
            Messages.ErrorOccuredMessage();
            return;
        }

        Messages.SuccessMessage("Student", "deleted");
    }

    public static void GetDetailsOfStudent() {
        AllStudents();
        Console.WriteLine("Please, Choose Student ID from list for Getting Detailed Information....");
        string inputAnswer = Console.ReadLine();
        int studentId;
        bool isTrue = int.TryParse(inputAnswer, out studentId);

        if (!isTrue)
        {
            Messages.InvalidInputMessage("Teacher Id");
        }
        var student = _context.Students.Include(s=>s.Group).ThenInclude(g => g.Teacher).FirstOrDefault(s=>s.Id==studentId);

        if (student is null)
        {
            Messages.NotFoundMessage("Student");
        }
        Console.WriteLine($"Student: {student.Name} {student.Surname},{student.Email},{student.BirthDate},Group Name:{student.Group.Name},Student's Teacher:{student.Group.Teacher.Name} {student.Group.Teacher.Surname}");
      

    }



}
