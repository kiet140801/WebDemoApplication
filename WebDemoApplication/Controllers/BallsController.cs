using Microsoft.AspNetCore.Mvc;
using WebDemoApplication.Models.Dtos;
using WebDemoApplication.Models.Entities;

namespace WebDemoApplication.Controllers
{
    public class BallsController : Controller
    {
        private readonly BallDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public BallsController(BallDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index()
        {
            var balls = _context.Balls.OrderByDescending(x => x.Id).ToList();
            return View(balls);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BallDto ballDto)
        {
            if(ballDto.ImageFile == null)
            {
                ModelState.AddModelError("error", "vui long nhap thong tin");
            }
            if(!ModelState.IsValid)
            {
                return View(ballDto);
            }
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(ballDto.ImageFile!.FileName);

            // ~/image/item1.jpg
            string imageFullPath = _environment.WebRootPath + "/Image/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                ballDto.ImageFile.CopyTo(stream);
            }

            var ball = new Ball()
            {
                Name = ballDto.Name,
                Price = ballDto.Price,
                ImageFileName = newFileName,
            };
            _context.Balls.Add(ball);
            _context.SaveChanges();

            return RedirectToAction("Index", "Balls");
        }
        public IActionResult Edit(Guid id)
        {
            var ball = _context.Balls.Find(id);
            if(ball == null)
            {
                return RedirectToAction("Index", "Balls");
            }
            var ballDto = new BallDto()
            {
                Name = ball.Name,
                Price = ball.Price,
            };
            ViewData["BallId"] = ball.Id;
            ViewData["ImageFileName"] = ball.ImageFileName;

            return View(ballDto);
        }
        [HttpPost]
        public IActionResult Edit(Guid id, BallDto ballDto)
        {
            var ball = _context.Balls.Find(id);
            if(ball == null)
            {
                return RedirectToAction("Index", "Balls");
            }
            if(!ModelState.IsValid)
            {
                ViewData["BallId"] = ball.Id;
                ViewData["ImageFileName"] = ball.ImageFileName;

                return View(ballDto);
            }
            string newFileName = ball.ImageFileName;
            if(ballDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(ballDto.ImageFile!.FileName);

                string imageFullPath = _environment.WebRootPath + "/Image/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    ballDto.ImageFile.CopyTo(stream);
                }

                string oldBallPath = _environment.WebRootPath + ball.ImageFileName;
                System.IO.File.Delete(oldBallPath);
            }

            ball.Name = ballDto.Name;
            ball.Price = ballDto.Price;
            ball.ImageFileName = newFileName;

            _context.SaveChanges();

            return RedirectToAction("Index", "Balls");
        }

        public IActionResult Delete(Guid id)
        {
            var ball = _context.Balls.Find(id);
            if(ball == null)
            {
                return RedirectToAction("Index", "Balls");
            }

            string imageBallPath = _environment.WebRootPath + "/Image/" + ball.ImageFileName;
            System.IO.File.Delete(imageBallPath);

            _context.Balls.Remove(ball);
            _context.SaveChanges(true);

            return RedirectToAction("Index", "Balls");
        }
    }
}
