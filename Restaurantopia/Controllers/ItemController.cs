using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurantopia.Entities.Models;
using Restaurantopia.InterFaces;
using Restaurantopia.Repositories;

namespace Restaurantopia.Controllers
{
    // ana loay elsayed
    public class ItemController : Controller
    {
        private IGenericRepository<Item> _Rep_Item;
        private IGenericRepository<Category> _Rep_Category;
        private IGenericRepository<OrderDetails> _Rep_Order;
        private IWebHostEnvironment _environment;
        private IUploadFile _uploadFile;

        public ItemController(IGenericRepository<Item> repository, IGenericRepository<Category> Rep, IWebHostEnvironment environment, IUploadFile uploadFile, IGenericRepository<OrderDetails> rep_Order)
        {
            _Rep_Item = repository;
            _Rep_Category = Rep;
            _environment = environment;
            _uploadFile = uploadFile;
            _Rep_Order = rep_Order;
        }

        public async Task<ActionResult> Menu(int? categoryId, string searchQuery)
        {
            // Get all items initially
            IEnumerable<Item> Items = await _Rep_Item.GetAllAsync(includes: new[] { "Category" });

            // Save selected category ID in ViewBag
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedCategoryName = null; // Initialize to null

            // Apply category filter if a categoryId is provided (i.e., not null or empty)
            if (categoryId.HasValue && categoryId.Value != 0) // Adjust this line to check against a specific value for "All Categories"
            {
                Items = Items.Where(item => item.CategoryId == categoryId.Value);

                // Get the selected category name if filtering by a specific category
                var selectedCategory = await _Rep_Category.GetByIdAsync(categoryId.Value);
                ViewBag.SelectedCategoryName = selectedCategory?.CategoryName; // Store the selected category name
            }
            else
            {
                // When "All Categories" is selected or no category is selected, we don't filter items
                ViewBag.SelectedCategoryName = "All Categories"; // Optional: you can set a default value
            }

            // Apply search filter if a search query is provided
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                Items = Items.Where(item => item.ItemTitle.ToLower().Contains(searchQuery.ToLower()));
                ViewBag.SearchQuery = searchQuery;
            }
            else
            {
                ViewBag.SearchQuery = null;
            }

            ViewBag.C_s = await _Rep_Category.GetAllAsync();
            return View(Items);
        }








        public async Task<ActionResult> Details(int id)
        {
            Item item = await _Rep_Item.GetByIdAsync(id);
            ViewBag.C_s = await _Rep_Category.GetAllAsync();

            return View(item);
        }


        public async Task<ActionResult> Create()
        {

            var categories = await _Rep_Category.GetAllAsync();
            var item = new Item() { categoryList = categories.ToList() };
            return View(item);
 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Item item)
        {
            try
            {

                if (item.ImageFile != null)
                {
                    string FilePath = await _uploadFile.UploadFileAsync("\\Images\\ItemImage\\",item.ImageFile);
                    item.ItemImage = FilePath;
                }
                    await _Rep_Item.AddAsync(item);
                    return RedirectToAction(nameof(Menu));
                
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(item);

            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.C_s = await _Rep_Category.GetAllAsync();
            if (id == null)
            {
                return NotFound();
            }
            Item item = await _Rep_Item.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Item item)
        {
            try
            {
                await _Rep_Item.UpdateAsync(item);

                return RedirectToAction(nameof(Menu));
 
 
            }
            catch
            {
                return View(item);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            ViewBag.C_s = await _Rep_Category.GetAllAsync();
            Item D_item = await _Rep_Item.GetByIdAsync(id);
            if (D_item == null)
            {
                return NotFound();
            }
            return View(D_item);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Item item)
        {
            try
            {
                if (id != item.Id)
                {
                    return BadRequest();
                }

                await _Rep_Item.DeleteAsync(id);
  
                return RedirectToAction(nameof(Menu));
 

            }
            catch
            {
                return View(item);
            }
        }
        public async Task<ActionResult> Order(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Item item = await _Rep_Item.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Order(Item item)
        {
            if (item.Quantity <= 0) // Validate Quantity
            {
                ModelState.AddModelError("Quantity", "Quantity must be greater than zero.");
                return View(item);
            }

            try
            {
                // Login 
                int customerId = 1;

                // Create a new OrderDetails object
                OrderDetails orderDetails = new OrderDetails
                {
                    ItemId = item.Id, // Use the selected item's ID
                    CustomerId = customerId,
                    Quantity = item.Quantity,
                    Total = (int)item.ItemPrice * item.Quantity, // Calculate total based on quantity
                    Date = DateTime.Now
                };

                // Add the new orderDetails object to the database
                await _Rep_Order.AddAsync(orderDetails);

                // Redirect to the OrderDetails index after adding the item
                return RedirectToAction("Index", "OrderDetails");
            }
            catch
            {
                return View(item);
            }
        }


    }
}
