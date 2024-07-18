using Microsoft.EntityFrameworkCore;
using Oriem.Constants;
using Oriem.Contexts;
using Oriem.Entities;
using Oriem.Services;
using Oriem.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage;

namespace Oriem.Services;

public static class GroupService
{
    private static readonly AppDbContext _context;
    static GroupService()
    {

        _context = new AppDbContext();
    }

    public static void GetAllGroups()
    {
        foreach (var group in _context.Groups.Include(g => g.Teacher).Include(g=>g.Students).ToList()  )
        {
            Console.WriteLine($"{group.Id}.{group.Name},Limit: {group.Limit}.  Teacher: {group.Teacher.Name} {group.Teacher.Surname}.[{group.BeginDate}-{group.EndDate}]. Student Count:{group.Students.Count}");
        }

    }
    public static void CreateGroup()
    {
    inputNewGroupName: Messages.InputMessage("Group Name");
        string newGroupName = Console.ReadLine();
        var group = _context.Groups.FirstOrDefault(g => g.Name == newGroupName);
        if (group != null)
        {
            Messages.AlreadyExistError("Group");
            return;
        }
        if (string.IsNullOrEmpty(newGroupName))
        {
            Messages.InvalidInputMessage("Group Name");
            goto inputNewGroupName;
        }
    inputNewGroupLimit: Messages.InputMessage("Group Limit");
        string newGroupLimit = Console.ReadLine();
        int groupLimit;
        bool isTrue = int.TryParse(newGroupLimit, out groupLimit);
        if (!isTrue)
        {
            Messages.InvalidInputMessage("Group Limit");
            goto inputNewGroupLimit;
        }
        TeacherService.GetAllTeachers();
    inputNewGroupTeacher: Messages.InputMessage("Group Teacher ID");
        string teacherId = Console.ReadLine();
        int Id;
        isTrue = int.TryParse(teacherId, out Id);
        if (!isTrue)
        {
            Messages.InvalidInputMessage("Teacher Id");
            goto inputNewGroupTeacher;
        }
        var teacher = _context.Teachers.FirstOrDefault(t => t.Id == Id);
        if (teacher == null)
        {
            Messages.NotFoundMessage("Teacher");
            goto inputNewGroupTeacher;
        }
    inputNewGroupBeginDate: Messages.InputMessage("Group Begin Date.Format: DD.MM.YYYY ");
        string newBeginDate = Console.ReadLine();
        DateOnly beginDate;
        isTrue = DateOnly.TryParseExact(newBeginDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out beginDate);

        if (!isTrue)
        {
            Messages.InvalidInputMessage("Begin Date");
            goto inputNewGroupBeginDate;
        }

    inputNewGroupEndDate: Messages.InputMessage("Group End Date.Format: DD-MM-YYYY. Minimum six-month continuity");
        string endDate = Console.ReadLine();
        DateOnly end;
        isTrue = DateOnly.TryParseExact(endDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out end);
        if (!isTrue || beginDate.AddMonths(6) > end)
        {
            Messages.InvalidInputMessage("End Date");
            goto inputNewGroupEndDate;
        }
        Group newGroup = new Group
        {
            Name = newGroupName,
            Limit = groupLimit,
            BeginDate = beginDate,
            EndDate = end,
            TeacherId = teacher.Id
        };
        _context.Groups.Add(newGroup);

        try
        {
            _context.SaveChanges();

        }
        catch (Exception)
        {

            Messages.ErrorOccuredMessage();
        }
        Messages.SuccessMessage("Group", "Added");
    }
    public static void UpdateGroup()
    {
        GetAllGroups();

    idInputMessage: Messages.InputMessage("Group Id for Update");
        string inputAnswer = Console.ReadLine();
        int groupId;
        bool isTrue = int.TryParse(inputAnswer, out groupId);
        if (!isTrue)
        {
            Messages.InvalidInputMessage("Group Id");
            goto idInputMessage;
        }
        var group = _context.Groups.Find(groupId);

        if (group is null)
        {
            Messages.NotFoundMessage("Group");
            goto idInputMessage;
        }

    NameInputMessage: Messages.WantToChangeMessage("Name");
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
                Messages.InvalidInputMessage("New Group Name");
                goto newNameInput;
            }

        }
    LimitInputMessage: Messages.WantToChangeMessage("Limit");
        inputAnswer = Console.ReadLine();
        isTrue = char.TryParse(inputAnswer, out value);
        if (!isTrue || !value.isValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto LimitInputMessage;
        }
        int limit = 0;
        if (value == 'y')
        {
        newLimitInput: Console.WriteLine("Yeni Limiti daxil edin");
            string newLimit = Console.ReadLine();
            isTrue = int.TryParse(newLimit, out limit);

            if (limit == 0 || !isTrue)
            {
                Messages.InvalidInputMessage("New Limit");
                goto newLimitInput;
            }

            if (group.Students != null && limit < group.Students.Count)
            {
                Messages.InvalidInputMessage("Limit must be greater than students' count.Limit ");
                goto LimitInputMessage;
            }


        }


    TeacherChangeMessage: Messages.WantToChangeMessage("Teacher");
        inputAnswer = Console.ReadLine();
        isTrue = char.TryParse(inputAnswer, out value);
        if (!isTrue || !value.isValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto TeacherChangeMessage;
        }
        int teacherId = 0;

        if (value == 'y')
        {
        newTeacherIdInput: Console.WriteLine("Yeni TeacherId daxil edin");
            string newLimit = Console.ReadLine();
            isTrue = int.TryParse(newLimit, out teacherId);

            if (teacherId == 0 || !isTrue)
            {
                Messages.InvalidInputMessage("New Teacher Id");
                goto newTeacherIdInput;
            }
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == teacherId);
            if (teacher == null)
            {
                Messages.NotFoundMessage("Teacher");
                goto newTeacherIdInput;
            }

        }
    BeginDateChangeMessage: Messages.WantToChangeMessage("Begin Date");
        inputAnswer = Console.ReadLine();
        isTrue = char.TryParse(inputAnswer, out value);
        if (!isTrue || !value.isValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto TeacherChangeMessage;
        }
        DateOnly begin = default;
    InputBeginDate:
        if (value == 'y')
        {
            Messages.InputMessage("New Begin Date.(Format: dd.MM.yyyy)");
            inputAnswer = Console.ReadLine();

            isTrue = DateOnly.TryParseExact(inputAnswer, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out begin);
            if (!isTrue)
            {

                Messages.InvalidInputMessage("Begin Date");
                goto InputBeginDate;


            }
        }


    EndDateChangeMessage: Messages.WantToChangeMessage("End Date");
        inputAnswer = Console.ReadLine();
        isTrue = char.TryParse(inputAnswer, out value);
        if (!isTrue || !value.isValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto EndDateChangeMessage;
        }
        DateOnly end = default;
    InputEndDate:
        if (value == 'y')
        {
            Messages.InputMessage("New End Date.(Format: dd.MM.yyyy) Minimum six-month continuity");
            inputAnswer = Console.ReadLine();

            isTrue = DateOnly.TryParseExact(inputAnswer, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out end);
            if (!isTrue)
            {

                Messages.InvalidInputMessage("End Date");
                goto InputEndDate;

            }
        }

        if (begin != default && end == default)
        {
            if (begin.AddMonths(6) > group.EndDate)
            {
                Messages.InvalidInputMessage("Group must have six-month continuity! Begin Date.");
                goto InputBeginDate;
            }
        }
        if (begin == default && end != default)
        {
            if (group.BeginDate.AddMonths(6) > end)
            {
                Messages.InvalidInputMessage("Group must have six-month continuity! End Date.");
                goto InputEndDate;
            }
        }
        if (begin != default && end != default)
        {
            if (begin.AddMonths(6) > end)
            {
                Messages.InvalidInputMessage("Group must have six-month continuity! End Date.");
                goto InputEndDate;
            }
        }


        if (newName != string.Empty)
        {
            group.Name = newName;
        }
        if (limit != 0)
        {
            group.Limit = limit;
        }
        if (begin != default)
        {
            group.BeginDate = begin;
        }
        if (end != default)
        {
            group.EndDate = end;
        }

        if (teacherId != 0)
        {
            group.TeacherId = teacherId;
        }

        Messages.SuccessMessage("Group", "Updated");


    }
    public static void RemoveGroup()
    {
        GetAllGroups();

    idInputMessage: Messages.InputMessage("Group Id for Remove");
        string inputAnswer = Console.ReadLine();
        int groupId;
        bool isTrue = int.TryParse(inputAnswer, out groupId);
        if (!isTrue)
        {
            Messages.InvalidInputMessage("Group Id");
            goto idInputMessage;
        }
        var group = _context.Groups.Find(groupId);

        if (group is null)
        {
            Messages.NotFoundMessage("Group");
            goto idInputMessage;
        }
        group.IsDeleted = true;
        Messages.SuccessMessage("Group", "Deleted");

    }
    public static void GetDetailsOfGroup()
    {
        GetAllGroups();

    idInputMessage: Messages.InputMessage("Group Id:");
        string inputAnswer = Console.ReadLine();
        int groupId;
        bool isTrue = int.TryParse(inputAnswer, out groupId);
        if (!isTrue)
        {
            Messages.InvalidInputMessage("Group Id");
            goto idInputMessage;
        }
        var group = _context.Groups.Include(g => g.Students).FirstOrDefault(g => g.Id == groupId);

        if (group is null)
        {
            Messages.NotFoundMessage("Group");
            goto idInputMessage;
        }
        Console.WriteLine($"{group.Id}.{group.Name},Limit: {group.Limit}. Teacher: {group.Teacher.Name} {group.Teacher.Surname}.[{group.BeginDate}-{group.EndDate}]");
        Console.Write("Students:");
        foreach (var student in group.Students)
        {
            Console.WriteLine($"{student.Name} {student.Surname}");
        }


    }

}

