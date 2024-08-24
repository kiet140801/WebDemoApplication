

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDemoApplication.Models.Entities;


namespace YourNamespace.Controllers
{
    public class CartController : Controller
    {
        private readonly BallDbContext _context;

        public CartController(BallDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(Guid id)
        {
            var ball = await _context.Balls.FindAsync(id);
            if (ball == null)
            {
                return NotFound();
            }

            HttpContext.Request.Cookies.TryGetValue("CartNumber", out var cartNumber); ///.GetString("CartNumber") ?? Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(cartNumber))
            {
                HttpContext.Response.Cookies.Append("CartNumber", Guid.NewGuid().ToString()); //.SetString("CartNumber", Guid.NewGuid().ToString());
            }

            //var cartItem = await _context.CartItems
            //    .FirstOrDefaultAsync(x => x.CartNumber.ToString() == cartNumber && x.BallId == id);

            //if (cartItem == null)
            //{
            //    cartItem = new CartItem
            //    {
            //        CartNumber = new Guid(cartNumber),
            //        BallId = id,
            //        Quantity = 1,
            //        Ball = ball
            //    };
            //    _context.CartItems.Add(cartItem);
            //}
            //else
            //{
            //    cartItem.Quantity += 1;
            //    _context.CartItems.Update(cartItem);
            //}

            //await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Carts"); 
        }

        public async Task<IActionResult> Index()
        {
            var cartNumber = HttpContext.Session.GetString("CartNumber");
            if (cartNumber == null)
            {
                return View(new List<CartItem>());
            }

            var cartItems = await _context.CartItems
                .Include(x => x.Ball)
                .Where(x => x.CartNumber.ToString() == cartNumber)
                .ToListAsync();

            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
