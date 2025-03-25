//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace sql_test.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class UserController(ApplicationDbContext _context) : ControllerBase
//    {
//        [HttpPost(Name = "CreateUser")]
//        public async Task<IActionResult> Create([FromBody] User user)
//        {
//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();

//            return Ok(user);
//        }


//        [HttpGet(Name = "GetAllUsers")]
//        public async Task<IActionResult> GetAllUsers()
//        {
//            var users = await _context.Users.ToListAsync();
//            return Ok(users);
//        }


//        [HttpGet("{userId}", Name = "GetUserById")]
//        public async Task<IActionResult> GetUserById(int userId)
//        {
//            var user = await _context.Users.FindAsync(userId);
//            if (user == null)
//            {
//                return NotFound();
//            }
//            return Ok(user);
//        }


//    }
//}
