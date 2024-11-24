
using _1111760.Data;
using _1111760.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using X.PagedList;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;

namespace _1111760.Controllers
{
    public class UsersController : Controller
    {
        private readonly CmsContext _context;
        public UsersController(CmsContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? page = 1)
        {
            var role = HttpContext.Session.GetString("UserRole");

            HttpContext.Session.SetString("Id", "Logined"); //打記號進來，名稱為Id的記號內容是Logined
            TempData["message"] = "Logged in!";


            if (HttpContext.Session.GetString("mark") == null)  //是否寫記號進來，記號名稱是Id
            {
                TempData["message"] = "Please Login!";
                return RedirectToAction("Login", "Users"); //重新導向到Login1(Users是Controller名稱)
            }
            //如果不是空的就顯示資料表
            const int pageSize = 8;
            ViewBag.usersModel = GetPagedProcess(page, pageSize);
            return View(await _context.Tablebrands1111760.Skip<User>(pageSize * ((page ?? 1) - 1)).Take(pageSize).ToListAsync());

           


            var users = await _context.Tablebrands1111760.ToListAsync();
            return View(users);
        }



        protected IPagedList<User> GetPagedProcess(int? page, int pageSize)
        {
            if (page.HasValue && page < 1)  
                return null;

            var listUnpaged = GetStuffFromDatabase();  //用 GetStuffFromDatabase方法獲取資料庫
            IPagedList<User> pagelist = listUnpaged.ToPagedList(page ?? 1, pageSize);

            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;

        }

        protected IQueryable<User> GetStuffFromDatabase()
        {
            return _context.Tablebrands1111760;
        }

        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null || _context.Tablebrands1111760 == null)
            {
                var msgObject = new
                {
                    stastuscode = StatusCodes.Status400BadRequest,
                    error = "無效的請求，必須提供Id編號"
                };

                return new BadRequestObjectResult(msgObject);
            }

            var user = await _context.Tablebrands1111760.FirstOrDefaultAsync(m => m.Id == Id);
            if (user == null)
                return NotFound();

            return View(user);

        }

        //GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Country,Year,Type")] User user)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "admin")
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)  
            {
                _context.Tablebrands1111760.Add(user);
                await _context.SaveChangesAsync();   

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "admin")
            {
                return Unauthorized();
            }

            if (Id == null || _context.Tablebrands1111760 == null)
            {
                return NotFound();
            }

            var user = await _context.Tablebrands1111760.FindAsync(Id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id, Name, Country,Year,Type")] User user)
        {
            if (Id != user.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Tablebrands1111760.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                        return NotFound();
                    else
                        throw;
                }


            }
            return RedirectToAction(nameof(Index));

        }

        private bool UserExists(int id)
        {
            return _context.Tablebrands1111760.Any(e => e.Id == id);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "admin")
            {
                return Unauthorized();
            }

            if (id == null || _context.Tablebrands1111760 == null)
            {
                return NotFound();
            }

            var user = await _context.Tablebrands1111760.FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tablebrands1111760 == null)
            {
                return Problem("Entity set 'CmsContext.Table' is null.");
            }

            var user = await _context.Tablebrands1111760.FindAsync(id);

            if (user != null)
            {
                _context.Tablebrands1111760.Remove(user);
                await _context.SaveChangesAsync(); 
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult InputQuery()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Query(int? page = 1)
        {
            const int pageSize = 4;
            ViewBag.usersModel = GetPagedProcess(page, pageSize);
            return View(await _context.Tablebrands1111760.Skip<User>(pageSize * ((page ?? 1) - 1)).Take(pageSize).ToListAsync());
            
        }

        [HttpPost]
        public async Task<IActionResult> Query(int Id, int Year, string Type)
        {
            var users = await (from p in _context.Tablebrands1111760
                               where p.Year < 2000 && p.Type == Type
                               orderby p.Id
                               select p).ToListAsync();

            return View(users);
           
        }




        [HttpGet]
        public async Task<IActionResult> SelectQuery()
        {
            var names = await (from p in _context.Tablebrands1111760
                               orderby p.Name
                               select p.Name).Distinct().ToListAsync();

            ViewBag.Mylist = names;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SelectName(string fName)
        {
            var users = await (from p in _context.Tablebrands1111760
                               where p.Name == fName
                               orderby p.Name
                               select p).Distinct().ToListAsync();

            return View(users);
        }



        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Id") == null)  //Id是記號名稱，null沒有記號
            {
                TempData["message"] = "Please Login!";
                return View();
            }
            else
            {
                return View();
            }

        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("Id") == null)  //Id是記號名稱
            {
                TempData["message"] = "Logged Out!";
                return View();
            }
            else
            {
                return View();
            }

        }


        [HttpGet]
        public IActionResult LoginCheck()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginCheck(int Number, string Account, string Password)
        {
            
                if (Account == null || Password == null)
            {
                TempData["message"] = "Please enter account and password!";
                return RedirectToAction("Login", "Users");
            }

            var users = await (from p in _context.Tableuses1111760
                               where p.Account == Account && p.Password == Password
                               orderby p.Number
                               select p).ToListAsync();

            if (users.Count != 0)
            {
                if (Account == "member00" && Password == "0000")
                {
                    HttpContext.Session.SetString("UserRole", "admin");
                }
                else
                {
                    HttpContext.Session.SetString("UserRole", "user");
                }

                HttpContext.Session.SetString("mark", "Id");
                TempData["message"] = "Logged in!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Login failed!";
                return RedirectToAction("Login", "Users");
            }
        }



    }
}
