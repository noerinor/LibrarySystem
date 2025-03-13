
using LibraryDLL.Data;
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

                Console.WriteLine("Enter login:");
                var login = Console.ReadLine();
                Console.WriteLine("Enter password:");
                var password = Console.ReadLine();
                Console.WriteLine("Enter email:");
                var email = Console.ReadLine();
                Console.WriteLine("Are you a librarian (1) or a reader (2)?");
                var roleChoice = Console.ReadLine();
                var role = roleChoice == "1" ? "Librarian" : "Reader";

                if (role == "Reader")
                {
                    Console.WriteLine("Enter first name:");
                    var firstName = Console.ReadLine();
                    Console.WriteLine("Enter last name:");
                    var lastName = Console.ReadLine();
                    Console.WriteLine("Enter document type:");
                    var documentType = Console.ReadLine();
                    Console.WriteLine("Enter document number:");
                    var documentNumber = Console.ReadLine();

                    var user = registrationService.RegisterUser(login, password, email, role, firstName, lastName, documentType, documentNumber);
                    Console.WriteLine($"Welcome, {user.Login}!");

                    var readerFunctions = new ReaderFunctions(context, user.Id);
                    ReaderMenu(readerFunctions);
                }
                else
                {
                    var user = registrationService.RegisterUser(login, password, email, role);
                    Console.WriteLine($"Welcome, {user.Login}!");

                    var librarianFunctions = new LibrarianFunctions(context);
                    LibrarianMenu(librarianFunctions);
                }
            }
        }
    }

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
                    Console.WriteLine("Enter title:");
                    var title = Console.ReadLine();
                    Console.WriteLine("Enter publishing code:");
                    var publishingCode = Console.ReadLine();
                    Console.WriteLine("Enter publishing code type:");
                    var publishingCodeType = Console.ReadLine();
                    Console.WriteLine("Enter year:");
                    var year = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter country:");
                    var country = Console.ReadLine();
                    Console.WriteLine("Enter city:");
                    var city = Console.ReadLine();

                    librarianFunctions.AddBook(title, publishingCode, publishingCodeType, year, country, city);
                    Console.WriteLine("Book added successfully!");
                    break;

                case "2": 
                    Console.WriteLine("Enter first name:");
                    var firstName = Console.ReadLine();
                    Console.WriteLine("Enter last name:");
                    var lastName = Console.ReadLine();
                    Console.WriteLine("Enter middle name (if any):");
                    var middleName = Console.ReadLine();
                    Console.WriteLine("Enter birth date (yyyy-MM-dd):");
                    var birthDate = DateTime.Parse(Console.ReadLine());

                    librarianFunctions.AddAuthor(firstName, lastName, middleName, birthDate);
                    Console.WriteLine("Author added successfully!");
                    break;

                case "3": 
                    Console.WriteLine("Enter login:");
                    var login = Console.ReadLine();
                    Console.WriteLine("Enter password:");
                    var password = Console.ReadLine();
                    Console.WriteLine("Enter email:");
                    var email = Console.ReadLine();
                    Console.WriteLine("Enter first name:");
                    var readerFirstName = Console.ReadLine();
                    Console.WriteLine("Enter last name:");
                    var readerLastName = Console.ReadLine();
                    Console.WriteLine("Enter document type:");
                    var documentType = Console.ReadLine();
                    Console.WriteLine("Enter document number:");
                    var documentNumber = Console.ReadLine();

                    librarianFunctions.AddReader(login, password, email, readerFirstName, readerLastName, documentType, documentNumber);
                    Console.WriteLine("Reader added successfully!");
                    break;

                case "4": 
                    Console.WriteLine("Enter the ID of the book you want to update:");
                    var bookId = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter new title:");
                    var newTitle = Console.ReadLine();
                    Console.WriteLine("Enter new publishing code:");
                    var newPublishingCode = Console.ReadLine();
                    Console.WriteLine("Enter new publishing code type:");
                    var newPublishingCodeType = Console.ReadLine();
                    Console.WriteLine("Enter new year:");
                    var newYear = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter new country:");
                    var newCountry = Console.ReadLine();
                    Console.WriteLine("Enter new city:");
                    var newCity = Console.ReadLine();

                    librarianFunctions.UpdateBook(bookId, newTitle, newPublishingCode, newPublishingCodeType, newYear, newCountry, newCity);
                    Console.WriteLine("Book information updated successfully!");
                    break;

                case "5": 
                    Console.WriteLine("Enter the ID of the author you want to update:");
                    var authorId = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter new first name:");
                    var newFirstName = Console.ReadLine();
                    Console.WriteLine("Enter new last name:");
                    var newLastName = Console.ReadLine();
                    Console.WriteLine("Enter new middle name (if any):");
                    var newMiddleName = Console.ReadLine();
                    Console.WriteLine("Enter new birth date (yyyy-MM-dd):");
                    var newBirthDate = DateTime.Parse(Console.ReadLine());

                    librarianFunctions.UpdateAuthor(authorId, newFirstName, newLastName, newMiddleName, newBirthDate);
                    Console.WriteLine("Author information updated successfully!");
                    break;

                case "6": 
                    Console.WriteLine("Enter the ID of the reader you want to update:");
                    var readerId = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter new first name:");
                    var newReaderFirstName = Console.ReadLine();
                    Console.WriteLine("Enter new last name:");
                    var newReaderLastName = Console.ReadLine();
                    Console.WriteLine("Enter new document type:");
                    var newDocumentType = Console.ReadLine();
                    Console.WriteLine("Enter new document number:");
                    var newDocumentNumber = Console.ReadLine();

                    librarianFunctions.UpdateReader(readerId, newReaderFirstName, newReaderLastName, newDocumentType, newDocumentNumber);
                    Console.WriteLine("Reader information updated successfully!");
                    break;

                case "7": 
                    Console.WriteLine("Enter the ID of the reader you want to delete:");
                    var deleteReaderId = int.Parse(Console.ReadLine());

                    librarianFunctions.DeleteReader(deleteReaderId);
                    Console.WriteLine("Reader deleted successfully!");
                    break;

                case "8": 
                    var borrowHistory = librarianFunctions.ViewBorrowHistory();
                    if (borrowHistory.Any())
                    {
                        Console.WriteLine("\nBorrowing history:");
                        foreach (var record in borrowHistory)
                        {
                            Console.WriteLine($"Book: {record.Book.Title}, Reader: {record.Reader.FirstName} {record.Reader.LastName}, Borrowed: {record.BorrowedDate.ToShortDateString()}, Due: {record.ReturnDate.ToShortDateString()}, Returned: {(record.ActualReturnDate.HasValue ? record.ActualReturnDate.Value.ToShortDateString() : "Not returned")}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No borrowing history found.");
                    }
                    break;

                case "9": 
                    var debtors = librarianFunctions.ViewDebtors();
                    if (debtors.Any())
                    {
                        Console.WriteLine("\nList of debtors:");
                        foreach (var debtor in debtors)
                        {
                            Console.WriteLine($"Reader: {debtor.Reader.FirstName} {debtor.Reader.LastName}, Book: {debtor.Book.Title}, Due Date: {debtor.ReturnDate.ToShortDateString()}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No debtors found.");
                    }
                    break;

                case "10": 
                    Console.WriteLine("Enter the ID of the reader:");
                    var readerHistoryId = int.Parse(Console.ReadLine());

                    var readerHistory = librarianFunctions.ViewReaderBorrowHistory(readerHistoryId);
                    if (readerHistory.Any())
                    {
                        Console.WriteLine($"\nBorrow history for reader ID {readerHistoryId}:");
                        foreach (var record in readerHistory)
                        {
                            Console.WriteLine($"Book: {record.Book.Title}, Borrowed: {record.BorrowedDate.ToShortDateString()}, Due: {record.ReturnDate.ToShortDateString()}, Returned: {(record.ActualReturnDate.HasValue ? record.ActualReturnDate.Value.ToShortDateString() : "Not returned")}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No borrow history found for this reader.");
                    }
                    break;

                case "11": 
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void ReaderMenu(ReaderFunctions readerFunctions)
    {
        
        var overdueBooks = readerFunctions.CheckOverdueBooks();
        if (overdueBooks.Any())
        {
            Console.WriteLine("\nYou have overdue books:");
            foreach (var overdueBook in overdueBooks)
            {
                Console.WriteLine($"Book: {overdueBook.Book.Title}, Due Date: {overdueBook.ReturnDate.ToShortDateString()}");
            }
        }
        else
        {
            Console.WriteLine("You have no overdue books.");
        }

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
                    Console.WriteLine("Enter title:");
                    var title = Console.ReadLine();

                    var booksByTitle = readerFunctions.SearchBooksByTitle(title);
                    if (booksByTitle.Any())
                    {
                        Console.WriteLine("\nFound books:");
                        foreach (var book in booksByTitle)
                        {
                            Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Authors: {string.Join(", ", book.Authors.Select(a => $"{a.FirstName} {a.LastName}"))}, Year: {book.Year}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No books found.");
                    }
                    break;

                case "2": 
                    Console.WriteLine("Enter author name:");
                    var authorName = Console.ReadLine();

                    var booksByAuthor = readerFunctions.SearchBooksByAuthor(authorName);
                    if (booksByAuthor.Any())
                    {
                        Console.WriteLine("\nFound books:");
                        foreach (var book in booksByAuthor)
                        {
                            Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Authors: {string.Join(", ", book.Authors.Select(a => $"{a.FirstName} {a.LastName}"))}, Year: {book.Year}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No books found.");
                    }
                    break;

                case "3": 
                    var borrowedBooks = readerFunctions.ViewBorrowedBooks();
                    if (borrowedBooks.Any())
                    {
                        Console.WriteLine("\nYour borrowed books:");
                        foreach (var borrowedBook in borrowedBooks)
                        {
                            Console.WriteLine($"Book: {borrowedBook.Book.Title}, Borrowed: {borrowedBook.BorrowedDate.ToShortDateString()}, Due: {borrowedBook.ReturnDate.ToShortDateString()}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You have no borrowed books.");
                    }
                    break;

                case "4": 
                    Console.WriteLine("Enter the ID of the book you want to borrow:");
                    var bookId = int.Parse(Console.ReadLine());

                    readerFunctions.BorrowBook(bookId);
                    Console.WriteLine("Book borrowed successfully!");
                    break;

                case "5": 
                    Console.WriteLine("Enter the ID of the book you want to return:");
                    var returnBookId = int.Parse(Console.ReadLine());

                    readerFunctions.ReturnBook(returnBookId);
                    Console.WriteLine("Book returned successfully!");
                    break;

                case "6": 
                    var returnedBooks = readerFunctions.ViewReturnedBooks();
                    if (returnedBooks.Any())
                    {
                        Console.WriteLine("\nYour returned books:");
                        foreach (var returnedBook in returnedBooks)
                        {
                            Console.WriteLine($"Book: {returnedBook.Book.Title}, Borrowed: {returnedBook.BorrowedDate.ToShortDateString()}, Returned: {returnedBook.ActualReturnDate.Value.ToShortDateString()}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You have no returned books.");
                    }
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