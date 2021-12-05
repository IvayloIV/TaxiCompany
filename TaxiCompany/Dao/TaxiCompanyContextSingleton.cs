using System.Linq;

namespace TaxiCompany.Dao
{
    public sealed class TaxiCompanyContextSingleton
    {
        private static ITaxiCompanyContext goodsContext;

        private TaxiCompanyContextSingleton() { }

        public static void InitContext(ITaxiCompanyContext goodsContext)
        {
            TaxiCompanyContextSingleton.goodsContext = goodsContext;
            if (goodsContext.Database != null)
            {
                goodsContext.Database.SqlQuery<int>("select 1").First();
            }
        }

        public static void DestroyContext()
        {
            goodsContext.Dispose();
        }

        public static ITaxiCompanyContext GetContext()
        {
            return goodsContext;
        }
    }
}
