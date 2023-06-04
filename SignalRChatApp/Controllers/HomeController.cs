using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalRChatApp.Models;
using SignalRChatApp.Repository;
using System.Diagnostics;
using System.Security.Claims;

namespace SignalRChatApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppDbContext _appDbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            var rooms = _appDbContext.Rooms
                                        .Include(r => r.Users)
                                        .Where(r => !r.Users.Any(u => u.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
                                        .ToList();
            return View(rooms);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "Admin, Moderation")]
        public async Task<IActionResult> Users()
        {
            var users = await _appDbContext.Users.ToListAsync();

            return View(users);
        }

        [HttpGet("{id}")]
        public IActionResult Room(int id)
        {
            var room = _appDbContext.Rooms.Include(r => r.Messages).FirstOrDefault(r => r.Id == id);

            return View(room);
        }

        public async Task<IActionResult> JoinRoom(int id)
        {
            var roomUser = new RoomUser()
            {
                RoomId = id,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
            };

            _appDbContext.RoomsUsers.Add(roomUser);

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Room", "Home", new { id = id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoom(string name, string password)
        {
            var room = new Room()
            {
                Name = name
            };

            if(string.IsNullOrEmpty(password))
            {
                room.Type = "Public";
            }
            else
            {
                room.Password = password;
                room.Type = "Private";
            }

            _appDbContext.Rooms.Add(room);

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int roomId, string message)
        {
            var _message = new Message()
            {
                RoomId = roomId,
                Text = message,
                UserName = User.Identity.Name,
                TimeStamp = DateTime.UtcNow
            };

            _appDbContext.Add(_message);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Room", new { id = roomId });
        }

        public IActionResult CheckRoomPassword(int id, string password)
        {
            var room = _appDbContext.Rooms.FirstOrDefault(r => r.Id == id);

            if(password != room.Password)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Room", "Home", new { id = id });
        }

        public async Task<IActionResult> LeaveRoom(int roomId)
        {
            var roomUser = _appDbContext.RoomsUsers
                                            //.Include(r => r.User)
                                            //.ThenInclude(r => r.Rooms)
                                            .Where(r => r.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value && r.RoomId == roomId)
                                            .FirstOrDefault();

            _appDbContext.RoomsUsers.Remove(roomUser);

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin, Moderation")]
        public async Task<IActionResult> KickUser(string username, int roomId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);

            var roomUser = _appDbContext.RoomsUsers
                                            .Where(r => r.UserId == user.Id && r.RoomId == roomId)
                                            .FirstOrDefault();

            _appDbContext.RoomsUsers.Remove(roomUser);

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Room", "Home", new { id = roomId });
        }

        [Authorize(Roles = "Admin, Moderation")]
        public async Task<IActionResult> BanUser(string username, int roomId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);

            var roomUser = _appDbContext.RoomsUsers
                                            .Where(r => r.UserId == user.Id && r.RoomId == roomId)
                                            .FirstOrDefault();

            _appDbContext.RoomsUsers.Remove(roomUser);

            var bannedUser = new BannedUser()
            {
                UserId = user.Id,
                RoomId = roomId
            };

            _appDbContext.BannedUsers.Add(bannedUser);

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Room", "Home", new { id = roomId });
        }
    }
}