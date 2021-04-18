using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Clothes.Controllers
{
    public class BaseController : Controller
    {
        protected UnitOfWork unitOfWork;

        public const string AUTH_TOKEN = "__clothes_auth";
        public new User User { get; set; }

        public BaseController()
        {
            unitOfWork = new UnitOfWork(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var credentials = Request.Cookies[AUTH_TOKEN]?.Value?.Split('_');
            if (credentials?.Length == 2)
            {
                Int32.TryParse(credentials[0], out int id);
                User = unitOfWork.Users.GetUserByIdHash(id, credentials[1]);
            }
            ViewBag.User = User;
            if (User != null)
            {
                var body = unitOfWork.Carts.FirstOrDefault(x => x.UserId == User.Id)?.OrderBody;
                var products = string.IsNullOrWhiteSpace(body) ? new List<int>() : Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(body);
                var sum = (from product in unitOfWork.Products
                           join id in products on product.Id equals id
                           select product.Price).Sum();
                ViewBag.CartSubHeader = $"${sum} ({products.Count}pcs.)";
            }
            base.OnActionExecuting(filterContext);
        }

        public void SetAuthCookie(User user)
        {
            if (user != null)
            {
                User = user;
                Response.Cookies[AUTH_TOKEN].Value = $"{User.Id}_{User.Password}";
            }
        }

        public void DeleteAuthCookie()
        {
            if (User != null)
            {
                Response.Cookies[AUTH_TOKEN].Value = null;
                User = null;
                ViewBag.User = null;
            }
        }
    }
}