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
    /// Endpoints para interactuar con la tabla Proveedors en la base de datos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class ProveedorsController : ControllerBase
    {
        private readonly IProveedorRepository _proveedorRepository;
        private readonly IMapper _mapper;

        public ProveedorsController(IProveedorRepository proveedorRepository,
            IMapper mapper)
        {
            _proveedorRepository = proveedorRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los proveedores
        /// </summary>
        /// <returns>List: Proveedors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProveedors()
        {
            try
            {
                var proveedors = await _proveedorRepository.FindAll();
                var response = _mapper.Map<IList<ProveedorGetDTO>>(proveedors);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Obtiene un Proveedor usando su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProveedor(int id)
        {
            try
            {
                var proveedor = await _proveedorRepository.FindById(id);

                if (proveedor == null)
                {
                    return NotFound();
                }
                var response = _mapper.Map<ProveedorGetDTO>(proveedor);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Crea un Proveedor
        /// </summary>
        /// <param name="proveedorDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ProveedorPostDTO proveedorDto)
        {
            try
            {
                if (proveedorDto == null)
                {
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var proveedor = _mapper.Map<Proveedor>(proveedorDto);
                var isSuccess = await _proveedorRepository.Create(proveedor);
                if (!isSuccess)
                {
                    return InternalError("Error al crear la Moneda");
                }
                return Created("Create", new { proveedor });
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Actualiza un registro de Proveedor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="proveedorDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] ProveedorUpdateDTO proveedorDto)
        {
            try
            {
                if (id < 1 || proveedorDto == null || id != proveedorDto.Id)
                {
                    return BadRequest();
                }

                var isExists = await _proveedorRepository.isExists(id);
                if (!isExists)
                {
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var proveedor = _mapper.Map<Proveedor>(proveedorDto);
                var isSuccess = await _proveedorRepository.Update(proveedor);
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
        /// Borra un registro de Proveedor
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
                var isExists = await _proveedorRepository.isExists(id);
                if (!isExists)
                {
                    return NotFound();
                }
                var proveedor = await _proveedorRepository.FindById(id);
                var isSuccess = await _proveedorRepository.Delete(proveedor);
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