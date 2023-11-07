using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;   //de jsonignore

namespace WebApiPrueba1.Models;

public partial class categoria
{
    //no estaba antes; por eso se ponia:
    //public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    //abajo
    public categoria() { 
        Productos   = new HashSet<Producto>();
    }

    
    public int IdCategoria { get; set; }

    public string? Descripcion { get; set; }

    //esto es para ignorar, que productos esta devolviendo nulo
    //ya qque ha ignorado esa referencia ciclica
    //no la quiero visualizar en el api respuesta del get, la propiedad de producto desde postman
    [JsonIgnore]
    public virtual ICollection<Producto> Productos { get; set; }

    //esto estaba sin la 
    //public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

}
