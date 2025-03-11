
using LibraryDLL.Data;
using LibraryDLL.Models;
using System;

namespace LibraryDLL.Services
{
    public class RegistrationService
    {
        private readonly LibraryContext _context;

        public RegistrationService(LibraryContext context)
        {
            _context = context;
        }

        public User RegisterUser()
        {
            Console.WriteLine("Register a new user.");
            Console.WriteLine("Enter login:");
            var login = Console.ReadLine();
            Console.WriteLine("Enter password:");
            var password = Console.ReadLine();
            Console.WriteLine("Enter email:");
            var email = Console.ReadLine();
            Console.WriteLine("Are you a librarian (1) or a reader (2)?");
            var roleChoice = Console.ReadLine();
            var role = roleChoice == "1" ? "Librarian" : "Reader";

            User user = null;

            if (role == "Reader")
            {
                Console.WriteLine("Enter first name:");
                var firstName = Console.ReadLine();
                Console.WriteLine("Enter last name:");
                var lastName = Console.ReadLine();
                Console.WriteLine("Enter document type (passport, driver's license, etc.):");
                var documentType = Console.ReadLine();
                Console.WriteLine("Enter document number:");
                var documentNumber = Console.ReadLine();

                var reader = new Reader
                {
                    Login = login,
                    Password = password,
                    Email = email,
                    Role = role,
                    FirstName = firstName,
                    LastName = lastName,
                    DocumentType = documentType,
                    DocumentNumber = documentNumber
                };

                _context.Users.Add(reader);
                user = reader;
            }
            else
            {
                var librarian = new User
                {
                    Login = login,
                    Password = password,
                    Email = email,
                    Role = role
                };

                _context.Users.Add(librarian);
                user = librarian;
            }

            _context.SaveChanges();
            Console.WriteLine("Registration successful!");

            return user; 
        }
    }
}