using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;

namespace BusinessSpecificLogic.Logic
{
    public interface IShiftLogic : ILogic<Shift>
    {
    }

    public class ShiftLogic : Logic<Shift>, IShiftLogic
    {
        public ShiftLogic(DbContext context, IRepository<Shift> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }
    }
}
