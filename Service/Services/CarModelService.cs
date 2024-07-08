using Dapper;
using Service.Models;
using System.Data;

namespace Service.Services;
public class CarModelService : ICarModelService
{
    private readonly IDbConnection _dbConnection;

    public CarModelService(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public IEnumerable<CarModelCore> GetCarModels()
    {
        return _dbConnection.Query<CarModelCore>("GetCarModels", commandType: CommandType.StoredProcedure);
    }

    public CarModelCore GetCarModel(int id)
    {
        string query = "SELECT * FROM CarModel WHERE Id = @Id";
        return _dbConnection.QueryFirstOrDefault<CarModelCore>(query, new { Id = id });
    }

    public CarModelCore CreateCarModel(CarModelCore carModel)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Brand", carModel.Brand);
        parameters.Add("@Class", carModel.Class);
        parameters.Add("@ModelName", carModel.ModelName);
        parameters.Add("@ModelCode", carModel.ModelCode);
        parameters.Add("@Description", carModel.Description);
        parameters.Add("@Features", carModel.Features);
        parameters.Add("@Price", carModel.Price);
        parameters.Add("@DateOfManufacturing", carModel.DateOfManufacturing);
        parameters.Add("@Active", carModel.Active);
        parameters.Add("@SortOrder", carModel.SortOrder);

        // Call the stored procedure and retrieve the generated ID
        int id = _dbConnection.ExecuteScalar<int>("InsertCarModel", parameters, commandType: CommandType.StoredProcedure);
        carModel.Id = id;

        return carModel;
    }
    public int SaveImage(int id, byte[] image)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@HeaderId", id);
        parameters.Add("@ImageData", image);
        // Call the stored procedure and retrieve the generated ID
        return _dbConnection.ExecuteScalar<int>("InsertImageStorage", parameters, commandType: CommandType.StoredProcedure);
    }
}
