﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;
using Restaurantopia.Entities.Models;
using Restaurantopia.InterFaces;

namespace Restaurantopia.Controllers
{
	public class OrderDetailsController : Controller
    {
        // GET: OrderDetailsController
        private IGenericRepository<OrderDetails> _orderrepository;
        private IGenericRepository<Item> _Rep_Item;


        public OrderDetailsController(IGenericRepository<OrderDetails> orderrepository, IGenericRepository<Item> rep_Item)
        {
            _orderrepository = orderrepository;
            _Rep_Item = rep_Item;
        }

        public async Task<ActionResult> Index()
        {
            var orderDetailsList = await _orderrepository.GetAllAsync();
            ViewBag.Orders = await _Rep_Item.GetAllAsync();  // Properly await the second async call
            return View(orderDetailsList);
        }

        public async Task<ActionResult> Details(int id)
        {
            var orderDetailsList = await _orderrepository.GetByIdAsync(id);
            ViewBag.Orders = await _Rep_Item.GetAllAsync();  // Properly await the second async call
            return View(orderDetailsList);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var orderDetails = await _orderrepository.GetByIdAsync(id);
            ViewBag.Orders =await _Rep_Item.GetAllAsync();
            if (orderDetails == null)
            {
                return NotFound();
            }
            return View(orderDetails);
        }

        // POST: OrderDetailsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, OrderDetails updatedOrderDetails)
        {
            try
            {
                // Fetch the existing order details
                var existingOrderDetails = await _orderrepository.GetByIdAsync(id);
                if (existingOrderDetails == null)
                {
                    return NotFound();
                }

                // Manual validation for Quantity
                if (updatedOrderDetails.Quantity <= 0)
                {
                    ModelState.AddModelError("Quantity", "Quantity must be greater than zero.");
                    return View(updatedOrderDetails); // Return view with the error message
                }

                // Update only the quantity
                existingOrderDetails.Quantity = updatedOrderDetails.Quantity;
                // Save the changesx    
                await _orderrepository.UpdateAsync(existingOrderDetails);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                ModelState.AddModelError("", "An error occurred while updating the order details. Please try again.");
                return View(updatedOrderDetails); // Return view with the error message
            }
        }



        // GET: OrderDetailsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            ViewBag.Orders = await _Rep_Item.GetAllAsync();

            OrderDetails D_item = await _orderrepository.GetByIdAsync(id);
            if (D_item == null)
            {
                return NotFound();
            }
            return View(D_item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, OrderDetails order)
        {
            try
            {
                if (id != order.Id)
                {
                    return BadRequest();
                }

                await _orderrepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(order);
            }
        }
      
    }
}