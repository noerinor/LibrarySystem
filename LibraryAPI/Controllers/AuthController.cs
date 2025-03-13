using LibraryAPI.Models;
using LibraryDLL.Data;
using LibraryDLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LibraryContext _context;

        public AuthController(LibraryContext context)
        {
            _context = context;
        }

        
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(RegisterModel model)
        {
            var user = new User
            {
                Login = model.Login,
                Password = model.Password,
                Email = model.Email,
                Role = model.Role
            };

            if (model.Role == "Reader")
            {
                var reader = new Reader
                {
                    Login = model.Login,
                    Password = model.Password,
                    Email = model.Email,
                    Role = model.Role,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DocumentType = model.DocumentType,
                    DocumentNumber = model.DocumentNumber
                };

                _context.Users.Add(reader);
            }
            else
            {
                _context.Users.Add(user);
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        
        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }
    }
}
