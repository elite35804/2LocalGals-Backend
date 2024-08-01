using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Web.UI;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Specialized;
using System.Net.Mail;

namespace Nexus
{
    public class Globals
    {
        //REAL WEB SITE
        //public const string baseUrl = @"http://portal.2localgals.com/";
        //public const string baseUrlSecure = @"https://portal.2localgals.com/";

        //BETA WEB SITE
        //public const string baseUrl = @"http://beta.2localgals.net/";
        //public const string baseUrlSecure = @"https://beta.2localgals.net/";

        public const string baseUrl = @"http://45.61.128.213:3000/";
        public const string baseUrlSecure = @"http://45.61.128.213:3000/";

        //public const string baseUrl = @"http://167.88.164.32:3000/";
        //public const string baseUrlSecure = @"http://167.88.164.32:3000/";
        #region UtcToMst
        public static DateTime UtcToMst(DateTime utc)
        {
            try
            {
                DateTime ret = utc.ToLocalTime();
                return ret;
            }
            catch
            {
                return utc;
            }
        }
        #endregion

        #region Decrypt
        public static string Decrypt(string cipherText)
        {
            try
            {
                if (!string.IsNullOrEmpty(cipherText))
                {
                    byte[] keyBytes = new byte[] 
                    {
                        0x57,0x00,0xE5,0x13,0x7A,0x17,0x61,0xB5,
                        0x75,0x90,0x05,0xD0,0x3C,0x17,0xF7,0x3A,
                        0xD3,0x56,0xC6,0x21,0x1F,0x52,0x02,0xEA,
                        0xD1,0x50,0x5F,0x2E,0x14,0xE7,0xC2,0x03
                    };

                    byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                    RijndaelManaged symmetricKey = new RijndaelManaged();
                    symmetricKey.Mode = CipherMode.CBC;
                    ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes("@1B2c3D4e5F6g7H8"));
                    using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }
            catch { }
            return cipherText;
        }
        #endregion

        #region Encrypt
        public static string Encrypt(string plainText)
        {
            try
            {
                byte[] cipherTextBytes;
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] initVectorBytes = Encoding.ASCII.GetBytes("@1B2c3D4e5F6g7H8");
                byte[] saltValueBytes = Encoding.ASCII.GetBytes("A!@$k233ie&");
                PasswordDeriveBytes password = new PasswordDeriveBytes("@@feklLLiE$", saltValueBytes, "SHA1", 5);
                byte[] keyBytes = password.GetBytes(32);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memoryStream.ToArray();
                    }
                }
                string cipherText = Convert.ToBase64String(cipherTextBytes);
                return cipherText;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region ForceSSL
        public static void ForceSSL(Page page)
        {
            try
            {
#if !DEBUG
                if (!page.Request.IsSecureConnection)
                    page.Response.Redirect("https://" + page.Request.ServerVariables["SERVER_NAME"] + page.Request.ServerVariables["URL"]);
#endif
            }
            catch { }
        }
        #endregion

        #region FormatPoints
        public static string FormatPoints(decimal value, decimal ratePerHour = 0)
        {
            try
            {
                string ret = (value * 100).ToString("n0");
                if (ratePerHour > 0) ret += " (" + Globals.FormatHours(value / ratePerHour) + " hours)";
                return ret;
            }
            catch
            {
                return "0";
            }
        }
        #endregion

