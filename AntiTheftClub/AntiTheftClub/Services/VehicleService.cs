using AntiTheftClub.DbModel;
using AntiTheftClub.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AntiTheftClub.Services
{
    public class PreRegisterationService
    {
        private DbEntities db;

        public PreRegisterationService(DbEntities db)
        {
            this.db = db;
        }

        public DataSet StoreTwoWheeler(TwoWheelerModel model)
        {
            SetBaseVales(model);

            return InsertNewRegister(
                model.AccountId,
                model.CategoryId,
                model.SubCategoryId,
                model.VehicleCountry,
                model.VehicleState,
                model.VehicleNoState,
                model.VehicleNoArea,
                model.VehicleNo.ToString(),
                model.ChassisNo,
                model.EngineNo,
                model.VehicleMake,
                model.ModelVariant,
                model.VehicleColor,
                model.ManufactureYear,
                model.ManufactureMonth,
                model.RegistrationDate,
                model.MobileMake,
                model.MobileModel,
                model.MobileColor,
                model.IMENo,
                model.IMENo2,
                model.CellNo,
                model.SimNo,
                model.OtherMake,
                model.OtherModel,
                model.OtherProductSerial,
                model.OtherHDDSerial,
                model.OtherMotherSerial,
                model.ProcessorSerial,
                model.MonitorSerial,
                model.MotherMakeId, model.ShopName, model.PurchaseCountry,
            model.PurchaseState, model.PurchaseDistrict, model.PurchaseCity, model.PurchaseAddress, model.PurchaseDate, model.PurchaseInvoiceNo,
            model.PurchaseComments, model.OwnerName, model.OwnerType, model.OwnerCountry, model.OwnerState, model.OwnerDistrict,
            model.OwnerCity, model.OwnerAddress, model.OwnerPincode, model.OwnerMobile, model.OwnerAlternativeMobile, model.OwnerEmail, model.PreviousOwnerName,
            model.PreviousOwnerCountry, model.PreviousOwnerState, model.PreviousOwnerDistrict, model.PreviousOwnerCity, model.PreviousOwnerAddress, model.InsuranceCompanyName,
            model.InsurancePolicyNo, model.InsuranceStartDate, model.InsuranceEndDate, model.PurchaseComments,
            model.VehiclePhotoName, model.RcCopyFrontPhotoName, model.OwnerPhoto, model.RcCopyBackPhotoName, model.Updated_by,
            model.Others, model.IPAddress, model.SuspendFlag);

        }

        private void SetBaseVales(TwoWheelerModel model)
        {
            model.MobileMake = 0;
            model.MobileModel = string.Empty;
            model.MobileColor = string.Empty;
            model.IMENo = string.Empty;
            model.IMENo2 = string.Empty;
            model.CellNo = string.Empty;
            model.SimNo = string.Empty;
            model.OtherMake = 0;
            model.MobileModel = string.Empty;
            model.MobileModel = string.Empty;
            model.OtherModel = string.Empty;
            model.OtherProductSerial = string.Empty;
            model.OtherHDDSerial = string.Empty;
            model.OtherMotherSerial = string.Empty;
            model.ProcessorSerial = string.Empty;
            model.MonitorSerial = string.Empty;
            model.MotherMakeId = string.Empty;
            model.VehiclePhotoName = string.Empty;
            model.RcCopyFrontPhotoName = string.Empty;
            model.OwnerPhoto = string.Empty;
            model.RcCopyBackPhotoName = string.Empty;
            model.Updated_by = string.Empty;
            model.Others = string.Empty;
            model.IPAddress = string.Empty;
            model.VehicleColor = model.VehicleColor ?? string.Empty;
            model.VehicleNoArea = model.VehicleNoArea ?? string.Empty;
            model.RegistrationDate = model.RegistrationDate.HasValue ? model.RegistrationDate.Value : DateTime.Parse("1900-01-01");
            model.OwnerAlternativeMobile = model.OwnerAlternativeMobile ?? string.Empty;
            model.InsuranceCompanyName = model.InsuranceCompanyName ?? string.Empty;
            model.InsurancePolicyNo = model.InsurancePolicyNo ?? string.Empty;
            model.InsuranceStartDate = model.InsuranceStartDate.HasValue ? model.InsuranceStartDate.Value : DateTime.Parse("1900-01-01");
            model.InsuranceEndDate = model.InsuranceEndDate.HasValue ? model.InsuranceEndDate.Value : DateTime.Parse("1900-01-01");
            model.InsuranceComments = model.InsuranceComments ?? string.Empty;
            model.ShopName = model.ShopName ?? string.Empty;
            model.PurchaseCountry = model.PurchaseCountry ?? string.Empty;
            model.PurchaseDate = model.PurchaseDate.HasValue ? model.PurchaseDate.Value : DateTime.Parse("1900-01-01");
            model.PurchaseState = model.PurchaseState ?? string.Empty;
            model.PurchaseDistrict = model.PurchaseDistrict.HasValue ? model.PurchaseDistrict.Value : 0;
            model.PurchaseCity = model.PurchaseCity.HasValue ? model.PurchaseCity.Value : 0;
            model.PurchaseAddress = model.PurchaseAddress ?? string.Empty;
            model.PurchaseInvoiceNo = model.PurchaseInvoiceNo ?? string.Empty;
            model.PurchaseComments = model.PurchaseComments ?? string.Empty;

              model.PreviousOwnerName = string.Empty;
            model.PreviousOwnerCountry = string.Empty;
            model.PreviousOwnerState = string.Empty;
            model.PreviousOwnerDistrict = model.PreviousOwnerDistrict.HasValue ? model.PreviousOwnerDistrict:0;
            model.PreviousOwnerCity = model.PreviousOwnerCity.HasValue ? model.PreviousOwnerCity : 0;
            model.PreviousOwnerAddress = string.Empty;
            model.SuspendFlag = model.SuspendFlag ?? string.Empty;
          
        }


        public DataSet CheckNewRegDuplicate(TwoWheelerModel model)
        {
            SetBaseVales(model);


            return CheckNewDuplicate(
                model.AccountId,
                model.CategoryId,
                model.SubCategoryId,
                model.VehicleCountry,
                model.VehicleState,
                model.VehicleNoState,
                model.VehicleNoArea,
                model.VehicleNo.ToString(),
                model.ChassisNo,
                model.EngineNo,
                model.VehicleMake,
                model.ModelVariant,
                model.VehicleColor,
                model.ManufactureYear,
                model.ManufactureMonth,
                model.RegistrationDate,
                model.MobileMake,
                model.MobileModel,
                model.MobileColor,
                model.IMENo,
                model.IMENo2,
                model.CellNo,
                model.SimNo,
                model.OtherMake,
                model.OtherModel,
                model.OtherProductSerial,
                model.OtherHDDSerial,
                model.OtherMotherSerial,
                model.ProcessorSerial,
                model.MonitorSerial,
                model.MotherMakeId,
                model.ShopName,
                model.PurchaseCountry,
            model.PurchaseState,
            model.PurchaseDistrict,
            model.PurchaseCity,
            model.PurchaseAddress,
            model.PurchaseDate,
            model.PurchaseInvoiceNo,
            model.PurchaseComments,
            model.OwnerName,
            model.OwnerType,
            model.OwnerCountry,
            model.OwnerState,
            model.OwnerDistrict,
            model.OwnerCity,
            model.OwnerAddress,
            model.OwnerPincode,
            model.OwnerMobile,
            model.OwnerAlternativeMobile,
            model.OwnerEmail,
            model.PreviousOwnerName,
            model.PreviousOwnerCountry,
            model.PreviousOwnerState,
            model.PreviousOwnerDistrict,
            model.PreviousOwnerCity,
            model.PreviousOwnerAddress,
            model.InsuranceCompanyName,
            model.InsurancePolicyNo,
            model.InsuranceStartDate,
            model.InsuranceEndDate,
            model.PurchaseComments,
            model.VehiclePhotoName,
            model.RcCopyFrontPhotoName,
            model.OwnerPhoto,
            model.RcCopyBackPhotoName,
            model.Updated_by,
            model.Others,
            model.IPAddress,
            model.SuspendFlag);

        }

        public DataSet CheckNewVehicle(TwoWheelerModel model)
        {

            SqlManager sqlManager = new SqlManager();

            DataSet ds = sqlManager.ExecuteStoredProcedure("sp_Check_Vehicle_Details",
            new SqlParameter("@Categoryid", model.CategoryId),
            new SqlParameter("@SubCategoryid", model.SubCategoryId),
            new SqlParameter("@VehicleState", model.VehicleNoState),
            new SqlParameter("@VehicleCode", model.VehicleNoArea),
            new SqlParameter("@VehicleNo", model.VehicleNo),
            new SqlParameter("@ChasisNo", model.ChassisNo),
            new SqlParameter("@EngineNo", model.EngineNo));
            return ds;



        }


        private DataSet InsertNewRegister(string UserID, int? Categoryid, int? SubCategoryid, string vcountrycd, string vstatecd,
            string VehicleState, string VehicleCode, string VehicleNo, string ChasisNo, string EngineNo, int? VehicleMake, string ModelVariant, string VehicleColor,
            string VehicleManuYear, string VehicleManuMonth, DateTime? RegisterDate, int? MobileMake, string MobileModel, string MobileColor, string IMENo,
            string IMENo2, string CellNo, string SimNo, int? OtherMake, string OtherModel, string OtherProductSerial, string OtherHDDSerial,
            string OtherMotherSerial, string ProcessorSerial, string MonitorSerial, string Makeid, string ShopName, string scountrycd,
            string sstatecd, int? sdistrictid, int? scityid, string ShopAddress, DateTime? PurchaseDate, string InvoiceNo,
            string OtherDetails, string OwnerName, string OwnerType, string ocountrycd, string ostatecd, int? odistrictid,
            int? ocityid, string Address, string Pincode, string PhoneNo, string AltMobile, string Emailid, string RCownername,
            string RCocountry, string RCostate, int? RCodistrict, int? RCocity, string RCaddress, string InsCompanyName,
            string InsPolicyNumber, DateTime? fromPeriodofInsurance, DateTime? toPeriodofInsurance, string AnyOther,
            string vehiclephoto, string rcphoto, string ownerphoto, string rcphotoback, string Updated_by,
            string others, string ipaddress, string suspendFlag)
        {
            SqlManager sqlManager = new SqlManager();
            DataSet ds = sqlManager.ExecuteStoredProcedure("sp_NewRegister",
             new SqlParameter("@Userid", UserID),
             new SqlParameter("@Categoryid", Categoryid),
             new SqlParameter("@SubCategoryid", SubCategoryid),
             new SqlParameter("@vcountrycd", vcountrycd),
             new SqlParameter("@vstatecd", vstatecd),
                //new SqlParameter("@vdistrictid", vdistrictid),
                //new SqlParameter("@vcityid", vcityid),
             new SqlParameter("@VehicleState", VehicleState),
             new SqlParameter("@VehicleCode", VehicleCode),
             new SqlParameter("@VehicleNo", VehicleNo),
             new SqlParameter("@ChasisNo", ChasisNo),
             new SqlParameter("@EngineNo", EngineNo),
             new SqlParameter("@VehicleMake", VehicleMake),
             new SqlParameter("@ModelVariant", ModelVariant),
             new SqlParameter("@VehicleColor", VehicleColor),
             new SqlParameter("@VehicleManuYear", VehicleManuYear),
             new SqlParameter("@VehicleManuMonth", VehicleManuMonth),
             new SqlParameter("@RegisterDate", RegisterDate),
             new SqlParameter("@MobileMake", MobileMake),
             new SqlParameter("@MobileModel", MobileModel),
             new SqlParameter("@MobileColor", MobileColor),
             new SqlParameter("@IMENo", IMENo),
             new SqlParameter("@IMENo2", IMENo2),
             new SqlParameter("@CellNo", CellNo),
             new SqlParameter("@SimNo", SimNo),
             new SqlParameter("@OtherMake", OtherMake),
             new SqlParameter("@OtherModel", OtherModel),
             new SqlParameter("@OtherProductSerial", OtherProductSerial),
             new SqlParameter("@OtherHDDSerial", OtherHDDSerial),
             new SqlParameter("@OtherMotherSerial", OtherMotherSerial),
             new SqlParameter("@ProcessorSerial", ProcessorSerial),
             new SqlParameter("@MonitorSerial", MonitorSerial),
             new SqlParameter("@Makeid", Makeid),
             new SqlParameter("@ShopName", ShopName),
             new SqlParameter("@scountrycd", scountrycd),
             new SqlParameter("@sstatecd", sstatecd),
             new SqlParameter("@sdistrictid", sdistrictid),
             new SqlParameter("@scityid", scityid),
             new SqlParameter("@ShopAddress", ShopAddress),
             new SqlParameter("@PurchaseDate", PurchaseDate),
             new SqlParameter("@InvoiceNo", InvoiceNo),
             new SqlParameter("@OtherDetails", OtherDetails),
             new SqlParameter("@OwnerName", OwnerName),
             new SqlParameter("@OwnerType", OwnerType),
             new SqlParameter("@ocountrycd", ocountrycd),
             new SqlParameter("@ostatecd", ostatecd),
             new SqlParameter("@odistrictid", odistrictid),
             new SqlParameter("@ocityid", ocityid),
             new SqlParameter("@Address", Address),
             new SqlParameter("@Pincode", Pincode),
             new SqlParameter("@PhoneNo", PhoneNo),
             new SqlParameter("@AltMobile", AltMobile),
             new SqlParameter("@Emailid", Emailid),
             new SqlParameter("@RCownername", RCownername),
             new SqlParameter("@RCocountry", RCocountry),
             new SqlParameter("@RCostate", RCostate),
             new SqlParameter("@RCodistrict", RCodistrict),
             new SqlParameter("@RCocity", RCocity),
             new SqlParameter("@RCaddress", RCaddress),
             new SqlParameter("@InsCompanyName", InsCompanyName),
             new SqlParameter("@InsPolicyNumber", InsPolicyNumber),
             new SqlParameter("@fromPeriodofIns", fromPeriodofInsurance),
             new SqlParameter("@toPeriodofIns", toPeriodofInsurance),
             new SqlParameter("@AnyOther", AnyOther),
             new SqlParameter("@vehiclephoto", vehiclephoto),
             new SqlParameter("@rcphoto", rcphoto),
             new SqlParameter("@ownerphoto", ownerphoto),
             new SqlParameter("@rcphotoback", rcphotoback),
             new SqlParameter("@Updated_by", Updated_by),
             new SqlParameter("@others", others),
             new SqlParameter("@ipaddress", ipaddress),
             new SqlParameter("@suspendFlag", suspendFlag)
             );
            return ds;


        }


        private DataSet CheckNewDuplicate(string UserID, int? Categoryid, int? SubCategoryid, string vcountrycd, string vstatecd,
            string VehicleState, string VehicleCode, string VehicleNo, string ChasisNo, string EngineNo, int? VehicleMake, string ModelVariant, string VehicleColor,
            string VehicleManuYear, string VehicleManuMonth, DateTime? RegisterDate, int? MobileMake, string MobileModel, string MobileColor, string IMENo,
            string IMENo2, string CellNo, string SimNo, int? OtherMake, string OtherModel, string OtherProductSerial, string OtherHDDSerial,
            string OtherMotherSerial, string ProcessorSerial, string MonitorSerial, string Makeid, string ShopName, string scountrycd,
            string sstatecd, int? sdistrictid, int? scityid, string ShopAddress, DateTime? PurchaseDate, string InvoiceNo,
            string OtherDetails, string OwnerName, string OwnerType, string ocountrycd, string ostatecd, int? odistrictid,
            int? ocityid, string Address, string Pincode, string PhoneNo, string AltMobile, string Emailid, string RCownername,
            string RCocountry, string RCostate, int? RCodistrict, int? RCocity, string RCaddress, string InsCompanyName,
            string InsPolicyNumber, DateTime? fromPeriodofInsurance, DateTime? toPeriodofInsurance, string AnyOther,
            string vehiclephoto, string rcphoto, string ownerphoto, string rcphotoback, string Updated_by,
            string others, string ipaddress, string suspendFlag)
        {
            SqlManager sqlManager = new SqlManager();
            DataSet ds = sqlManager.ExecuteStoredProcedure("sp_Check_New_Details_dup",
            new SqlParameter("@Userid", UserID),
            new SqlParameter("@Categoryid", Categoryid),
            new SqlParameter("@SubCategoryid", SubCategoryid),
            new SqlParameter("@vcountrycd", vcountrycd),
            new SqlParameter("@vstatecd", vstatecd),
            new SqlParameter("@VehicleState", VehicleState),
            new SqlParameter("@VehicleCode", VehicleCode),
            new SqlParameter("@VehicleNo", VehicleNo),
            new SqlParameter("@ChasisNo", ChasisNo),
            new SqlParameter("@EngineNo", EngineNo),
            new SqlParameter("@VehicleMake", VehicleMake),
            new SqlParameter("@ModelVariant", ModelVariant),
            new SqlParameter("@VehicleColor", VehicleColor),
            new SqlParameter("@VehicleManuYear", VehicleManuYear),
            new SqlParameter("@VehicleManuMonth", VehicleManuMonth),
            new SqlParameter("@RegisterDate", RegisterDate),
            new SqlParameter("@MobileMake", MobileMake),
            new SqlParameter("@MobileModel", MobileModel),
            new SqlParameter("@MobileColor", MobileColor),
            new SqlParameter("@IMENo", IMENo),
            new SqlParameter("@IMENo2", IMENo),
            new SqlParameter("@CellNo", CellNo),
            new SqlParameter("@SimNo", SimNo),
            new SqlParameter("@OtherMake", OtherMake),
            new SqlParameter("@OtherModel", OtherModel),
            new SqlParameter("@OtherProductSerial", OtherProductSerial),
            new SqlParameter("@OtherHDDSerial", OtherHDDSerial),
            new SqlParameter("@OtherMotherSerial", OtherMotherSerial),
            new SqlParameter("@ProcessorSerial", ProcessorSerial),
            new SqlParameter("@Makeid", Makeid),
            new SqlParameter("@MonitorSerial", MonitorSerial),
            new SqlParameter("@ShopName", ShopName),
            new SqlParameter("@scountrycd", scountrycd),
            new SqlParameter("@sstatecd", sstatecd),
            new SqlParameter("@sdistrictid", sdistrictid),
            new SqlParameter("@scityid", scityid),
            new SqlParameter("@ShopAddress", ShopAddress),
            new SqlParameter("@PurchaseDate", PurchaseDate),
            new SqlParameter("@InvoiceNo", InvoiceNo),
            new SqlParameter("@OtherDetails", OtherDetails),
            new SqlParameter("@OwnerName", OwnerName),
            new SqlParameter("@OwnerType", OwnerType),
            new SqlParameter("@ocountrycd", ocountrycd),
            new SqlParameter("@ostatecd", ostatecd),
            new SqlParameter("@odistrictid", odistrictid),
            new SqlParameter("@ocityid", ocityid),
            new SqlParameter("@Address", Address),
            new SqlParameter("@Pincode", Pincode),
            new SqlParameter("@PhoneNo", PhoneNo),
            new SqlParameter("@AltMobile", AltMobile),
            new SqlParameter("@Emailid", Emailid),
            new SqlParameter("@RCownername", RCownername),
            new SqlParameter("@RCocountry", RCocountry),
            new SqlParameter("@RCostate", RCostate),
            new SqlParameter("@RCodistrict", RCodistrict),
            new SqlParameter("@RCocity", RCocity),
            new SqlParameter("@RCaddress", RCaddress),
            new SqlParameter("@InsCompanyName", InsCompanyName),
            new SqlParameter("@InsPolicyNumber", InsPolicyNumber),
            new SqlParameter("@fromPeriodofIns", fromPeriodofInsurance),
            new SqlParameter("@toPeriodofIns", toPeriodofInsurance),
            new SqlParameter("@AnyOther", AnyOther),
            new SqlParameter("@vehiclephoto", vehiclephoto),
            new SqlParameter("@rcphoto", rcphoto),
            new SqlParameter("@ownerphoto", ownerphoto),
            new SqlParameter("@rcphotoback", rcphotoback),
            new SqlParameter("@Updated_by", Updated_by),
            new SqlParameter("@ipaddress", ipaddress));
            return ds;


        }






    }




}