using Wallet.Domain.Utils.Page;

namespace CommonTestUtilities.Requests.Page
{
    public class PagedListBuilder
    {
        public static PagedList<T> Build<T>(
            List<T> items,
            int page = 1,
            int pageSize = 10)
        {
            return new PagedList<T>(
                items: items,
                page: page,
                pageSize: pageSize,
                totalCount: items.Count
            );
        }
    }
}
