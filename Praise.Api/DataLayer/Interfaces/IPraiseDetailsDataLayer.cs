using Praise.Models;

namespace Praise.Api.DataLayer.Interfaces
{
    public interface IPraiseDetailsDataLayer
    {
        Task<PraiseDetails> Read(Guid id);
        Task<IEnumerable<PraiseDetails>> ReadByPraiseSenderEmail(string email, int? pageNumber, int? numberPerPage);
        Task<IEnumerable<PraiseDetails>> ReadByPraiseReceiverEmail(string email, int? pageNumber, int? numberPerPage);
        Task<PraiseDetails> Save(PraiseDetails praiseDO);
    }
}