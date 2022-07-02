using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIVeterinariaMimascot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SpecieController : ControllerBase
    {                
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly ILogger<SpecieController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public SpecieController(ILogger<SpecieController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IEnumerable<Specie> Get()
        {
            var species = new List<Specie>();

            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(connectionString))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "dbo.ShowAllSpecies";
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        species.Add(
                            new Specie
                            {
                                Id = myReader.GetInt32(0),
                                Nombre = myReader.GetString(1)
                            }
                        );
                    }
                }
            }

            return species;
        }

        [HttpGet("{id}")]
        public Specie Get(int id)
        {
            Specie specie = new Specie();

            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(connectionString))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "dbo.ShowSpecieById";

                    myCommand.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())

                        specie = new Specie
                        {
                            Id = myReader.GetInt32(0),
                            Nombre = myReader.GetString(1)
                        };
                }
            }
            return specie;
        }
              
        [HttpPost]
        public void Post([FromBody] string nombre)
        {
            using (SqlConnection myCon = new SqlConnection(connectionString))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "dbo.SaveEditSpecie";

                    myCommand.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = 0;
                    myCommand.Parameters.AddWithValue("@Nombre", SqlDbType.NVarChar).Value = nombre;                   

                    myCommand.ExecuteNonQuery();

                }
            }
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string nombre)
        {
            using (SqlConnection myCon = new SqlConnection(connectionString))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand("", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "dbo.SaveEditSpecie";

                    myCommand.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;
                    myCommand.Parameters.AddWithValue("@Nombre", SqlDbType.NVarChar).Value = nombre;
                    
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
                    myCommand.CommandText = "dbo.DeleteSpecieById";

                    myCommand.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                    myCommand.ExecuteNonQuery();

                }
            }
        }
    }
}
