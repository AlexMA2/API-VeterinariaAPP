using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIVeterinariaMimascot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class PetController : ControllerBase
    {

        private readonly IHttpContextAccessor _contextAccessor;

        private readonly ILogger<PetController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public PetController(ILogger<PetController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IEnumerable<Pet> Get()
        {

            var pets = new List<Pet>();            

            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(connectionString))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "dbo.ShowAllPets";
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        pets.Add(
                            new Pet
                            {
                                Id = myReader.GetInt32(0),
                                Nombre = myReader.GetString(1),
                                Edad = (double)myReader.GetDecimal(2),
                                Genero = myReader.GetString(3),
                                Especie = myReader.GetString(4),
                                Problema = myReader.GetString(5),
                                FechaReservada = myReader.GetDateTime(6)
                            }
                        );
                    }
                }
            }

            return pets;

        }
    
   
        [HttpGet("{id}/{filter}")]
        public IEnumerable<Pet> Get(int id, string filter)
        {
            var pets = new List<Pet>();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(connectionString))
            {
               
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("", myCon))
                {
                    if (filter == "I")
                    {
                        var pet = new Pet();
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.CommandText = "dbo.ShowPetById";

                        myCommand.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                        myReader = myCommand.ExecuteReader();
                        while (myReader.Read())
                        {

                            pets.Add(
                               new Pet
                               {
                                   Id = myReader.GetInt32(0),
                                   Nombre = myReader.GetString(1),
                                   Edad = (double)myReader.GetDecimal(2),
                                   Genero = myReader.GetString(3),
                                   Especie = myReader.GetString(4),
                                   Problema = myReader.GetString(5),
                                   FechaReservada = myReader.GetDateTime(6)
                               }
                            );

                        }
                        
                    }
                    if (filter != "I"){
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.CommandText = "dbo.ShowPetsBySpecie";

                        myCommand.Parameters.AddWithValue("@Specie", SqlDbType.NVarChar).Value = filter;
                        
                        myReader = myCommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            
                            pets.Add(
                               new Pet
                               {
                                   Id = myReader.GetInt32(0),
                                   Nombre = myReader.GetString(1),
                                   Edad = (double)myReader.GetDecimal(2),
                                   Genero = myReader.GetString(3),
                                   Especie = myReader.GetString(4),
                                   Problema = myReader.GetString(5),
                                   FechaReservada = myReader.GetDateTime(6)
                               }
                            );
                        }
                        
                    }
                    
                }
            }
            return pets;
        }


        [HttpPost]        
        public void Post([FromBody] string nombre, double edad, string genero, int especie, string problema, DateTime fechaReservada)
        {
            Console.WriteLine(nombre);
            Console.WriteLine(edad);
            Console.WriteLine(genero);
            Console.WriteLine(especie);
            Console.WriteLine(problema);
            Console.WriteLine(fechaReservada);

            using (SqlConnection myCon = new SqlConnection(connectionString))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "dbo.SaveEdit";

                    myCommand.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = 0;
                    myCommand.Parameters.AddWithValue("@Nombre", SqlDbType.NVarChar).Value = nombre;
                    myCommand.Parameters.AddWithValue("@Edad", SqlDbType.Decimal).Value = edad;
                    myCommand.Parameters.AddWithValue("@Genero", SqlDbType.NChar).Value = genero;
                    myCommand.Parameters.AddWithValue("@Especie", SqlDbType.Int).Value = especie;
                    myCommand.Parameters.AddWithValue("@Problema", SqlDbType.NVarChar).Value =  problema;
                    myCommand.Parameters.AddWithValue("@FechaReservada", SqlDbType.DateTime).Value = fechaReservada;

                    myCommand.ExecuteNonQuery();
                   
                }
            }
        }
       
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string nombre, double edad, string genero, int especie, string problema, DateTime fechaReservada)
        {
            using (SqlConnection myCon = new SqlConnection(connectionString))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "dbo.SaveEdit";

                    myCommand.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;
                    myCommand.Parameters.AddWithValue("@Nombre", SqlDbType.NVarChar).Value = nombre;
                    myCommand.Parameters.AddWithValue("@Edad", SqlDbType.Decimal).Value = edad;
                    myCommand.Parameters.AddWithValue("@Genero", SqlDbType.NChar).Value = genero;
                    myCommand.Parameters.AddWithValue("@Especie", SqlDbType.Int).Value = especie;
                    myCommand.Parameters.AddWithValue("@Problema", SqlDbType.NVarChar).Value = problema;
                    myCommand.Parameters.AddWithValue("@FechaReservada", SqlDbType.DateTime).Value = fechaReservada;

                    myCommand.ExecuteNonQuery();

                }
            }
        }

      
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (SqlConnection myCon = new SqlConnection(connectionString))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "dbo.DeletePetById";

                    myCommand.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;
                    
                    myCommand.ExecuteNonQuery();

                }
            }
        }
    }
}
