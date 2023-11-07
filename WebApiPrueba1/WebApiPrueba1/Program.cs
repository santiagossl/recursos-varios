using Microsoft.EntityFrameworkCore;    //sss
using System.Text.Json.Serialization;       //para ignorar los ciclos 
using WebApiPrueba1.Models;                //para llamar a mi contexto las voy a usar


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//se ponde el nombre de la clase DbApicontext
//configurado el contexto con la cadena de conxion
builder.Services.AddDbContext<DbapiContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL")));

//2.- Esta es despues de hacer consulta get como lista, y se la incluia el catalogo
//sirve para ignorar los ciclos , ignorar las referencias ciclicas
//nota: aquí productos sale null, si eso no se desea , se debe de hace :se debe de ir a modelo de categoria a la 
//propiedad de productos que es referencial, se la debe de ignorar
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    //con control punto buscamos la referencia que se necesita
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
