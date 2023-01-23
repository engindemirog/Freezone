using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freezone.Core.Persistence.Paging
{
    public class GetListResponse<T>:BasePageableModel
    {
        public IList<T> Items { get; set; }
    }
}
