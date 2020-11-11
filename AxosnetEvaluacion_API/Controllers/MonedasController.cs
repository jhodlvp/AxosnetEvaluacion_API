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
    /// Endpoints para interactuar con la tabla Monedas en la base de datos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class MonedasController : ControllerBase
    {
        private readonly IMonedaRepository _monedaRepository;
        private readonly IMapper _mapper;

        public MonedasController(IMonedaRepository monedaRepository,
            IMapper mapper)
        {
            _monedaRepository = monedaRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todas las monedas
        /// </summary>
        /// <returns>List: Monedas</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMonedas()
        {
            try
            {
                var monedas = await _monedaRepository.FindAll();
                var response = _mapper.Map<IList<MonedaGetDTO>>(monedas);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Obtiene un registro de Moneda usando su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto: Moneda</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMoneda(int id)
        {
            try
            {
                var moneda = await _monedaRepository.FindById(id);

                if (moneda == null)
                {
                    return NotFound();
                }
                var response = _mapper.Map<MonedaGetDTO>(moneda);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Crea una moneda
        /// </summary>
        /// <param name="monedaDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] MonedaPostDTO monedaDTO)
        {
            try
            {
                if (monedaDTO == null)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var moneda = _mapper.Map<Moneda>(monedaDTO);
                var isSuccess = await _monedaRepository.Create(moneda);
                if (!isSuccess)
                {
                    return InternalError("Error al crear la Moneda");
                }
                return Created("Create", new { moneda });
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Actualiza un registro de Moneda
        /// </summary>
        /// <param name="id"></param>
        /// <param name="monedaDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] MonedaUpdateDTO monedaDTO)
        {
            try
            {
                if (id < 1 || monedaDTO == null || id != monedaDTO.Id)
                {
                    return BadRequest();
                }

                var isExists = await _monedaRepository.isExists(id);
                if (!isExists)
                {
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var moneda = _mapper.Map<Moneda>(monedaDTO);
                var isSuccess = await _monedaRepository.Update(moneda);
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
        /// Borra un registro de Moneda
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
                var isExists = await _monedaRepository.isExists(id);
                if (!isExists)
                {
                    return NotFound();
                }
                var moneda = await _monedaRepository.FindById(id);
                var isSuccess = await _monedaRepository.Delete(moneda);
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
    }
}
