using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PROG7311_GLMSApp.Data;
using PROG7311_GLMSApp.Models;
using PROG7311_GLMSApp.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace PROG7311_GLMSApp.Controllers
{
    public class ContractsController : Controller
    {
        private readonly PROG7311_GLMSAppContext _context;
        private readonly ContractService _contractService;
        private readonly IWebHostEnvironment _environment;

        public ContractsController(PROG7311_GLMSAppContext context, ContractService contractService,IWebHostEnvironment environment)
        {
            _context = context;
            _contractService = contractService;
            _environment = environment;
        }

        // GET: Contracts
        public async Task<IActionResult> Index(DateOnly? startDate, DateOnly? endDate, string status)
        {
            var allContracts = await _contractService.GetAllContractsAsync();


            if (startDate != null || endDate != null)
            {
                if (startDate != null & endDate != null)
                {
                    var contracts = _contractService.FilterByDateRange(startDate, endDate);
                    return View(contracts);
                }
                else
                {
                    TempData["Error"] = "Both start date and end date must be provided for filtering";
                }
            }

            if (!string.IsNullOrEmpty(status))
            {
                var contracts = _contractService.FilterByStatus(status);
                return View(contracts);
            }
            return View(allContracts);

        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _contractService.GetContractByIdAsync(id.Value);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.ClientId = await _contractService.ClientNames();

            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractId,StartDate,EndDate,Status,ServiceLevel,ClientId,FilePath")] Contract contract, IFormFile? file)
        {
            if (file != null)
            {
                var allowedExtension = ".pdf";
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (allowedExtension != extension)
                {
                    TempData["Error"] = "Invalid file type. Only PDF files are allowed.";
                    return View(contract);
                }
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fileUploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                contract.FilePath = file.FileName;
                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }


            if (ModelState.IsValid)
            {
                try
                {
                    await _contractService.CreateAsync(contract);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }

            ViewBag.ClientId = await _contractService.ClientNames();
            return View(contract);
        }


        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.ClientId = await _contractService.ClientNames();
            var contract = await _contractService.GetContractByIdAsync(id.Value);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);

        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContractId,StartDate,EndDate,Status,ServiceLevel,ClientId")] Contract contract)
        {
            if (id != contract.ContractId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _contractService.UpdateAsync(contract);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_contractService.ContractExists(contract.ContractId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ClientId = await _contractService.ClientNames();
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.ClientId = await _contractService.ClientNames();
            var contract = await _contractService.GetContractByIdAsync(id.Value);
            if (contract == null)
            {
                return NotFound();
            }
            ViewBag.ClientId = await _contractService.ClientNames();
            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _contractService.GetContractByIdAsync(id);

            if (contract != null)
            {
                await _contractService.Delete(contract.ContractId);
            }

            return RedirectToAction(nameof(Index));
        }
         
        [HttpGet]
        public IActionResult DownloadFile(string fileName)
        {
            string filePath = Path.Combine(_environment.WebRootPath, "fileUploads", fileName);
           
            if (!System.IO.File.Exists(filePath))
                return NotFound();
          var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/pdf", fileName);
        }
    }
}

