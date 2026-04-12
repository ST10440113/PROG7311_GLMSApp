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
    public class ContractsController : Controller
    {
        private readonly PROG7311_GLMSAppContext _context;
        private readonly ContractService _contractService;

        public ContractsController(PROG7311_GLMSAppContext context, ContractService contractService)
        {
            _context = context;
            _contractService = contractService;
        }

        // GET: Contracts
        public async Task<IActionResult> Index()
        {
            var allContracts = await _contractService.GetAllContractsAsync();
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
        public async Task<IActionResult> Create([Bind("ContractId,StartDate,EndDate,Status,ServiceLevel,ClientId")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                await _contractService.CreateAsync(contract);
                return RedirectToAction(nameof(Index));
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

       
    }
}
