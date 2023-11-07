

using System;
using System.Collections.Generic;

namespace WebApiPrueba1.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string? CodigoBarra { get; set; }

    public string? Descripcion { get; set; }

    public string? Marca { get; set; }

    public int? IdCategoria { get; set; }

    public decimal? Precio { get; set; }

    //public virtual categoria? IdCategoriaNavigation { get; set; }
    //se cambio de nombre y se debe de ir al contexto para poder corregirlo. el contexto es lo de la bd con EF
    public virtual categoria? oCategoria { get; set; }

}
