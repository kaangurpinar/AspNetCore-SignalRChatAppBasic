using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalRChatApp.Repository;
using System.Security.Claims;

namespace SignalRChatApp.ViewComponents
{
    public class RoomsViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public RoomsViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var rooms = await _context.RoomsUsers
                                                .Include(u => u.Room)
                                                .Where(u => u.UserId == userId)
                                                .Select(u => u.Room)
                                                .ToListAsync();

            return View(rooms);
        }
    }
}
