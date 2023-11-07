using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using WebApiPrueba1.Models;



namespace WebApiPrueba1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        //declarar un objeto de Dbapicontext
        // con _dbcontex podemos usar las operaciones crud par los modelos prod y cat
        public readonly DbapiContext _dbcontex;

        //crear el constructor, recibimos el contexto
        //estamos preparados para usar el contexto de la bd para el CRUD
        public ProductoController(DbapiContext _contex)
        {
            //asigno el valor de _contex a dbcontex de arriba tipo dbapiContext
            _dbcontex = _contex;
        }

        //Empezaremos con las api, y Lo necesario para el crud

        //******************************************************************************************************************
        //INICIO: GET
        //******************************************************************************************************************
        [HttpGet] //esde tipo get , obtener, listar
        //Crea una ruta para poder llamar a la api desde el Swagger, postman es donde se ejecuta
        [Route("Lista")]
        public IActionResult Lista() //este nombre pudo ser otro no necesariamente lista
        {
            //crear un objeto de lista de producto, se llamara lista, el cual va a ser una
            //lista de productos
            List<Producto> lista = new List<Producto>();

            try
            {
                //opcion 1
                //se va a llamar a mi lista, utilizando a _dbcontex, usando el modelo de mi tabla
                // de productos, y quiero que retorne una lista
                //de esta forma no esta incluida la tabla categoria, o los datos relacionadas
                //por eso salia nulo en el postman
                //lista = _dbcontex.Productos.ToList();

                //opció2: la de arriba fue la primera sin incluir la información de la categoria
                //lo va a devolver como una lista
                //de esta forma daría previamente un error a las referencias ciclicas, no se puede convertir en objeto json
                //no tiene fin. se debe de decir que ignore la referencis ciclicas a tavés de program.cs
                lista = _dbcontex.Productos.Include(c => c.oCategoria).ToList();


                //aqui crea un nuevo json (mensaje=ok)adentro, despues del stuscodes con el 200ok,
                //para devolver la lista y el mensaje ok
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = lista });

            }
            catch (Exception ex)
            {
                //aquí devuelve el error
                return StatusCode(StatusCodes.Status200OK, new { mensaje =ex.Message, Response = lista });
            }
            //se ejecuta el proyecto para realizar las pruebas de la api en postman
            //cuando se ejecuta se alza ej. http://localhost:5259/swagger/index.html , es una docuemntacion de las api
            //que se vaya a crear. Se dede de bajar postman en la pc , logonearse y ahi poner el localhost de
            //la ejecución del proyecto webapi para escoger el metodo get o el que sea 

        }
        //******************************************************************************************************************
        //FINAL: GET
        //******************************************************************************************************************

        //******************************************************************************************************************
        //INICIO: GET dos - CON PI ID
        //******************************************************************************************************************

        [HttpGet] //esde tipo get , obtener, listar
        //Crea una ruta para poder llamar a la api desde el Swagger, postman es donde se ejecuta
        //también se debe de especificar que se necesita un parametro idProducto que es entero. debe estar dentro de {}
        //debe de ser referencia al que recibe obtener del actionResult
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto) //este nombre pudo ser otro no necesariamente Obtener, tiene parametro id
        {
            //crear un objeto de lista de producto, se llamara lista, el cual va a ser una
            //lista de productos
            //esto estaba antes en el get solito
            //List<Producto> lista = new List<Producto>();

            //aqui creo un objeto producto y lo instancio. esta es una forma
            //la que sigue es la mejor
            //Producto oProducto = new Producto();

            //Antes del try se va a realizar una busqueda de este producto
            Producto oProducto = _dbcontex.Productos.Find(idProducto);

            if (oProducto == null) {
                return BadRequest("Producto no Encontrado");
            }

            try
            {
                //opcion 1
                //se va a llamar a mi lista, utilizando a _dbcontex, usando el modelo de mi tabla
                // de productos, y quiero que retorne una lista
                //de esta forma no esta incluida la tabla categoria, o los datos relacionadas
                //por eso salia nulo en el postman
                //lista = _dbcontex.Productos.ToList();

                //opció2: la de arriba fue la primera sin incluir la información de la categoria
                //lo va a devolver como una lista
                //de esta forma daría previamente un error a las referencias ciclicas, no se puede convertir en objeto json
                //no tiene fin. se debe de decir que ignore la referencis ciclicas a tavés de program.cs
                //lista = _dbcontex.Productos.Include(c => c.oCategoria).ToList();

                oProducto = _dbcontex.Productos.Include(c => c.oCategoria).Where(p => p.IdProducto == idProducto).FirstOrDefault();


                //aqui crea un nuevo json (mensaje=ok)adentro, despues del stuscodes con el 200ok,
                //para devolver la lista y el mensaje ok
                //return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = lista });

                //devuelve el objeto
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = oProducto });

            }
            catch (Exception ex)
            {
                //aquí devuelve el error
                //return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, Response = lista });

                //devuelve el mensaje de error y el objeto
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, Response = oProducto });

            }
            //se ejecuta el proyecto para realizar las pruebas de la api en postman
            //cuando se ejecuta se alza ej. http://localhost:5259/swagger/index.html , es una docuemntacion de las api
            //que se vaya a crear. Se dede de bajar postman en la pc , logonearse y ahi poner el localhost de
            //la ejecución del proyecto webapi para escoger el metodo get o el que sea 

        }
        //******************************************************************************************************************
        //FINAL:  GET dos - CON PI ID
        //******************************************************************************************************************


        //GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG
        //INICIO: GRABAR
        //GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG
        [HttpPost]
        [Route("Guardar")]
        //recibe un frombody de tipo producto que se va a llamar objeto
        //Cuando utilizas el verbo HttpPost en un método de un controlador 
        //de una Web API en Visual Studio 2022, es común recibir datos
        //del cliente en el cuerpo de la solicitud(RequestBody). 
        //Para capturar estos datos en tu acción, 
        //puedes utilizar el atributo[FromBody].
        public ActionResult Guardar([FromBody] Producto objeto)
        {
            //NOTA: En postman debemos seleccionar el post, e ir a la pestaña BODY,
            //      la opción raw , y seleccionar el tiop Json en la parte iz final
            //      en lugar de text
            //      el json que vamos a solicitar, lo vamos a obtener del swagger,
            //      desplegando el api de guardar y copiar todo el ejemplo que nos 
            //      proporciona de json. Luego lo pegamos en postman
            //      borro el objeto de categoria y IdProducto que no lo necesitamos
            //      e ingreso los datos que quiero insertar


            try
            {
                //agrego el objeto al modelo de mi producto
                _dbcontex.Productos.Add(objeto);
                //aquí guardo los cambios
                _dbcontex.SaveChanges();

                //ahora no devuelve el objeto con el mensaje, sólo el mensaje de ok
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok"});
            }
            catch (Exception ex)
            {
                   //solo devuelve el mensaje de error, no el objeto
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        
        }




        //GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG
        //INICIO: GRABAR
        //GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG

    }
}
