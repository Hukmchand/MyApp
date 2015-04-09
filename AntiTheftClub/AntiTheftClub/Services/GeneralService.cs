using AntiTheftClub.DbModel;
using AntiTheftClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using AntiTheftClub.Helphers;

namespace AntiTheftClub.Services
{
    public class GeneralService
    {
        private DbEntities db;
        MemoryCacher memoryCacher;



        public GeneralService(DbEntities db)
        {
            this.db = db;
            memoryCacher = new MemoryCacher();
        }

        public async Task<List<KeyValuePairModel>> GetVehicleMake(int? vehicleType)
        {
            string VcMakeToken = string.Format("{0}-{1}", "VcMakeToken", vehicleType.HasValue ? vehicleType.Value.ToString() : string.Empty);
            List<KeyValuePairModel> result = memoryCacher.GetValue(VcMakeToken) as List<KeyValuePairModel>;

            if (result == null)
            {
                var makeList = await this.db.sp_GetVehicleMake(vehicleType).ToListAsync();

                result = makeList.Select(ed => new KeyValuePairModel()
                            {
                                id = ed.Makeid,
                                name = ed.MakeDesc
                            }).ToList();

                memoryCacher.Add(VcMakeToken, result);
            }

            return result;

        }




        public async Task<string> GetVehicleMakeName(int? vehicleType, int makeId)
        {
            var makeList = await this.db.sp_GetVehicleMake(vehicleType).ToListAsync();

            var r = makeList.FirstOrDefault(ed => ed.Makeid == makeId);
            if (r != null)
            {
                return r.MakeDesc;
            }
            else

                return string.Empty;

        }

        internal async Task<List<CodeNamePairModel>> GetCountries()
        {
            string countryToken = "countryToken";
            List<CodeNamePairModel> result = memoryCacher.GetValue(countryToken) as List<CodeNamePairModel>;

            if (result == null)
            {

                var makeList = await this.db.sp_GetCountry().ToListAsync();

                result = makeList.Select(ed => new CodeNamePairModel()
                           {
                               id = ed.Countrycd,
                               name = ed.CountryName
                           }).ToList();
                memoryCacher.Add(countryToken, result);
            }

            return result;
        }

        internal async Task<string> GetCountryName(string countryCode)
        {
            var makeList = await this.db.sp_GetCountry().ToListAsync();

            var r = makeList.FirstOrDefault(ed => ed.Countrycd == countryCode);
            if (r != null)
            {
                return r.CountryName;
            }
            else

                return string.Empty;
        }

        internal async Task<List<CodeNamePairModel>> GetStateByCountry(string countryCode)
        {
            string VcMakeToken = string.Format("{0}-{1}", "GetStateByCountry", countryCode);
            List<CodeNamePairModel> result = memoryCacher.GetValue(VcMakeToken) as List<CodeNamePairModel>;

            if (result == null)
            {
                var makeList = await this.db.sp_GetStateFilter(countryCode).ToListAsync();

                result = makeList.Select(ed => new CodeNamePairModel()
                           {
                               id = ed.Statecd,
                               name = ed.StateName
                           }).ToList();
                memoryCacher.Add(VcMakeToken, result);
            }

            return result;
        }

        internal async Task<string> GetStateName(string countryCode, string stateCode)
        {
            var makeList = await this.db.sp_GetStateFilter(countryCode).ToListAsync();

            var r = makeList.FirstOrDefault(ed => ed.Statecd == stateCode);

            if (r != null)
            {
                return r.StateName;
            }
            else
            {
                return string.Empty;
            }
        }

        internal async Task<List<CodeNamePairModel>> GetDistricts(string countryCode, string stateCode)
        {
            string VcMakeToken = string.Format("{0}-{1}-{2}", "GetDistricts", countryCode, stateCode);
            List<CodeNamePairModel> result = memoryCacher.GetValue(VcMakeToken) as List<CodeNamePairModel>;

            if (result == null)
            {
                var makeList = await this.db.sp_GetDistrictFilter(countryCode, stateCode).ToListAsync();

                result = makeList.Select(ed => new CodeNamePairModel()
                {
                    id = ed.Districtid,
                    name = ed.DistrictName
                }).ToList();
            }

            return result;
        }

        internal async Task<string> GetDistrictName(string countryCode, string stateCode, int? districtCode)
        {

            var makeList = await this.db.sp_GetDistrictFilter(countryCode, stateCode).ToListAsync();

            string v = districtCode.HasValue ? districtCode.Value.ToString() : string.Empty;

            var r = makeList.FirstOrDefault(ed => ed.Districtid == v);

            if (r != null)
            {
                return r.DistrictName;
            }
            else
            {
                return string.Empty;
            }
        }

        internal async Task<List<CodeNamePairModel>> GetVehicleNoByState(string stateCode)
        {
            string VcMakeToken = string.Format("{0}-{1}", "GetVehicleNoByState", stateCode);
            List<CodeNamePairModel> result = memoryCacher.GetValue(VcMakeToken) as List<CodeNamePairModel>;

            if (result == null)
            {

                var makeList = await this.db.sp_GetVehicleNo(stateCode).ToListAsync();

                result = makeList.Select(ed => new CodeNamePairModel()
               {
                   id = ed,
                   name = ed
               }).ToList();

                result.Insert(0, new CodeNamePairModel() { id = "FOR REGN", name = "FOR REGN" });

            }

            return result;

        }

        internal async Task<List<CodeNamePairModel>> GetLookUp(string p)
        {
            string VcMakeToken = string.Format("{0}-{1}", "GetLookUp", p);
            List<CodeNamePairModel> result = memoryCacher.GetValue(VcMakeToken) as List<CodeNamePairModel>;

            if (result == null)
            {
                var makeList = await this.db.sp_Lookup(p).ToListAsync();

                result = makeList.Select(ed => new CodeNamePairModel()
                {
                    id = ed.Codeval,
                    name = ed.CodeDesc
                }).ToList();
            }

            return result;

        }

        internal async Task<List<CodeNamePairModel>> GetCities(string countryCode, string stateCode, int? districtCode)
        {
            string VcMakeToken = string.Format("{0}-{1}-{2}-{3}", "GetCities", countryCode, stateCode, districtCode.HasValue ?districtCode.Value.ToString():string.Empty);
            List<CodeNamePairModel> result = memoryCacher.GetValue(VcMakeToken) as List<CodeNamePairModel>;

            if (result == null)
            {
                var makeList = await this.db.sp_GetCityFilter(countryCode, stateCode, districtCode).ToListAsync();

                result = makeList.Select(ed => new CodeNamePairModel()
                {
                    id = ed.Cityid,
                    name = ed.CityName

                }).ToList();
            }
            return result;
        }

        internal async Task<string> GetCitieName(string countryCode, string stateCode, int? districtCode, int? cityCode)
        {
            var makeList = await this.db.sp_GetCityFilter(countryCode, stateCode, districtCode).ToListAsync();

            var v = cityCode.HasValue ? cityCode.Value.ToString() : string.Empty;

            var r = makeList.FirstOrDefault(ed => ed.Cityid == v);

            if (r != null)
            {
                return r.CityName;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}