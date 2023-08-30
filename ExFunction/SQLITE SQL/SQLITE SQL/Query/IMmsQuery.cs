using MDAS.Model.datatables;

namespace MDAS.IDAL.Query
{
    public interface IMmsQuery
    {
        public string BuildQueryMms(ExtendedRequestObject Object, string scadaIpList);
        public string BuildQueryMmsCount(ExtendedRequestObject Object, string scadaIpList);
        public string BuildQueryScadaMms(ExtendedRequestObject Object, string scadaIpList);
        public string BuildQueryScadaMmsCount(ExtendedRequestObject Object, string scadaIpList);
    }
}
