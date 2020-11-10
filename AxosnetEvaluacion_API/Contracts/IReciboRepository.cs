using AxosnetEvaluacion_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxosnetEvaluacion_API.Contracts
{
    public interface IReciboRepository : IRepositoryBase<Recibo>
    {
        Task<IList<Recibo>> FilterByProveedor(int idProveedor);
        Task<IList<Recibo>> FilterByFecha(DateTime fecha);
        Task<IList<Recibo>> FilterByRangoFecha(DateTime fechaInicio, DateTime fechaFin);
    }
}
