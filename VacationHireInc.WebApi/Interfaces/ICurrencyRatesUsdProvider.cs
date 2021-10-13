// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System.Collections.Generic;
using System.Threading.Tasks;

namespace VacationHireInc.WebApi.Interfaces
{
    public interface ICurrencyRatesUsdProvider
    {
        Task<Dictionary<string, decimal>> GetRatesAsync();
    }
}
