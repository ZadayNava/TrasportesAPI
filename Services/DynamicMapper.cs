using System.Reflection;
namespace TrasportesAPI.Services
{
    public class DynamicMapper
    {
        //metodo que mapea de forma dinamica diferentes tipos de objetos
        //por ejemplo: modelo originales a DTO y viceversa
        //dejamos de hacer las cosas a mano


            //el objeto t es como una varibale var en js, que pueden represenatr cualquier cosa, aqui lo estamos personaliando
        public static TDestination Map<Tsource, TDestination>(Tsource source) 
            where Tsource : class //se declara una clase abstracta como tipo de objeto de entrada
            where TDestination : class, new() //se declara una clase abstracta como tipo de objeto de salida
        {
            //valido si existee y contiene informacion la clase origen
            if (source == null) throw new ArgumentNullException("source");
            var destination = new TDestination(); //creo una instancia del objeto de salida

            //recuperar las propiedades (los atributos de mis elementos) usando la biblioteca system.reflexion
            //Mediante reflexión, puedes acceder a las propiedades de un tipo (clase, estructura, etc.) en tiempo de ejecución, incluso si no conoces el tipo exacto en tiempo de compilación.
            //GetProperties: Devuelve un array con todas las propiedades públicas del tipo especificado.
            //BindingFlags: Opciones que especifican qué miembros buscar(públicos, privados, estáticos, etc.).
            //using System.Reflection; es necesario importar las libreria

            var sourceProperties = typeof(Tsource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //estamos separando los obejtos 
            var destinationProperties = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //recorro todos los atributos y propiedaes del objeto de origen para equipararlos con e objeto de salida
            foreach (var sourceProperty in sourceProperties)
            {
                //recupero cada propiedad de la clase donde empate tanto el nombre de la propiedad como el tipo de dato (aqui es donde se mapea los obetos)
                var destinationProperty = destinationProperties.FirstOrDefault(dp => dp.Name.ToLower() == sourceProperty.Name.ToLower() && dp.PropertyType == sourceProperty.PropertyType);
                //si la propiedad es accesible y tiene valor, paso el datos de origen al destino
                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    //GetValue: lee el valor actual de la propiedad para un objeto
                    //SetValue: Establece un nuevo valor para la propiedad de un objeto
                    var value = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, value);
                }
            }
            //retorno el unevo tipo de objeto ya mapeado
            return destination;
        }
    }
}
