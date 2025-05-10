using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ABPD_4
{ 
    // http://localhost:5000/api/animals
    // POST http://localhost:5000/api/animals \
    // -H "Content-Type: application/json" \
    // -d '{"name": "Reksio", "category": "Pies", "weight": 25.0, "furColor": "Brązowy"}'
    // [{"id":1,"name":"Narkotek","category":"Pies","weight":20.5,"furColor":"Brązowy"},{"id":2,"name":"Buszek","category":"Kot","weight":50,"furColor":"Czarny"},{"id":3,"name":"Miauczek","category":"Kot","weight":4.3,"furColor":"Czarny"},{"id":4,"name":"Skoczek","category":"Królik","weight":2.1,"furColor":"Biały"},{"id":5,"name":"Grzmiotek","category":"Koń","weight":500,"furColor":"Kasztanowy"}]
    
    
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }

    [ApiController]
    [Route("api/animals")]
    public class AnimalsController : ControllerBase
    {
        private static List<Animal> Animals = new List<Animal>
        {
            new Animal { Id = 1, Name = "NarKotek", Category = "Pies", Weight = 20.5, FurColor = "Brązowy" },
            new Animal { Id = 2, Name = "Buszek", Category = "Kot", Weight = 50.0, FurColor = "Czarny" },
            new Animal { Id = 3, Name = "Miauczek", Category = "Kot", Weight = 4.3, FurColor = "Czarny" },
            new Animal { Id = 4, Name = "Skoczek", Category = "Królik", Weight = 2.1, FurColor = "Biały" },
            new Animal { Id = 5, Name = "Grzmiotek", Category = "Koń", Weight = 500.0, FurColor = "Kasztanowy" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Animal>> GetAnimals()
        {
            return Ok(Animals);
        }

        [HttpGet("{id}")]
        public ActionResult<Animal> GetAnimalById(int id)
        {
            var animal = Animals.FirstOrDefault(a => a.Id == id);
            if (animal != null)
            {
                return Ok(animal);
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<Animal> AddAnimal([FromBody] Animal animal)
        {
            animal.Id = Animals.Max(a => a.Id) + 1;
            Animals.Add(animal);
            return CreatedAtAction(nameof(GetAnimalById), new { id = animal.Id }, animal);
        }

        [HttpPut("{id}")]
        public ActionResult EditAnimal(int id, [FromBody] Animal updatedAnimal)
        {
            var animal = Animals.FirstOrDefault(a => a.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            animal.Name = updatedAnimal.Name;
            animal.Category = updatedAnimal.Category;
            animal.Weight = updatedAnimal.Weight;
            animal.FurColor = updatedAnimal.FurColor;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAnimal(int id)
        {
            var animal = Animals.FirstOrDefault(a => a.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            Animals.Remove(animal);
            return NoContent();
        }
        
        [HttpGet("search")]
        public ActionResult<IEnumerable<Animal>> SearchAnimalsByName([FromQuery] string name)
        {
            var result = Animals
                .Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(result);
        }
    }

    [ApiController]
    [Route("api/animals/{animalId}/visits")]
    public class VisitsController : ControllerBase
    {
        private static List<Visit> Visits = new List<Visit>
        {
            new Visit
            {
                Id = 1, AnimalId = 1, VisitDate = DateTime.Now.AddDays(10),
                Description = "Szczepienie", Price = 100
            },
            new Visit
            {
                Id = 2, AnimalId = 2, VisitDate = DateTime.Now.AddDays(5), Description = "Badanie",
                Price = 50
            }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Visit>> GetVisits(int animalId)
        {
            var visits = Visits.Where(v => v.AnimalId == animalId).ToList();
            return Ok(visits);
        }

        [HttpPost]
        public ActionResult<Visit> AddVisit(int animalId, [FromBody] Visit visit)
        {
            visit.Id = Visits.Max(v => v.Id) + 1;
            visit.AnimalId = animalId;
            Visits.Add(visit);
            return CreatedAtAction(nameof(GetVisits), new { animalId = visit.AnimalId }, visit);
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
    
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Weight { get; set; }
        public string FurColor { get; set; }
    }

    public class Visit
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public DateTime VisitDate { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}