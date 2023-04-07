using NUnit.Framework;
using QuickMartDataAccessLayer;
using QuickMartDataAccessLayer.Models;
using Models = QuickMartDataAccessLayer.Models;
using System;

namespace QuickMart.UnitTest
{
    public class Tests
    {

        public QuickMartDBContext context=new QuickMartDBContext();
        public QuickMartRepository _repository;

        //public Tests(QuickMartDBContext context) { 
        //    _context = context;
        //    _repository = new QuickMartRepository(_context);
        //}


        [Test]
        public void ValidateLogin()
        {
            //QuickMartDBContext context = new QuickMartDBContext();
            _repository = new QuickMartRepository(context);
            var actual=_repository.ValidateCredentials("franken@gmail.com", "BSBEV@1234");
            var expected = 2;
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void ValidateRegistrationForm()
        {
            _repository = new QuickMartRepository(context);
            var actual = _repository.RegisterUser("test@gmail.com", "test@123", "M", DateTime.Now, "Berlin,Germany");
            var expected = false;
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void ValidateAddCart() {
            Models.PurchaseDetails purchaseDetails = new PurchaseDetails();
            purchaseDetails.ProductId = "P124";
            purchaseDetails.QuantityPurchased = 1;
            purchaseDetails.EmailId = "franken@gmail.com";
            purchaseDetails.DateOfPurchase = DateTime.Now;
            _repository = new QuickMartRepository(context);
            var actual = _repository.AddToCart(purchaseDetails);
            var expected = 2056.0;
            Assert.AreEqual(expected, actual);


        }

        [Test]
        public void ValidateRemoveItemFromCart()
        {
            _repository = new QuickMartRepository(context);
            var actual = _repository.RemoveFromPurchaseDetails("Abstract Hand painted Oil Painting on Canvas", "franken@gmail.com", "1");
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateAddProduct()
        {
            Models.Products product = new Products();
            product.ProductId = "P199";
            product.ProductName= "Test";
            product.Price = 100;
            product.QuantityAvailable = 10;
            product.CategoryId = 4;
            _repository = new QuickMartRepository(context);
            var actual = _repository.AddProduct(product);
            var expected = true;
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void ValidateUpdateProduct()
        {
            Models.Products product = new Products();
            product.ProductName = "TestUpdateProduct";
            product.ProductId = "P199";
            product.Price = 200;
            product.QuantityAvailable = 20;
            product.CategoryId = 5;
            _repository = new QuickMartRepository(context);
            var actual = _repository.UpdateProduct(product);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateDeleteProduct()
        {
            _repository = new QuickMartRepository(context);
            var actual = _repository.DeleteProduct("P199");
            var expected = true;
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ValidateAddCategory()
        {
            Models.Categories category = new Categories();
            category.CategoryName = "Test";
            //category.CategoryId = 9;
            _repository = new QuickMartRepository(context);
            var actual=_repository.AddCategory(category);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUpdateCategory()
        {
            Models.Categories category = new Categories();
            category.CategoryName = "TestUpdateCategory";
            category.CategoryId = 8;
            _repository = new QuickMartRepository(context);
            var actual = _repository.UpdateCategory(category);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateDeleteCategory()
        {
            _repository = new QuickMartRepository(context);
            var actual = _repository.DeleteCategory(8);
            var expected = true;
            Assert.AreEqual(expected, actual);
        }
    }
}