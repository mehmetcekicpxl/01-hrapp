using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HrApp.Models;
using HrApp.Repositories.Interfaces;

namespace HrApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        // Controller praat alleen met de repository, niet met de database context
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetAll();
            return employees != null ?
                        View(employees) :
                        Problem("De entiteit set voor werknemers is leeg.");
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Haal de specifieke werknemer op via de repository
            var employee = await _employeeRepository.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Voeg de nieuwe werknemer toe
                await _employeeRepository.Add(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Werk de werknemer bij via de repository
                    await _employeeRepository.Update(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Gebruik de await keyword omdat de methode nu asynchroon is
                    if (!await EmployeeExists(employee.EmployeeId))
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
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Zoek eerst de werknemer op voordat we deze verwijderen
            var employee = await _employeeRepository.GetById(id);
            if (employee != null)
            {
                // Verwijder via de repository in plaats van de context
                await _employeeRepository.Delete(employee);
            }

            return RedirectToAction(nameof(Index));
        }

        // Hulpmethode om te controleren of de werknemer bestaat in de database
        private async Task<bool> EmployeeExists(int id)
        {
            var employee = await _employeeRepository.GetById(id);
            return employee != null;
        }
    }
}