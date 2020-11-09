using AxosnetEvaluacion_API.Contracts;
using AxosnetEvaluacion_API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxosnetEvaluacion_API.Services
{
    public class MonedaRepository : IMonedaRepository
    {
        private readonly ApplicationDbContext _db;

        public MonedaRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Moneda entity)
        {
            await _db.Monedas.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Moneda entity)
        {
            _db.Monedas.Remove(entity);
            return await Save();
        }

        public async Task<IList<Moneda>> FindAll()
        {
            var monedas = await _db.Monedas.ToListAsync();
            return monedas;
        }

        public async Task<Moneda> FindById(int id)
        {
            var moneda = await _db.Monedas.FindAsync(id);
            return moneda;
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Monedas.AnyAsync(q => q.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Moneda entity)
        {
            _db.Monedas.Update(entity);
            return await Save();
        }
    }
}
