
using LibraryDLL.Data;
using LibraryDLL.Functions;
using LibraryDLL.Services;
using System;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new LibraryContext())
        {
            Console.WriteLine("Login (1) or Register (2)?");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine("Enter login:");
                var login = Console.ReadLine();
                Console.WriteLine("Enter password:");
                var password = Console.ReadLine();

                var user = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
                if (user != null)
                {
                    Console.WriteLine($"Welcome, {user.Login}!");
                    if (user.Role == "Librarian")
                    {
                        var librarianFunctions = new LibrarianFunctions(context);
                        LibrarianMenu(librarianFunctions);
                    }
                    else
                    {
                        var readerFunctions = new ReaderFunctions(context, user.Id);
                        ReaderMenu(readerFunctions);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid login or password.");
                }
            }
            else if (choice == "2")
            {
                var registrationService = new RegistrationService(context);
                var user = registrationService.RegisterUser();

                if (user != null)
                {
                    Console.WriteLine($"Welcome, {user.Login}!");
                    if (user.Role == "Librarian")
                    {
                        var librarianFunctions = new LibrarianFunctions(context);
                        LibrarianMenu(librarianFunctions);
                    }
                    else
                    {
                        var readerFunctions = new ReaderFunctions(context, user.Id);
                        ReaderMenu(readerFunctions);
                    }
                }
                else
                {
                    Console.WriteLine("Registration failed.");
                }
            }
        }
    }
    //Менюшка бібліотекаря
    static void LibrarianMenu(LibrarianFunctions librarianFunctions)
    {
        while (true)
        {
            Console.WriteLine("\nLibrarian Menu:");
            Console.WriteLine("1. Add a book");
            Console.WriteLine("2. Add an author");
            Console.WriteLine("3. Add a reader");
            Console.WriteLine("4. Update book information");
            Console.WriteLine("5. Update author information");
            Console.WriteLine("6. Update reader information");
            Console.WriteLine("7. Delete a reader");
            Console.WriteLine("8. View borrowing history");
            Console.WriteLine("9. View debtors");
            Console.WriteLine("10. View reader's borrow history");
            Console.WriteLine("11. Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    librarianFunctions.AddBook();
                    break;
                case "2":
                    librarianFunctions.AddAuthor();
                    break;
                case "3":
                    librarianFunctions.AddReader();
                    break;
                case "4":
                    librarianFunctions.UpdateBook();
                    break;
                case "5":
                    librarianFunctions.UpdateAuthor();
                    break;
                case "6":
                    librarianFunctions.UpdateReader();
                    break;
                case "7":
                    librarianFunctions.DeleteReader();
                    break;
                case "8":
                    librarianFunctions.ViewBorrowHistory();
                    break;
                case "9":
                    librarianFunctions.ViewDebtors();
                    break;
                case "10":
                    librarianFunctions.ViewReaderBorrowHistory();
                    break;
                case "11":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    //Менюшка читача
    static void ReaderMenu(ReaderFunctions readerFunctions)
    {
       
        readerFunctions.CheckOverdueBooks();

        while (true)
        {
            Console.WriteLine("\nReader Menu:");
            Console.WriteLine("1. Search books by title");
            Console.WriteLine("2. Search books by author");
            Console.WriteLine("3. View borrowed books");
            Console.WriteLine("4. Borrow a book");
            Console.WriteLine("5. Return a book");
            Console.WriteLine("6. View returned books");
            Console.WriteLine("7. Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    readerFunctions.SearchBooksByTitle();
                    break;
                case "2":
                    readerFunctions.SearchBooksByAuthor();
                    break;
                case "3":
                    readerFunctions.ViewBorrowedBooks();
                    break;
                case "4":
                    readerFunctions.BorrowBook();
                    break;
                case "5":
                    readerFunctions.ReturnBook();
                    break;
                case "6":
                    readerFunctions.ViewReturnedBooks();
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}