using AxosnetEvaluacion_API.Contracts;
using AxosnetEvaluacion_API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxosnetEvaluacion_API.Services
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly ApplicationDbContext _db;

        public ProveedorRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Proveedor entity)
        {
            await _db.Proveedors.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Proveedor entity)
        {
            _db.Proveedors.Remove(entity);
            return await Save();
        }

        public async Task<IList<Proveedor>> FindAll()
        {
            var proveedors = await _db.Proveedors.ToListAsync();
            return proveedors;
        }

        public async Task<Proveedor> FindById(int id)
        {
            var proveedor = await _db.Proveedors.FindAsync(id);
            return proveedor;
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Proveedors.AnyAsync(q => q.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Proveedor entity)
        {
            _db.Proveedors.Update(entity);
            return await Save();
        }
    }
}
