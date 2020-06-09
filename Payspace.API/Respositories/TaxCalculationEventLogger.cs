using Payspace.API.Models;
using Payspace.Domain;
using Payspace.Domain.Interface;

namespace Payspace.API.Respositories
{
    //a logger is a cross-cutting concern. i'll put the implementation here
    //since it's no going to be used elsewhere really.
    //i would also pump this to a bus to be written offline, because this sort of
    //thing doesn't have to connect to a db and hold up an online request
    //a little bit of waiting for the event to appear should be a good trade-off
    //for speed.

    //also note we can inject a different version of this interface into the 
    //container and write these events elsewhere without changing much.

    public class TaxCalculationEventLogger : ITaxCalculationEventLogger
    {
        private readonly AppDbContext _dbContext;

        public TaxCalculationEventLogger(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void LogEvent(TaxCalculationEvent taxEvent)
        {
            _dbContext.TaxCalculationEvents.Add(taxEvent);
            _dbContext.SaveChangesAsync();
        }
    }
}
