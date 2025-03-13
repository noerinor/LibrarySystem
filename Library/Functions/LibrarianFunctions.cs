
using LibraryDLL.Data;
using LibraryDLL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class LibrarianFunctions
{
    private readonly LibraryContext _context;

    public LibrarianFunctions(LibraryContext context)
    {
        _context = context;
    }

    
    public void AddBook(string title, string publishingCode, string publishingCodeType, int year, string country, string city)
    {
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
    }

    
    public void AddAuthor(string firstName, string lastName, string middleName, DateTime birthDate)
    {
        var author = new Author
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            BirthDate = birthDate
        };

        _context.Authors.Add(author);
        _context.SaveChanges();
    }

    
    public void UpdateBook(int bookId, string title, string publishingCode, string publishingCodeType, int year, string country, string city)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
        if (book != null)
        {
            book.Title = title;
            book.PublishingCode = publishingCode;
            book.PublishingCodeType = publishingCodeType;
            book.Year = year;
            book.Country = country;
            book.City = city;

            _context.SaveChanges();
        }
    }

    
    public void UpdateAuthor(int authorId, string firstName, string lastName, string middleName, DateTime birthDate)
    {
        var author = _context.Authors.FirstOrDefault(a => a.Id == authorId);
        if (author != null)
        {
            author.FirstName = firstName;
            author.LastName = lastName;
            author.MiddleName = middleName;
            author.BirthDate = birthDate;

            _context.SaveChanges();
        }
    }

    public void AddReader(string login, string password, string email, string firstName, string lastName, string documentType, string documentNumber)
    {
        var reader = new Reader
        {
            Login = login,
            Password = password,
            Email = email,
            Role = "Reader", 
            FirstName = firstName,
            LastName = lastName,
            DocumentType = documentType,
            DocumentNumber = documentNumber
        };

        _context.Users.Add(reader);
        _context.SaveChanges();
    }
    
    public void UpdateReader(int readerId, string firstName, string lastName, string documentType, string documentNumber)
    {
        var reader = _context.Readers.FirstOrDefault(r => r.Id == readerId);
        if (reader != null)
        {
            reader.FirstName = firstName;
            reader.LastName = lastName;
            reader.DocumentType = documentType;
            reader.DocumentNumber = documentNumber;

            _context.SaveChanges();
        }
    }

    
    public void DeleteReader(int readerId)
    {
        var reader = _context.Readers.FirstOrDefault(r => r.Id == readerId);
        if (reader != null)
        {
            _context.Users.Remove(reader);
            _context.SaveChanges();
        }
    }

    
    public List<BorrowedBook> ViewBorrowHistory()
    {
        return _context.BorrowedBooks
            .Include(bb => bb.Book)
            .Include(bb => bb.Reader)
            .OrderBy(bb => bb.ReturnDate)
            .ToList();
    }

    
    public List<BorrowedBook> ViewDebtors()
    {
        return _context.BorrowedBooks
            .Include(bb => bb.Reader)
            .Include(bb => bb.Book)
            .Where(bb => bb.ActualReturnDate == null && bb.ReturnDate < DateTime.Now)
            .ToList();
    }

    
    public List<BorrowedBook> ViewReaderBorrowHistory(int readerId)
    {
        return _context.BorrowedBooks
            .Include(bb => bb.Book)
            .Where(bb => bb.ReaderId == readerId)
            .OrderBy(bb => bb.BorrowedDate)
            .ToList();
    }
}