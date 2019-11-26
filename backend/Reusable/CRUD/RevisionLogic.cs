using System.Data.Entity;

namespace Reusable
{
    /*
     * "Revision" is a document, but its logic is "Logic" instead of "DocumentLogic" in purpose
     * because we want to avoid "DocumentLogic" validations.
     */

    public interface IRevisionLogic : ILogic<Revision>
    {
    }

    public class RevisionLogic : Logic<Revision>, IRevisionLogic
    {
        public RevisionLogic(DbContext context, IDocumentRepository<Revision> repository, LoggedUser LoggedUser) : base(context, repository, LoggedUser)
        {
        }
    }
}