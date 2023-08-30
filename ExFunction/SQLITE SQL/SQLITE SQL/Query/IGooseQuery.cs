using MDAS.Model.datatables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDAS.IDAL.Query
{
	public interface IGooseQuery
	{
		public string BuildQueryGoose(ExtendedRequestObject Object);
		public string BuildQueryGooseCount(ExtendedRequestObject Object);
	}
}
