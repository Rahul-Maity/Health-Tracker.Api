using HealthTracker.DataService.Data;

namespace HealthTracker.Api.Controllers.v1;

public class TestController:BaseController
{

    public TestController(UnitOfWork unitOfWork):base(unitOfWork)
    {
        
    }
}
