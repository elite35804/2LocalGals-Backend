using Nexus;
using System;
using System.IO;
using System.Web;
using System.Web.Http;
using TwoLocalGals.DTO;

namespace TwoLocalGals.Controllers.APIs
{
    [RoutePrefix("api/v1")]
    [Authorize]
    public class ScheduleController : ApiController
    {
        [HttpGet]
        [Route("schedule/GetEarningByDateRange/{UserID}")]
        public IHttpActionResult GetUserEarning(string UserID, DateTime start, DateTime end)
        {

            return Ok();
        }


        [HttpGet]
        [Route("jobinfo/{CustomerID:int}")]
        public IHttpActionResult GetJobInfo(int CustomerID)
        {
            JobInfo user = Database.GetJobInfoCustomerById(CustomerID);


            return Ok(user);
        }



        [HttpGet]
        [Route("GetContractorInfo/{ContractorID:int}")]
        public IHttpActionResult GetContractorInfo(int ContractorID)
        {
            ContractorStruct contractor = Database.GetContractorByID(ContractorID);
            if (!string.IsNullOrEmpty(contractor.ContractorPic))
            {
                var Request = HttpContext.Current.Request;
                var ImageUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, $"/ContratorPics/{contractor.ContractorPic}");
                contractor.ContractorPic = ImageUrl;
            }

            return Ok(contractor);

        }
        
        [HttpPost]
        [Route("UpdateContractorInfo/{ContractorID:int}")]
        public IHttpActionResult UpdateContractorInfo(int ContractorID)
        {

            var httpRequest = HttpContext.Current.Request;
            ContractorInfo contractor = new ContractorInfo();
            contractor.businessName = httpRequest.Form["businessName"];
            contractor.address = httpRequest.Form["address"];
            contractor.city = httpRequest.Form["city"];
            contractor.state = httpRequest.Form["state"];
            contractor.zip = httpRequest.Form["zip"];

            if (string.IsNullOrEmpty(contractor.address) || string.IsNullOrEmpty(contractor.city) || string.IsNullOrEmpty(contractor.state) || string.IsNullOrEmpty(contractor.zip))
            {
                return BadRequest("Required fields Address, City, State, and Zip");
            }

            DBRow row = new DBRow();
            row.SetValue("businessName", contractor.businessName);
            row.SetValue("address", contractor.address);
            row.SetValue("city", contractor.city);
            row.SetValue("state", contractor.state);
            row.SetValue("zip", contractor.zip);


            if (httpRequest.Files.Count > 0)
            {
                try
                {

                    var postedFile = httpRequest.Files[0];
                    var getExtension = Path.GetExtension(postedFile.FileName);
                    var fileName = Guid.NewGuid().ToString() + getExtension;
                    var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ContratorPics/"));
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    postedFile.SaveAs(Path.Combine(filePath,fileName));

                    row.SetValue("ContractorPic", fileName);


                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                  
                }
            }


            string error = Database.DynamicSetWithKeyInt("Contractors", "contractorID", ref ContractorID, row);
            if (error != null)
                return BadRequest(error);


            return Ok(new { status = "contrator info has updated successfully." });


        }



    }
}