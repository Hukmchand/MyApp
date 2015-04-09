using AntiTheftClub.DbModel;
using AntiTheftClub.Helphers;
using AntiTheftClub.Models;
using AntiTheftClub.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace AntiTheftClub.Controllers
{
    public class TwoWheelerController : ApiController
    {
        private DbEntities db;
        private PreRegisterationService vehicleService;
        private AccountServices accountServices;
        private GeneralService generalService;
        private EmailManager emailManager;
        NewVehicleValidator validator;

        public TwoWheelerController()
        {
            this.db = new DbEntities();
            this.vehicleService = new PreRegisterationService(db);
            this.accountServices = new AccountServices(db);
            this.generalService = new GeneralService(db);
            this.emailManager = new EmailManager();
            validator = new NewVehicleValidator();
        }

        // GET api/twowheeler
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/twowheeler/5
        public string Get(int id)
        {
            return "value";
        }

        [ResponseType(typeof(ResponseModel))]
        public IHttpActionResult Post(TwoWheelerModel twoWheelerData)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            twoWheelerData.SubCategoryId = 1;
            twoWheelerData.CategoryId = 1;
            twoWheelerData.SubCategoryName = "Two Wheeler";
            twoWheelerData.CategoryName = "Vehicle";

            ResponseModel model = new ResponseModel();
            model.IsSuccess = true;
            twoWheelerData.AccountId = accountServices.GetAccountIdByEmail(this.User.Identity.Name);
            twoWheelerData.FullName = accountServices.GetFullNameByEmail(this.User.Identity.Name);
            var newVehicleResult = this.vehicleService.CheckNewVehicle(twoWheelerData);

            model.Message = validator.CheckNewVehicle(newVehicleResult, twoWheelerData.AccountId);



            if (model.Message == MessageCodes.NewDuplicate)
            {
                if (twoWheelerData.IsDataFilled == false)
                {
                    FillData(twoWheelerData);
                }
                emailManager.sendmailNewvehexists(newVehicleResult.Tables[0].Rows[0]["result"].ToString().Trim(), newVehicleResult.Tables[0].Rows[0]["ownername"].ToString().Trim(), newVehicleResult.Tables[0].Rows[0]["Email"].ToString().Trim(), newVehicleResult.Tables[0].Rows[0]["OwnerTypeDesc"].ToString().Trim(), newVehicleResult.Tables[0].Rows[0]["SubCategoryDesc"].ToString().Trim(), newVehicleResult.Tables[0].Rows[0]["MakeDesc"].ToString().Trim(), newVehicleResult.Tables[0].Rows[0]["ModelVariant"].ToString().Trim(), newVehicleResult.Tables[0].Rows[0]["VehicleState"].ToString().Trim(), newVehicleResult.Tables[0].Rows[0]["VehicleCode"].ToString().Trim(), newVehicleResult.Tables[0].Rows[0]["VehicleNo"].ToString().Trim(), newVehicleResult.Tables[0].Rows[0]["ChasisNo"].ToString().Trim(), newVehicleResult.Tables[0].Rows[0]["EngineNo"].ToString().Trim(), twoWheelerData);
            }


            if (string.IsNullOrEmpty(model.Message) || model.Message == MessageCodes.NewDuplicate)
            {
                if (model.Message == MessageCodes.NewDuplicate)
                {
                    twoWheelerData.SuspendFlag = "N";
                    twoWheelerData.IsNewDuplicate = true;
                }
                if (twoWheelerData.IsDataFilled == false)
                {
                    FillData(twoWheelerData);
                }
                var result = this.vehicleService.StoreTwoWheeler(twoWheelerData);

                model.IsSuccess = this.validator.ConfirmVehicleReg(result);

                var dschkNewDup = this.vehicleService.CheckNewRegDuplicate(twoWheelerData);

                var vCode = dschkNewDup.Tables[0].Rows[0]["result"].ToString().Trim();

                string emailTypeToSend = this.validator.ValidateNewDuplicate(vCode);

                if (emailTypeToSend == MessageCodes.VehicleFreeWithCode)
                {
                    this.emailManager.sendmailVehicleFree(vCode, twoWheelerData);
                }
                if (emailTypeToSend == MessageCodes.VehicleFree)
                {
                    this.emailManager.sendmailVehicleFree(string.Empty, twoWheelerData);
                }

                if (emailTypeToSend == MessageCodes.StopDuplicate)
                {
                    twoWheelerData.SuspendFlag = "S";
                    this.emailManager.sendmailvehexists(dschkNewDup.Tables[0].Rows[0]["ownername"].ToString().Trim(), dschkNewDup.Tables[0].Rows[0]["Email"].ToString().Trim(), dschkNewDup.Tables[0].Rows[0]["OwnerTypeDesc"].ToString().Trim(), dschkNewDup.Tables[0].Rows[0]["SubCategoryDesc"].ToString().Trim(), dschkNewDup.Tables[0].Rows[0]["MakeDesc"].ToString().Trim(), dschkNewDup.Tables[0].Rows[0]["ModelVariant"].ToString().Trim(), dschkNewDup.Tables[0].Rows[0]["VehicleState"].ToString().Trim(), dschkNewDup.Tables[0].Rows[0]["VehicleCode"].ToString().Trim(), dschkNewDup.Tables[0].Rows[0]["VehicleNo"].ToString().Trim(), dschkNewDup.Tables[0].Rows[0]["ChasisNo"].ToString().Trim(), dschkNewDup.Tables[0].Rows[0]["EngineNo"].ToString().Trim(), dschkNewDup.Tables[0].Rows[0]["result"].ToString().Trim(), twoWheelerData);
                    this.emailManager.sendmailVehicleFreeSuspend(twoWheelerData.SuspendFlag, dschkNewDup, twoWheelerData);
                }

                if (model.Message == MessageCodes.NewDuplicate || twoWheelerData.IsNewDuplicate)
                {
                    emailManager.sendmailVehicleFreeSuspend(twoWheelerData.SuspendFlag, newVehicleResult, twoWheelerData);
                }

            }
            else
            {
                model.IsSuccess = false;
                model.Message = string.IsNullOrEmpty(model.Message) ? "Opps Something went wrong..try again later" : model.Message;
            }

            return this.Ok<ResponseModel>(model);
        }


        // PUT api/twowheeler/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/twowheeler/5
        public void Delete(int id)
        {
        }

        private async void FillData(TwoWheelerModel model)
        {
            model.IsDataFilled = true;
            model.OwnerCountryName = await generalService.GetCountryName(model.OwnerCountry);
            model.OwnerStateName = await generalService.GetStateName(model.OwnerCountry, model.OwnerState);
            model.OwnerDistrictName = await generalService.GetDistrictName(model.OwnerCountry, model.OwnerState, model.OwnerDistrict);
            model.OwnerCityName = await generalService.GetCitieName(model.OwnerCountry, model.OwnerState, model.OwnerDistrict, model.OwnerCity);

            model.VehicleCountryName = await generalService.GetCountryName(model.VehicleCountry);
            model.VehicleStateName = await generalService.GetStateName(model.VehicleCountry, model.VehicleState);

            model.VehicleMakeName = await generalService.GetVehicleMakeName(1, model.VehicleMake);

            //model.PurchaseCountryName = await generalService.GetCountryName(model.PurchaseCountry);
            //model.PurchaseStateName = await generalService.GetStateName(model.PurchaseCountry, model.PurchaseState);
            //model.PurchaseDistrictName = await generalService.GetDistrictName(model.PurchaseCountry, model.PurchaseState, model.PurchaseDistrict);
            //model.PurchaseCityName = await generalService.GetCitieName(model.PurchaseCountry, model.PurchaseState, model.PurchaseDistrict, model.PurchaseCity);


            //model.PreviousOwnerCountryName = await generalService.GetCountryName(model.PreviousOwnerCountry);
            //model.PreviousOwnerStateName = await generalService.GetStateName(model.PreviousOwnerCountry, model.PreviousOwnerState);
            //model.PreviousOwnerDistrictName = await generalService.GetDistrictName(model.PreviousOwnerCountry, model.PreviousOwnerState, model.PreviousOwnerDistrict);
            //model.PreviousOwnerCityName = await generalService.GetCitieName(model.PreviousOwnerCountry, model.PreviousOwnerState, model.PreviousOwnerDistrict, model.PreviousOwnerCity);



        }
    }
}

