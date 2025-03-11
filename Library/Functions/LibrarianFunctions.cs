
using LibraryDLL.Data;
using LibraryDLL.Models;
using LibraryDLL.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LibraryDLL.Functions
{
    public class LibrarianFunctions
    {
        private readonly LibraryContext _context;

        public LibrarianFunctions(LibraryContext context)
        {
            _context = context;
        }

        public void AddBook()
        {
            Console.WriteLine("Enter the book title:");
            var title = Console.ReadLine();
            Console.WriteLine("Enter the publisher code (ISBN, BBC, etc.):");
            var publishingCode = Console.ReadLine();
            Console.WriteLine("Enter the publisher code type:");
            var publishingCodeType = Console.ReadLine();
            Console.WriteLine("Enter the year of publication:");
            var year = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the country of publication:");
            var country = Console.ReadLine();
            Console.WriteLine("Enter the city of publication:");
            var city = Console.ReadLine();

            var book = new Book
            {
                Title = title,
                PublishingCode = publishingCode,
                PublishingCodeType = publishingCodeType,
                Year = year,
                Country = country,
                City = city
            };

            _context.Books.Add(book);
            _context.SaveChanges();
            Console.WriteLine("Book added successfully!");
        }

        public void UpdateBook()
        {
            Console.WriteLine("Enter the ID of the book you want to update.:");
            var bookId = int.Parse(Console.ReadLine());

            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            Console.WriteLine("Enter a new book title");
            book.Title = Console.ReadLine();
            Console.WriteLine("Enter a new publisher code:");
            book.PublishingCode = Console.ReadLine();
            Console.WriteLine("Enter a new publisher code type:");
            book.PublishingCodeType = Console.ReadLine();
            Console.WriteLine("Enter the new year of publication:");
            book.Year = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter a new country of publication:");
            book.Country = Console.ReadLine();
            Console.WriteLine("Enter a new city of publication:");
            book.City = Console.ReadLine();

            _context.SaveChanges();
            Console.WriteLine("Book information successfully updated!");
        }


        public void DeleteBook()
        {
            Console.WriteLine("Enter the ID of the book you want to delete.:");
            var bookId = int.Parse(Console.ReadLine());

            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            _context.Books.Remove(book);
            _context.SaveChanges();
            Console.WriteLine("Book successfully deleted.!");
        }


        public void AddAuthor()
        {
            Console.WriteLine("Enter the author's name:");
            var firstName = Console.ReadLine();
            Console.WriteLine("Enter the author's last name:");
            var lastName = Console.ReadLine();
            Console.WriteLine("Enter the author's middle name (if any):");
            var middleName = Console.ReadLine();
            Console.WriteLine("Enter the author's date of birth (yyyy-mm-dd):");
            var birthDate = DateTime.Parse(Console.ReadLine());

            var author = new Author
            {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                BirthDate = birthDate
            };

            _context.Authors.Add(author);
            _context.SaveChanges();
            Console.WriteLine("Author successfully added!");
        }

        public void UpdateAuthor()
        {
            Console.WriteLine("Enter the ID of the author you want to update:");
            var authorId = int.Parse(Console.ReadLine());

            var author = _context.Authors.FirstOrDefault(a => a.Id == authorId);
            if (author == null)
            {
                Console.WriteLine("Author not found.");
                return;
            }

            Console.WriteLine("Enter new first name:");
            author.FirstName = Console.ReadLine();
            Console.WriteLine("Enter new last name:");
            author.LastName = Console.ReadLine();
            Console.WriteLine("Enter new middle name (if any):");
            author.MiddleName = Console.ReadLine();
            Console.WriteLine("Enter new birth date (yyyy-MM-dd):");
            author.BirthDate = DateTime.Parse(Console.ReadLine());

            _context.SaveChanges();
            Console.WriteLine("Author information updated successfully!");
        }

        public void AddReader()
        {
            var registrationService = new RegistrationService(_context);
            registrationService.RegisterUser();
        }

        public void UpdateReader()
        {
            Console.WriteLine("Enter the ID of the reader you want to update:");
            var readerId = int.Parse(Console.ReadLine());

            var reader = _context.Readers.FirstOrDefault(r => r.Id == readerId);
            if (reader == null)
            {
                Console.WriteLine("Reader not found.");
                return;
            }

            Console.WriteLine("Enter new first name:");
            reader.FirstName = Console.ReadLine();
            Console.WriteLine("Enter new last name:");
            reader.LastName = Console.ReadLine();
            Console.WriteLine("Enter new document type:");
            reader.DocumentType = Console.ReadLine();
            Console.WriteLine("Enter new document number:");
            reader.DocumentNumber = Console.ReadLine();

            _context.SaveChanges();
            Console.WriteLine("Reader information updated successfully!");
        }

        public void DeleteReader()
        {
            Console.WriteLine("Enter the ID of the reader you want to delete:");
            var readerId = int.Parse(Console.ReadLine());

            var reader = _context.Readers.FirstOrDefault(r => r.Id == readerId);
            if (reader == null)
            {
                Console.WriteLine("Reader not found.");
                return;
            }

            _context.Users.Remove(reader);
            _context.SaveChanges();
            Console.WriteLine("Reader deleted successfully!");
        }

        public void ViewBorrowHistory()
        {
            var borrowedBooks = _context.BorrowedBooks
                .Include(bb => bb.Book)
                .Include(bb => bb.Reader)
                .OrderBy(bb => bb.ReturnDate)
                .ToList();

            Console.WriteLine("\nLoan history:");
            foreach (var borrowedBook in borrowedBooks)
            {
                Console.WriteLine($"Book: {borrowedBook.Book.Title}, Reader: {borrowedBook.Reader.FirstName} {borrowedBook.Reader.LastName}, Taken: {borrowedBook.BorrowedDate.ToShortDateString()}, Return to: {borrowedBook.ReturnDate.ToShortDateString()}, {(borrowedBook.ActualReturnDate.HasValue ? "Returned: " + borrowedBook.ActualReturnDate.Value.ToShortDateString() : "Not Returned")}");
            }
        }

        public void ViewDebtors()
        {
            var debtors = _context.BorrowedBooks
                .Include(bb => bb.Reader)
                .Include(bb => bb.Book)
                .Where(bb => bb.ActualReturnDate == null && bb.ReturnDate < DateTime.Now)
                .ToList();

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
        }
        public void ViewReaderBorrowHistory()
        {
            Console.WriteLine("Enter the ID of the reader:");
            var readerId = int.Parse(Console.ReadLine());

            var reader = _context.Readers.FirstOrDefault(r => r.Id == readerId);
            if (reader == null)
            {
                Console.WriteLine("Reader not found.");
                return;
            }

            var borrowHistory = _context.BorrowedBooks
                .Include(bb => bb.Book)
                .Where(bb => bb.ReaderId == readerId)
                .OrderBy(bb => bb.BorrowedDate)
                .ToList();

            if (borrowHistory.Any())
            {
                Console.WriteLine($"\nBorrow history for {reader.FirstName} {reader.LastName}:");
                foreach (var record in borrowHistory)
                {
                    Console.WriteLine($"Book: {record.Book.Title}, Borrowed: {record.BorrowedDate.ToShortDateString()}, Due: {record.ReturnDate.ToShortDateString()}, Returned: {(record.ActualReturnDate.HasValue ? record.ActualReturnDate.Value.ToShortDateString() : "Not returned")}");
                }
            }
            else
            {
                Console.WriteLine("No borrow history found for this reader.");
            }
        }


    }

}