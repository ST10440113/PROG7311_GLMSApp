using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG7311_GLMSApp.Data;
using PROG7311_GLMSApp.Models;
using PROG7311_GLMSApp.Services;

namespace PROG7311_GLMSApp.Controllers
{
    public class ServiceRequestsController : Controller
    {
        
        private readonly ServiceRequestService _serviceRequestService;

        public ServiceRequestsController(ServiceRequestService serviceRequestService)
        {
           
            _serviceRequestService = serviceRequestService;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var allServiceRequests = await _serviceRequestService.GetAllServiceRequestsAsync();
            return View(allServiceRequests);
        }

        // GET: ServiceRequests/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _serviceRequestService.GetServiceRequestByIdAsync(id.Value);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.ContractId = await _serviceRequestService.GetContractsWithClients();
            return View();
        }

        // POST: ServiceRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceRequestId,Cost,Description,Status,ContractId")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceRequestService.Create(serviceRequest);
                   TempData["Success"] = "Service request status for contract " + serviceRequest.ContractId + " is now " + serviceRequest.Status;
                    return RedirectToAction(nameof(Index));
                    
                }
                catch (InvalidOperationException ex)
                {
                   TempData["Error"] = ex.Message;
                }
            }
            
           ViewBag.ContractId = await _serviceRequestService.GetContractsWithClients();
            
            return View(serviceRequest);
        }
      
        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _serviceRequestService.GetServiceRequestByIdAsync(id.Value);
            if (serviceRequest == null)
            {
                return NotFound();
            }
            ViewBag.ContractId = await _serviceRequestService.GetContractsByServiceRequestId(serviceRequest.ServiceRequestId);
            return View(serviceRequest);
        }

        // POST: ServiceRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceRequestId,Cost,Description,Status,ContractId")] ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.ServiceRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                 await  _serviceRequestService.UpdateAsync(serviceRequest);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var existingServiceRequest = _serviceRequestService.ServiceRequestExists(serviceRequest.ServiceRequestId);
                    if (!existingServiceRequest)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Success"] = "Service request status for contract " + serviceRequest.ContractId + " is now " + serviceRequest.Status;
                return RedirectToAction(nameof(Index));
            }
           ViewBag.ContractId = await _serviceRequestService.GetContractsByServiceRequestId(serviceRequest.ServiceRequestId);
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _serviceRequestService.GetServiceRequestByIdAsync(id.Value);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           await _serviceRequestService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}  
