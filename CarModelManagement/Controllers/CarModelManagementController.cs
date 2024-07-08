using Microsoft.AspNetCore.Mvc;
using Service.Services;
using CarModelManagement.Models;
using Service.Models;
using System.IO;
using System.Transactions;

namespace CarModelManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarModelManagementController : ControllerBase
    {
        private readonly ICarModelService _carModelService;

        public CarModelManagementController(ICarModelService carModelService)
        {
            _carModelService = carModelService;
        }
        [HttpGet("GetCarModels")]
        public ActionResult<IEnumerable<CarModel>> GetCarModels()
        {
            var carModels = _carModelService.GetCarModels();

            foreach (var model in carModels)
            {
                model.Image = Convert.ToBase64String(model.ImageData);
                model.ImageData = null; 
            }
            return Ok(carModels);
        }

        [HttpGet("GetCarModels/{id}")]
        public ActionResult<CarModel> GetCarModels(int id)
        {
            var carModel = _carModelService.GetCarModel(id);
            if (carModel == null)
            {
                return NotFound();
            }
            return Ok(carModel);
        }

        [HttpPost]
        public ActionResult<CarModel> CreateCarModel([FromForm] CarModel carModel)
        {
            try
            {
                var carModelCore = new CarModelCore
                {
                   
                    Brand = carModel.Brand,
                    Class = carModel.Class,
                    ModelName = carModel.ModelName,
                    ModelCode = carModel.ModelCode,
                    Description = carModel.Description,
                    Features = carModel.Features,
                    Price = carModel.Price,
                    DateOfManufacturing = carModel.DateOfManufacturing,
                    Active = carModel.Active,
                    SortOrder = carModel.SortOrder
                };
                using (var scope = new TransactionScope())
                {
                    var createdCarModel = _carModelService.CreateCarModel(carModelCore);

                    // Example: Saving images (assuming using _carModelService to save)
                    foreach (var image in carModel.Images)
                    {
                        // Handle file upload logic, save to storage, etc.
                        if (image.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                // Copy the image data to a memory stream
                                image.CopyToAsync(memoryStream);

                                // Convert the image data to a byte array
                                byte[] imageData = memoryStream.ToArray();

                                // Save image to database using _carModelService
                                int imageId = _carModelService.SaveImage(createdCarModel.Id, imageData);

                            }
                        }
                    }
                    scope.Complete(); // Commit transaction if no exceptions
                    return CreatedAtAction(nameof(GetCarModels), new { id = createdCarModel.Id }, createdCarModel);
                }
                
                
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error creating car model: {ex.Message}");
                return BadRequest("Error creating car model");
            }
        }

    }
}