        #region FormatPoints
        public static decimal FormatPoints(string value)
        {
            try
            {
                return decimal.Round(decimal.Parse(value), 0) / 100.0m;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region FormatMoney
        public static string FormatMoney(decimal value)
        {
            try
            {
                value = decimal.Round(value, 2);
                if (value < 0)
                    return "-" + (value * -1.0m).ToString("C2");
                return value.ToString("C2");
            }
            catch
            {
                return "$0.00";
            }
        }
        #endregion

        #region FormatMoney
        public static decimal FormatMoney(string value)
        {
            try
            {
                return decimal.Round(decimal.Parse(value, NumberStyles.Currency), 2);
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region FormatHours
        public static string FormatHours(decimal value)
        {
            try
            {
                return value.ToString("N2");
            }
            catch
            {
                return "0";
            }
        }
        #endregion

        #region FormatHours
        public static decimal FormatHours(string value)
        {
            try
            {
                return decimal.Round(decimal.Parse(value), 2);
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region FormatPercent
        public static decimal FormatPercent(string value, bool round = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value)) return 0;
                if (round) return decimal.Round(decimal.Parse(value.Replace("%", ""), NumberStyles.Any));
                return decimal.Parse(value.Replace("%", ""), NumberStyles.Any);
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region FormatPercent
        public static string FormatPercent(decimal value, bool round = true)
        {
            try
            {
                if (round) return decimal.Round(value).ToString("N0") + " %";
                return value.ToString("N3") + " %";
            }
            catch
            {
                return "0 %";
            }
        }
        #endregion

        #region FormatPhone
        public static string FormatPhone(string phone)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(phone))
                {
                    string numbers = "";
                    foreach (char i in phone.ToCharArray())
                    {
                        if (i != ' ' && i != '(' && i != ')' && i != '-')
                        {
                            if ((byte)i >= 0x30 && (byte)i <= 0x39)
                            {
                                numbers += i;
                            }
                            else return phone;
                        }
                    }

                    if (numbers.Length == 10)
                        return "(" + numbers.Substring(0, 3) + ") " + numbers.Substring(3, 3) + "-" + numbers.Substring(6, 4);
                }
            }
            catch { }
            return phone;
        }
        #endregion

        #region FormatCard
        public static string FormatCard(string card, bool lastFourOnly)
        {
            try
            {
                card = Globals.OnlyNumbers(card);
                if (card.Length == 16)
                {
                    if (lastFourOnly)
                    {
                        return "xxxx xxxx xxxx " + card.Substring(12, 4);
                    }
                    else
                    {
                        return card.Substring(0, 4) + " " + card.Substring(4, 4) + " " + card.Substring(8, 4) + " " + card.Substring(12, 4);
                    }
                }
                if (card.Length == 15)
                {
                    if (lastFourOnly)
                    {
                        return "xxxx xxxxxx x" + card.Substring(11, 4);
                    }
                    else
                    {
                        return card.Substring(0, 4) + " " + card.Substring(4, 6) + " " + card.Substring(10, 5);
                    }
                }
            }
            catch { }
            return "";
        }
        #endregion

        #region FormatCardLastFour
        public static string FormatCardLastFour(string card)
        {
            try
            {
                card = Globals.OnlyNumbers(card);
                return card.Substring(card.Length - 4, 4);
            }
            catch
            {
                return "Invalid";
            }

        }
        #endregion

        #region FormatCardExpYear
        public static string FormatCardExpYear(string value)
        {
            int ret = SafeIntParse(value);
            if (ret <= 0) return value;
            if (ret < 2000) ret += 2000;
            return ret.ToString();
        }
        #endregion

        #region OnlyNumbers
        public static string OnlyNumbers(string value)
        {
            try
            {
                string ret = "";
                foreach (char i in (value ?? "").ToCharArray())
                    if ((byte)i >= 0x30 && (byte)i <= 0x39) ret += i;
                return ret;
            }
            catch
            {
                return value;
            }
        }
        #endregion

        #region FormatCustomerTitle
        public static string FormatCustomerTitle(object first, object last, object business)
        {
            try
            {
                string ret = "";
                if (first != DBNull.Value && (string)first != "")
                    ret += first + " ";
                if (last != DBNull.Value && (string)last != "")
                    ret += last + " ";
                if (business != DBNull.Value && (string)business != "")
                    ret += "[" + business + "]";
                return ret.Trim();
            }
            catch
            {
                return "Unknown Customer";
            }
        }
        public static string FormatCustomerTitle(DBRow row)
        {
            try
            {
                string ret = "";
                if (!string.IsNullOrEmpty(row.GetString("firstName")))
                    ret += row.GetString("firstName") + " ";
                if (!string.IsNullOrEmpty(row.GetString("lastName")))
                    ret += row.GetString("lastName") + " ";
                if (!string.IsNullOrEmpty(row.GetString("businessName")))
                    ret += "[" + row.GetString("businessName") + "]";
                return ret.Trim();
            }
            catch
            {
                return "Unknown Customer";
            }
        }
        #endregion

        #region FormatContractorTitle
        public static string FormatContractorTitle(object first, object last, object business)
        {
            try
            {
                string ret = (first != null && first != DBNull.Value && (string)first != "") ? (string)first : "";
                if (last != null && last != DBNull.Value && (string)last != "") ret += " " + (string)last;
                if (business != null && business != DBNull.Value && (string)business != "") ret = (string)business;
                return ret.Trim();
            }
            catch
            {
                return "Unknown Contractor";
            }
        }
        #endregion

        #region FormatFullName
        public static string FormatFullName(string first, string last, string alternative)
        {
            try
            {
                if (string.IsNullOrEmpty(last)) return first ?? alternative;
                if (string.IsNullOrEmpty(first)) return last ?? alternative;
                return first + " " + last;
            }
            catch
            {
                return alternative;
            }
        }
        #endregion

        #region LogoutUser
        public static void LogoutUser(Page page)
        {
            try
            {
                FormsAuthentication.SignOut();
                page.Session.Abandon();

                //Clear authentication cookie
                HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                cookie1.Expires = DateTime.Now.AddYears(-1);
                page.Response.Cookies.Add(cookie1);

                //Clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
                HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
                cookie2.Expires = DateTime.Now.AddYears(-1);
                page.Response.Cookies.Add(cookie2);

                Globals.CleanAllCookies();

                page.Response.Redirect("http://45.61.128.213:3000/");
            }
            catch { }
        }
        #endregion

        #region DeleteCookie
        public static void DeleteCookie(string cookieName)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Expires = DateTime.Now.AddDays(-1d);
                HttpContext.Current.Response.Cookies.Add(cookie);

                HttpCookie protectedCookie = new HttpCookie(cookieName);
                protectedCookie.Path = @"/Protected";
                protectedCookie.Expires = DateTime.Now.AddDays(-1d);
                HttpContext.Current.Response.Cookies.Add(protectedCookie);
            }
            catch { }
        }
        #endregion

        #region CleanAllCookies
        public static void CleanAllCookies()
        {
            try
            {
                for (int i = HttpContext.Current.Request.Cookies.Count - 1; i >= 0; i--)
                    DeleteCookie(HttpContext.Current.Request.Cookies[i].Name);
            }
            catch { }
        }
        #endregion

