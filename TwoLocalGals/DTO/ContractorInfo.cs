using System.Web;

namespace TwoLocalGals.DTO
{
    public class ContractorInfo
    {

        public HttpPostedFileBase ContractorPic { get; set; }
        public string businessName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }
}
