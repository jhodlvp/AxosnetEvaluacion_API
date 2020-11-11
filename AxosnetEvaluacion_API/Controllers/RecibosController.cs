using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AxosnetEvaluacion_API.Contracts;
using AxosnetEvaluacion_API.Data;
using AxosnetEvaluacion_API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AxosnetEvaluacion_API.Controllers
{
    /// <summary>
    /// Endpoints para interactuar con la tabla Recibos en la base de datos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class RecibosController : ControllerBase
    {
        private readonly IReciboRepository _reciboRepository;
        private readonly IMapper _mapper;

        public RecibosController(IReciboRepository reciboRepository,
            IMapper mapper)
        {
            _reciboRepository = reciboRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los recibos
        /// </summary>
        /// <returns>List: Recibos</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecibos()
        {
            try
            {
                var recibos = await _reciboRepository.FindAll();
                var response = _mapper.Map<IList<ReciboGetDTO>>(recibos);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Obtiene un Recibo usando su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecibo(int id)
        {
            try
            {
                var recibo = await _reciboRepository.FindById(id);

                if (recibo == null)
                {
                    return NotFound();
                }
                var response = _mapper.Map<ReciboGetDTO>(recibo);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Obtiene Recibos usando IdProveedor
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List: Recibos</returns>
        [HttpGet]
        [Route("proveedor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecibosByProveedor([FromQuery]int idProveedor)
        {
            try
            {
                var recibos = await _reciboRepository.FilterByProveedor(idProveedor);
                var response = _mapper.Map<IList<ReciboGetDTO>>(recibos);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Obtiene Recibos de una fecha exacta
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns>List: Recibos</returns>
        [HttpGet]
        [Route("fecha")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecibosByFecha([FromQuery] string fecha)
        {
            try
            {
                DateTime? fechaDt = stringToDate(fecha);
                if (fechaDt == null)
                {
                    return BadRequest();
                }
                var recibos = await _reciboRepository.FilterByFecha((DateTime)fechaDt);
                var response = _mapper.Map<IList<ReciboGetDTO>>(recibos);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Obtiene Recibos de un rango de fechas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns>List: Recibos</returns>
        [HttpGet]
        [Route("rangofechas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecibosByFecha([FromQuery] string fechaInicio, [FromQuery] string fechaFin)
        {
            try
            {
                DateTime? fechaInicioDt = stringToDate(fechaInicio);
                DateTime? fechaFinDt = stringToDate(fechaFin);
                if (fechaInicioDt == null || fechaFinDt == null)
                {
                    return BadRequest();
                }
                var recibos = await _reciboRepository.FilterByRangoFecha((DateTime)fechaInicioDt, (DateTime)fechaFinDt);
                var response = _mapper.Map<IList<ReciboGetDTO>>(recibos);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Crea un Recibo
        /// </summary>
        /// <param name="reciboDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ReciboPostDTO reciboDto)
        {
            try
            {
                if (reciboDto == null)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var recibo = _mapper.Map<Recibo>(reciboDto);
                var isSuccess = await _reciboRepository.Create(recibo);
                if (!isSuccess)
                {
                    return InternalError("Error al crear la Moneda");
                }
                return Created("Create", new { recibo });
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Actualiza un registro de Recibo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reciboDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] ReciboUpdateDTO reciboDto)
        {
            try
            {
                if (id < 1 || reciboDto == null || id != reciboDto.Id)
                {
                    return BadRequest();
                }

                var isExists = await _reciboRepository.isExists(id);
                if (!isExists)
                {
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var recibo = await _reciboRepository.FindById(id);
                recibo.Comentarios = reciboDto.Comentarios;
                var isSuccess = await _reciboRepository.Update(recibo);
                if (!isSuccess)
                {
                    return InternalError("Error al actualizar el registro");
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Borra un registro de Recibo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest();
                }
                var isExists = await _reciboRepository.isExists(id);
                if (!isExists)
                {
                    return NotFound();
                }
                var recibo = await _reciboRepository.FindById(id);
                var isSuccess = await _reciboRepository.Delete(recibo);
                if (!isSuccess)
                {
                    return InternalError("Error al eliminar registro");
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        private ObjectResult InternalError(string message)
        {
            return StatusCode(500, message);
        }

        private DateTime? stringToDate(string fecha)
        {
            try
            {
                string[] fechaSplit = fecha.Split('-');
                int year = int.Parse(fechaSplit[0]);
                int month = int.Parse(fechaSplit[1]);
                int day = int.Parse(fechaSplit[2]);

                DateTime fechaDt = new DateTime(year, month, day);
                return fechaDt;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
