using HrApp.Repositories.Interfaces;
using HrApp.Data;
using HrApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            // Haal alle werknemers op uit de database
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetById(int? id)
        {
            if (id == null) return null;

            // Zoek een specifieke werknemer op basis van ID
            return await _context.Employees.FindAsync(id);
        }

        public async Task Add(Employee employee)
        {
            // Voeg een nieuwe werknemer toe en sla de wijzigingen op
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Employee employee)
        {
            // Werk de gegevens van de werknemer bij
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Employee employee)
        {
            // Verwijder de werknemer uit de database
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}