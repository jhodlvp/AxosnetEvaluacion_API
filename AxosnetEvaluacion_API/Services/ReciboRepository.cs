using AxosnetEvaluacion_API.Contracts;
using AxosnetEvaluacion_API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxosnetEvaluacion_API.Services
{
    public class ReciboRepository : IReciboRepository
    {
        private readonly ApplicationDbContext _db;

        public ReciboRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Recibo entity)
        {
            await _db.Recibos.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Recibo entity)
        {
            _db.Recibos.Remove(entity);
            return await Save();
        }

        public async Task<IList<Recibo>> FilterByFecha(DateTime fecha)
        {
            DateTime inicioDia = new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0);
            DateTime finDia = new DateTime(fecha.Year, fecha.Month, fecha.Day, 23, 59, 59);
            var recibos = await _db.Recibos.Where(r => (r.Fecha >= inicioDia) && (r.Fecha <= finDia))
                .ToListAsync();
            return recibos;
        }

        public async Task<IList<Recibo>> FilterByProveedor(int idProveedor)
        {
            var recibos = await _db.Recibos.Where(r => r.IdProveedor == idProveedor)
                .ToListAsync();
            return recibos;
        }

        public async Task<IList<Recibo>> FilterByRangoFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            DateTime inicio = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day, 0, 0, 0);
            DateTime fin = new DateTime(fechaFin.Year, fechaFin.Month, fechaFin.Day, 23, 59, 59);
            var recibos = await _db.Recibos.Where(r => (r.Fecha >= inicio) && (r.Fecha <= fin))
                .ToListAsync();
            return recibos;
        }

        public async Task<IList<Recibo>> FindAll()
        {
            var recibos = await _db.Recibos.ToListAsync();
            return recibos;
        }

        public async Task<Recibo> FindById(int id)
        {
            var recibo = await _db.Recibos.FindAsync(id);
            return recibo;
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Recibos.AnyAsync(q => q.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Recibo entity)
        {
            _db.Recibos.Update(entity);
            return await Save();
        }
    }
}
