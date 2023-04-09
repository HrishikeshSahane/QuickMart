using QuickMartDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace QuickMartDataAccessLayer
{
    public class QuickMartRepository
    {
        private readonly QuickMartDBContext _context;


        public QuickMartRepository(QuickMartDBContext context)
        {
            _context = context;
        }

        public List<Models.Categories> GetCategories()
        {
            try
            {
                return _context.Categories.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool AddCategory(Categories catObj)
        {
            try
            {
                _context.Categories.Add(catObj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateCategory(Categories catObj)
        {
            try
            {
                var catObjFromDB = _context.Categories.Where(x => x.CategoryId == catObj.CategoryId).Select(x => x).FirstOrDefault();
                catObjFromDB.CategoryName = catObj.CategoryName;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //public bool DeleteCategory(Categories catObj)
        //{
        //    try
        //    {
        //        var categoryToBeDeleted = _context.Categories.Where(x => x.CategoryId == catObj.CategoryId).FirstOrDefault();
        //        _context.Categories.Remove(categoryToBeDeleted);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public bool DeleteCategory(int CategoryId)
        {
            try
            {
                
                var products = _context.Products.Where(x => x.CategoryId == CategoryId).ToList();
                if(products.Count > 0)
                {
                    foreach (var product in products)
                    {
                        var purchaseDetails = _context.PurchaseDetails.Where(x => x.ProductId == product.ProductId).ToList();
                        if(purchaseDetails.Count > 0) {
                        _context.PurchaseDetails.RemoveRange(purchaseDetails);
                        //_context.SaveChanges();
                        }
                    }
                    _context.Products.RemoveRange(products);
                    //_context.SaveChanges();
                }

                var categoryToBeDeleted = _context.Categories.Where(x => x.CategoryId == CategoryId).FirstOrDefault();
                _context.Categories.Remove(categoryToBeDeleted);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public byte? GetRoleIdByUserId(string userId)
        {
            try
            {
                var roleId = _context.Users.Where(x => x.EmailId == userId).Select(x => x.RoleId).FirstOrDefault();
                //var roleName = context.Roles.Where(x => x.RoleId == roleId).Select(x => x.RoleName).FirstOrDefault();
                return roleId;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string GetRoleName(int roleId)
        {
            try
            {
                var roleName = _context.Roles.Where(x => x.RoleId == roleId).Select(x => x.RoleName).FirstOrDefault();
                return roleName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Users GetUserDetails(string emailId)
        {
            Users usermodel = new Users();
            try
            {

                usermodel = (from ctx in _context.Users where ctx.EmailId.ToLower() == emailId.ToLower() select ctx).FirstOrDefault();
                return usermodel;
            }
            catch(Exception ex)
            {
                return usermodel;
            }

        }

        public Users UpdateProfile(Users usermodel,out bool status)
        {
            status = false;
            try
            {
                Users dbUser = _context.Users.Where(x => x.EmailId == usermodel.EmailId).FirstOrDefault();
                dbUser.Address = usermodel.Address;
                if (!String.IsNullOrEmpty(usermodel.UserPassword))
                {
                    if (usermodel.UserPassword != dbUser.UserPassword)
                    {
                        dbUser.UserPassword = usermodel.UserPassword;
                    }

                }
                if (dbUser.DateOfBirth != usermodel.DateOfBirth)
                {
                    dbUser.DateOfBirth = usermodel.DateOfBirth;
                }

                _context.Users.Update(dbUser);
                _context.SaveChanges();
                status = true;
                return dbUser;
            }
            catch(Exception ex)
            { return usermodel; }


        }
        public List<Models.Products> GetProducts()
        {
            try
            {
                return _context.Products.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string GetNextProductId()
        {
            string productId = _context.Products.OrderByDescending(x => x.ProductId).Select(x => x.ProductId).FirstOrDefault().ToString();
            int id = Convert.ToInt32(productId.Substring(1, productId.Length - 1)) + 1;
            return "P" + id.ToString();
        }

        public bool AddProduct(Products product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateProduct(Models.Products product)
        {
            try
            {
                Products dbProduct = _context.Products.Find(product.ProductId);
                dbProduct.ProductName = product.ProductName;
                dbProduct.QuantityAvailable = product.QuantityAvailable;
                dbProduct.Price = product.Price;
                dbProduct.CategoryId = product.CategoryId;

                _context.Products.Update(dbProduct);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteProduct(string productId)
        {
            try
            {
                Models.Products prodObj = _context.Products.Find(productId);
                var purchaseDetails = _context.PurchaseDetails.Where(x => x.ProductId == prodObj.ProductId).ToList();
                if (purchaseDetails.Count > 0)
                {
                    _context.PurchaseDetails.RemoveRange(purchaseDetails);
                }
                _context.Products.Remove(prodObj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool PurchaseProduct(Models.PurchaseDetails purchaseDetails)
        {
            try
            {
                purchaseDetails.DateOfPurchase = purchaseDetails.DateOfPurchase.ToUniversalTime();
                _context.PurchaseDetails.Add(purchaseDetails);

                Products products = (from c in _context.Products where c.ProductId == purchaseDetails.ProductId select c).FirstOrDefault();
                products.QuantityAvailable -= purchaseDetails.QuantityPurchased;
                _context.Products.Update(products);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<string> FetchTopProducts()
        {
            List<string> productList = new List<string>();
            try
            {
                var list = (from c in _context.PurchaseDetails
                            join p in _context.Products on
                            c.ProductId equals p.ProductId
                            select p.ProductName).Distinct().ToList<string>();
                if (list.Count > 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        productList.Add(list[i]);
                    }
                }
                else
                {
                    productList = list;
                }

            }
            catch
            {

                productList = null;
            }
            return productList;
        }

        public List<string> FetchTopProducts(string emailId)
        {
            List<string> productList = new List<string>();
            try
            {
                var list = (from c in _context.PurchaseDetails
                            join p in _context.Products on
                            c.ProductId equals p.ProductId
                            where c.EmailId == emailId
                            select p.ProductName).Distinct().ToList<string>();
                if (list.Count > 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        productList.Add(list[i]);
                    }
                }
                else
                {
                    productList = list;
                }

            }
            catch
            {

                productList = null;
            }
            return productList;
        }

        //EXERCISE
        public List<string> RecentPurchases()
        {
            List<string> purchaseList = new List<string>();
            try
            {


                var list = (from c in _context.PurchaseDetails
                            join p in _context.Products on
                            c.ProductId equals p.ProductId
                            orderby c.DateOfPurchase descending
                            select p.ProductName).Distinct().ToList<string>();

                purchaseList.Add(list[0]);
                purchaseList.Add(list[1]);
                purchaseList.Add(list[2]);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                purchaseList = null;

            }
            return purchaseList;
        }

        public List<MostPurchases> MostPurchases()
        {
            List<MostPurchases> mostPurchases = new List<MostPurchases>();
            //var results = from ctx in _context.PurchaseDetails
            //              group ctx by ctx.ProductId into x
            //              join y in _context.Products on x.FirstOrDefault().ProductId equals y.ProductId
            //              select new MostPurchases
            //              {

            //                  ProductId = x.Key,
            //                  ProductName=(from ctx in _context.Products where ctx.ProductId==x.Key select ctx.ProductName).FirstOrDefault(),
            //                  TotalQuantity = x.Sum(x1 => x1.QuantityPurchased).ToString(),
            //              };
            try
            {
                mostPurchases = _context.PurchaseDetails.GroupBy(x => x.ProductId).Select(pd => new MostPurchases
                {
                    ProductId = pd.Key,
                    ProductName = (from ctx in _context.Products where ctx.ProductId == pd.Key select ctx.ProductName).FirstOrDefault(),
                    TotalQuantity =pd.Sum(x => x.QuantityPurchased),
                }).Take(5).OrderByDescending(x => x.TotalQuantity).ToList();
            }
            catch(Exception ex)
            {
                return mostPurchases;
            }


            return mostPurchases;
        }

        //public List<string> RecentPurchases()
        //{
        //    //List<PurchaseDetails> purchaseList = new List<PurchaseDetails>();
        //    List<string> purchaseList = new List<string>();
        //    try
        //    {


        //        var list = (from c in _context.PurchaseDetails
        //                    join p in _context.Products on
        //                    c.ProductId equals p.ProductId join u in _context.Users on c.EmailId equals u.EmailId
        //                    orderby c.PurchaseId descending
        //                    select p.ProductName).ToList();

        //        purchaseList.Add(list[0]);
        //        purchaseList.Add(list[1]);
        //        purchaseList.Add(list[2]);
        //    }

        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        purchaseList = null;

        //    }
        //    return purchaseList;
        //}


        public bool RegisterUser(string EmailId, string UserPassword, string Gender, DateTime DateOfBirth, string Address)
        {
            try
            {
                Users user = (from c in _context.Users where c.EmailId.ToLower() == EmailId.ToLower() select c).FirstOrDefault();
                if(user == null)
                {
                    Users userObj = new Users();
                    userObj.EmailId = EmailId;
                    userObj.UserPassword = UserPassword;
                    userObj.Gender = Gender;
                    userObj.DateOfBirth = DateOfBirth;
                    userObj.Address = Address;
                    userObj.RoleId = 2;

                    _context.Users.Add(userObj);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool AddFeedback(Feedback feedObj)
        {
            try
            {
                _context.Feedbacks.Add(feedObj);
                _context.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }

        }

        public byte? ValidateCredentials(string userId, string password)
        {
           
            try
            {
                Users users = _context.Users.Find(userId);
                byte? roleId = null;
                if (users.UserPassword == password)
                {
                    roleId = users.RoleId;
                }
                else
                {
                    roleId = 0;
                }

                return roleId;
            }
            catch(Exception ex)
            {
                return 0;
            }

        }

        public decimal AddToCart(PurchaseDetails purchaseDetails)
        {
            try
            {
                Products productDetails = new Products();
                Cart cartDetails = new Cart();
                List<Items> ItemsList= new List<Items>();
                Items item = new Items();
                string jsonString = String.Empty;
                string deserialization = String.Empty;
                productDetails = (from ctx in _context.Products where purchaseDetails.ProductId == ctx.ProductId select ctx).FirstOrDefault();
                Cart cartObj = (from ctx in _context.Cart where ctx.EmailId == purchaseDetails.EmailId select ctx).FirstOrDefault();

                if (cartObj == null)
                {
                    
                    item.ProductName = productDetails.ProductName;
                    item.QuantityPurchased = purchaseDetails.QuantityPurchased;
                    item.TotalPrice = purchaseDetails.QuantityPurchased * productDetails.Price;
                    ItemsList.Add(item);
                    jsonString = JsonConvert.SerializeObject(ItemsList);
                    
                    cartDetails.EmailId = purchaseDetails.EmailId;
                    cartDetails.DateOfPurchase = purchaseDetails.DateOfPurchase;
                    cartDetails.Items = jsonString;
                    _context.Cart.Add(cartDetails);
                    _context.SaveChanges();
                    return item.TotalPrice;
                    
                }
                else
                {
                    List<Items> existingItemsList = JsonConvert.DeserializeObject<List<Items>>(cartObj.Items);                   
                        item.ProductName = productDetails.ProductName;
                        item.QuantityPurchased = purchaseDetails.QuantityPurchased;
                        item.TotalPrice = purchaseDetails.QuantityPurchased * productDetails.Price;
                        existingItemsList.Add(item);      
                     cartObj.Items = JsonConvert.SerializeObject(existingItemsList);
                    _context.Cart.Update(cartObj);
                    _context.SaveChanges();
                    return item.TotalPrice;

                }
                
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                return 0;
            }
            //return true;


        }

        public List<Items> GetCartDetails(string username,out string jsonstring)
        {
            jsonstring = String.Empty;
            //emailId = String.Empty;
            Cart cartDetails = new Cart();
            List<Items> itemList = new List<Items>();
            try
            {
                cartDetails = (from ctx in _context.Cart where ctx.EmailId.Contains(username) select ctx).FirstOrDefault();
                if (cartDetails != null)
                {
                    jsonstring = cartDetails.Items;
                    itemList = JsonConvert.DeserializeObject<List<Items>>(cartDetails.Items);
                }
            }
            catch(Exception ex)
            {
                return itemList;
            }


            return itemList;
        }

        public List<Items> RemoveItemFromCart(string productName,string userId,out string jsonstring)
        {
            jsonstring = String.Empty;
            Cart cartDetails = new Cart();
            List<Items> itemList = new List<Items>();
            cartDetails = (from ctx in _context.Cart where ctx.EmailId.Contains(userId) select ctx).FirstOrDefault();
            if (cartDetails != null)
            {
                itemList = JsonConvert.DeserializeObject<List<Items>>(cartDetails.Items);
                Items item = itemList.Where(x => x.ProductName == productName).FirstOrDefault();
                itemList.Remove(item);
                cartDetails.Items =jsonstring= JsonConvert.SerializeObject(itemList);
                _context.Cart.Update(cartDetails);
                _context.SaveChanges();
                return itemList;
            }
            return itemList;

        }


        public bool RemoveFromPurchaseDetails(string productName, string userId, string qtypurchased)
        {
            bool isRemoved = false;
            try
            {

                Products product = (from p in _context.Products where p.ProductName == productName select p).FirstOrDefault();
                product.QuantityAvailable += Convert.ToInt32(qtypurchased);
                PurchaseDetails purchaseDetails = (from ctx in _context.PurchaseDetails where ctx.EmailId == userId && ctx.QuantityPurchased == Convert.ToInt32(qtypurchased) && ctx.ProductId == product.ProductId select ctx).OrderByDescending(x=>x.DateOfPurchase).FirstOrDefault();
                _context.PurchaseDetails.Remove(purchaseDetails);
                _context.SaveChanges();
                _context.Products.Update(product);
                isRemoved = true ;
            }
            catch(Exception ex)
            {
                isRemoved = false;
            }
            return isRemoved;
        } 

        public bool SaveOrder(string jsontringitemlist, decimal totalprice, string emailId)
        {
            
            Order order = new Order();
            order.FinalItems = jsontringitemlist;
            order.EmailId = emailId;
            order.TotalCost = Convert.ToInt32(totalprice);
            order.IsPaymentReceived = false;
            order.OrderDate = DateTime.Now.ToUniversalTime();
            _context.Orders.Add(order);

            Cart cart = (from ctx in _context.Cart where ctx.Items == jsontringitemlist && ctx.EmailId == emailId select ctx).FirstOrDefault();
            _context.Cart.Remove(cart);

            _context.SaveChanges();
            return true;
        }

        public List<Order> FetchAllOrders()
        {
            List<Order> orderList = new List<Order>();
            try
            {
                orderList = (from ctx in _context.Orders select ctx).ToList();
                return orderList;
            }
            catch(Exception ex)
            {
                return orderList;
            }
        }

        public List<Items> DeserializaOrdersString(string jsonstring)
        {
            List<Items> itemList = new List<Items>();
            try
            {
                itemList = JsonConvert.DeserializeObject<List<Items>>(jsonstring);
                return itemList;
            }
            catch
            {
                return itemList;
            }

        }

        public List<Order> UpdateSelectedOrder(string orderId)
        {
            List<Order> orderList = new List<Order>();
            try
            {
                Order order = (from ctx in _context.Orders where ctx.OrderId == Convert.ToInt32(orderId) select ctx).FirstOrDefault();
                order.IsPaymentReceived = true;
                _context.Orders.Update(order);
                _context.SaveChanges();
                orderList = (from ctx in _context.Orders select ctx).ToList();
                return orderList;
            }
            catch(Exception ex)
            {
                return orderList;
            }

        }

        public List<Products> GetDepletedProducts()
        {
            List<Products> products = new List<Products>();
            products = (from ctx in _context.Products where ctx.QuantityAvailable < 10 select ctx).ToList();
            return products;
        }
    }
}
