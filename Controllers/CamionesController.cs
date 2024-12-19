using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TrasportesAPI.Services;

namespace TrasportesAPI.Controllers
{
    [Route("api/[controller]")] //se declara el espacio nombre
    [ApiController] //establece el trato del controlador
    public class CamionesController : ControllerBase //LAs apis heredan de controller base
    {
        //variables para interfaz y el contexto
        private readonly ICamiones _service;
        private readonly TransportContext  _context;

        //constructor para inicializar mi servicio y mi contexto 
        //dependency inyection
        public CamionesController(ICamiones service, TransportContext context)
        {
            _service = service;

            _context = context;

        }

        //GET
        [HttpGet]//declarador
        [Route("getCamiones")]//es necesario para identificarlo
        public List<Camiones_DTO> getCamiones() 
        {
            //creo una lista de objeto DTO y la lleno con mi servicio
            List<Camiones_DTO> lista = new List<Camiones_DTO>();
            return lista;//retorno la lista al exterior
        }

        //GET by ID
        [HttpGet]//declarador
        [Route("getCamion/{id}")]//es necesario para identificarlo
        public Camiones_DTO getCamion(int id)
        {
            //creo una lista de objeto DTO y la lleno con mi servicio
            Camiones_DTO camion = _service.GetCamio(id);
            return camion;//retorno la lista al exterior
        }

        //Post (insertar)
        [HttpPost]
        [Route("insertCamion")]
        //los metodos IActionResult retornan una respuesta API en un formato establecido, capaz de ser leido por cuaquier cliente HTTP
        //por otro lado, la sentencia [FromBody] determina que existe contenido en el cuerpo de la peticion
        public IActionResult insertCamion([FromBody] Camiones_DTO camion)
        {
            //consumo mi servicio
            string respuesta = _service.InsertCamion(camion);
            //retorno un nuevo objeto del tipo ok, siendo este un tipo de respuesta http
            //se genera un nuevo objeto cola respuesta (new {respuesta}) para que esta tenga un formato aslida JSON y no texto plano
            return Ok(new { respuesta });
        }

        //PUT (actualizar)
        [HttpPut]
        [Route("updateCamion")]
        public IActionResult updateCamion([FromBody] Camiones_DTO camion)
        {
            string respuesta = _service.UpdateCamion(camion);
            return Ok(new { respuesta });
        }

        //Delete
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult udeleteCamion(int id)
        {
            string respuesta = _service.DeleteCamion(id);
            return Ok(new { respuesta });
        }
    }
}
