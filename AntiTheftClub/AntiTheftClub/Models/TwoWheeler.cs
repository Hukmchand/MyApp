using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AntiTheftClub.Models
{
    public class TwoWheelerModel
    {
        public int VehicleMake { get; set; }
        public string VehicleMakeName { get; set; }
        public string ModelVariant { get; set; }
        public string VehicleColor { get; set; }
        public string VehicleCountry { get; set; }
        public string VehicleState { get; set; }
        public string VehicleNoState { get; set; }
        public string VehicleNoArea { get; set; }
        public int VehicleNo { get; set; }
        public string ChassisNo { get; set; }
        public string EngineNo { get; set; }
        public string ManufactureMonth { get; set; }
        public string ManufactureYear { get; set; }
        public DateTime? RegistrationDate { get; set; }


        public string OwnerName { get; set; }
        public string OwnerType { get; set; }
        public string OwnerCountry { get; set; }
        public string OwnerState { get; set; }
        public int? OwnerCity { get; set; }
        public int? OwnerDistrict { get; set; }
        public string OwnerDistrictName { get; set; }
        public string OwnerPincode { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerMobile { get; set; }
        public string OwnerAlternativeMobile { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerPhoto { get; set; }
        public string OwnerCityName { get; set; }
        public string OwnerStateName { get; set; }
        public string OwnerCountryName { get; set; }

        public string PreviousOwnerName { get; set; }
        public string PreviousOwnerCountry { get; set; }
        public string PreviousOwnerState { get; set; }
        public int? PreviousOwnerCity { get; set; }
        public int? PreviousOwnerDistrict { get; set; }
        public string PreviousOwnerAddress { get; set; }

        public string ShopName { get; set; }
        public string PurchaseCountry { get; set; }
        public string PurchaseState { get; set; }
        public int? PurchaseDistrict { get; set; }
        public int? PurchaseCity { get; set; }
        public string PurchaseAddress { get; set; }
        public string PurchaseInvoiceNo { get; set; }
        public string PurchaseComments { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public int? SubCategoryId { get; set; }
        public int? CategoryId { get; set; }

        public string IPAddress { get; set; }
        public string SuspendFlag { get; set; }
       
        public string AccountId { get; set; }

        public int? MobileMake { get; set; }

        public string MobileModel { get; set; }

        public string MobileColor { get; set; }

        public string IMENo { get; set; }

        public string IMENo2 { get; set; }

        public string CellNo { get; set; }

        public string SimNo { get; set; }

        public int? OtherMake { get; set; }

        public string OtherModel { get; set; }

        public string OtherProductSerial { get; set; }

        public string OtherHDDSerial { get; set; }

        public string OtherMotherSerial { get; set; }

        public string ProcessorSerial { get; set; }

        public string MonitorSerial { get; set; }

        public string Others { get; set; }

        public string Updated_by { get; set; }

        public string RcCopyBackPhotoName { get; set; }

        public string RcCopyFrontPhotoName { get; set; }

        public string VehiclePhotoName { get; set; }


        public DateTime? InsuranceEndDate { get; set; }

        public DateTime? InsuranceStartDate { get; set; }

        public string InsurancePolicyNo { get; set; }

        public string InsuranceCompanyName { get; set; }
        public string InsuranceComments { get; set; }
        
        public string MotherMakeId { get; set; }

        public string FullName { get; set; }

        public string SubCategoryName { get; set; }

        public string CategoryName { get; set; }

        public bool IsNewDuplicate { get; set; }

        public string PreviousOwnerCountryName { get; set; }

        public string PreviousOwnerStateName { get; set; }

        public string PreviousOwnerDistrictName { get; set; }

        public string PreviousOwnerCityName { get; set; }

        public string VehicleStateName { get; set; }

        public string VehicleCountryName { get; set; }

        public string PurchaseCountryName { get; set; }

        public string PurchaseStateName { get; set; }

        public string PurchaseDistrictName { get; set; }

        public string PurchaseCityName { get; set; }

        public bool IsDataFilled { get; set; }
    }

    public class ResponseModel
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class TwoWheelerOptions
    {
        public List<KeyValuePairModel> vehicleMakeOptions { get; set; }
        public List<CodeNamePairModel> manufactureYearOptions { get; set; }
        public List<CodeNamePairModel> countryOptions { get; set; }
        public List<CodeNamePairModel> ownerTypesOptions { get; set; }
        public List<CodeNamePairModel> ownerCountryOptions { get; set; }
    }
}