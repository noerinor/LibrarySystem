
using LibraryDLL.Data;
using LibraryDLL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

    
    public List<Book> SearchBooksByTitle(string title)
    {
        return _context.Books
            .Where(b => b.Title.Contains(title))
            .ToList();
    }

    
    public List<Book> SearchBooksByAuthor(string authorName)
    {
        return _context.Books
            .Where(b => b.Authors.Any(a => a.FirstName.Contains(authorName) || a.LastName.Contains(authorName)))
            .ToList();
    }

    
    public List<BorrowedBook> ViewBorrowedBooks()
    {
        return _context.BorrowedBooks
            .Include(bb => bb.Book)
            .Where(bb => bb.ReaderId == _readerId)
            .OrderBy(bb => bb.ReturnDate)
            .ToList();
    }

    
    public void BorrowBook(int bookId)
    {
        var borrowedBook = new BorrowedBook
        {
            BookId = bookId,
            ReaderId = _readerId,
            BorrowedDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(30) // дефолтні 30 днів
        };

        _context.BorrowedBooks.Add(borrowedBook);
        _context.SaveChanges();
    }

    
    public void ReturnBook(int bookId)
    {
        var borrowedBook = _context.BorrowedBooks
            .FirstOrDefault(bb => bb.BookId == bookId && bb.ReaderId == _readerId && bb.ActualReturnDate == null);

        if (borrowedBook != null)
        {
            borrowedBook.ActualReturnDate = DateTime.Now;
            _context.SaveChanges();
        }
    }

    
    public List<BorrowedBook> ViewReturnedBooks()
    {
        return _context.BorrowedBooks
            .Include(bb => bb.Book)
            .Where(bb => bb.ReaderId == _readerId && bb.ActualReturnDate != null)
            .OrderByDescending(bb => bb.ActualReturnDate)
            .ToList();
    }

    
    public List<BorrowedBook> CheckOverdueBooks()
    {
        return _context.BorrowedBooks
            .Include(bb => bb.Book)
            .Where(bb => bb.ReaderId == _readerId && bb.ActualReturnDate == null && bb.ReturnDate < DateTime.Now)
            .ToList();
    }
}