using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;

namespace BusinessSpecificLogic.Logic
{
    public interface ISkillLogic : ILogic<Skill>
    {
    }

    public class SkillLogic : Logic<Skill>, ISkillLogic
    {
        public SkillLogic(DbContext context, IRepository<Skill> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }
    }
}
