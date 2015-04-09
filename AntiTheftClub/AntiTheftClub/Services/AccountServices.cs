using AntiTheftClub.DbModel;
using AntiTheftClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AntiTheftClub.Services
{
    public class AccountServices
    {
        private DbEntities entity;


        public AccountServices(DbEntities db)
        {
            entity = db;
        }

        public bool Register(RegisterViewModel newUser)
        {
            bool isError = false;
            string accountId = string.Empty;
            string tempPassowrd = string.Format("{0}123",newUser.FullName.Substring(0,5));
            string cCode = "IND";

            if(newUser.CountryCode != "in")
                cCode = newUser.CountryCode;

            var result = entity.NewSignUp(string.Empty, newUser.FullName,cCode, newUser.CountryDailCode, newUser.MobileNumber, newUser.Email, tempPassowrd, tempPassowrd, 1, "1234", newUser.IpAddress).First();
            
            if(result.Validate=="0")
            {
                isError = true;
            }

            entity.UpdateNewUserId(newUser.NewUserId, result.id);

            entity.SaveChangesAsync();
            return isError;
        }

        public string GetFullName(string userId)
        {
            return entity.GetUserProfile(userId).First().ownername;
        }


        public string GetAccountId(string userId)
        {
            return entity.GetAccountId(userId).First().userid;
        }

        public string GetAccountIdByEmail(string email)
        {
            var result = entity.AspNetUsers.FirstOrDefault(ed => ed.Email == email);
            if (result != null)
            {
                return GetAccountId(result.Id);
            }

            return string.Empty;
        }

        public string GetFullNameByEmail(string email)
        {
            var result = entity.AspNetUsers.FirstOrDefault(ed => ed.Email == email);
            if (result != null)
            {
                return GetFullName(result.Id);
            }

            return string.Empty;
        }

    }
}