
using LibraryDLL.Data;
using LibraryDLL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public class ReaderFunctions
{
    private readonly LibraryContext _context;
    private readonly int _readerId;

    public ReaderFunctions(LibraryContext context, int readerId)
    {
        _context = context;
        _readerId = readerId;
    }

    public void SearchBooksByTitle()
    {
        Console.WriteLine("Write a name of book:");
        var title = Console.ReadLine();

        var books = _context.Books
            .Where(b => b.Title.Contains(title))
            .ToList();

        if (books.Any())
        {
            Console.WriteLine("\nBooks Found:");
            foreach (var book in books)
            {
                Console.WriteLine($"ID: {book.Id}, Name: {book.Title}, Authors: {string.Join(", ", book.Authors.Select(a => $"{a.FirstName} {a.LastName}"))}, Year: {book.Year}");
            }
        }
        else
        {
            Console.WriteLine("Books by your request havent been found.");
        }
    }

    public void SearchBooksByAuthor()
    {
        Console.WriteLine("Write name or surname of author:");
        var authorName = Console.ReadLine();

        var books = _context.Books
            .Where(b => b.Authors.Any(a => a.FirstName.Contains(authorName) || a.LastName.Contains(authorName)))
            .ToList();

        if (books.Any())
        {
            Console.WriteLine("\nBooks Found:");
            foreach (var book in books)
            {
                Console.WriteLine($"ID: {book.Id}, Name: {book.Title}, Authors: {string.Join(", ", book.Authors.Select(a => $"{a.FirstName} {a.LastName}"))}, YEar: {book.Year}");
            }
        }
        else
        {
            Console.WriteLine("Books by your request havent been found.");
        }
    }

    public void ViewBorrowedBooks()
    {
        var borrowedBooks = _context.BorrowedBooks
            .Include(bb => bb.Book)
            .Where(bb => bb.ReaderId == _readerId)
            .OrderBy(bb => bb.ReturnDate)
            .ToList();

        if (borrowedBooks.Any())
        {
            Console.WriteLine("\nBooks you have taken:");
            foreach (var borrowedBook in borrowedBooks)
            {
                var status = borrowedBook.ActualReturnDate.HasValue ? "Returned" : "Not Returned";
                Console.WriteLine($"Book: {borrowedBook.Book.Title}, Taken: {borrowedBook.BorrowedDate.ToShortDateString()}, Return till: {borrowedBook.ReturnDate.ToShortDateString()}, Status: {status}");
            }
        }
        else
        {
            Console.WriteLine("You dont have taken books.");
        }
    }

    public void BorrowBook()
    {
        Console.WriteLine("Write book ID that you want to take:");
        var bookId = int.Parse(Console.ReadLine());

        var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
        if (book == null)
        {
            Console.WriteLine("Book not found.");
            return;
        }

        var borrowedBook = new BorrowedBook
        {
            BookId = bookId,
            ReaderId = _readerId,
            BorrowedDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(30) // дефолтні 30 днів
        };

        _context.BorrowedBooks.Add(borrowedBook);
        _context.SaveChanges();
        Console.WriteLine("Book taken!");
    }

    public void ReturnBook()
    {
        Console.WriteLine("Enter the ID of the book you want to return:");
        var bookId = int.Parse(Console.ReadLine());

        var borrowedBook = _context.BorrowedBooks
            .FirstOrDefault(bb => bb.BookId == bookId && bb.ReaderId == _readerId && bb.ActualReturnDate == null);

        if (borrowedBook == null)
        {
            Console.WriteLine("Book not found or already returned.");
            return;
        }

        borrowedBook.ActualReturnDate = DateTime.Now;
        _context.SaveChanges();
        Console.WriteLine("Book returned successfully!");
    }
    public void ViewReturnedBooks()
    {
        var returnedBooks = _context.BorrowedBooks
            .Include(bb => bb.Book)
            .Where(bb => bb.ReaderId == _readerId && bb.ActualReturnDate != null)
            .OrderByDescending(bb => bb.ActualReturnDate)
            .ToList();

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
    }

    public void CheckOverdueBooks()
    {
        var overdueBooks = _context.BorrowedBooks
            .Include(bb => bb.Book)
            .Where(bb => bb.ReaderId == _readerId && bb.ActualReturnDate == null && bb.ReturnDate < DateTime.Now)
            .ToList();

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
    }

}