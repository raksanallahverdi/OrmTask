using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Oriem.Constants
{
    public class Messages
    {
        public static void InvalidInputMessage(string title)=>Console.WriteLine($"{title} is invalid ");
        public static void InputMessage(string title) => Console.WriteLine($"Please enter {title}");
        public static void NotFoundMessage(string title) => Console.WriteLine($"{title} is not FOUND ");
        public static void AlreadyExistError(string title) => Console.WriteLine($"{title} is already exist");
        public static void SuccessMessage(string title,string process) => Console.WriteLine($"{title} successfully {process}");

        public static void ErrorOccuredMessage() => Console.WriteLine("Error Occured");

        public static void WantToChangeMessage(string title) => Console.WriteLine($"Do you want to change {title}, Y or N?" );

    }
}
