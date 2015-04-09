using AntiTheftClub.DbModel;
using AntiTheftClub.Models;
using AntiTheftClub.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AntiTheftClub.Controllers
{
    public class CommonController : ApiController
    {
        private DbEntities db;
        private GeneralService generalService;

        public CommonController()
        {
            this.db = new DbEntities();
            this.generalService = new GeneralService(db);
        }

        // GET api/common
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public async Task<TwoWheelerOptions> GetTwoWheelerOption()
        {
            TwoWheelerOptions result = new TwoWheelerOptions();
            result.vehicleMakeOptions = await this.generalService.GetVehicleMake(1);
            result.manufactureYearOptions = await this.generalService.GetLookUp("MY");
            result.countryOptions = new List<CodeNamePairModel>() { new CodeNamePairModel() { id = "IND",name="India" } };
            result.ownerTypesOptions = await this.generalService.GetLookUp("PT");
            result.ownerCountryOptions = await this.generalService.GetCountries();

            if (result == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return result;
        }

        public async Task<List<CodeNamePairModel>> GetStateByCountry(string countryCode)
        {
            List<CodeNamePairModel> stateList = new List<CodeNamePairModel>();

            stateList = await this.generalService.GetStateByCountry(countryCode);

            return stateList;
        }


        public async Task<List<CodeNamePairModel>> GetDistricts(string countryCode, string stateCode)
        {
            List<CodeNamePairModel> stateList = new List<CodeNamePairModel>();

            stateList = await this.generalService.GetDistricts(countryCode, stateCode);

            return stateList;
        }

        public async Task<List<CodeNamePairModel>> GetCities(string countryCode, string stateCode,string districtCode)
        {
            List<CodeNamePairModel> stateList = new List<CodeNamePairModel>();

            int distCode;

            Int32.TryParse(districtCode,out distCode);

            stateList = await this.generalService.GetCities(countryCode, stateCode,distCode);

            return stateList;
        }

        public async Task<List<CodeNamePairModel>> GetVehicleNoByState(string stateCode)
        {
            List<CodeNamePairModel> stateList = new List<CodeNamePairModel>();

            stateList = await this.generalService.GetVehicleNoByState(stateCode);

            return stateList;
        }
               
        

    }
}
