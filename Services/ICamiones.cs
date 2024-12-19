using DTO;
using TrasportesAPI.Models;

namespace TrasportesAPI.Services
{
    public interface ICamiones
    {
        //es una estructura que define un contrato o conjunto de métodos y
        //propiedades que una clase debe implementar.
        //Una interfaz establece un conjunto de requisitos que cualquier clase
        //que la implemente debe seguir. Estos requisitos son declarados en la
        //interfaz en forma de firmas de métodos y propiedades,
        //pero la interfaz en sí misma no proporciona ninguna implementación
        //de estos métodos o propiedades.Es responsabilidad de las clases que
        //implementan la interfaz proporcionar las implementaciones concretas de
        //estos miembros.

        //Las interfaces son útiles para lograr la abstracción y la reutilización
        //de código en C#.

        //GET
        List<Camiones_DTO> GetCamiones();

        //GETbyID
        Camiones_DTO GetCamio(int id);

        //INSERT (POST)
        string InsertCamion(Camiones_DTO camion);

        //UPDATE (PUT)
        string UpdateCamion(Camiones_DTO camion);

        //DELETE (DELETE)
        string DeleteCamion(int id);
    }
    // la clase que impleenta a interfaz y declara la implementacin de la logica de los metodos existentes
    public class CamionesService : ICamiones //agregar implementar interfaz
    {
        //variables para crear el contexto(inyecccion de dependencias)
        private readonly TransportesContext _context;

        //costructor para inicializar el contexto
        public CamionesService(TransportesContext context)
        {
            _context = context;
        }
        //Implementacion de metodos
        public string DeleteCamion(int id)
        {
            try
            {
                //obtengo primero el camion de la base de datos
                Camiones _camion = _context.Camiones.Find(id);
                if (_camion == null) 
                {
                    return $"No se encontro algun objeto con identificador {id}";
                }

                //remuevo el obeto del contexto
                _context.Camiones.Remove(_camion);
                //impacto la BD
                _context.SaveChanges();
                //respondo
                return $"Camion {id} elimindao con exito";
            }
            catch (Exception ex) 
            {
                return "Error: "+ex.Message;
            }
        }

        public Camiones_DTO GetCamio(int id)
        {
            //LinQ
            /*_context.Camiones.Find(id);*///DTO
            //dynamicMApper (lo consumes)
            Camiones origen = _context.Camiones.Find(id);
            Camiones_DTO resultado = DynamicMapper.Map<Camiones,Camiones_DTO>(origen);
            return resultado;
        }

        public List<Camiones_DTO> GetCamiones()
        {
            try
            {
                //Lista de camiones del original
                List<Camiones> lista_origial = _context.Camiones.ToList();
                //lista de DTOS
                List<Camiones_DTO> lista_salida = new List<Camiones_DTO>();
                //recorro cada camion y genero un nuevo DTO con DynamicMapper
                foreach (var cam in lista_origial)
                {
                    //usamos el dynamicmapper para convertir los objetos
                    Camiones_DTO DTO = DynamicMapper.Map<Camiones, Camiones_DTO>(cam);
                    lista_salida.Add(DTO);
                }
                //RETORNO LA LISTA CON LOS OBJETOS YA MAPEADOS
                return lista_salida;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public string InsertCamion(Camiones_DTO camion)
        {
            try
            {
                //creo un camion del modelo original
                Camiones _camion = new Camiones();
                //asigno los valores del objeto DTO del parametro al objeto del modelo original
                _camion = DynamicMapper.Map<Camiones_DTO, Camiones>(camion);
                //añadimos el objeto al contexto
                _context.Camiones.Add(_camion);
                //impactamos la BD 
                _context.SaveChanges();
                return "Camion insertado con exito";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message; ;
            }
        }

        public string UpdateCamion(Camiones_DTO camion)
        {
            try
            {
                //creo un camion del modelo original
                Camiones _camion = new Camiones();
                //asigno los valores del objeto DTO del parametro al objeto del modelo original
                _camion = DynamicMapper.Map<Camiones_DTO, Camiones>(camion);
                //modifico el estado del objeto en el contexto
                _context.Entry(_camion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                //impactamos la BD 
                _context.SaveChanges();
                //respondo
                return $"Camion {_camion.ID_Camion} actualizado con exito";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message; ;
            }
        }
    }


}
