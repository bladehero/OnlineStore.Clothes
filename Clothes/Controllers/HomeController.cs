using Clothes.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Clothes.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
            ViewBag.Categories = unitOfWork.Categories.Take(5);
            ViewBag.Tags = unitOfWork.Tags.Take(5);
            ViewBag.BestSellers = GetBestSellers().Take(5);
        }

        ~HomeController()
        {
            unitOfWork.Dispose();
        }

        public ActionResult Index()
        {
            var model = new IndexVM
            {
                Products = unitOfWork.Products.Random(10).ToList(),
                Tags = unitOfWork.Tags.Random(6)
            };
            return View(model);
        }

        public ActionResult Checkout()
        {
            if (User == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var cart = unitOfWork.Carts.FirstOrDefault(x => x.UserId == User.Id);
            var productIds = new List<int>();
            if (!string.IsNullOrWhiteSpace(cart?.OrderBody))
            {
                productIds.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<int>>(cart.OrderBody));
            }
            var orderProducts = from product in unitOfWork.Products
                                join id in productIds on product.Id equals id
                                group product by product.Id into p
                                select new CheckoutProductVM
                                {
                                    Product = unitOfWork.Products.FirstOrDefault(x => x.Id == p.Key),
                                    Quantity = p.Count(),
                                };
            return View(orderProducts);
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Account()
        {
            return View();
        }

        public ActionResult Products(int? id)
        {
            var model = new ProductsVM
            {
                Products = (from taggedProduct in unitOfWork.TaggedProducts
                            join tag in unitOfWork.Tags on taggedProduct.TagId equals tag.Id
                            join product in unitOfWork.Products on taggedProduct.ProductId equals product.Id
                            where !id.HasValue || tag.Id == id
                            select product),
                SelectedTag = id.HasValue ? unitOfWork.Tags.FirstOrDefault(x => x.Id == id) : null
            };
            model.SearchString = id.HasValue ? $"Tag \"{model.SelectedTag?.Name}\"" : null;
            return View(model);
        }

        public ActionResult Category(int? id)
        {
            var model = new ProductsVM
            {
                Products = unitOfWork.Products.Find(x => !id.HasValue || x.CategoryId == id.Value),
                SelectedCategory = id.HasValue ? unitOfWork.Categories.FirstOrDefault(x => x.Id == id) : null
            };
            model.SearchString = id.HasValue ? $"Category \"{model.SelectedCategory?.Name}\"" : null;
            return View("Products", model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterVM registerVM)
        {
            var user = new User
            {
                Email = registerVM.Email,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Password = registerVM.Password
            };
            unitOfWork.Users.Insert(user);
            SetAuthCookie(unitOfWork.Users.GetUserByCredentials(user.Email, registerVM.Password));
            return Redirect("Index");
        }

        public ActionResult Single(int id)
        {
            var model = new SingleVM
            {
                Product = unitOfWork.Products.FirstOrDefault(x => x.Id == id),
                ProductPictures = unitOfWork.ProductPictures.Find(x => x.ProductId == id),
                OtherProducts = unitOfWork.Products.Random(3)
            };

            return View(model);
        }

        public ActionResult Tags(int? id, bool isTag = true)
        {
            var model = new TagsVM
            {
                SelectedTag = isTag ? id : null,
                Tags = isTag ? unitOfWork.Tags : from taggedProduct in unitOfWork.TaggedProducts.Find(x => x.ProductId == id)
                                                 join tag in unitOfWork.Tags on taggedProduct.TagId equals tag.Id
                                                 select tag
            };
            return PartialView(model);
        }

        public ActionResult Categories(int? id)
        {
            var model = new CategoriesVM
            {
                SelectedCategory = id,
                Categories = unitOfWork.Categories
            };
            return PartialView(model);
        }

        public ActionResult BestSellers()
        {
            var model = new BestSellersVM
            {
                Products = GetBestSellers()
            };
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SendMessage(SendMessageVM sendMessageVM)
        {
            var message = new Message
            {
                Email = sendMessageVM.Email,
                Name = sendMessageVM.Name,
                Phone = sendMessageVM.Phone,
                Text = sendMessageVM.Text
            };
            try
            {
                unitOfWork.Messages.Insert(message);
            }
            catch (Exception)
            {
                return Redirect("Index");
            }
            return View("Contact");
        }

        [HttpPost]
        public ActionResult Subscribe(SubscribeVM subscribeVM)
        {
            var newsletter = new Newsletter
            {
                Email = subscribeVM.Email
            };
            try
            {
                unitOfWork.Newsletters.Insert(newsletter);
            }
            catch (Exception ex)
            {

            }
            return Redirect("Index");
        }

        public ActionResult Search(SearchVM searchVM)
        {
            var search = searchVM.Search?.ToLower();
            var model = new ProductsVM
            {
                Products = (from taggedProduct in unitOfWork.TaggedProducts
                            join tag in unitOfWork.Tags on taggedProduct.TagId equals tag.Id
                            join product in unitOfWork.Products on taggedProduct.ProductId equals product.Id
                            join category in unitOfWork.Categories on product.CategoryId equals category.Id
                            where string.IsNullOrWhiteSpace(searchVM.Search)
                                  || category.Name.ToLower().Contains(search)
                                  || product.Name.ToLower().Contains(search)
                                  || tag.Name.ToLower().Contains(search)
                            select product).Distinct()
            };
            model.SearchString = string.IsNullOrWhiteSpace(search) ? null : $"Search \"{search}\" ({model.Products.Count()})";
            return View("Products", model);
        }

        public ActionResult Login(LoginVM loginVM)
        {
            var user = unitOfWork.Users.GetUserByCredentials(loginVM.Email, loginVM.Password);
            SetAuthCookie(user);
            return Redirect("Index");
        }

        public ActionResult Logout()
        {
            DeleteAuthCookie();
            return Redirect("Index");
        }

        public ActionResult AddToCart(int id)
        {
            if (User == null)
            {
                return RedirectToAction("Register", "Home");
            }
            var cart = unitOfWork.Carts.FirstOrDefault(x => x.UserId == User.Id);
            var products = new List<int>();
            products.Add(id);
            if (!string.IsNullOrWhiteSpace(cart?.OrderBody))
            {
                products.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<int>>(cart.OrderBody));
            }
            else if(cart == null || cart.Id == 0)
            {
                cart = new Cart
                {
                    UserId = User.Id
                };
            }
            cart.DateTime = DateTime.Now;
            cart.OrderBody = Newtonsoft.Json.JsonConvert.SerializeObject(products);
            unitOfWork.Carts.Merge(cart);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Buy()
        {
            if (User == null)
            {
                return RedirectToAction("Register", "Home");
            }
            var cart = unitOfWork.Carts.FirstOrDefault(x => x.UserId == User.Id);
            var products = new List<int>();
            if (!string.IsNullOrWhiteSpace(cart?.OrderBody))
            {
                products.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<int>>(cart.OrderBody));
            }
            cart.DateTime = DateTime.Now;
            cart.OrderBody = null;
            unitOfWork.Carts.Merge(cart);
            var order = new Order
            {
                DateTime = DateTime.Now,
                UserId = User.Id
            };
            unitOfWork.Orders.Insert(order);
            var orderList = new List<OrderProduct>();
            foreach (var product in products.GroupBy(x => x).Select(x => new { Id = x.FirstOrDefault(), Quantity = x.Count() }))
            {
                orderList.Add(new OrderProduct { OrderId = order.Id, ProductId = product.Id, Quantity = product.Quantity });
            }
            orderList.ForEach(x => unitOfWork.OrderProducts.Insert(x));
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RemoveFromCart(int id)
        {
            if (User == null)
            {
                return RedirectToAction("Register", "Home");
            }
            var cart = unitOfWork.Carts.FirstOrDefault(x => x.UserId == User.Id);
            var products = new List<int>();
            if (!string.IsNullOrWhiteSpace(cart?.OrderBody))
            {
                products.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<int>>(cart.OrderBody));
            }
            else
            {
                cart = new Cart
                {
                    UserId = User.Id
                };
            }
            products = products.Where(x => x != id).ToList();
            cart.DateTime = DateTime.Now;
            cart.OrderBody = Newtonsoft.Json.JsonConvert.SerializeObject(products);
            unitOfWork.Carts.Merge(cart);
            return RedirectToAction("Checkout", "Home");
        }

        public void ChangeQuantity(ChangeQuantityVM changeQuantityVM)
        {
            if (User == null) return;
            var cart = unitOfWork.Carts.FirstOrDefault(x => x.UserId == User.Id);
            var products = new List<int>();
            if (!string.IsNullOrWhiteSpace(cart?.OrderBody))
            {
                products.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<int>>(cart.OrderBody));
            }
            else
            {
                cart = new Cart
                {
                    UserId = User.Id
                };
            }
            products = products.Where(x => x != changeQuantityVM.ProductId).ToList();
            for (int i = 0; i < changeQuantityVM.Quantity; i++)
            {
                products.Add(changeQuantityVM.ProductId);
            }
            cart.DateTime = DateTime.Now;
            cart.OrderBody = Newtonsoft.Json.JsonConvert.SerializeObject(products);
            unitOfWork.Carts.Merge(cart);
        }

        private IEnumerable<Product> GetBestSellers()
        {
            return (from tag in unitOfWork.Tags.Find(x => x.Name == "Top")
                    join taggedProduct in unitOfWork.TaggedProducts on tag.Id equals taggedProduct.TagId
                    join product in unitOfWork.Products on taggedProduct.ProductId equals product.Id
                    select product).Distinct();
        }
    }
}