using AntiTheftClub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace AntiTheftClub.Helphers
{
    public class EmailManager
    {
        EmailService emailService = new EmailService();

     public   void sendmailNewvehexists(string result, string OwnerName, string Email, string OwnerType, string SubCategory, string Make, string ModelVariant, string vehst, string vehcd, string vehno, string chasisno, string engineno, TwoWheelerModel loggedUserData)
        {
            IdentityMessage emailMessage = new IdentityMessage();

            emailMessage.Destination = Email;
            emailMessage.Subject = "Your New Vehicle is again registered as new registration in antitheftclub.com";

            string strBody = "Hi " + OwnerName + " ";
            strBody = strBody + "<br/><br/>Thanks for using antitheftclub.com ";
            strBody = strBody + "<br/><br/>Your : &nbsp;" + OwnerType;
            strBody = strBody + "<br/>Category : &nbsp;" + SubCategory.ToUpper();
            strBody = strBody + "<br/>Make : &nbsp;" + Make.ToUpper();
            strBody = strBody + "<br/>Model : &nbsp;" + ModelVariant.ToUpper();
            if (loggedUserData.SubCategoryId != 5)
            {
                strBody = strBody + "<br/>Vehicle No: &nbsp;" + vehst.ToUpper() + "&nbsp;" + vehcd.ToUpper() + "&nbsp;" + vehno.ToUpper();
            }
            strBody = strBody + "<br/>Chassis No. : &nbsp;" + chasisno.ToUpper();
            strBody = strBody + "<br/>Engine No. : &nbsp;" + engineno.ToUpper();

            strBody = strBody + "<br/>in the same Vehicle,";

            if (result == "1")
            {
                strBody = strBody + "&nbsp; Vehicle number &nbsp;";
            }
            else if (result == "2")
            {
                strBody = strBody + "&nbsp; Chassis number &nbsp;";
            }
            else if (result == "3")
            {
                strBody = strBody + "&nbsp; Engine number &nbsp;";
            }

            strBody = strBody + "is again being register as New Registration by other person";
            strBody = strBody + "<br/>we are not sure if it is you or sometime mistyping data entry may occurred or some mischief has happened with your newly registered product";
            strBody = strBody + "<br/>So we share the details of the same with you.";
            strBody = strBody + "<br/>Our given information may help you.";

            strBody = strBody + "<br/><br/>Details of person who trying to register as New Registration mentioned below. ";

            strBody = strBody + "<br/><br/>Name : &nbsp;" + loggedUserData.OwnerName;
            if (string.IsNullOrEmpty(loggedUserData.OwnerCountryName) == false)
            {
                strBody = strBody + "<br/>Country : &nbsp;" + loggedUserData.OwnerCountryName;
            }
            if (string.IsNullOrEmpty(loggedUserData.OwnerStateName) == false)
            {
                strBody = strBody + "<br/>State : &nbsp;" + loggedUserData.OwnerStateName;
            }
            if (loggedUserData.OwnerDistrict != null && loggedUserData.OwnerDistrictName != "")
            {
                strBody = strBody + "<br/>District  : &nbsp;" + loggedUserData.OwnerDistrictName;
            }
            if (string.IsNullOrEmpty(loggedUserData.OwnerCityName) == false)
            {
                strBody = strBody + "<br/>City : &nbsp;" + loggedUserData.OwnerCityName;
            }
            strBody = strBody + "<br/>Address : &nbsp;" + loggedUserData.OwnerAddress;
            strBody = strBody + "<br/>Mobile  : &nbsp;" + loggedUserData.OwnerMobile;
            strBody = strBody + "<br/>E-mail ID : &nbsp;" + loggedUserData.OwnerEmail;
            strBody = strBody + "<br/>Trying Reg Date : &nbsp;" + DateTime.Now.ToString("dd-MM-yyyy");
            strBody = strBody + "<br/>IP address from where he registered this product : &nbsp;" + loggedUserData.IPAddress;
            strBody = strBody + "<br/><br/> (Location of this IP address can be found on any IP location tracking website)";
            strBody = strBody + "<br/><br/> Regards,";
            strBody = strBody + "<br/>antitheftclub.com Team<br/>";
            emailMessage.Body = strBody;


             emailService.Send(emailMessage);
        }




        public async void sendmailVehicleFree(string result, TwoWheelerModel loggedUserData)
        {
            IdentityMessage emailMessage = new IdentityMessage();

            emailMessage.Destination = loggedUserData.OwnerEmail;


            emailMessage.Subject = "New Registration Activated in antitheftclub.com";

            string strBody = "Hi " + loggedUserData.OwnerName + " ";
            strBody = strBody + "<br/><br/>Thanks for using antitheftclub.com ";

            strBody = strBody + "<br/><br/>Your New : &nbsp;" + loggedUserData.OwnerType;
            strBody = strBody + "<br/>Category : &nbsp;" + loggedUserData.CategoryName;
            strBody = strBody + "<br/>Make : &nbsp;" + loggedUserData.VehicleMakeName;
            strBody = strBody + "<br/>Model : &nbsp;" + loggedUserData.ModelVariant;
            if (result == "")
            {
                if (loggedUserData.SubCategoryId != 5)
                {
                    strBody = strBody + "<br/>Vehicle No.: &nbsp;" + loggedUserData.VehicleNoState + "&nbsp;" + loggedUserData.VehicleNoArea + "&nbsp;" + loggedUserData.VehicleNo.ToString();
                }
                strBody = strBody + "<br/>Chassis No.: &nbsp;" + loggedUserData.ChassisNo;
                strBody = strBody + "<br/>Engine No.: &nbsp;" + loggedUserData.EngineNo;
                strBody = strBody + "<br/>is registered";
            }
            else
            {
                if (loggedUserData.SubCategoryId != 5)
                {
                    strBody = strBody + "<br/>Vehicle No.: &nbsp;" + loggedUserData.VehicleNoState + "&nbsp;" + loggedUserData.VehicleNoArea + "&nbsp;" + loggedUserData.VehicleNo.ToString();
                }
                strBody = strBody + "<br/>Chassis No.: &nbsp;" + loggedUserData.ChassisNo;
                strBody = strBody + "<br/>Engine No.: &nbsp;" + loggedUserData.EngineNo;
                strBody = strBody + "<br/>is registered";
                strBody = strBody + "<br/>In the same Vehicle";
                if (result == "1SS")
                {
                    strBody = strBody + "<br/>Vehicle No.: &nbsp;" + loggedUserData.VehicleNoState + "&nbsp;" + loggedUserData.VehicleNoArea + "&nbsp;" + loggedUserData.VehicleNo.ToString();
                }
                else if (result == "2SS")
                {
                    strBody = strBody + "<br/>Chassis No.: &nbsp;" + loggedUserData.ChassisNo;
                }
                else if (result == "3SS")
                {
                    strBody = strBody + "<br/>Engine No.: &nbsp;" + loggedUserData.EngineNo;
                }
                strBody = strBody + "<br/>is already registered as stolen/lost Registration by you";
                strBody = strBody + "<br/>The stolen/lost Registration has been <b>Deactivated</b>";
            }
            strBody = strBody + "<br/>New Profile Registration Activated Successfully and Live in antitheftclub.com.";
            strBody = strBody + "<br/><br/>Check/verify through New Search Screen.";
            strBody = strBody + "<br/><br/>You may click and check <a href='http://www.antitheftclub.com/NewSearch.aspx'>www.antitheftclub.com</a>";
            strBody = strBody + "<br/><br/>You may login to <a href='http://www.antitheftclub.com'>www.antitheftclub.com</a>";
            strBody = strBody + "<br/><br/> Regards,";
            strBody = strBody + "<br/>antitheftclub.com Team<br/>";
            emailMessage.Body = strBody;


            emailService.Send(emailMessage);
        }


        internal async void sendmailVehicleFreeSuspend(string ActiveFlag, DataSet dssuspend, TwoWheelerModel loggedUserData)
        {

            IdentityMessage emailMessage = new IdentityMessage();

            emailMessage.Destination = loggedUserData.OwnerEmail;
            emailMessage.Subject = "Your new vehicle register activation is suspended in antitheftclub.com";

            string strBody = "Hi " + loggedUserData.FullName + " ";
            strBody = strBody + "<br/><br/>Thanks for using antitheftclub.com ";

            strBody = strBody + "<br/><br/>Your New : &nbsp;" + loggedUserData.OwnerType;
            strBody = strBody + "<br/>Category : &nbsp;" + loggedUserData.SubCategoryName;
            strBody = strBody + "<br/>Make : &nbsp;" + loggedUserData.VehicleMakeName;
            strBody = strBody + "<br/>Model : &nbsp;" + loggedUserData.ModelVariant;
            if (loggedUserData.SubCategoryId != 5)
            {
                strBody = strBody + "<br/>Vehicle No.: &nbsp;" + loggedUserData.VehicleNoState + "&nbsp;" + loggedUserData.VehicleNoArea + "&nbsp;" + loggedUserData.VehicleNo;
            }
            strBody = strBody + "<br/>Chassis No.: &nbsp;" + loggedUserData.ChassisNo;
            strBody = strBody + "<br/>Engine No.: &nbsp;" + loggedUserData.EngineNo;

            strBody = strBody + "<br/>Your new vehicle registration is registered successfully and the activation is suspended as the vehicle records you entered is already registered by someone.";
            strBody = strBody + "<br/>We are not sure if it is you or sometime mistyping data entry may occurred or some mischief has happened with your newly registered product.";


            if (dssuspend != null && dssuspend.Tables != null && dssuspend.Tables[0] != null && dssuspend.Tables[0].Rows.Count > 0)
            {
                strBody = strBody + "<br/>So we share the details of the same with you.";
                strBody = strBody + "<br/>Our given information may help you.";

                if (ActiveFlag == "N")
                {
                    strBody = strBody + "<br/><br/>Details of person who already registered your vehicle as New Registration is mentioned below. ";
                }
                else if (ActiveFlag == "S")
                {
                    strBody = strBody + "<br/><br/>Details of person who already registered your vehicle as Stolen/Lost Registration is mentioned below. ";
                }
                strBody = strBody + "<br/><br/>Name : &nbsp;" + dssuspend.Tables[0].Rows[0]["ownername"].ToString().Trim().ToUpper();
                if (dssuspend.Tables[0].Rows[0]["countryname"].ToString().Trim() != null && dssuspend.Tables[0].Rows[0]["countryname"].ToString().Trim() != "")
                {
                    strBody = strBody + "<br/>Country : &nbsp;" + dssuspend.Tables[0].Rows[0]["countryname"].ToString().Trim().ToUpper();
                }
                if (dssuspend.Tables[0].Rows[0]["statename"].ToString().Trim() != null && dssuspend.Tables[0].Rows[0]["statename"].ToString().Trim() != "")
                {
                    strBody = strBody + "<br/>State : &nbsp;" + dssuspend.Tables[0].Rows[0]["statename"].ToString().Trim().ToUpper();
                }
                if (dssuspend.Tables[0].Rows[0]["districtname"].ToString().Trim() != null && dssuspend.Tables[0].Rows[0]["districtname"].ToString().Trim() != "" && dssuspend.Tables[0].Rows[0]["districtname"].ToString().Trim() != "0")
                {
                    strBody = strBody + "<br/>District  : &nbsp;" + dssuspend.Tables[0].Rows[0]["districtname"].ToString().Trim().ToUpper();
                }
                if (dssuspend.Tables[0].Rows[0]["cityname"].ToString().Trim() != null && dssuspend.Tables[0].Rows[0]["cityname"].ToString().Trim() != "" && dssuspend.Tables[0].Rows[0]["cityname"].ToString().Trim() != "0")
                {
                    strBody = strBody + "<br/>City : &nbsp;" + dssuspend.Tables[0].Rows[0]["cityname"].ToString().Trim().ToUpper();
                }
                strBody = strBody + "<br/>Address : &nbsp;" + dssuspend.Tables[0].Rows[0]["Address"].ToString().Trim().ToUpper();
                strBody = strBody + "<br/>Mobile  : &nbsp;" + dssuspend.Tables[0].Rows[0]["PhoneNo"].ToString().Trim().ToUpper();
                strBody = strBody + "<br/>E-mail ID : &nbsp;" + dssuspend.Tables[0].Rows[0]["Emailid"].ToString().Trim().ToLower();
                strBody = strBody + "<br/>Reg Date : &nbsp;" + Convert.ToDateTime(dssuspend.Tables[0].Rows[0]["Insert_date"]).ToString("dd-MM-yyyy");
                strBody = strBody + "<br/>IP address from where he registered this product : &nbsp;" + dssuspend.Tables[0].Rows[0]["ipaddress"].ToString().Trim();
                strBody = strBody + "<br/>(Location of this IP address can be found on any IP location tracking website) ";
                strBody = strBody + "<br/>With this details you contact him and take further action (Or)";
                strBody = strBody + "<br/>You should show the original records to nearest police station to claim Authenticity of the vehicle.<br/>";
            }
            strBody = strBody + "<br/>Regards,";
            strBody = strBody + "<br/>antitheftclub.com Team<br/>";
            emailMessage.Body = strBody;



            await emailService.SendAsync(emailMessage);
        }

      public   void sendmailvehexists(string ownername, string Email, string OwnerType, string SubCategory, string Make, string ModelVariant, string VehicleState, string VehicleCode, string VehicleNo, string ChasisNo, string EngineNo, string result, TwoWheelerModel loggedUserData)
        {

            IdentityMessage emailMessage = new IdentityMessage();

            emailMessage.Destination = Email;

            emailMessage.Subject = "Your Stolen/Lost Vehicle is again newly registered by other person in antitheftclub.com";

            //string strBody = "Hi " + Session["UserName"].ToString().Trim() + " ";
            string strBody = "Hi " + ownername + " ";
            strBody = strBody + "<br/><br/>Thanks for using antitheftclub.com ";

            strBody = strBody + "<br/><br/>Your : &nbsp;" + OwnerType;
            strBody = strBody + "<br/>Category : &nbsp;" + SubCategory.ToUpper();
            strBody = strBody + "<br/>Make : &nbsp;" + Make.ToUpper();
            strBody = strBody + "<br/>Model : &nbsp;" + ModelVariant.ToUpper();
            strBody = strBody + "<br/>Vehicle No.: &nbsp;" + VehicleState.ToUpper() + "&nbsp;" + VehicleCode.ToUpper() + "&nbsp;" + VehicleNo.ToUpper();
            strBody = strBody + "<br/>Chassis No.: &nbsp;" + ChasisNo.ToUpper();
            strBody = strBody + "<br/>Engine No.: &nbsp;" + EngineNo.ToUpper();

            strBody = strBody + "<br/>in the same Vehicle,";
            if (result == "1DSA" || result == "1DSI")
            {
                strBody = strBody + "&nbsp; Vehicle number &nbsp;";
            }
            else if (result == "2DSA" || result == "2DSI")
            {
                strBody = strBody + "&nbsp; Chassis number &nbsp;";
            }
            else if (result == "3DSA" || result == "3DSI")
            {
                strBody = strBody + "&nbsp; Engine number &nbsp;";
            }

            strBody = strBody + "is again being register as New Registration by other person";
            strBody = strBody + "<br/>we are not sure if it is you or sometime mistyping data entry may occurred or some mischief has happened with your Stolen/Lost registered product";

            strBody = strBody + "<br/>So we share the details of the same with you.";
            strBody = strBody + "<br/>Our given information may help you.";

            strBody = strBody + "<br/><br/>Details of person who trying to register as New Registration mentioned below. ";

            strBody = strBody + "<br/><br/>Name : &nbsp;" + loggedUserData.OwnerName;
            if (string.IsNullOrEmpty(loggedUserData.OwnerCountryName) == false)
            {
                strBody = strBody + "<br/>Country : &nbsp;" + loggedUserData.OwnerCountryName;
            }
            if (string.IsNullOrEmpty(loggedUserData.OwnerStateName) == false)
            {
                strBody = strBody + "<br/>State : &nbsp;" + loggedUserData.OwnerStateName;
            }
            if (loggedUserData.OwnerDistrict != null && loggedUserData.OwnerDistrictName != "")
            {
                strBody = strBody + "<br/>District  : &nbsp;" + loggedUserData.OwnerDistrictName;
            }
            if (string.IsNullOrEmpty(loggedUserData.OwnerCityName) == false)
            {
                strBody = strBody + "<br/>City : &nbsp;" + loggedUserData.OwnerCityName;
            }
            strBody = strBody + "<br/>Address : &nbsp;" + loggedUserData.OwnerAddress;
            strBody = strBody + "<br/>Mobile  : &nbsp;" + loggedUserData.OwnerMobile;
            strBody = strBody + "<br/>E-mail ID : &nbsp;" + loggedUserData.OwnerEmail;
            strBody = strBody + "<br/>Trying Reg Date : &nbsp;" + DateTime.Now.ToString("dd-MM-yyyy");
            strBody = strBody + "<br/>IP address from where he registered this product : &nbsp;" + loggedUserData.IPAddress;
            strBody = strBody + "<br/><br/> (Location of this IP address can be found on any IP location tracking website)";
            strBody = strBody + "<br/><br/> Regards,";
            strBody = strBody + "<br/>antitheftclub.com Team<br/>";
            emailMessage.Body = strBody;


            this.emailService.Send(emailMessage);
        }
    }
}