        #region GetCookieValue
        public static string GetCookieValue(string cookieName)
        {
            try
            {
                if (HttpContext.Current.Request.Cookies[cookieName] != null)
                    return HttpContext.Current.Request.Cookies[cookieName].Value ?? "";
            }
            catch { }
            return "";
        }
        #endregion

        #region SetCookieValue
        public static void SetCookieValue(string cookieName, string value)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(cookieName, value);
                if (HttpContext.Current.Request.Url.Segments.Length >= 2)
                    cookie.Path = HttpContext.Current.Request.Url.Segments[HttpContext.Current.Request.Url.Segments.Length - 2];
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch { }
        }
        #endregion

        #region GetUsername
        public static string GetUsername()
        {
            try
            {
                return HttpContext.Current.User.Identity.Name;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region GetFranchiseMask
        public static int GetFranchiseMask()
        {
            try
            {
                string[] values = Decrypt(GetCookieValue("CAB")).Split('|');
                if (values[0] == HttpContext.Current.User.Identity.Name)
                    return SafeIntParse(values[1]);
            }
            catch { }
            return 0;
        }
        #endregion

        #region GetUserAccess
        public static int GetUserAccess(Page page)
        {
            try
            {
                string[] values = Decrypt(GetCookieValue("CAB")).Split('|');
                if (values[0] == HttpContext.Current.User.Identity.Name)
                    return SafeIntParse(values[2]);
            }
            catch { }
            return 0;
        }
        #endregion

        #region GetUserContractorID
        public static int GetUserContractorID(Page page)
        {
            try
            {
                string[] values = Decrypt(GetCookieValue("CAB")).Split('|');
                if (values[0] == HttpContext.Current.User.Identity.Name)
                    return SafeIntParse(values[3]);
            }
            catch { }
            return 0;
        }
        #endregion

        #region GetFranchiseByMask
        public static FranchiseStruct GetFranchiseByMask(int franchiseMask)
        {
            foreach (FranchiseStruct fran in Database.GetFranchiseList())
            {
                if ((fran.franchiseMask & franchiseMask) != 0)
                    return fran;
            }
            return new FranchiseStruct();
        }
        #endregion

        #region SetUserValues
        public static void SetUserValues(UserStruct user)
        {
            Globals.SetCookieValue("CAB", Globals.Encrypt(user.username + "|" + user.franchiseMask + "|" + user.access + "|" + user.contractorID));
        }
        #endregion

        #region GetPortalCustomerID
        public static int GetPortalCustomerID(Page page)
        {
            try
            {
                string[] values = Decrypt(GetCookieValue("BAC")).Split('|');
                if (values[0] == HttpContext.Current.User.Identity.Name)
                    return SafeIntParse(values[1]);
            }
            catch { }
            return 0;
        }
        #endregion

        #region GetPortalFranchiseMask
        public static int GetPortalFranchiseMask(Page page)
        {
            try
            {
                string[] values = Decrypt(GetCookieValue("BAC")).Split('|');
                if (values[0] == HttpContext.Current.User.Identity.Name)
                    return SafeIntParse(values[2]);
            }
            catch { }
            return 0;
        }
        #endregion

        #region GetPortalCustomerEmail
        public static string GetPortalCustomerEmail(Page page)
        {
            try
            {
                string[] values = Decrypt(GetCookieValue("BAC")).Split('|');
                if (values[0] == HttpContext.Current.User.Identity.Name)
                    return values[3];
            }
            catch { }
            return "";
        }
        #endregion

        #region SetPortalValues
        public static void SetPortalValues(CustomerStruct customer)
        {
            Globals.SetCookieValue("BAC", Globals.Encrypt(Globals.FormatFullName(customer.firstName, customer.lastName, "Customer") + "|" + customer.customerID + "|" + customer.franchiseMask + "|" + customer.email));
        }
        #endregion

        #region DateTimeParse
        public static DateTime DateTimeParse(string value)
        {
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        #endregion

        #region DateTimeParseSql
        public static DateTime DateTimeParseSql(string value)
        {
            try
            {
                DateTime ret = Convert.ToDateTime(value);
                if (ret < new DateTime(1900, 1, 1, 0, 0, 0)) ret = new DateTime(1900, 1, 1, 0, 0, 0);
                if (ret > new DateTime(9999, 1, 1, 0, 0, 0)) ret = new DateTime(9999, 1, 1, 0, 0, 0);
                return ret;
            }
            catch
            {
                return new DateTime(1900, 1, 1, 0, 0, 0);
            }
        }
        #endregion

        #region SafeIntParse
        public static int SafeIntParse(string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region SafeIntParse
        public static decimal SafeDecimalParse(string value)
        {
            try
            {
                return decimal.Parse(value);
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region SafeDateParse
        public static DateTime SafeDateParse(string value)
        {
            try
            {
                return DateTime.Parse(value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        #endregion

        #region SafeSqlString
        public static string SafeSqlString(object value)
        {
            try
            {
                if (value == DBNull.Value) return "";
                return (string)value;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region GetContractorList
        public static ListItem[] GetContractorList(int franchiseMask, int contractorID, int subType, out ContractorStruct selectedContractor, bool onlyActive, bool onlyReps)
        {
            selectedContractor = new ContractorStruct();
            List<ListItem> ret = new List<ListItem>();
            try
            {
                bool found = false;
                List<ContractorStruct> contractorList = Database.GetContractorList(franchiseMask, -1, onlyReps, onlyActive, false, false, "firstName, lastName");
                foreach (ContractorStruct contractor in contractorList)
                {
                    if (subType > 0)
                    {
                        foreach (int index in Globals.BitMaskToIndexList(contractor.contractorType))
                        {
                            ListItem item = new ListItem(contractor.title, contractor.contractorID.ToString());
                            switch (index)
                            {
                                case 2: item.Text = "[C] " + item.Text; break;
                                case 3: item.Text = "[W] " + item.Text; break;
                                case 4: item.Text = "[H] " + item.Text; break;
                                case 5: item.Text = "[L] " + item.Text; break;
                            }
                            item.Value += "|" + index;

                            if (contractor.contractorID == contractorID && index == subType)
                            {
                                selectedContractor = contractor;
                                item.Selected = true;
                                found = true;
                            }
                            ret.Add(item);
                        }
                    }
                    else
                    {
                        ListItem item = new ListItem(contractor.title, contractor.contractorID.ToString());
                        if (contractor.contractorID == contractorID)
                        {
                            selectedContractor = contractor;
                            item.Selected = true;
                            found = true;
                        }
                        ret.Add(item);
                    }
                }

                if (!found && contractorID > 0)
                {
                    ContractorStruct contractor = Database.GetContractorByID(franchiseMask, contractorID);

                    if (subType > 0)
                    {
                        ListItem item = new ListItem(contractor.title, contractor.contractorID.ToString());
                        switch (subType)
                        {
                            case 2: item.Text = "[C] " + item.Text; break;
                            case 3: item.Text = "[W] " + item.Text; break;
                            case 4: item.Text = "[H] " + item.Text; break;
                            case 5: item.Text = "[L] " + item.Text; break;
                        }
                        item.Value += "|" + subType;
                        selectedContractor = contractor;
                        item.Selected = true;
                        ret.Add(item);
                    }
                    else
                    {
                        ListItem item = new ListItem(contractor.title, contractor.contractorID.ToString());
                        selectedContractor = contractor;
                        item.Selected = true;
                        ret.Add(item);
                    }
                }
            }
            catch { }
            return ret.ToArray();
        }
        #endregion

        #region CreditCardExpired
        public static bool CreditCardExpired(string month, string year)
        {
            try
            {
                DateTime creditDate = Convert.ToDateTime(month + "/1/" + year);
                return creditDate < UtcToMst(DateTime.UtcNow);
            }
            catch { }
            return true;
        }
        #endregion

        #region IDToMask
        public static int IDToMask(int value)
        {
            try
            {
                if (value > 0 && value < 32)
                    return 1 << (value - 1);
            }
            catch { }
            return 0;
        }
        #endregion

        #region GetFranchiseList
        public static ListItem[] GetFranchiseList(int franchiseMask, int franchiseID, out FranchiseStruct selectedFranchise)
        {
            selectedFranchise = new FranchiseStruct();
            List<ListItem> ret = new List<ListItem>();
            try
            {
                foreach (FranchiseStruct franchise in Database.GetFranchiseList())
                {
                    if ((franchiseMask & franchise.franchiseMask) != 0)
                    {
                        ListItem item = new ListItem(franchise.franchiseName, franchise.franchiseID.ToString());
                        if (franchise.franchiseID == franchiseID)
                        {
                            selectedFranchise = franchise;
                            item.Selected = true;
                        }
                        ret.Add(item);
                    }
                }
            }
            catch { }
            return ret.ToArray();
        }
        public static ListItem[] GetFranchiseList(int includeMask, int selectedMask)
        {
            List<ListItem> ret = new List<ListItem>();
            try
            {
                var franchiseList = Database.GetFranchiseList();
                foreach (FranchiseStruct franchise in franchiseList)
                {
                    ListItem item = new ListItem(franchise.franchiseName, franchise.franchiseID.ToString("D2"));
                    var fMask = IDToMask(franchise.franchiseID);
                    if ((selectedMask & fMask) > 0)
                        item.Selected = true;
                    if ((includeMask & fMask) > 0)
                        ret.Add(item);
                }
            }
            catch { }
            return ret.ToArray();
        }

        public static ListItem[] GetFranchiseListNoMask(int includeMask, int selectedMask)
        {
            List<ListItem> ret = new List<ListItem>();
            try
            {
                var franchiseList = Database.GetFranchiseList();
                foreach (FranchiseStruct franchise in franchiseList)
                {
                    ListItem item = new ListItem(franchise.franchiseName, franchise.franchiseID.ToString("D2"));
                    var fMask = IDToMask(franchise.franchiseID);
                    if ((selectedMask & fMask) > 0)
                        item.Selected = true;
                    // if ((includeMask & fMask) > 0)
                    ret.Add(item);
                }
            }
            catch { }
            return ret.ToArray();
        }

        #endregion

        #region GetContractorTypeList
        public static ListItem[] GetContractorTypeList(int selectedMask)
        {
            List<ListItem> ret = new List<ListItem>();
            try
            {
                ListItem item;

                item = new ListItem("Housekeeping", "1");
                if ((selectedMask & 1) != 0) item.Selected = true;
                ret.Add(item);

                item = new ListItem("Carpet Cleaning", "2");
                if ((selectedMask & 2) != 0) item.Selected = true;
                ret.Add(item);

                item = new ListItem("Window Washing", "4");
                if ((selectedMask & 4) != 0) item.Selected = true;
                ret.Add(item);

                item = new ListItem("Home Guard", "8");
                if ((selectedMask & 8) != 0) item.Selected = true;
                ret.Add(item);
            }
            catch { }
            return ret.ToArray();
        }
        #endregion

        #region GetServicesList
        public static ListItem[] GetServicesList(int serviceMask)
        {
            List<ListItem> ret = new List<ListItem>();
            try
            {
                ListItem item;

                item = new ListItem("Housekeeping", "1");
                item.Selected = true;
                ret.Add(item);

                if ((serviceMask & 2) != 0)
                    ret.Add(new ListItem("Carpet Cleaning", "2"));

                if ((serviceMask & 4) != 0)
                    ret.Add(new ListItem("Window Washing", "3"));

                if ((serviceMask & 8) != 0)
                    ret.Add(new ListItem("Home Guard", "4"));
            }
            catch { }
            return ret.ToArray();
        }
        #endregion

        #region GetUserList
        public static ListItem[] GetUserList(string username, out UserStruct selectedUser, int includeMask, int access)
        {
            selectedUser = new UserStruct();
            List<ListItem> ret = new List<ListItem>();
            try
            {
                foreach (UserStruct user in Database.GetUserList())
                {
                    if ((user.access <= access && (includeMask & user.franchiseMask) != 0) || access >= 9)
                    {
                        ListItem item = new ListItem(user.username);
                        if (user.username == username)
                        {
                            item.Selected = true;
                            selectedUser = user;
                        }
                        ret.Add(item);
                    }
                }
            }
            catch { }
            return ret.ToArray();
        }
        #endregion

        #region CalculateDiscount
        public static decimal CalculateDiscountPercent(decimal hours, decimal rate, decimal serviceFee, decimal percent, DateTime appDate)
        {
            try
            {
                decimal discount = hours * rate;
                if (appDate < new DateTime(2021, 1, 15)) discount += serviceFee;
                discount *= (percent / 100M);
                return discount;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region ExtractSalesTaxFromTotal
        public static decimal ExtractSalesTaxFromTotal(decimal total, decimal tips, decimal tax)
        {
            if (tax <= 0) return 0;
            decimal nonTaxedTotal = (total - tips) / (1.0M + (tax / 100M));
            return (total - tips) - nonTaxedTotal;
        }
        #endregion

        #region CalculateAppointmentTotal
        public static decimal CalculateAppointmentTotal(AppStruct app)
        {
            try
            {
                decimal total = (app.appType <= 1) ? app.customerHours * app.customerRate : 0;
                total += app.customerServiceFee;
                total -= CalculateDiscountPercent(app.appType <= 1 ? app.customerHours : 0, app.customerRate, app.customerServiceFee, app.customerDiscountPercent + app.customerDiscountReferral, app.appointmentDate);
                total -= app.customerDiscountAmount;
                total += app.contractorTips;
                total += app.customerSubContractor;
                total += (total - app.contractorTips) * (app.salesTax / 100M);
                return total;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region CalculateAppointmentContractorTotal
        public static decimal CalculateAppointmentContractorTotal(AppStruct app, ContractorStruct con, FranchiseStruct fran)
        {
            try
            {
                decimal total = (app.appType <= 1) ? app.contractorHours * app.contractorRate : 0;
                total *= (100.0m - Globals.ParseScheduleFee(fran.scheduleFeeString, app.appointmentDate)) / 100.0m;
                total += app.customerSubContractor * ((100.0m - con.serviceSplit) / 100.0m);
                total += app.customerServiceFee;
                total += app.contractorTips;
                total += app.contractorAdjustAmount;
                return total;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region FormatedTableHeaderCell
        public static TableHeaderCell FormatedTableHeaderCell(string text, int width)
        {
            try
            {
                TableHeaderCell cell = new TableHeaderCell();
                if (width > 0) cell.Width = width;
                cell.Text = text;
                return cell;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region FormatedTableCell
        public static TableCell FormatedTableCell(string text)
        {
            try
            {
                TableCell cell = new TableCell();
                cell.Text = text;
                return cell;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region StartOfWeek
        public static DateTime StartOfWeek(DateTime value)
        {
            try
            {
                int diff = DayOfWeek.Monday - value.DayOfWeek;
                diff = (diff - 7) % 7;
                return value.AddDays(diff).Date;
            }
            catch
            {
                return value;
            }
        }
        #endregion

        #region GetDayOfWeek
        public static int GetDayOfWeek(DateTime value)
        {
            try
            {
                return ((int)value.DayOfWeek + 6) % 7;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region SetDropDownList
        public static void SetDropDownList(ref DropDownList list, string text)
        {
            bool add = true;
            for (int i = 0; i < list.Items.Count && add; i++)
                if (list.Items[i].Text == text) add = false;

            if (add) list.Items.Insert(0, new ListItem(text));
            list.Text = text;
        }
        #endregion

        #region TimeOnly
        public static DateTime TimeOnly(DateTime value)
        {
            try
            {
                return new DateTime(1900, 1, 1, value.Hour, value.Minute, value.Second);
            }
            catch
            {
                return value;
            }
        }
        public static DateTime TimeOnly(int hour, int minute, int second)
        {
            try
            {
                return new DateTime(1900, 1, 1, hour, minute, second);
            }
            catch
            {
                return new DateTime(1900, 1, 1, 0, 0, 0);
            }
        }
        #endregion

        #region FormatDateRange
        public static void FormatDateRange(ref DateTime startDate, ref DateTime endDate)
        {
            try
            {
                startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
                endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
            }
            catch { }
        }
        #endregion

        #region SplitEmail
        public static string[] SplitEmail(string email)
        {
            try
            {
                string[] split = email.Split(new char[] { ',', ' ' });
                for (int i = 0; i < split.Length; i++)
                    split[i] = split[i].Trim();
                return split;
            }
            catch
            {
                return new string[0];
            }
        }
        #endregion

        #region ValidEmail
        public static bool ValidEmail(string email)
        {
            try
            {
                bool ret = false;
                foreach (string e in SplitEmail(email))
                {
                    MailAddress m = new MailAddress(e);
                    ret = true;
                }
                return ret;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region BuildQueryString
        public static string BuildQueryString(string queryString, string key, object value)
        {
            if (queryString == null) queryString = "";
            if (queryString.Contains(key + "=")) return queryString;
            if (queryString.Contains("?")) return queryString + "&" + HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(value.ToString());
            return queryString + "?" + HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(value.ToString());
        }
        #endregion

        #region Base64Encode
        public static string Base64Encode(string value)
        {
            try
            {
                byte[] bytes = new byte[value.Length * sizeof(char)];
                System.Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region Base64Decode
        public static string Base64Decode(string value)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(value);
                char[] chars = new char[bytes.Length / sizeof(char)];
                System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
                return new string(chars);
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region IsPaymentCreditCard
        public static bool IsPaymentCreditCard(string paymentType)
        {
            try
            {
                paymentType = paymentType.ToLower();
                if (paymentType.Contains("card")) return true;
                if (paymentType.Contains("visa")) return true;
                if (paymentType.Contains("discover")) return true;
                if (paymentType.Contains("master")) return true;
                if (paymentType.Contains("american express")) return true;
                if (paymentType.Contains("need cc")) return true;
                if (paymentType.Contains("auth")) return true;
                if (paymentType.Contains("capture")) return true;
            }
            catch { }
            return false;
        }
        #endregion 

        #region SetPeviousPage
        public static void SetPreviousPage(Page page, string[] filterList)
        {
            try
            {
                string absolute = page.Request.Url.AbsolutePath.ToString();
                string name = "PreviousPageUrl(" + absolute + ")";

                if (page.Request.UrlReferrer != null)
                {
                    string referrer = page.Request.UrlReferrer.ToString();
                    if (!referrer.Contains("Login.aspx"))
                    {
                        if (filterList != null)
                        {
                            foreach (string filter in filterList)
                            {
                                if (referrer.Contains(filter))
                                {
                                    return;
                                }
                            }
                        }

                        if (referrer.Contains(absolute))
                            return;

                        page.Session[name] = referrer;
                    }
                    else page.Session[name] = null;
                }
                else page.Session[name] = null;
            }
            catch { }
        }
        #endregion

        #region GetPeviousPage
        public static string GetPeviousPage(Page page, string altUrl)
        {
            try
            {
                string absolute = page.Request.Url.AbsolutePath.ToString();
                string name = "PreviousPageUrl(" + absolute + ")";
                string url = ((string)page.Session[name]) ?? altUrl;
                url = BuildQueryString(url, "DoScroll", "Y");
                return url;
            }
            catch
            {
                return altUrl;
            }
        }
        #endregion

        #region RedirectToPeviousPage
        public static void RedirectToPeviousPage(Page page, string altUrl)
        {
            try
            {
                string absolute = page.Request.Url.AbsolutePath.ToString();
                string name = "PreviousPageUrl(" + absolute + ")";
                string prevUrl = (string)page.Session[name];
                if (prevUrl != null) page.Response.Redirect(prevUrl);
                else page.Response.Redirect(altUrl);
            }
            catch
            {
                page.Response.Redirect(altUrl);
            }
        }
        #endregion

        #region ClearPeviousPage
        public static void ClearPeviousPage(Page page)
        {
            try
            {
                string absolute = page.Request.Url.AbsolutePath.ToString();
                string name = "PreviousPageUrl(" + absolute + ")";
                page.Session[name] = null;
            }
            catch { }
        }
        #endregion

        #region ParseScheduleFee
        public static decimal ParseScheduleFee(string scheduleFeeString, DateTime appDate)
        {
            decimal ret = 0;
            try
            {
                foreach (string i in scheduleFeeString.Split(','))
                {
                    string[] split = i.Split(':');
                    if (split.Length == 1)
                    {
                        ret = Globals.FormatMoney(i);
                    }
                    if (split.Length == 2)
                    {    
                        DateTime date = Globals.SafeDateParse(split[0]);
                        decimal value = Globals.FormatMoney(split[1]);
                        if (appDate >= date) ret = value;
                    }
                }
            }
            catch { }
            return ret;
        }
        #endregion

        #region FindControlRecursive
        public static Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id) return root;
            foreach (Control control in root.Controls)
            {
                Control foundControl = FindControlRecursive(control, id);
                if (foundControl != null) return foundControl;
            }
            return null;
        }
        #endregion

        #region CustomerIDToPassphrase
        public static string CustomerIDToPassphrase(int customerID)
        {
            try
            {
                byte key = 0;
                byte[] bytes = BitConverter.GetBytes(customerID);
                byte[] hash = MD5.Create().ComputeHash(bytes);
                for (int i = 0; i < hash.Length; i++)
                    key ^= hash[i];
                customerID |= (key << 24);
                customerID ^= ((key << 16) | (key << 8) | key);
                customerID ^= 0xBADBEEF;
                return EncodeZBase32((uint)customerID);
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region PassphraseToCustomerID
        public static int PassphraseToCustomerID(string passphrase)
        {
            try
            {
                if (!string.IsNullOrEmpty(passphrase))
                {
                    uint customerID = DecodeZBase32(passphrase.ToUpper());
                    customerID ^= 0xBADBEEF;
                    uint key = customerID >> 24;
                    customerID ^= ((key << 16) | (key << 8) | key);
                    customerID &= 0xFFFFFF;
                    byte[] bytes = BitConverter.GetBytes(customerID);
                    byte[] hash = MD5.Create().ComputeHash(bytes);
                    for (int i = 0; i < hash.Length; i++)
                        key ^= hash[i];
                    if (key != 0) customerID = 0;
                    return (int)customerID;
                }
            }
            catch { }
            return 0;
        }
        #endregion

        #region EncodeZBase32
        public static string EncodeZBase32(uint value)
        {
            string ret = "";
            string encode = "YBNDRFG8EJKMCPQXOT1UWISZA345H769";
            while (value > 0)
            {
                ret = encode[(int)(value % 32)] + ret;
                value /= 32;
            }
            return ret;
        }
        #endregion

        #region DecodeZBase32
        public static uint DecodeZBase32(string value)
        {
            uint ret = 0;
            value = value.ToUpper().Trim();
            string decode = "YBNDRFG8EJKMCPQXOT1UWISZA345H769";
            foreach (char i in value)
            {
                ret *= 32;
                ret |= (uint)decode.IndexOf(i);
            }
            return ret;
        }
        #endregion

        #region Get32BitGUID
        public static int Get32BitGUID()
        {
            return Guid.NewGuid().GetHashCode() & 0x7FFFFFFF;
        }
        #endregion

        #region GetReferralDiscount
        public static decimal GetReferralDiscount(List<CustomerStruct> referralList)
        {
            decimal percent = 0.0m;
            foreach (CustomerStruct referral in referralList)
            {
                if (referral.accountStatus.ToLower().Trim() == "active")
                    percent += 5;
            }
            if (percent > 0) percent += 5;
            if (percent > 30) percent = 30;
            return percent;
        }
        #endregion

        #region IsPreferedCustomerTime
        public static bool IsPreferedCustomerTime(string timePrefix, string time, ref DateTime start, DateTime end, TimeSpan jobTime)
        {
            try
            {
                if (string.IsNullOrEmpty(timePrefix) || string.IsNullOrEmpty(time))
                {
                    return false;
                }
                if (timePrefix == "A.M.")
                {
                    return start.TimeOfDay <= TimeSpan.FromHours(12.45);
                }
                if (timePrefix == "P.M.")
                {
                    if ((end.TimeOfDay - jobTime) >= TimeSpan.FromHours(13))
                    {
                        if (start.TimeOfDay < TimeSpan.FromHours(13))
                            start = start.Date + TimeSpan.FromHours(12);
                        return true;
                    }
                    return false;
                }
                if (time == "Any Time")
                {
                    return false;
                }
                if (timePrefix == "Flexible")
                {
                    return false;
                }
                if (timePrefix == "Before")
                {
                    return start.TimeOfDay <= DateTime.Parse(time).TimeOfDay;
                }
                if (timePrefix == "After")
                {
                    if ((end.TimeOfDay - jobTime) >= DateTime.Parse(time).TimeOfDay)
                    {
                        if (start.TimeOfDay < DateTime.Parse(time).TimeOfDay)
                            start = start.Date + DateTime.Parse(time).TimeOfDay;
                        return true;
                    }
                    return false;
                }
                if (timePrefix == "Done By")
                {
                    return start.TimeOfDay + jobTime <= DateTime.Parse(time).TimeOfDay;
                }
                if (timePrefix == "Must Be")
                {
                    if ((start.TimeOfDay <= DateTime.Parse(time).TimeOfDay) && ((end.TimeOfDay - jobTime) >= DateTime.Parse(time).TimeOfDay))
                    {
                        start = start.Date + DateTime.Parse(time).TimeOfDay;
                        return true;
                    }
                    return false;
                }
            }
            catch { }
            return false;
        }
        #endregion

        #region CheckPreferedCustomerTime
        public static bool CheckPreferedCustomerTime(string timePrefix, string time, DateTime start, DateTime end)
        {
            try
            {
                TimeSpan jobTime = end - start;
                if (string.IsNullOrEmpty(timePrefix) || string.IsNullOrEmpty(time))
                {
                    return true;
                }
                if (timePrefix == "A.M.")
                {
                    return start.TimeOfDay <= TimeSpan.FromHours(12.45);
                }
                if (timePrefix == "P.M.")
                {
                    if ((end.TimeOfDay - jobTime) >= TimeSpan.FromHours(13))
                    {
                        if (start.TimeOfDay < TimeSpan.FromHours(13))
                            start = start.Date + TimeSpan.FromHours(12);
                        return true;
                    }
                    return false;
                }
                if (time == "Any Time")
                {
                    return true;
                }
                if (timePrefix == "Flexible")
                {
                    return true;
                }
                if (timePrefix == "Before")
                {
                    return start.TimeOfDay <= DateTime.Parse(time).TimeOfDay;
                }
                if (timePrefix == "After")
                {
                    if ((end.TimeOfDay - jobTime) >= DateTime.Parse(time).TimeOfDay)
                    {
                        if (start.TimeOfDay < DateTime.Parse(time).TimeOfDay)
                            start = start.Date + DateTime.Parse(time).TimeOfDay;
                        return true;
                    }
                    return false;
                }
                if (timePrefix == "Done By")
                {
                    return start.TimeOfDay + jobTime <= DateTime.Parse(time).TimeOfDay;
                }
                if (timePrefix == "Must Be")
                {
                    if ((start.TimeOfDay <= DateTime.Parse(time).TimeOfDay) && ((end.TimeOfDay - jobTime) >= DateTime.Parse(time).TimeOfDay))
                    {
                        start = start.Date + DateTime.Parse(time).TimeOfDay;
                        return true;
                    }
                    return false;
                }
            }
            catch { }
            return false;
        }
        #endregion

        #region CleanAddr
        public static string CleanAddr(string addr)
        {
            try
            {
                if (!string.IsNullOrEmpty(addr))
                    return addr.Split(new char[] { '(', '[', '{', '"', '\'', '|' })[0].Trim();
            }
            catch { }
            return addr;
        }
        #endregion

        #region ReplaceEmailDefinedStrings
        public static string ReplaceEmailDefinedStrings(string value, FranchiseStruct fran,  string firstName, string lastName)
        {
            try
            {
                string companyLogo = @"<img src=""" + baseUrl + @"2LG_Logo_Small.jpg"" alt=""Logo"" />";
                string letterheadLogo = @"<img src=""" + baseUrl + @"2LG_Letterhead.png"" alt=""Logo"" />";
                string customerFirstName = firstName ?? "";
                string customerLastName = lastName ?? "";
                string customerFullName = Globals.FormatFullName(firstName, lastName, "Customer");
                string franchiseName = fran.franchiseName ?? "Two Local Gals";
                string franchisePhone = Globals.FormatPhone(fran.phone ?? "");
                string franchiseWebsite = fran.webLink ?? "https://www.2localgals.com/";
                string franchiseEmail = fran.email ?? "2localgalshousekeeping@gmail.com";
                string franchiseAddress = fran.address ?? "";
                string franchiseCity = fran.city ?? "";
                string franchiseState = fran.state ?? "";
                string franchiseZip = fran.zip ?? "";

                value = value.Replace("[CompanyLogo]", companyLogo);
                value = value.Replace("[LetterheadLogo]", letterheadLogo);
                value = value.Replace("[CustomerFirstName]", customerFirstName);
                value = value.Replace("[CustomerLastName]", customerLastName);
                value = value.Replace("[CustomerFullName]", customerFullName);
                value = value.Replace("[FranchiseName]", franchiseName);
                value = value.Replace("[FranchisePhone]", franchisePhone);
                value = value.Replace("[FranchiseWebsite]", franchiseWebsite);
                value = value.Replace("[FranchiseEmail]", franchiseEmail);
                value = value.Replace("[FranchiseAddress]", franchiseAddress);
                value = value.Replace("[FranchiseCity]", franchiseCity);
                value = value.Replace("[FranchiseState]", franchiseState);
                value = value.Replace("[FranchiseZip]", franchiseZip);
                value = value.Replace(@"https://drive.google.com/file/d/", @"http://drive.google.com/uc?export=view&id=");
                value = value.Replace(@"/view?usp=sharing", @"");
                return value;
            }
            catch { }
            return value;
        }
        #endregion

        #region FormatAppStatus
        public static string FormatAppStatus(int status)
        {
            try
            {
                switch(status)
                {
                    case 0: return "Active";
                    case 1: return "Rescheduled";
                    case 2: return "Canceled";
                }
            }
            catch { }
            return "Unknown";
        }
        #endregion

        #region FormatSubContractorDisc
        public static string FormatSubContractorDisc(string value)
        {
            try
            {
                return string.Join(", ", value.Split('|'));
            }
            catch
            {
                return "Other Services";
            }
        }
        #endregion

        #region BitMaskToIndexList
        public static int[] BitMaskToIndexList(int mask)
        {
            List<int> ret = new List<int>();
            for (int i = 0; i < 32; i++)
            {
                if (((mask >> i) & 1) != 0)
                {
                    ret.Add(i + 1);
                }
            }
            return ret.ToArray();
        }
        #endregion

        #region IndexToServiceType
        public static string IndexToServiceType(int index)
        {
            switch (index)
            {
                case 1: return "Housekeeping";
                case 2: return "Carpet Cleaning";
                case 3: return "Window Washing";
                case 4: return "Home Guard";
                default: return "Unknown";
            }
        }
        #endregion

        #region Within365Days
        public static bool WithinContractYear(DateTime contractDate, DateTime value)
        {
            if (contractDate > new DateTime(1990,1,1))
            {
                if (value >= contractDate && value <= (contractDate + TimeSpan.FromDays(365)))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}