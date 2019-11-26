using BusinessSpecificLogic.EF;
using Reusable;
using System;
using System.Data.Entity;

namespace BusinessSpecificLogic.Logic
{
    public interface ITokenLogic : ILogic<Token>
    {
    }

    public class TokenLogic : Logic<Token>, ITokenLogic
    {
        public TokenLogic(DbContext context, IRepository<Token> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }

        protected override void onBeforeSaving(Token entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            if (mode == OPERATION_MODE.ADD)
            {
                entity.CreatedAt = DateTimeOffset.Now;
            }
        }
    }
}
