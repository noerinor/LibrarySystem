using System;
using System.Collections.Generic;

namespace LibraryDLL.Models
{
    
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } 
    }

    
    public class Reader : User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }

    
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PublishingCode { get; set; }
        public string PublishingCodeType { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public ICollection<Author> Authors { get; set; }
        public ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }

    
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Book> Books { get; set; }
    }

    
    public class BorrowedBook
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int ReaderId { get; set; }
        public Reader Reader { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
    }
}