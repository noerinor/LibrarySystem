
using LibraryDLL.Data;
using LibraryDLL.Models;

namespace LibraryDLL.Services;
public class RegistrationService
{
    private readonly LibraryContext _context;

    public RegistrationService(LibraryContext context)
    {
        _context = context;
    }

    
    public User RegisterUser(string login, string password, string email, string role, string firstName = null, string lastName = null, string documentType = null, string documentNumber = null)
    {
        User user = null;

        if (role == "Reader")
        {
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
        return user; 
    }
}