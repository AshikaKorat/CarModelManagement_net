using Service.Models;

namespace Service.Services;
public interface ICarModelService
{
    IEnumerable<CarModelCore> GetCarModels();
    CarModelCore GetCarModel(int id);
    CarModelCore CreateCarModel(CarModelCore carModel);
    int SaveImage(int id, byte[] image);
}
