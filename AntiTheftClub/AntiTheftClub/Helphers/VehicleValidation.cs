using AntiTheftClub.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AntiTheftClub.Helphers
{
    public class NewVehicleValidator
    {

        public string CheckNewVehicle(DataSet dschkvehicle, string loggedUserAccountId)
        {
            string resultMessage = string.Empty;

            if (dschkvehicle != null && dschkvehicle.Tables[0] != null && dschkvehicle.Tables[0].Rows[0] != null && dschkvehicle.Tables[0].Rows.Count > 0 && dschkvehicle.Tables[0].Rows[0]["result"].ToString().Trim() != "0" && loggedUserAccountId == dschkvehicle.Tables[0].Rows[0]["Userid"].ToString().Trim())
            {
                string result = dschkvehicle.Tables[0].Rows[0]["result"].ToString().Trim();
                string activeFlag = dschkvehicle.Tables[0].Rows[0]["Active_flag"].ToString().Trim().ToUpper();
                if (result == "1")
                {
                    if (activeFlag == "A")
                    {
                        resultMessage = "Your entered Vehicle number is already registered by you as New Registration.Plz Try Again!";


                    }
                    else
                    {
                        resultMessage = "Your entered Vehicle number is already registered by you as New Registration, please make payment to activate your registration!";


                    }
                }
                else if (result == "2")
                {
                    if (activeFlag == "A")
                    {

                        resultMessage = "Your entered Chassis number is already registered by you as New Registration.Plz Try Again!";


                    }
                    else
                    {
                        resultMessage = "Your entered Chassis number is already registered by you as New Registration, please make payment to activate your registration!";


                    }
                }
                else if (result == "3")
                {
                    if (activeFlag == "A")
                    {
                        resultMessage = "Your entered Engine number is already registered by you as New Registration.Plz Try Again!";


                    }
                    else
                    {
                        resultMessage = "Your entered Engine number is already registered by you as New Registration, please make payment to activate your registration!";


                    }
                }
            }
            else if (dschkvehicle != null && dschkvehicle.Tables[0] != null && dschkvehicle.Tables[0].Rows[0] != null && dschkvehicle.Tables[0].Rows.Count > 0 && dschkvehicle.Tables[0].Rows[0]["result"].ToString().Trim() != "0" && loggedUserAccountId != dschkvehicle.Tables[0].Rows[0]["Userid"].ToString().Trim())
            {
                resultMessage = MessageCodes.NewDuplicate;

            }

            return resultMessage;
        }

        internal bool ConfirmVehicleReg(System.Data.DataSet ds)
        {
            bool isConfirmed = false;

            if (ds != null && ds.Tables != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0 && (ds.Tables[0].Rows[0]["Validate"].ToString().Trim() == "1" || ds.Tables[0].Rows[0]["Validate"].ToString().Trim() == "2"))
            {
                isConfirmed = true;
            }

            return isConfirmed;
        }

        internal string ValidateNewDuplicate(string vCode)
        {
            if (vCode == "1SS" || vCode == "2SS" || vCode == "3SS")
            {
                return MessageCodes.VehicleFreeWithCode;
            }
            else if (vCode == "1DSA" || vCode == "1DSI" || vCode == "2DSA" || vCode == "2DSI" || vCode == "3DSA" || vCode == "3DSI")
            {
                return MessageCodes.StopDuplicate;
            }
            else
            {
                return MessageCodes.VehicleFree;
            }
        }

    }
}