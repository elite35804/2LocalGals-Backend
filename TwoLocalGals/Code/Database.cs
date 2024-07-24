using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Web.UI.WebControls;
using TwoLocalGals.DTO;
using Nexus.Protected;
using TwoLocalGals.Protected;

namespace Nexus
{
    public struct UserStruct
    {
        public string username;
        public string password;
        public int franchiseMask;
        public int access;
        public int contractorID;
    }

    public struct FranchiseStruct
    {
        public int franchiseID;
        public int franchiseMask;
        public string franchiseName;
        public string email;
        public string emailPassword;
        public string emailSmtp;
        public int emailPort;
        public bool emailSecure;
        public string phone;
        public decimal defaultRatePerHour;
        public decimal defaultServiceFee;
        public decimal defaultScheduleFee;
        public decimal rewardsPercentage;
        public decimal carPercentage;
        public decimal suppliesPercentage;
        public decimal salesTax;
        public string address;
        public string city;
        public string state;
        public string zip;
        public string webLink;
        public bool notesEnabled;
        public string notesGeneral;
        public string notesAccounting;
        public DateTime sendSchedules;
        public string ePNAccount;
        public string restrictKey;
        public DateTime batchTime;
        public string advertisementList;
        public string adjustmentList;
        public string paymentList;
        public string partnerCategoryList;
        public string scheduleFeeString;
        public string smsUsername;
        public string smsPassword;
        public string reviewUsLink;
        public string FranchiseImg;
    }

    public struct ContractorStruct
    {
        public int contractorID;
        public int contractorType;
        public int franchiseMask;
        public DateTime dateCreated;
        public string title;
        public string firstName;
        public string lastName;
        public string businessName;
        public string address;
        public string city;
        public string state;
        public string zip;
        public string bestPhone;
        public string alternatePhone;
        public string email;
        public string ssn;
        public string notes;
        public string paymentType;
        public string paymentDay;
        public string team;
        public DateTime startDay;
        public DateTime endDay;
        public decimal score;
        public decimal hourlyRate;
        public decimal serviceSplit;
        public DateTime hireDate;
        public DateTime birthday;
        public DateTime waiverDate;
        public DateTime waiverUpdateDate;
        public DateTime insuranceDate;
        public DateTime insuranceUpdateDate;
        public DateTime backgroundCheck;
        public bool accountRep;
        public bool applicant;
        public bool active;
        public bool scheduled;
        public bool sendSchedules;
        public DateTime lastSchedule;
        public bool sendPayroll;
        public DateTime lastPayroll;
        public int customerFreuqency;
        public bool SendSchedulesByEmail;
        public string ContractorPic;
        public string longitude;
        public string latitude;
        public bool ShareLocation;

    }

    public struct CustomerStruct
    {
        public int franchiseMask;
        public int customerID;
        public int referredBy;
        public int bookedBy;
        public decimal points;
        public DateTime bookedDate;
        public DateTime lastUpdate;
        public string customerTitle;
        public string accountStatus;
        public string accountType;
        public bool newBuilding;
        public int sectionMask;
        public string customNote;
        public string businessName;
        public string companyContact;
        public string firstName;
        public string lastName;
        public string spouseName;
        public string locationAddress;
        public string locationCity;
        public string locationState;
        public string locationZip;
        public bool billingSame;
        public string billingName;
        public string billingAddress;
        public string billingCity;
        public string billingState;
        public string billingZip;
        public string bestPhone;
        public bool bestPhoneCell;
        public string alternatePhoneOne;
        public bool alternatePhoneOneCell;
        public string alternatePhoneTwo;
        public bool alternatePhoneTwoCell;
        public string email;
        public string advertisement;
        public string preferredContact;
        public string paymentType;
        public string creditCardNumber;
        public string creditCardExpMonth;
        public string creditCardExpYear;
        public string creditCardCCV;
        public decimal serviceFee;
        public decimal ratePerHour;
        public decimal discountPercent;
        public decimal discountAmount;
        public string estimatedHours;
        public string estimatedCC;
        public string estimatedWW;
        public string estimatedHW;
        public string estimatedCL;
        public string estimatedPrice;
        public bool rewardsEnabled;
        public bool sendPromotions;
        public bool welcomeLetter;
        public bool loginInfoSent;
        public bool quoteReply;
        public DateTime reviewUsDate;
        public string quoteValue;
        public string NC_Notes;
        public string NC_Special;
        public string NC_Details;
        public string NC_FrequencyDay;
        public string NC_PreferedTime;
        public string NC_Frequency;
        public string NC_DayOfWeek;
        public string NC_TimeOfDayPrefix;
        public string NC_TimeOfDay;
        public string NC_Bathrooms;
        public string NC_Bedrooms;
        public string NC_SquareFootage;
        public bool NC_Vacuum;
        public bool NC_DoDishes;
        public bool NC_ChangeBed;
        public string NC_Pets;
        public bool NC_FlooringCarpet;
        public bool NC_FlooringHardwood;
        public bool NC_FlooringTile;
        public bool NC_FlooringLinoleum;
        public bool NC_FlooringSlate;
        public bool NC_FlooringMarble;
        public string NC_EnterHome;
        public bool NC_RequiresKeys;
        public bool NC_Organize;
        public string NC_CleanRating;
        public string NC_CleaningType;
        public string NC_GateCode;
        public string NC_GarageCode;
        public string NC_DoorCode;
        public bool NC_RequestEcoCleaners;
        public bool DC_Blinds;
        public string DC_BlindsAmount;
        public string DC_BlindsCondition;
        public bool DC_Windows;
        public string DC_WindowsAmount;
        public bool DC_WindowsSills;
        public bool DC_Walls;
        public string DC_WallsDetail;
        public bool DC_Baseboards;
        public bool DC_DoorFrames;
        public bool DC_LightSwitches;
        public bool DC_VentCovers;
        public bool DC_InsideVents;
        public bool DC_Pantry;
        public bool DC_LaundryRoom;
        public bool DC_CeilingFans;
        public string DC_CeilingFansAmount;
        public bool DC_LightFixtures;
        public bool DC_KitchenCuboards;
        public string DC_KitchenCuboardsDetail;
        public bool DC_BathroomCuboards;
        public string DC_BathroomCuboardsDetail;
        public bool DC_Oven;
        public bool DC_Refrigerator;
        public string DC_OtherOne;
        public string DC_OtherTwo;

        public string CC_SquareFootage;
        public string CC_RoomCountSmall;
        public string CC_RoomCountLarge;
        public bool CC_PetOdorAdditive;
        public string CC_Details;
        public string WW_BuildingStyle;
        public string WW_BuildingLevels;
        public bool WW_VaultedCeilings;
        public bool WW_PostConstruction;
        public string WW_WindowCount;
        public string WW_WindowType;
        public string WW_InsidesOutsides;
        public bool WW_Razor;
        public string WW_RazorCount;
        public bool WW_HardWater;
        public string WW_HardWaterCount;
        public bool WW_FrenchWindows;
        public string WW_FrenchWindowCount;
        public bool WW_StormWindows;
        public string WW_StormWindowCount;
        public bool WW_Screens;
        public string WW_ScreensCount;
        public bool WW_Tracks;
        public string WW_TracksCount;
        public bool WW_Wells;
        public string WW_WellsCount;
        public bool WW_Gutters;
        public string WW_GuttersFeet;
        public string WW_Details;
        public string HW_Frequency;
        public string HW_StartDate;
        public string HW_EndDate;
        public bool HW_GarbageCans;
        public string HW_GarbageDay;
        public bool HW_PlantsWatered;
        public string HW_PlantsWateredFrequency;
        public bool HW_Thermostat;
        public string HW_ThermostatTemperature;
        public bool HW_Breakers;
        public string HW_BreakersLocation;
        public bool HW_CleanBeforeReturn;
        public string HW_Details;

        public bool TakePic;
    }


    public struct AppStruct
    {
        public int appointmentID;
        public int appStatus;
        public int appType;
        public int franchiseMask;
        public DateTime dateCreated;
        public DateTime dateUpdated;
        public DateTime appointmentDate;
        public DateTime startTime;
        public DateTime endTime;
        public int customerID;
        public string customerTitle;
        public string customerTitleCustomNote;
        public string customerAccountStatus;
        public decimal customerHours;
        public decimal customerRate;
        public decimal customerServiceFee;
        public decimal customerSubContractor;
        public decimal customerDiscountAmount;
        public decimal customerDiscountPercent;
        public decimal customerDiscountReferral;
        public string customerPaymentType;
        public string customerCardExpMonth;
        public string customerCardExpYear;
        public string customerEmail;
        public string customerPhoneOne;
        public bool customerPhoneOneCell;
        public string customerPhoneTwo;
        public bool customerPhoneTwoCell;
        public string customerPhoneThree;
        public bool customerPhoneThreeCell;
        public string customerPreferredContact;
        public string customerAddress;
        public string customerCity;
        public string customerState;
        public string customerZip;
        public string customerSpecial;
        public string customerTimeOfDayPrefix;
        public string customerTimeOfDay;
        public string customerDayOfWeek;
        public bool customerKeys;
        public int contractorID;
        public int contractorType;
        public string contractorTitle;
        public decimal contractorHours;
        public decimal contractorRate;
        public decimal contractorTips;
        public decimal contractorAdjustAmount;
        public string contractorAdjustType;
        public string contractorPhone;
        public decimal amountPaid;
        public bool paymentFinished;
        public int recurrenceID;
        public int recurrenceType;
        public int weeklyFrequency;
        public int monthlyWeek;
        public int monthlyDay;
        public decimal combinedFee;
        public bool confirmed;
        public bool leftMessage;
        public bool sentSMS;
        public bool sentWeekSMS;
        public bool sentEmail;
        public bool keysReturned;
        public bool followUpSent;
        public bool remove;
        public string username;
        public bool usernameBooked;
        public decimal salesTax;
        public bool ShareLocation;
        public bool JobCompleted;
        public string Notes;
        public DateTime? jobStartTime;
        public DateTime? jobEndTime;
        public string longitude;
        public string latitude;

    }


    public struct AppAttachments
    {
        public int id;
        public int AppointmentId;
        public string ImageURL;
    }

    public struct ConAppStruct
    {
        public int appointmentID;
        public int appType;
        public DateTime appointmentDate;
        public DateTime startTime;
        public DateTime endTime;
        public int contractorID;
        public string contractorTitle;
        public string contractorAddress;
        public string contractorCity;
        public string contractorState;
        public string contractorZip;
        public string contractorPhone;
        public int customerID;
        public string customerTitle;
        public string customerAccountStatus;
        public string paymentType;
        public decimal customerServiceFee;
        public decimal customerRate;
        public decimal customerCollect;
        public string bestPhone;
        public string alternatePhoneOne;
        public string alternatePhoneTwo;
        public string address;
        public string city;
        public string state;
        public string zip;
        public string details;
        public string general;
        public string instructions;
        public bool keys;
    }

    public struct UnavailableStruct
    {
        public int unavailableID;
        public int contractorID;
        public DateTime dateCreated;
        public DateTime dateRequested;
        public DateTime startTime;
        public DateTime endTime;
        public int recurrenceID;
        public int recurrenceType;
        public string contractorTitle;
        public bool contractorActive;
        public bool contractorScheduled;
    }

    public struct QuoteTemplate
    {
        public int franchiseMask;
        public string templateName;
        public string templateValue;
    }

    public struct TransactionStruct
    {
        public int transID;
        public string transType;
        public string paymentID;
        public string paymentType;
        public string lastFourCard;
        public int auth;
        public bool batched;
        public int customerID;
        public DateTime dateCreated;
        public DateTime dateApply;
        public bool isVoid;
        public decimal total;
        public decimal hoursBilled;
        public decimal hourlyRate;
        public decimal serviceFee;
        public decimal subContractorCC;
        public decimal subContractorWW;
        public decimal subContractorHW;
        public decimal subContractorCL;
        public decimal discountAmount;
        public decimal discountPercent;
        public decimal discountReferral;
        public decimal tips;
        public decimal salesTax;
        public string email;
        public bool emailSent;
        public string notes;
        public string username;
        public string customerTitle;
        public string customerFirstName;
        public string customerLastName;
        public string customerBusinessName;
        public string customerAddress;
        public string customerCity;
        public string customerState;
        public string customerZip;
        public string customerAccountStatus;

        public bool IsAuth()
        {
            return (auth & 1) > 0;
        }
    }

    public struct ServiceRequestStruct
    {
        public int serviceRequestID;
        public int customerID;
        public DateTime dateCreated;
        public DateTime requestDate;
        public string timePrefix;
        public string timeSuffix;
        public string notes;
    }

    public struct FollowUpStruct
    {
        public int appointmentID;
        public DateTime createdOn;
        public int schedulingSatisfaction;
        public int timeManagement;
        public int professionalism;
        public int cleaningQuality;
        public string notes;
    }

    public struct GiftCardStruct
    {
        public int giftCardID;
        public int customerID;
        public string paymentType;
        public string paymentID;
        public string lastFourCard;
        public bool isVoid;
        public bool batched;
        public DateTime dateCreated;
        public decimal amount;
        public int redeemed;
        public string giverName;
        public string recipientName;
        public string recipientEmail;
        public string billingEmail;
        public string message;
        public string customerTitle;
        public string redeemedTitle;
        public string username;
    }

    public struct DrivingRoute
    {
        public decimal distance;
        public decimal travelTime;
    }


    public struct JobLogsStruct
    {
        public int id;
        public int ContractorId;
        public int CustomerId;
        public int AppointmentId;
        public string Content;
        public bool Checked;
        public string CheckedBy;
        public DateTime CreatedAt;
        public bool IsGeneral;
    }

    #region DBRow
    public class DBRow
    {
        public Dictionary<string, object> valueDict = new Dictionary<string, object>();

        public DBRow SetValue(string key, object value, bool overrideValue = false)
        {
            try
            {
                if (!valueDict.ContainsKey(key))
                    valueDict.Add(key, value == DBNull.Value ? null : value);
                else if (overrideValue)
                    valueDict[key] = value == DBNull.Value ? null : value;
            }
            catch { }
            return this;
        }

        public object this[string key]
        {
            get
            {
                try { if (valueDict.ContainsKey(key)) return valueDict[key]; }
                catch { }
                return null;
            }
        }

        public int GetInt(string key)
        {
            try { if (valueDict.ContainsKey(key)) return (int)valueDict[key]; }
            catch { }
            return 0;
        }

        public decimal GetDecimal(string key)
        {
            try { if (valueDict.ContainsKey(key)) return (decimal)valueDict[key]; }
            catch { }
            return 0;
        }

        public string GetString(string key)
        {
            try { if (valueDict.ContainsKey(key)) return Globals.SafeSqlString(valueDict[key]); }
            catch { }
            return "";
        }

        public DateTime GetDate(string key)
        {
            try { if (valueDict.ContainsKey(key)) return (DateTime)valueDict[key]; }
            catch { }
            return DateTime.MinValue;
        }

        public bool GetBool(string key)
        {
            try { if (valueDict.ContainsKey(key)) return (bool)valueDict[key]; }
            catch { }
            return false;
        }

        public string GetSqlUpdateString()
        {
            string ret = "";
            foreach (string key in valueDict.Keys)
            {
                if (ret == "") ret = key + " = @" + key;
                else ret += "," + key + "= @" + key;
            }
            return ret;
        }

        public string GetSqlInsertColumnsString()
        {
            return "(" + string.Join(",", valueDict.Keys) + ")";
        }

        public string GetSqlInsertValuesString()
        {
            return "(@" + string.Join(",@", valueDict.Keys) + ")";
        }
    }
    #endregion

    public class Database
    {
        //COPY DATABASE
        /*#if DEBUG
        public static string connString = Globals.Decrypt("0Yoi9E9/xA4R0LZj/J+HMP4pg3xt6BywZcms2j+UVXfyhW+YPibTO17xbSk9JvgE75re0kSfn9VOanDhS+p/OuejnzAv6hIRDvLV7XLtjLw="); //HostWay Extern
        #else
        public static string connString = Globals.Decrypt("VBUCrHUIHuNWAltYOicnJS5UiM7KB+c1eCu45gNJeL8ykTtN+XQ94Vn0dE7wtT86VQVysSIjG0w1pY4yC90slFeVlMA8GoKEI7IRR0HPwE0="); //HostWay Intern
        #endif*/


        //HOSTWAY DATABASE
        /*
        public static string connString = Globals.Decrypt("0Yoi9E9/xA4R0LZj/J+HMP4pg3xt6BywZcms2j+UVXfyhW+YPibTO17xbSk9JvgENbq6LOTfwsf7n3LcAM4CAQ=="); //HostWay Extern
        #else
        public static string connString = Globals.Decrypt("VBUCrHUIHuNWAltYOicnJS5UiM7KB+c1eCu45gNJeL8ykTtN+XQ94Vn0dE7wtT86wNl0msas60C/Opoh6oA+Xg=="); //HostWay Intern
        */

        //AZURE DATABASE
#if DEBUG
        //public static string connString = Globals.Decrypt("LKPNvjf2Ut3VRkydHYCJvBx2melCfKdSrcAn58Qzs/RzQoNJ70YI/75WfHHX1Trz4PiK0NydHhws6i6Y7euUSkwRtASDws0bw5W/NxftKg+SplxEhVb4RV7lenmdFMXwbWe5v66zzl+Bb6LuULNZNCsSTRVo2i5xO7Eunlqa6483anHenMmI6z6a8nxNhxFO87c2/Ug8jN3RIYGyNg46y7Eki969FX0Din0UCxArvK7al983duFeeVCS5is0nuS8F/T7APQnlbma1TnluK2pN5OcRsI1ngyUSO9QSA8y1wUukKAVK0OpCL1gt5IffocMe89kDJkh8/Un/IYrMvh84w==");
        public static string connString = Globals.Decrypt("eP3OLvp9ZfQlXtsgnHrCRLDqmOkp4OnnU4R13/kYnztlhzwEXPKpZwma14QjDIk5WUx2MNHSfclRfO74H+cEVDBowufqWs5LYhG8X5tpn8GzJmf6FUul8SV63hxelDhp5YavHvn8uf2Qito/32AaIDIgynPDMp17RUm+jvIz7PTcGo+s9fKjEeMomw1/tKBXM0nCJseOEIThl8pWZTCofw==");




#else
        public static string connString = Globals.Decrypt("LKPNvjf2Ut3VRkydHYCJvOfH68lcPvIJz/tUAsYk5wkT+6+kso0UNlQR13HuH7idtfcxrvCG9v7GeMU6BVcrY8SIoLX9KGThjK1yk1QAxWer/ScgdGiDLq2v93vevMf5GjsZOr+xjxrTA9A8CCtujQE2VAROvDtiqD6yDSDfNgpNN+DufaCr/nmaIwQXVciSM9cxWskh950v3/g+FNtHm9ErF8QYqsJhs4qEiDjifolIE6PR0dC+wIzk+XpQofJrfuHMGznGBn59Em3TJRHeFyXdO3Jkdvk2WpNgZ3W0oZ3VPEEVsm5eDVmTk/IX2UGUJH65jQCkuEA6i6j7SvRDUw==");
#endif

        #region LogThis
        public static void LogThis(string message, Exception ex)
        {
            SqlConnection sqlConnection = null;
            try
            {
                DateTime mst = Globals.UtcToMst(DateTime.UtcNow);
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                if (ex != null) message += ", EX: " + ex.Message;

#if DEBUG
                Debug.WriteLine(mst.ToString() + " v" + version + ": " + message);
#else
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
	            INSERT INTO AppLog 
		            (dateCreated,
                    version,
                    message)
	            VALUES
		            (@dateCreated,
                    @version,
                    @message)";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"dateCreated", mst));
                cmd.Parameters.Add(new SqlParameter(@"version", version));
                cmd.Parameters.Add(new SqlParameter(@"message", message));
                cmd.ExecuteNonQuery();
#endif
            }
            catch { }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetDBRows
        private static List<DBRow> GetDBRows(SqlDataReader sqlDataReader)
        {
            List<DBRow> rowList = new List<DBRow>();
            while (sqlDataReader.Read())
            {
                DBRow row = new DBRow();
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    string key = sqlDataReader.GetName(i);
                    object value = sqlDataReader.GetValue(i);
                    row.SetValue(key, value);
                }
                rowList.Add(row);
            }
            return rowList;
        }
        #endregion

        #region GetDBRow
        public static DBRow GetDBRow(SqlDataReader sqlDataReader)
        {
            if (sqlDataReader.Read())
            {
                DBRow row = new DBRow();
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    string key = sqlDataReader.GetName(i);
                    object value = sqlDataReader.GetValue(i);
                    row.SetValue(key, value);
                }
                return row;
            }
            return null;
        }
        #endregion

        #region SetDBRows
        private static void SetDBRows(SqlCommand cmd, DBRow row)
        {
            foreach (string key in row.valueDict.Keys)
            {
                object value = row[key];
                cmd.Parameters.Add(new SqlParameter(key, value ?? DBNull.Value));
            }
        }
        #endregion

        #region DynamicSetWithKey
        public static string DynamicSetWithKeyInt(string tableName, string keyName, ref int keyValue, DBRow row)
        {
            object keyValueObject = (object)keyValue;
            string ret = DynamicSetWithKey(tableName, keyName, ref keyValueObject, row);
            keyValue = (int)keyValueObject;
            return ret;
        }
        public static string DynamicSetWithKeyString(string tableName, string keyName, ref string keyValue, DBRow row)
        {
            object keyValueObject = (object)keyValue;
            string ret = DynamicSetWithKey(tableName, keyName, ref keyValueObject, row);
            keyValue = (string)keyValueObject;
            return ret;
        }
        public static string DynamicSetWithKey(string tableName, string keyName, ref object keyValue, DBRow row)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    IF EXISTS (SELECT * FROM [{0}] WHERE {2} = @keyValue)
                    BEGIN
	                    UPDATE 
		                    [{0}]
	                    SET
                            {1}
                        OUTPUT INSERTED.{2}
	                    WHERE
		                    {2} = @keyValue;
                    END
                    ELSE
                    BEGIN 
	                    INSERT INTO [{0}] 
		                    {3}
                        OUTPUT INSERTED.{2}
                        VALUES
                            {4};
                    END";

                cmdText = string.Format(cmdText, tableName, row.GetSqlUpdateString(), keyName, row.GetSqlInsertColumnsString(), row.GetSqlInsertValuesString());

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("keyValue", keyValue));
                SetDBRows(cmd, row);
                sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                    keyValue = (object)sqlDataReader[keyName];
                return null;
            }
            catch (Exception ex)
            {
                return "SQL DynamicSetWithKey EX: " + ex.Message;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region DynamicDeleteWithKey
        public static string DynamicDeleteWithKey(string tableName, string keyName, object keyValue)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    DELETE FROM
                        [{0}]
	                WHERE
		                {1} = @keyValue";

                cmdText = string.Format(cmdText, tableName, keyName);
                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("keyValue", keyValue));
                cmd.ExecuteNonQuery();
                return null;
            }
            catch (Exception ex)
            {
                return "SQL DynamicDeleteWithKey EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region ValidateUser
        public static UserStruct ValidateUser(string username, string password)
        {
            UserStruct ret = new UserStruct();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        username,
                        franchiseMask,
                        access,
                        contractorID
                    FROM
                        Users
                    WHERE
                        LOWER(username) = LOWER(@username) AND
                        password = @password COLLATE Latin1_General_CS_AS";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("username", username));
                cmd.Parameters.Add(new SqlParameter("password", (object)password ?? DBNull.Value));
                sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    ret.username = (string)sqlDataReader["username"];
                    ret.franchiseMask = (int)sqlDataReader["franchiseMask"];
                    ret.access = (int)sqlDataReader["access"];
                    ret.contractorID = sqlDataReader["contractorID"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorID"];
                }
            }
            catch (Exception ex)
            {
                ret = new UserStruct();
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region GetUserList
        public static List<UserStruct> GetUserList()
        {
            List<UserStruct> ret = new List<UserStruct>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    Users.username,
                    Users.access,
                    Users.contractorID,
	                Users.franchiseMask AS userMask,
	                Contractors.franchiseMask AS contractorMask
                FROM
                    Users LEFT OUTER JOIN Contractors ON Users.contractorID = Contractors.contractorID
                ORDER BY
                    username";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    UserStruct user = new UserStruct();
                    user.username = (string)sqlDataReader["username"];
                    user.access = (int)sqlDataReader["access"];
                    user.contractorID = sqlDataReader["contractorID"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorID"];
                    user.franchiseMask = (int)sqlDataReader["userMask"];
                    if (user.franchiseMask == -1)
                        user.franchiseMask = sqlDataReader["contractorMask"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorMask"];
                    ret.Add(user);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion
        #region GetUserByID
        public static UsersDTO GetUserById(string userId)
        {

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            UsersDTO user = new UsersDTO();

            try
            {

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    Users.username,
                    Users.access,
                    Users.contractorID,
	                Users.franchiseMask AS userMask,
	                Contractors.franchiseMask AS contractorMask
                FROM
                    Users
LEFT OUTER JOIN Contractors ON Users.contractorID = Contractors.contractorID
                where Users.username = " + $"'{userId}'";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    user.username = (string)sqlDataReader["username"];
                    user.access = (int)sqlDataReader["access"];
                    user.contractorID = sqlDataReader["contractorID"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorID"];
                    user.franchiseMask = (int)sqlDataReader["userMask"];
                    if (user.franchiseMask == -1)
                        user.franchiseMask = sqlDataReader["contractorMask"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorMask"];
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return user;
        }
        #endregion

        #region SetUser
        public static string SetUser(string username, UserStruct user)
        {
            SqlConnection sqlConnection = null;
            try
            {
                if (user.username == "")
                    return "Username Cannot be Blank";

                if (username == null && user.password == "")
                    return "Password Cannot be Blank";

                if (user.password != "" && user.password.Length < 6)
                    return "Password must be at least 6 charcters long";

                if (user.franchiseMask == 0)
                    return "You must assign at least one franchise to user";

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    IF @usernameKey IS NOT NULL
                    BEGIN
	                    UPDATE 
		                    Users
	                    SET
                            username = @username,
                            password = (CASE WHEN @password IS NULL THEN password ELSE @password END),
                            access = @access,
                            franchiseMask = @franchiseMask,
                            contractorID = @contractorID
	                    WHERE
		                    username = @usernameKey
                    END
                    ELSE
                    BEGIN 
	                    INSERT INTO Users 
		                    (username,
                            password,
                            access,
                            franchiseMask,
                            contractorID)
	                    VALUES
		                    (@username,
                            @password,
                            @access,
                            @franchiseMask,
                            @contractorID)
                    END";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"usernameKey", (object)username ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"username", user.username));
                cmd.Parameters.Add(new SqlParameter(@"password", user.password == "" ? DBNull.Value : (object)user.password));
                cmd.Parameters.Add(new SqlParameter(@"access", user.access));
                cmd.Parameters.Add(new SqlParameter(@"franchiseMask", user.franchiseMask));
                cmd.Parameters.Add(new SqlParameter(@"contractorID", user.contractorID == 0 ? DBNull.Value : (object)user.contractorID));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("PK_Accounts")) return "Username already exists, cannot insert duplicates";
                return "SQL SetUser EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region DeleteUser
        public static string DeleteUser(string username)
        {
            SqlConnection sqlConnection = null;
            try
            {
                if (username == null || username == "")
                    return "Cannot Delete New User";

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"DELETE FROM Users WHERE username = @username";
                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"username", username));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL DeleteUser EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetFranchiseList
        public static List<FranchiseStruct> GetFranchiseList()
        {
            List<FranchiseStruct> ret = new List<FranchiseStruct>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        Franchise
                    ORDER BY
                        franchiseName";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    FranchiseStruct franchise = new FranchiseStruct();
                    franchise.franchiseID = (int)sqlDataReader["franchiseID"];
                    franchise.franchiseMask = Globals.IDToMask(franchise.franchiseID);
                    franchise.franchiseName = Globals.SafeSqlString(sqlDataReader["franchiseName"]);
                    franchise.email = Globals.SafeSqlString(sqlDataReader["email"]);
                    franchise.emailSmtp = Globals.SafeSqlString(sqlDataReader["emailSmtp"]);
                    franchise.emailPassword = Globals.SafeSqlString(sqlDataReader["emailPassword"]);
                    franchise.emailPort = (int)sqlDataReader["emailPort"];
                    franchise.emailSecure = (bool)sqlDataReader["emailSSL"];
                    franchise.phone = Globals.SafeSqlString(sqlDataReader["phone"]);
                    franchise.defaultRatePerHour = (decimal)sqlDataReader["defaultRatePerHour"];
                    franchise.defaultServiceFee = (decimal)sqlDataReader["defaultServiceFee"];
                    franchise.defaultScheduleFee = (decimal)sqlDataReader["defaultScheduleFee"];
                    franchise.rewardsPercentage = (decimal)sqlDataReader["rewardsPercentage"];
                    franchise.carPercentage = (decimal)sqlDataReader["carPercentage"];
                    franchise.suppliesPercentage = (decimal)sqlDataReader["suppliesPercentage"];
                    franchise.salesTax = (decimal)sqlDataReader["salesTax"];
                    franchise.address = Globals.SafeSqlString(sqlDataReader["address"]);
                    franchise.city = Globals.SafeSqlString(sqlDataReader["city"]);
                    franchise.state = Globals.SafeSqlString(sqlDataReader["defaultState"]);
                    franchise.zip = Globals.SafeSqlString(sqlDataReader["zip"]);
                    franchise.webLink = Globals.SafeSqlString(sqlDataReader["webLink"]);
                    franchise.notesEnabled = (bool)sqlDataReader["notesEnabled"];
                    franchise.notesGeneral = Globals.SafeSqlString(sqlDataReader["notesGeneral"]);
                    franchise.notesAccounting = Globals.SafeSqlString(sqlDataReader["notesAccounting"]);
                    franchise.sendSchedules = Globals.TimeOnly((DateTime)sqlDataReader["sendSchedules"]);
                    franchise.ePNAccount = Globals.SafeSqlString(sqlDataReader["ePNAccount"]);
                    franchise.restrictKey = Globals.SafeSqlString(sqlDataReader["restrictKey"]);
                    franchise.batchTime = Globals.TimeOnly((DateTime)sqlDataReader["batchTime"]);
                    franchise.advertisementList = Globals.SafeSqlString(sqlDataReader["advertisementList"]);
                    franchise.adjustmentList = Globals.SafeSqlString(sqlDataReader["adjustmentList"]);
                    franchise.paymentList = Globals.SafeSqlString(sqlDataReader["paymentList"]);
                    franchise.partnerCategoryList = Globals.SafeSqlString(sqlDataReader["partnerCategoryList"]);
                    franchise.scheduleFeeString = Globals.SafeSqlString(sqlDataReader["scheduleFeeString"]);
                    franchise.smsUsername = Globals.SafeSqlString(sqlDataReader["smsUsername"]);
                    franchise.smsPassword = Globals.SafeSqlString(sqlDataReader["smsPassword"]);
                    franchise.reviewUsLink = Globals.SafeSqlString(sqlDataReader["reviewUsLink"]);
                    franchise.FranchiseImg = Globals.SafeSqlString(sqlDataReader["FranchiseImg"]);
                    ret.Add(franchise);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region SetFranchise
        public static string SetFranchise(int franchiseID, ref FranchiseStruct franchise)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {

                if (string.IsNullOrEmpty(franchise.franchiseName))
                    return "Franchise Name Cannot be Blank";

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    IF @franchiseID > 0
                    BEGIN
	                    UPDATE 
		                    Franchise
	                    SET
                            franchiseName = @franchiseName,
                            email = @email,
                            emailSmtp = @emailSmtp,
                            emailPassword = (CASE WHEN @emailPassword IS NULL THEN emailPassword ELSE @emailPassword END),
                            emailPort = @emailPort,
                            emailSSL = @emailSSL,
                            phone = @phone,
                            defaultRatePerHour = @defaultRatePerHour,
                            defaultServiceFee = @defaultServiceFee,
                            defaultScheduleFee = @defaultScheduleFee,
                            rewardsPercentage = @rewardsPercentage,
                            carPercentage = @carPercentage,
                            suppliesPercentage = @suppliesPercentage,
                            salesTax = @salesTax,
                            address = @address,
                            city = @city,
                            defaultState = @defaultState,
                            zip = @zip,
                            webLink = @webLink,
                            notesEnabled = @notesEnabled,
                            sendSchedules = @sendSchedules,
                            ePNAccount = @ePNAccount,
                            restrictKey = @restrictKey,
                            batchTime = @batchTime,
                            advertisementList = @advertisementList,
                            adjustmentList = @adjustmentList,
                            paymentList = @paymentList,
                            partnerCategoryList = @partnerCategoryList,
                            scheduleFeeString = @scheduleFeeString,
                            smsUsername = @smsUsername,
                            smsPassword = @smsPassword,
                            reviewUsLink = @reviewUsLink,
                            FranchiseImg =@FranchiseImg
	                    WHERE
		                    franchiseID = @franchiseID;
                        SELECT @franchiseID AS franchiseID;
                    END
                    ELSE
                    BEGIN
                        SET @franchiseID = (SELECT MAX(franchiseID) + 1 FROM Franchise);
	                    INSERT INTO Franchise 
		                    (franchiseID,
                            franchiseName,
                            email,
                            emailSmtp,
                            emailPassword,
                            emailPort,
                            emailSSL,
                            phone,
                            defaultRatePerHour,
                            defaultServiceFee,
                            defaultScheduleFee,
                            rewardsPercentage,
                            carPercentage,
                            suppliesPercentage,
                            salesTax,
                            address,
                            city,
                            defaultState,
                            zip,
                            webLink,
                            notesEnabled,
                            sendSchedules,
                            ePNAccount,
                            restrictKey,
                            batchTime,
                            advertisementList,
                            adjustmentList,
                            paymentList,
                            partnerCategoryList,
                            scheduleFeeString,
                            smsUsername,
                            smsPassword,
                            reviewUsLink,
FranchiseImg)
	                    VALUES
                            (@franchiseID,
                            @franchiseName,
                            @email,
                            @emailSmtp,
                            @emailPassword,
                            @emailPort,
                            @emailSSL,
                            @phone,
                            @defaultRatePerHour,
                            @defaultServiceFee,
                            @defaultScheduleFee,
                            @rewardsPercentage,
                            @carPercentage,
                            @suppliesPercentage,
                            @salesTax,
                            @address,
                            @city,
                            @defaultState,
                            @zip,
                            @webLink,
                            @notesEnabled,
                            @sendSchedules,
                            @ePNAccount,
                            @restrictKey,
                            @batchTime,
                            @advertisementList,
                            @adjustmentList,
                            @paymentList,
                            @partnerCategoryList,
                            @scheduleFeeString,
                            @smsUsername,
                            @smsPassword,
                            @reviewUsLink,
@FranchiseImg)
                        SELECT @franchiseID AS franchiseID;
                    END";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"franchiseID", franchiseID));
                cmd.Parameters.Add(new SqlParameter(@"franchiseName", (object)franchise.franchiseName ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"email", (object)franchise.email ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"emailSmtp", (object)franchise.emailSmtp ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"emailPassword", (object)franchise.emailPassword ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"emailPort", franchise.emailPort));
                cmd.Parameters.Add(new SqlParameter(@"emailSSL", franchise.emailSecure));
                cmd.Parameters.Add(new SqlParameter(@"phone", (object)franchise.phone ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"defaultRatePerHour", franchise.defaultRatePerHour));
                cmd.Parameters.Add(new SqlParameter(@"defaultServiceFee", franchise.defaultServiceFee));
                cmd.Parameters.Add(new SqlParameter(@"defaultScheduleFee", franchise.defaultScheduleFee));
                cmd.Parameters.Add(new SqlParameter(@"rewardsPercentage", franchise.rewardsPercentage));
                cmd.Parameters.Add(new SqlParameter(@"carPercentage", franchise.carPercentage));
                cmd.Parameters.Add(new SqlParameter(@"suppliesPercentage", franchise.suppliesPercentage));
                cmd.Parameters.Add(new SqlParameter(@"salesTax", franchise.salesTax));
                cmd.Parameters.Add(new SqlParameter(@"address", (object)franchise.address ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"city", (object)franchise.city ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"defaultState", (object)franchise.state ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"zip", (object)franchise.zip ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"webLink", (object)franchise.webLink ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"notesEnabled", franchise.notesEnabled));
                cmd.Parameters.Add(new SqlParameter(@"sendSchedules", franchise.sendSchedules));
                cmd.Parameters.Add(new SqlParameter(@"ePNAccount", (object)franchise.ePNAccount ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"restrictKey", (object)franchise.restrictKey ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"batchTime", franchise.batchTime));
                cmd.Parameters.Add(new SqlParameter(@"advertisementList", (object)franchise.advertisementList ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"adjustmentList", (object)franchise.adjustmentList ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"paymentList", (object)franchise.paymentList ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"partnerCategoryList", (object)franchise.partnerCategoryList ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"scheduleFeeString", (object)franchise.scheduleFeeString ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"smsUsername", (object)franchise.smsUsername ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"smsPassword", (object)franchise.smsPassword ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"reviewUsLink", (object)franchise.reviewUsLink ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"FranchiseImg", (object)franchise.FranchiseImg ?? DBNull.Value));

                sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                    franchise.franchiseID = (int)sqlDataReader["franchiseID"];

                return null;
            }
            catch (Exception ex)
            {
                return "SQL SetFranchise EX: " + ex.Message;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetFranchiseServiceMask
        public static int GetFranchiseServiceMask(int franchiseMask)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    contractorType
                FROM 
                    Contractors
                WHERE
                    franchiseMask & @franchiseMask > 0
                GROUP BY
                    contractorType";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                sqlDataReader = cmd.ExecuteReader();

                int mask = 0;
                while (sqlDataReader.Read())
                {
                    mask |= (int)sqlDataReader["contractorType"];
                }
                return mask;
            }
            catch
            {
                return 0;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region CopyQuoteTemplates
        public static string CopyQuoteTemplates(int franchiseSrcID, int franchiseDstID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                if (franchiseSrcID <= 0)
                    return "Invalid Source Franchise";
                if (franchiseDstID <= 0)
                    return "Invalid Destination Franchise";

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    INSERT INTO 
                        QuoteTemplate (templateName, franchiseMask, templateValue)
                    SELECT 
                        templateName, @dstMask, templateValue
                    FROM 
                        QuoteTemplate
                    WHERE 
                        franchiseMask = @srcMask";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("srcMask", Globals.IDToMask(franchiseSrcID)));
                cmd.Parameters.Add(new SqlParameter("dstMask", Globals.IDToMask(franchiseDstID)));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL CopyQuoteTemplates EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetFranchiseDropDown
        public static List<string> GetFranchiseDropDown(int franchiseMask, string columnName)
        {
            List<string> ret = new List<string>();
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        {0}
                    FROM
                        Franchise
                    WHERE 
                        Power(2,(franchiseID-1)) & @franchiseMask > 0";

                cmdText = string.Format(cmdText, columnName);

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    foreach (string value in Globals.SafeSqlString(sqlDataReader[columnName]).Split('|'))
                    {
                        if (!string.IsNullOrEmpty(value) && !ret.Contains(value))
                            ret.Add(value);
                    }
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region UpdateFranchiseNotes
        public static string UpdateFranchiseNotes(FranchiseStruct franchise)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE 
		            Franchise
	            SET
                    notesGeneral = @notesGeneral,
                    notesAccounting = @notesAccounting
	            WHERE
		            franchiseID = @franchiseID;";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"franchiseID", franchise.franchiseID));
                cmd.Parameters.Add(new SqlParameter(@"notesGeneral", (object)franchise.notesGeneral ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"notesAccounting", (object)franchise.notesAccounting ?? DBNull.Value));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL UpdateFranchiseNotes EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region DeleteFranchise
        public static string DeleteFranchise(int franchiseID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                if (franchiseID <= 0)
                    return "Cannot Delete New User";

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"DELETE FROM Franchise WHERE franchiseID = @franchiseID";
                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"franchiseID", franchiseID));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL DeleteFranchise EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region SearchCustomerList
        public static List<CustomerStruct> SearchCustomerList(int franchiseMask, string searchString)
        {
            List<CustomerStruct> customerList = new List<CustomerStruct>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT TOP 200
                        customerID,
                        businessName,
                        firstName,
                        lastName,
                        bestPhone,
                        alternatePhoneOne,
                        alternatePhoneTwo,
                        email,
                        locationAddress
                    FROM
                        Customers
                    WHERE
                        franchiseMask & @franchiseMask > 0 AND
                        CONCAT(firstName,lastName,businessName,bestPhone,alternatePhoneOne,alternatePhoneTwo,email,locationAddress,customerID) LIKE @searchString
                    ORDER BY
                         firstName, lastName, businessName, bestPhone, alternatePhoneOne, alternatePhoneTwo, email, locationAddress";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("searchString", "%" + searchString + "%"));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    CustomerStruct customer = new CustomerStruct();
                    customer.customerID = (int)sqlDataReader["customerID"];
                    customer.customerTitle = Globals.FormatCustomerTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                    customer.bestPhone = Globals.SafeSqlString(sqlDataReader["bestPhone"]);
                    customer.alternatePhoneOne = Globals.SafeSqlString(sqlDataReader["alternatePhoneOne"]);
                    customer.alternatePhoneTwo = Globals.SafeSqlString(sqlDataReader["alternatePhoneTwo"]);
                    customer.email = Globals.SafeSqlString(sqlDataReader["email"]);
                    customer.locationAddress = Globals.SafeSqlString(sqlDataReader["locationAddress"]);
                    customerList.Add(customer);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return customerList;
        }
        #endregion

        #region GetCustomerById

        public static JobInfo GetJobInfoCustomerById(int CustomerId)
        {
            JobInfo customer = new JobInfo();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
businessName,
                        firstName,
                        lastName,
                        bestPhone,
bestPhoneCell,
                        alternatePhoneOne,
alternatePhoneOneCell,
                        alternatePhoneTwo,
alternatePhoneTwoCell

                    FROM
                        Customers
                    WHERE
                       CustomerId = " + $"'{CustomerId}'";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    customer.customerName = Globals.FormatCustomerTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                    customer.bestPhone = Globals.SafeSqlString(sqlDataReader["bestPhone"]);
                    customer.alternatePhoneOne = Globals.SafeSqlString(sqlDataReader["alternatePhoneOne"]);
                    customer.alternatePhoneTwo = Globals.SafeSqlString(sqlDataReader["alternatePhoneTwo"]);
                    customer.bestPhoneCell = Convert.ToBoolean(sqlDataReader["bestPhoneCell"]);
                    customer.alternatePhoneTwoCell = Convert.ToBoolean(sqlDataReader["alternatePhoneTwoCell"]);
                    customer.alternatePhoneOneCell = Convert.ToBoolean(sqlDataReader["alternatePhoneOneCell"]);
                }
            }
            catch (Exception ex) { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return customer;
        }




        #endregion


        #region GetContractorList
        public static List<ContractorStruct> GetContractorList(int franchiseMask, int contractorType, bool onlyReps, bool onlyActive, bool onlyScheduled, bool showApplicants, string orderBy = "firstName, lastName")
        {
            List<ContractorStruct> ret = new List<ContractorStruct>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        Contractors
                    WHERE
                        franchiseMask & @franchiseMask > 0 AND
                        (@onlyReps = 0 OR accountRep = 1) AND
                        (@onlyActive = 0 OR active = 1) AND
                        (@onlyScheduled = 0 OR scheduled = 1) AND
                        (@showApplicants = 1 OR applicant = 0) AND
                        contractorType & @contractorType > 0
                    ORDER BY
                        " + orderBy;

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("contractorType", contractorType));
                cmd.Parameters.Add(new SqlParameter("onlyReps", onlyReps));
                cmd.Parameters.Add(new SqlParameter("onlyActive", onlyActive));
                cmd.Parameters.Add(new SqlParameter("onlyScheduled", onlyScheduled));
                cmd.Parameters.Add(new SqlParameter("showApplicants", showApplicants));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    ContractorStruct contractor = new ContractorStruct();
                    contractor.contractorID = (int)sqlDataReader["contractorID"];
                    contractor.contractorType = (int)sqlDataReader["contractorType"];
                    contractor.franchiseMask = (int)sqlDataReader["franchiseMask"];
                    contractor.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                    contractor.title = Globals.FormatContractorTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                    contractor.firstName = Globals.SafeSqlString(sqlDataReader["firstName"]);
                    contractor.lastName = Globals.SafeSqlString(sqlDataReader["lastName"]);
                    contractor.businessName = Globals.SafeSqlString(sqlDataReader["businessName"]);
                    contractor.address = Globals.SafeSqlString(sqlDataReader["address"]);
                    contractor.city = Globals.SafeSqlString(sqlDataReader["city"]);
                    contractor.state = Globals.SafeSqlString(sqlDataReader["state"]);
                    contractor.zip = Globals.SafeSqlString(sqlDataReader["zip"]);
                    contractor.bestPhone = Globals.SafeSqlString(sqlDataReader["bestPhone"]);
                    contractor.alternatePhone = Globals.SafeSqlString(sqlDataReader["alternatePhone"]);
                    contractor.email = Globals.SafeSqlString(sqlDataReader["email"]);
                    contractor.ssn = Globals.SafeSqlString(sqlDataReader["ssn"]);
                    contractor.paymentType = Globals.SafeSqlString(sqlDataReader["paymentType"]);
                    contractor.paymentDay = Globals.SafeSqlString(sqlDataReader["paymentDay"]);
                    contractor.notes = Globals.SafeSqlString(sqlDataReader["notes"]);
                    contractor.team = Globals.SafeSqlString(sqlDataReader["team"]);
                    contractor.startDay = (DateTime)sqlDataReader["startDay"];
                    contractor.endDay = (DateTime)sqlDataReader["endDay"];
                    contractor.score = (decimal)sqlDataReader["score"];
                    contractor.hourlyRate = (decimal)sqlDataReader["hourlyRate"];
                    contractor.serviceSplit = (decimal)sqlDataReader["serviceSplit"];
                    contractor.hireDate = (DateTime)sqlDataReader["hireDate"];
                    contractor.birthday = (DateTime)sqlDataReader["birthday"];
                    contractor.waiverDate = (DateTime)sqlDataReader["waiverDate"];
                    contractor.waiverUpdateDate = (DateTime)sqlDataReader["waiverUpdateDate"];
                    contractor.insuranceDate = (DateTime)sqlDataReader["insuranceDate"];
                    contractor.insuranceUpdateDate = (DateTime)sqlDataReader["insuranceUpdateDate"];
                    contractor.backgroundCheck = (DateTime)sqlDataReader["backgroundCheck"];
                    contractor.accountRep = (bool)sqlDataReader["accountRep"];
                    contractor.applicant = (bool)sqlDataReader["applicant"];
                    contractor.active = (bool)sqlDataReader["active"];
                    contractor.scheduled = (bool)sqlDataReader["scheduled"];
                    contractor.sendSchedules = (bool)sqlDataReader["sendSchedules"];
                    contractor.lastSchedule = (DateTime)sqlDataReader["lastSchedule"];
                    contractor.sendPayroll = (bool)sqlDataReader["sendPayroll"];
                    contractor.lastPayroll = (DateTime)sqlDataReader["lastPayroll"];
                    contractor.SendSchedulesByEmail = (bool)sqlDataReader["SendSchedulesByEmail"];
                    contractor.ContractorPic = Convert.ToString(sqlDataReader["ContractorPic"]);
                    contractor.longitude = Convert.ToString(sqlDataReader["longitude"]);
                    contractor.latitude = Convert.ToString(sqlDataReader["latitude"]);
                    contractor.ShareLocation = (bool)sqlDataReader["ShareLocation"];

                    ret.Add(contractor);
                }
            }
            catch (Exception ex) {
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

            return ret;
        }
        #endregion

        #region GetContractorByID
        public static ContractorStruct GetContractorByID(int franchiseMask, int contractorID)
        {
            ContractorStruct contractor = new ContractorStruct();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        Contractors
                    WHERE
                        franchiseMask & @franchiseMask > 0 AND
                        @contractorID = contractorID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("contractorID", contractorID));
                sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    contractor.contractorID = (int)sqlDataReader["contractorID"];
                    contractor.contractorType = (int)sqlDataReader["contractorType"];
                    contractor.franchiseMask = (int)sqlDataReader["franchiseMask"];
                    contractor.title = Globals.FormatContractorTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                    contractor.firstName = Globals.SafeSqlString(sqlDataReader["firstName"]);
                    contractor.lastName = Globals.SafeSqlString(sqlDataReader["lastName"]);
                    contractor.businessName = Globals.SafeSqlString(sqlDataReader["businessName"]);
                    contractor.address = Globals.SafeSqlString(sqlDataReader["address"]);
                    contractor.city = Globals.SafeSqlString(sqlDataReader["city"]);
                    contractor.state = Globals.SafeSqlString(sqlDataReader["state"]);
                    contractor.zip = Globals.SafeSqlString(sqlDataReader["zip"]);
                    contractor.bestPhone = Globals.SafeSqlString(sqlDataReader["bestPhone"]);
                    contractor.alternatePhone = Globals.SafeSqlString(sqlDataReader["alternatePhone"]);
                    contractor.email = Globals.SafeSqlString(sqlDataReader["email"]);
                    contractor.ssn = Globals.Decrypt(Globals.SafeSqlString(sqlDataReader["ssn"]));
                    contractor.paymentType = Globals.SafeSqlString(sqlDataReader["paymentType"]);
                    contractor.paymentDay = Globals.SafeSqlString(sqlDataReader["paymentDay"]);
                    contractor.notes = Globals.SafeSqlString(sqlDataReader["notes"]);
                    contractor.team = Globals.SafeSqlString(sqlDataReader["team"]);
                    contractor.startDay = (DateTime)sqlDataReader["startDay"];
                    contractor.endDay = (DateTime)sqlDataReader["endDay"];
                    contractor.score = (decimal)sqlDataReader["score"];
                    contractor.hourlyRate = (decimal)sqlDataReader["hourlyRate"];
                    contractor.serviceSplit = (decimal)sqlDataReader["serviceSplit"];
                    contractor.hireDate = (DateTime)sqlDataReader["hireDate"];
                    contractor.birthday = (DateTime)sqlDataReader["birthday"];
                    contractor.waiverDate = (DateTime)sqlDataReader["waiverDate"];
                    contractor.waiverUpdateDate = (DateTime)sqlDataReader["waiverUpdateDate"];
                    contractor.insuranceDate = (DateTime)sqlDataReader["insuranceDate"];
                    contractor.insuranceUpdateDate = (DateTime)sqlDataReader["insuranceUpdateDate"];
                    contractor.backgroundCheck = (DateTime)sqlDataReader["backgroundCheck"];
                    contractor.accountRep = (bool)sqlDataReader["accountRep"];
                    contractor.applicant = (bool)sqlDataReader["applicant"];
                    contractor.active = (bool)sqlDataReader["active"];
                    contractor.scheduled = (bool)sqlDataReader["scheduled"];
                    contractor.sendSchedules = (bool)sqlDataReader["sendSchedules"];
                    contractor.lastSchedule = (DateTime)sqlDataReader["lastSchedule"];
                    contractor.sendPayroll = (bool)sqlDataReader["sendPayroll"];
                    contractor.lastPayroll = (DateTime)sqlDataReader["lastPayroll"];
                    contractor.SendSchedulesByEmail = (bool)sqlDataReader["SendSchedulesByEmail"];

                    contractor.longitude = Convert.ToString(sqlDataReader["longitude"]);
                    contractor.latitude = Convert.ToString(sqlDataReader["latitude"]);
                    contractor.ShareLocation = (bool)sqlDataReader["ShareLocation"];
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

            return contractor;
        }
        #endregion
        #region GetContractorByID
        public static ContractorStruct GetContractorByID(int contractorID)
        {
            ContractorStruct contractor = new ContractorStruct();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        Contractors
                    WHERE
                        @contractorID = contractorID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("contractorID", contractorID));
                sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    contractor.contractorID = (int)sqlDataReader["contractorID"];
                    contractor.contractorType = (int)sqlDataReader["contractorType"];
                    contractor.franchiseMask = (int)sqlDataReader["franchiseMask"];
                    contractor.title = Globals.FormatContractorTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                    contractor.firstName = Globals.SafeSqlString(sqlDataReader["firstName"]);
                    contractor.lastName = Globals.SafeSqlString(sqlDataReader["lastName"]);
                    contractor.businessName = Globals.SafeSqlString(sqlDataReader["businessName"]);
                    contractor.address = Globals.SafeSqlString(sqlDataReader["address"]);
                    contractor.city = Globals.SafeSqlString(sqlDataReader["city"]);
                    contractor.state = Globals.SafeSqlString(sqlDataReader["state"]);
                    contractor.zip = Globals.SafeSqlString(sqlDataReader["zip"]);
                    contractor.bestPhone = Globals.SafeSqlString(sqlDataReader["bestPhone"]);
                    contractor.alternatePhone = Globals.SafeSqlString(sqlDataReader["alternatePhone"]);
                    contractor.email = Globals.SafeSqlString(sqlDataReader["email"]);
                    contractor.ssn = Globals.Decrypt(Globals.SafeSqlString(sqlDataReader["ssn"]));
                    contractor.paymentType = Globals.SafeSqlString(sqlDataReader["paymentType"]);
                    contractor.paymentDay = Globals.SafeSqlString(sqlDataReader["paymentDay"]);
                    contractor.notes = Globals.SafeSqlString(sqlDataReader["notes"]);
                    contractor.team = Globals.SafeSqlString(sqlDataReader["team"]);
                    contractor.startDay = (DateTime)sqlDataReader["startDay"];
                    contractor.endDay = (DateTime)sqlDataReader["endDay"];
                    contractor.score = (decimal)sqlDataReader["score"];
                    contractor.hourlyRate = (decimal)sqlDataReader["hourlyRate"];
                    contractor.serviceSplit = (decimal)sqlDataReader["serviceSplit"];
                    contractor.hireDate = (DateTime)sqlDataReader["hireDate"];
                    contractor.birthday = (DateTime)sqlDataReader["birthday"];
                    contractor.waiverDate = (DateTime)sqlDataReader["waiverDate"];
                    contractor.waiverUpdateDate = (DateTime)sqlDataReader["waiverUpdateDate"];
                    contractor.insuranceDate = (DateTime)sqlDataReader["insuranceDate"];
                    contractor.insuranceUpdateDate = (DateTime)sqlDataReader["insuranceUpdateDate"];
                    contractor.backgroundCheck = (DateTime)sqlDataReader["backgroundCheck"];
                    contractor.accountRep = (bool)sqlDataReader["accountRep"];
                    contractor.applicant = (bool)sqlDataReader["applicant"];
                    contractor.active = (bool)sqlDataReader["active"];
                    contractor.scheduled = (bool)sqlDataReader["scheduled"];
                    contractor.sendSchedules = (bool)sqlDataReader["sendSchedules"];
                    contractor.lastSchedule = (DateTime)sqlDataReader["lastSchedule"];
                    contractor.sendPayroll = (bool)sqlDataReader["sendPayroll"];
                    contractor.lastPayroll = (DateTime)sqlDataReader["lastPayroll"];
                    contractor.SendSchedulesByEmail = (bool)sqlDataReader["SendSchedulesByEmail"];
                    contractor.ContractorPic = Convert.ToString(sqlDataReader["ContractorPic"]);

                    contractor.longitude = Convert.ToString(sqlDataReader["longitude"]);
                    contractor.latitude = Convert.ToString(sqlDataReader["latitude"]);
                    contractor.ShareLocation = (bool)sqlDataReader["ShareLocation"];
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

            return contractor;
        }
        #endregion

        #region ------------------Job Logs-----------------

        public static List<JobLogsStruct> GetJobLogs(int AppointmentId, int contractorID, bool isGeneral)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        JobLogs
                    WHERE
                        AppointmentId = @AppointmentId and
                        @contractorID = contractorID and 
                        IsGeneral = @IsGeneral";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("AppointmentId", AppointmentId));
                cmd.Parameters.Add(new SqlParameter("contractorID", contractorID));
                cmd.Parameters.Add(new SqlParameter("IsGeneral", isGeneral));
                sqlDataReader = cmd.ExecuteReader();
                List<JobLogsStruct> jobLogs = new List<JobLogsStruct>();
                while (sqlDataReader.Read())
                {
                    JobLogsStruct log = new JobLogsStruct();
                    log.id = (int)sqlDataReader["id"];
                    log.CustomerId = (int)sqlDataReader["CustomerId"];
                    log.AppointmentId = (int)sqlDataReader["AppointmentId"];
                    log.ContractorId = (int)sqlDataReader["ContractorId"];
                    log.Content = (string)sqlDataReader["content"];
                    log.Checked = (bool)sqlDataReader["Checked"];
                    log.CheckedBy = (string)sqlDataReader["CheckedBy"];
                    log.CreatedAt = (DateTime)sqlDataReader["CreatedAt"];
                    log.IsGeneral = (bool)sqlDataReader["IsGeneral"];
                    jobLogs.Add(log);
                }
                return jobLogs;
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return null;
        }

        public static string updateJobLog(JobLogsStruct job)
        {

            SqlConnection sqlConnection = null;
            try
            {
                SqlDataReader sqlDataReader = null;
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                Select * from JobLogs where 
                        AppointmentId = @AppointmentId and
                        @contractorID = contractorID and 
                        Content = @content";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"AppointmentId", job.AppointmentId));
                cmd.Parameters.Add(new SqlParameter(@"contractorID", job.ContractorId));
                cmd.Parameters.Add(new SqlParameter(@"Content", job.Content));
                sqlDataReader = cmd.ExecuteReader();

             

                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    var id = (int)sqlDataReader["id"];
                    if (sqlConnection != null)
                        sqlConnection.Close();

                    sqlConnection.Open();

                    string cmdText1 = @"
                    Update JobLogs set  Checked = @Checked,  CreatedAt = GetDate(), CheckedBy = @CreatedBy  , IsGeneral = @IsGeneral
                    Where id = @id";

                    SqlCommand cmd1 = new SqlCommand(cmdText1, sqlConnection);
                    cmd1.Parameters.Add(new SqlParameter(@"Checked", job.Checked));
                    cmd1.Parameters.Add(new SqlParameter(@"CreatedBy", job.CheckedBy));
                    cmd1.Parameters.Add(new SqlParameter(@"IsGeneral", job.IsGeneral));
                    cmd1.Parameters.Add(new SqlParameter(@"id", id));
                    cmd1.ExecuteNonQuery();

                }
                else
                {
                    if (sqlConnection != null)
                        sqlConnection.Close();

                    sqlConnection.Open();

                    string cmdText2 = @"
                    Insert into JobLogs  ([ContractorId]
                    ,[CustomerId]
                    ,[AppointmentId]
                    ,[Content]
                    ,[Checked]
                    ,[CheckedBy]
                    ,[CreatedAt]
                    ,[IsGeneral])  Values " +
                    $"({job.ContractorId} , {job.CustomerId} , {job.AppointmentId} ,'{job.Content}',{(job.Checked ? 1 : 0) },'{job.CheckedBy}', GetDate() ,{(job.IsGeneral ? 1 : 0)} )";

                    SqlCommand cmd2 = new SqlCommand(cmdText2, sqlConnection);
                    cmd2.ExecuteNonQuery();

                }

                //string cmdText3 = @"
                //    Update customers set @content = @Checked
                //    Where appointmentID = @id";

                //SqlCommand cmd3 = new SqlCommand(cmdText3, sqlConnection);
                //cmd3.Parameters.Add(new SqlParameter(@"id", job.AppointmentId));
                //cmd3.Parameters.Add(new SqlParameter(@"content", job.Content));
                //cmd3.Parameters.Add(new SqlParameter(@"Checked", job.Checked));
                //cmd3.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

        }

        #endregion


        #region GetContractorDynamicByID
        public static DBRow GetContractorDynamicByID(int franchiseMask, int contractorID)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        Contractors
                    WHERE
                        franchiseMask & @franchiseMask > 0 AND
                        @contractorID = contractorID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("contractorID", contractorID));
                sqlDataReader = cmd.ExecuteReader();

                return GetDBRow(sqlDataReader);
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return null;
        }
        #endregion

        #region GetContractorPartners
        public static List<string> GetContractorPartners(DateTime appointmentDate, int appType, int customerID, int excludeContractorID)
        {
            List<string> ret = new List<string>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
	                C.firstName,
	                C.lastName,
                    C.businessName,
                    C.bestPhone
                FROM
	                Appointments A,
	                Contractors C
                WHERE
	                A.contractorID = C.contractorID AND
	                A.appointmentDate = @appointmentDate AND
                    A.appType = @appType AND
	                A.customerID = @customerID AND
                    A.contractorID != @excludeContractorID AND
                    A.appStatus = 0
                ORDER BY
	                C.firstName, C.lastName";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("appointmentDate", appointmentDate.Date));
                cmd.Parameters.Add(new SqlParameter("appType", appType));
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                cmd.Parameters.Add(new SqlParameter("excludeContractorID", excludeContractorID));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    string title = Globals.FormatContractorTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                    string phone = Globals.SafeSqlString(sqlDataReader["bestPhone"]); Globals.SafeSqlString(sqlDataReader["bestPhone"]);
                    ret.Add(title + (string.IsNullOrEmpty(phone) ? "" : " " + Globals.FormatPhone(phone)));
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

            return ret;
        }
        #endregion

        #region GetCustomerFrequency
        public static Dictionary<int, int> GetCustomerFrequency(int customerID)
        {
            Dictionary<int, int> ret = new Dictionary<int, int>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                if (customerID > 0)
                {
                    sqlConnection = new SqlConnection(connString);
                    sqlConnection.Open();

                    string cmdText = @"
                    SELECT
	                    contractorID,
	                    COUNT(*) as count
                    FROM
	                    Appointments
                    WHERE
	                    customerID = @customerID AND
                        contractorID IS NOT NULL AND
                        appointmentDate > DATEADD(day, -90, GETUTCDATE()) 
                    GROUP BY
	                    contractorID
                    ORDER BY
	                    contractorID";

                    SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                    cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                    sqlDataReader = cmd.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        int contractorID = (int)sqlDataReader["contractorID"];
                        int count = (int)sqlDataReader["count"];
                        ret.Add(contractorID, count);
                    }
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

            return ret;
        }
        #endregion

        #region SetContractorScheduleSent
        public static string SetContractorScheduleSent(int contractorID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
	                    UPDATE 
	                        Contractors
	                    SET
                            lastSchedule = @lastSchedule
	                    WHERE
		                    contractorID = @contractorID;";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"contractorID", contractorID));
                cmd.Parameters.Add(new SqlParameter(@"lastSchedule", Globals.UtcToMst(DateTime.UtcNow)));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL SetContractorScheduleSent EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region SetContractorPayrollSent
        public static string SetContractorPayrollSent(int contractorID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
	                    UPDATE 
	                        Contractors
	                    SET
                            lastPayroll = @lastPayroll
	                    WHERE
		                    contractorID = @contractorID;";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"contractorID", contractorID));
                cmd.Parameters.Add(new SqlParameter(@"lastPayroll", Globals.UtcToMst(DateTime.UtcNow)));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL SetContractorPayrollSent EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region DeleteContractor
        public static string DeleteContractor(int contractorID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                if (contractorID <= 0)
                    return "Cannot Delete New Contractor";

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    DELETE FROM 
                        Contractors 
                    WHERE 
                        contractorID = @contractorID";
                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"contractorID", contractorID));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL DeleteContractor EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetCustomers
        public static List<CustomerStruct> GetCustomers(int franchiseMask, string where, string orderBy, SqlParameter[] paramArray = null)
        {
            List<CustomerStruct> ret = new List<CustomerStruct>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        Customers
                    WHERE
                        franchiseMask & @franchiseMask > 0 {0}
                    ORDER BY
                        " + orderBy;

                cmdText = string.Format(cmdText, where == null ? "" : " AND " + where);

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                if (paramArray != null)
                {
                    foreach (SqlParameter param in paramArray)
                        cmd.Parameters.Add(param);
                }
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    CustomerStruct customer = new CustomerStruct();
                    customer.customerID = (int)sqlDataReader["customerID"];
                    customer.franchiseMask = (int)sqlDataReader["franchiseMask"];
                    customer.points = (decimal)sqlDataReader["points"];
                    customer.referredBy = (int)sqlDataReader["referredBy"];
                    customer.bookedBy = sqlDataReader["bookedBy"] == DBNull.Value ? 0 : (int)sqlDataReader["bookedBy"];
                    customer.bookedDate = Globals.UtcToMst((DateTime)sqlDataReader["bookedDate"]);
                    customer.lastUpdate = Globals.UtcToMst((DateTime)sqlDataReader["lastUpdate"]);
                    customer.customerTitle = Globals.FormatCustomerTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                    customer.accountStatus = Globals.SafeSqlString(sqlDataReader["accountStatus"]);
                    customer.accountType = Globals.SafeSqlString(sqlDataReader["accountType"]);
                    customer.customNote = Globals.SafeSqlString(sqlDataReader["customNote"]);
                    customer.newBuilding = (bool)sqlDataReader["newBuilding"];
                    customer.sectionMask = (int)sqlDataReader["sectionMask"];
                    customer.businessName = Globals.SafeSqlString(sqlDataReader["businessName"]);
                    customer.companyContact = Globals.SafeSqlString(sqlDataReader["companyContact"]);
                    customer.firstName = Globals.SafeSqlString(sqlDataReader["firstName"]);
                    customer.lastName = Globals.SafeSqlString(sqlDataReader["lastName"]);
                    customer.spouseName = Globals.SafeSqlString(sqlDataReader["spouseName"]);
                    customer.locationAddress = Globals.SafeSqlString(sqlDataReader["locationAddress"]);
                    customer.locationCity = Globals.SafeSqlString(sqlDataReader["locationCity"]);
                    customer.locationState = Globals.SafeSqlString(sqlDataReader["locationState"]);
                    customer.locationZip = Globals.SafeSqlString(sqlDataReader["locationZip"]);
                    customer.billingSame = (bool)sqlDataReader["billingSame"];
                    customer.billingAddress = Globals.SafeSqlString(sqlDataReader["billingAddress"]);
                    customer.billingCity = Globals.SafeSqlString(sqlDataReader["billingCity"]);
                    customer.billingState = Globals.SafeSqlString(sqlDataReader["billingState"]);
                    customer.billingZip = Globals.SafeSqlString(sqlDataReader["billingZip"]);
                    customer.bestPhone = Globals.SafeSqlString(sqlDataReader["bestPhone"]);
                    customer.bestPhoneCell = (bool)sqlDataReader["bestPhoneCell"];
                    customer.alternatePhoneOne = Globals.SafeSqlString(sqlDataReader["alternatePhoneOne"]);
                    customer.alternatePhoneOneCell = (bool)sqlDataReader["alternatePhoneOneCell"];
                    customer.alternatePhoneTwo = Globals.SafeSqlString(sqlDataReader["alternatePhoneTwo"]);
                    customer.alternatePhoneTwoCell = (bool)sqlDataReader["alternatePhoneTwoCell"];
                    customer.email = Globals.SafeSqlString(sqlDataReader["email"]);
                    customer.advertisement = Globals.SafeSqlString(sqlDataReader["advertisement"]);
                    customer.paymentType = Globals.SafeSqlString(sqlDataReader["paymentType"]);
                    customer.NC_RequiresKeys = (bool)sqlDataReader["NC_RequiresKeys"];
                    customer.quoteReply = (bool)sqlDataReader["quoteReply"];
                    customer.rewardsEnabled = (bool)sqlDataReader["rewardsEnabled"];
                    customer.sendPromotions = (bool)sqlDataReader["sendPromotions"];
                    ret.Add(customer);
                }
            }
            catch (Exception ex)
            {
                Debug.Print("GetCustomers EX: " + ex.Message);
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region GetCustomersExcelReport
        public static CustomerStruct[] GetCustomersExcelReport(int franchiseMask)
        {
            SortedList<int, CustomerStruct> dict = new SortedList<int, CustomerStruct>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
	                    Customers.customerID,
	                    Customers.sectionMask,
	                    Customers.franchiseMask,
                        Customers.firstName,
                        Customers.lastName,
                        Customers.businessName,
                        Customers.accountStatus,
                        Customers.advertisement,
                        Customers.paymentType,
                        Customers.locationAddress,
                        Customers.locationCity,
                        Customers.locationState,
                        Customers.locationZip,
                        Customers.bestPhone,
                        Customers.alternatePhoneOne,
                        Customers.alternatePhoneTwo,
                        Customers.email,
	                    Appointments.appointmentDate
                    FROM
                        Customers LEFT OUTER JOIN Appointments ON Customers.customerID = Appointments.customerID
                    WHERE
                        Customers.franchiseMask & @franchiseMask > 0
                    ORDER BY
                        franchiseMask, accountStatus, firstName, lastName, businessName, Appointments.appointmentDate desc";


                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    CustomerStruct customer = new CustomerStruct();
                    customer.customerID = (int)sqlDataReader["customerID"];
                    customer.sectionMask = (int)sqlDataReader["sectionMask"];
                    customer.franchiseMask = (int)sqlDataReader["franchiseMask"];
                    customer.firstName = Globals.SafeSqlString(sqlDataReader["firstName"]);
                    customer.lastName = Globals.SafeSqlString(sqlDataReader["lastName"]);
                    customer.businessName = Globals.SafeSqlString(sqlDataReader["businessName"]);
                    customer.accountStatus = Globals.SafeSqlString(sqlDataReader["accountStatus"]);
                    customer.advertisement = Globals.SafeSqlString(sqlDataReader["advertisement"]);
                    customer.paymentType = Globals.SafeSqlString(sqlDataReader["paymentType"]);
                    customer.locationAddress = Globals.SafeSqlString(sqlDataReader["locationAddress"]);
                    customer.locationCity = Globals.SafeSqlString(sqlDataReader["locationCity"]);
                    customer.locationState = Globals.SafeSqlString(sqlDataReader["locationState"]);
                    customer.locationZip = Globals.SafeSqlString(sqlDataReader["locationZip"]);
                    customer.bestPhone = Globals.SafeSqlString(sqlDataReader["bestPhone"]);
                    customer.alternatePhoneOne = Globals.SafeSqlString(sqlDataReader["alternatePhoneOne"]);
                    customer.alternatePhoneTwo = Globals.SafeSqlString(sqlDataReader["alternatePhoneTwo"]);
                    customer.email = Globals.SafeSqlString(sqlDataReader["email"]);
                    customer.lastUpdate = sqlDataReader["appointmentDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)sqlDataReader["appointmentDate"];
                    if (customer.lastUpdate.Date > DateTime.UtcNow.Date)
                        customer.lastUpdate = DateTime.MinValue;

                    if (!dict.ContainsKey(customer.customerID))
                    {
                        dict.Add(customer.customerID, customer);
                    }
                    else
                    {
                        if (customer.lastUpdate > dict[customer.customerID].lastUpdate)
                            dict[customer.customerID] = customer;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print("GetCustomers EX: " + ex.Message);
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            CustomerStruct[] ret = new CustomerStruct[dict.Values.Count];
            dict.Values.CopyTo(ret, 0);
            return ret;
        }
        #endregion

        #region GetCustomerReferrals
        public static List<CustomerStruct> GetCustomerReferrals(int customerID)
        {
            List<CustomerStruct> ret = new List<CustomerStruct>();
            if (customerID > 0)
            {
                SqlConnection sqlConnection = null;
                SqlDataReader sqlDataReader = null;
                try
                {
                    sqlConnection = new SqlConnection(connString);
                    sqlConnection.Open();

                    string cmdText = @"
                    SELECT
                        customerID,
                        franchiseMask,
                        accountStatus,
                        firstName,
                        lastName,
                        businessName
                    FROM
                        Customers
                    WHERE
                        referredBy = @customerID
                    ORDER BY
                        accountStatus, firstName, lastName, businessName";

                    SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                    cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                    sqlDataReader = cmd.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        CustomerStruct customer = new CustomerStruct();
                        customer.customerID = (int)sqlDataReader["customerID"];
                        customer.franchiseMask = (int)sqlDataReader["franchiseMask"];
                        customer.customerTitle = Globals.FormatCustomerTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                        customer.accountStatus = Globals.SafeSqlString(sqlDataReader["accountStatus"]);
                        ret.Add(customer);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print("GetCustomerReferrals EX: " + ex.Message);
                }
                finally
                {
                    if (sqlDataReader != null)
                        sqlDataReader.Close();
                    if (sqlConnection != null)
                        sqlConnection.Close();
                }
            }
            return ret;
        }
        #endregion

        #region GetNextCustomer
        public static int GetNextCustomer(int franchiseMask, int customerID, string statusFilter, bool previous)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT TOP 1
                        customerID
                    FROM
                        Customers
                    WHERE
                        accountStatus = @statusFilter AND
                        franchiseMask & @franchiseMask > 0 AND
                        (customerID > @customerID OR @customerID = 0)
                    ORDER BY
                        customerID";

                if (previous)
                {
                    cmdText = @"
                    SELECT TOP 1
                        customerID
                    FROM
                        Customers
                    WHERE
                        accountStatus = @statusFilter AND
                        franchiseMask & @franchiseMask > 0 AND
                        (customerID < @customerID OR @customerID = 0)
                    ORDER BY
                        customerID DESC";
                }

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                cmd.Parameters.Add(new SqlParameter("statusFilter", statusFilter));
                sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                    return (int)sqlDataReader["customerID"];
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return 0;
        }
        #endregion

        #region GetCustomerByID
        public static string GetCustomerByID(int franchiseMask, int customerID, out CustomerStruct customer)
        {
            customer = new CustomerStruct();
            if (customerID == 0) return null;

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        Customers
                    WHERE
                        customerID = @customerID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                sqlDataReader = cmd.ExecuteReader();

                if (!sqlDataReader.Read())
                    return "SQL GetCustomer: Record (CustomerID=" + customerID + ") does not exist.";

                if (((int)sqlDataReader["franchiseMask"] & franchiseMask) == 0)
                    return "Cannot access CustomerID (" + customerID + ") it does not belong to your assigned Franchise";

                customer.customerID = (int)sqlDataReader["customerID"];
                customer.franchiseMask = (int)sqlDataReader["franchiseMask"];
                customer.points = (decimal)sqlDataReader["points"];
                customer.referredBy = (int)sqlDataReader["referredBy"];
                customer.bookedBy = sqlDataReader["bookedBy"] == DBNull.Value ? 0 : (int)sqlDataReader["bookedBy"];
                customer.bookedDate = Globals.UtcToMst((DateTime)sqlDataReader["bookedDate"]);
                customer.lastUpdate = Globals.UtcToMst((DateTime)sqlDataReader["lastUpdate"]);
                customer.customerTitle = Globals.FormatCustomerTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                customer.accountStatus = Globals.SafeSqlString(sqlDataReader["accountStatus"]);
                customer.accountType = Globals.SafeSqlString(sqlDataReader["accountType"]);
                customer.newBuilding = (bool)sqlDataReader["newBuilding"];
                customer.sectionMask = (int)sqlDataReader["sectionMask"];
                customer.customNote = Globals.SafeSqlString(sqlDataReader["customNote"]);
                customer.businessName = Globals.SafeSqlString(sqlDataReader["businessName"]);
                customer.companyContact = Globals.SafeSqlString(sqlDataReader["companyContact"]);
                customer.firstName = Globals.SafeSqlString(sqlDataReader["firstName"]);
                customer.lastName = Globals.SafeSqlString(sqlDataReader["lastName"]);
                customer.spouseName = Globals.SafeSqlString(sqlDataReader["spouseName"]);
                customer.locationAddress = Globals.SafeSqlString(sqlDataReader["locationAddress"]);
                customer.locationCity = Globals.SafeSqlString(sqlDataReader["locationCity"]);
                customer.locationState = Globals.SafeSqlString(sqlDataReader["locationState"]);
                customer.locationZip = Globals.SafeSqlString(sqlDataReader["locationZip"]);
                customer.billingSame = (bool)sqlDataReader["billingSame"];
                customer.billingName = Globals.SafeSqlString(sqlDataReader["billingName"]);
                customer.billingAddress = Globals.SafeSqlString(sqlDataReader["billingAddress"]);
                customer.billingCity = Globals.SafeSqlString(sqlDataReader["billingCity"]);
                customer.billingState = Globals.SafeSqlString(sqlDataReader["billingState"]);
                customer.billingZip = Globals.SafeSqlString(sqlDataReader["billingZip"]);
                customer.bestPhone = Globals.SafeSqlString(sqlDataReader["bestPhone"]);
                customer.bestPhoneCell = (bool)sqlDataReader["bestPhoneCell"];
                customer.alternatePhoneOne = Globals.SafeSqlString(sqlDataReader["alternatePhoneOne"]);
                customer.alternatePhoneOneCell = (bool)sqlDataReader["alternatePhoneOneCell"];
                customer.alternatePhoneTwo = Globals.SafeSqlString(sqlDataReader["alternatePhoneTwo"]);
                customer.alternatePhoneTwoCell = (bool)sqlDataReader["alternatePhoneTwoCell"];
                customer.email = Globals.SafeSqlString(sqlDataReader["email"]);
                customer.advertisement = Globals.SafeSqlString(sqlDataReader["advertisement"]);
                customer.preferredContact = Globals.SafeSqlString(sqlDataReader["preferredContact"]);
                customer.paymentType = Globals.SafeSqlString(sqlDataReader["paymentType"]);
                customer.creditCardNumber = Globals.Decrypt(Globals.SafeSqlString(sqlDataReader["creditCardNumber"]));
                customer.creditCardExpMonth = Globals.SafeSqlString(sqlDataReader["creditCardExpMonth"]);
                customer.creditCardExpYear = Globals.SafeSqlString(sqlDataReader["creditCardExpYear"]);
                customer.creditCardCCV = Globals.SafeSqlString(sqlDataReader["creditCardCCV"]);
                customer.serviceFee = (decimal)sqlDataReader["serviceFee"];
                customer.ratePerHour = (decimal)sqlDataReader["ratePerHour"];
                customer.discountPercent = (decimal)sqlDataReader["discountPercent"];
                customer.discountAmount = (decimal)sqlDataReader["discountAmount"];
                customer.estimatedHours = Globals.SafeSqlString(sqlDataReader["estimatedHours"]);
                customer.estimatedCC = Globals.SafeSqlString(sqlDataReader["estimatedCC"]);
                customer.estimatedWW = Globals.SafeSqlString(sqlDataReader["estimatedWW"]);
                customer.estimatedHW = Globals.SafeSqlString(sqlDataReader["estimatedHW"]);
                customer.estimatedCL = Globals.SafeSqlString(sqlDataReader["estimatedCL"]);
                customer.estimatedPrice = Globals.SafeSqlString(sqlDataReader["estimatedPrice"]);
                customer.welcomeLetter = (bool)sqlDataReader["welcomeLetter"];
                customer.loginInfoSent = (bool)sqlDataReader["loginInfoSent"];
                customer.quoteReply = (bool)sqlDataReader["quoteReply"];
                customer.quoteValue = Globals.SafeSqlString(sqlDataReader["quoteValue"]);
                customer.rewardsEnabled = (bool)sqlDataReader["rewardsEnabled"];
                customer.sendPromotions = (bool)sqlDataReader["sendPromotions"];
                customer.reviewUsDate = (DateTime)sqlDataReader["reviewUsDate"];
                customer.NC_Notes = Globals.SafeSqlString(sqlDataReader["NC_Notes"]);
                customer.NC_Special = Globals.SafeSqlString(sqlDataReader["NC_Special"]);
                customer.NC_Details = Globals.SafeSqlString(sqlDataReader["NC_Details"]);
                customer.NC_FrequencyDay = Globals.SafeSqlString(sqlDataReader["NC_FrequencyDay"]);
                customer.NC_PreferedTime = Globals.SafeSqlString(sqlDataReader["NC_PreferedTime"]);
                customer.NC_Frequency = Globals.SafeSqlString(sqlDataReader["NC_Frequency"]);
                customer.NC_DayOfWeek = Globals.SafeSqlString(sqlDataReader["NC_DayOfWeek"]);
                customer.NC_TimeOfDayPrefix = Globals.SafeSqlString(sqlDataReader["NC_TimeOfDayPrefix"]);
                customer.NC_TimeOfDay = Globals.SafeSqlString(sqlDataReader["NC_TimeOfDay"]);
                customer.NC_Bathrooms = Globals.SafeSqlString(sqlDataReader["NC_Bathrooms"]);
                customer.NC_Bedrooms = Globals.SafeSqlString(sqlDataReader["NC_Bedrooms"]);
                customer.NC_SquareFootage = Globals.SafeSqlString(sqlDataReader["NC_SquareFootage"]);
                customer.NC_Vacuum = (bool)sqlDataReader["NC_Vacuum"];
                customer.NC_DoDishes = (bool)sqlDataReader["NC_DoDishes"];
                customer.NC_ChangeBed = (bool)sqlDataReader["NC_ChangeBed"];
                customer.NC_Pets = Globals.SafeSqlString(sqlDataReader["NC_Pets"]);
                customer.NC_FlooringCarpet = (bool)sqlDataReader["NC_FlooringCarpet"];
                customer.NC_FlooringHardwood = (bool)sqlDataReader["NC_FlooringHardwood"];
                customer.NC_FlooringTile = (bool)sqlDataReader["NC_FlooringTile"];
                customer.NC_FlooringLinoleum = (bool)sqlDataReader["NC_FlooringLinoleum"];
                customer.NC_FlooringSlate = (bool)sqlDataReader["NC_FlooringSlate"];
                customer.NC_FlooringMarble = (bool)sqlDataReader["NC_FlooringMarble"];
                customer.NC_EnterHome = Globals.SafeSqlString(sqlDataReader["NC_EnterHome"]);
                customer.NC_RequiresKeys = (bool)sqlDataReader["NC_RequiresKeys"];
                customer.NC_Organize = (bool)sqlDataReader["NC_Organize"];
                customer.NC_CleanRating = Globals.SafeSqlString(sqlDataReader["NC_CleanRating"]);
                customer.NC_CleaningType = Globals.SafeSqlString(sqlDataReader["NC_CleaningType"]);
                customer.NC_GateCode = Globals.SafeSqlString(sqlDataReader["NC_GateCode"]);
                customer.NC_GarageCode = Globals.SafeSqlString(sqlDataReader["NC_GarageCode"]);
                customer.NC_DoorCode = Globals.SafeSqlString(sqlDataReader["NC_DoorCode"]);
                customer.NC_RequestEcoCleaners = (bool)sqlDataReader["NC_RequestEcoCleaners"];
                customer.DC_Blinds = (bool)sqlDataReader["DC_Blinds"];
                customer.DC_BlindsAmount = Globals.SafeSqlString(sqlDataReader["DC_BLindsAmount"]);
                customer.DC_BlindsCondition = Globals.SafeSqlString(sqlDataReader["DC_BlindsCondition"]);
                customer.DC_Windows = (bool)sqlDataReader["DC_Windows"];
                customer.DC_WindowsAmount = Globals.SafeSqlString(sqlDataReader["DC_WindowsAmount"]);
                customer.DC_WindowsSills = (bool)sqlDataReader["DC_WindowsSills"];
                customer.DC_Walls = (bool)sqlDataReader["DC_Walls"];
                customer.DC_WallsDetail = Globals.SafeSqlString(sqlDataReader["DC_WallsDetail"]);
                customer.DC_Baseboards = (bool)sqlDataReader["DC_Baseboards"];
                customer.DC_DoorFrames = (bool)sqlDataReader["DC_DoorFrames"];
                customer.DC_LightSwitches = (bool)sqlDataReader["DC_LightSwitches"];
                customer.DC_VentCovers = (bool)sqlDataReader["DC_VentCovers"];
                customer.DC_InsideVents = (bool)sqlDataReader["DC_InsideVents"];
                customer.DC_Pantry = (bool)sqlDataReader["DC_Pantry"];
                customer.DC_LaundryRoom = (bool)sqlDataReader["DC_LaundryRoom"];
                customer.DC_CeilingFans = (bool)sqlDataReader["DC_CeilingFans"];
                customer.DC_CeilingFansAmount = Globals.SafeSqlString(sqlDataReader["DC_CeilingFansAmount"]);
                customer.DC_LightFixtures = (bool)sqlDataReader["DC_LightFixtures"];
                customer.DC_KitchenCuboards = (bool)sqlDataReader["DC_KitchenCuboards"];
                customer.DC_KitchenCuboardsDetail = Globals.SafeSqlString(sqlDataReader["DC_KitchenCuboardsDetail"]);
                customer.DC_BathroomCuboards = (bool)sqlDataReader["DC_BathroomCuboards"];
                customer.DC_BathroomCuboardsDetail = Globals.SafeSqlString(sqlDataReader["DC_BathroomCuboardsDetail"]);
                customer.DC_Oven = (bool)sqlDataReader["DC_Oven"];
                customer.DC_Refrigerator = (bool)sqlDataReader["DC_Refrigerator"];
                customer.DC_OtherOne = Globals.SafeSqlString(sqlDataReader["DC_OtherOne"]);
                customer.DC_OtherTwo = Globals.SafeSqlString(sqlDataReader["DC_OtherTwo"]);
                customer.CC_SquareFootage = Globals.SafeSqlString(sqlDataReader["CC_SquareFootage"]);
                customer.CC_RoomCountSmall = Globals.SafeSqlString(sqlDataReader["CC_RoomCountSmall"]);
                customer.CC_RoomCountLarge = Globals.SafeSqlString(sqlDataReader["CC_RoomCountLarge"]);
                customer.CC_PetOdorAdditive = (bool)sqlDataReader["CC_PetOdorAdditive"];
                customer.CC_Details = Globals.SafeSqlString(sqlDataReader["CC_Details"]);
                customer.WW_BuildingStyle = Globals.SafeSqlString(sqlDataReader["WW_BuildingStyle"]);
                customer.WW_BuildingLevels = Globals.SafeSqlString(sqlDataReader["WW_BuildingLevels"]);
                customer.WW_VaultedCeilings = (bool)sqlDataReader["WW_VaultedCeilings"];
                customer.WW_PostConstruction = (bool)sqlDataReader["WW_PostConstruction"];
                customer.WW_WindowCount = Globals.SafeSqlString(sqlDataReader["WW_WindowCount"]);
                customer.WW_WindowType = Globals.SafeSqlString(sqlDataReader["WW_WindowType"]);
                customer.WW_InsidesOutsides = Globals.SafeSqlString(sqlDataReader["WW_InsidesOutsides"]);
                customer.WW_Razor = (bool)sqlDataReader["WW_Razor"];
                customer.WW_RazorCount = Globals.SafeSqlString(sqlDataReader["WW_RazorCount"]);
                customer.WW_HardWater = (bool)sqlDataReader["WW_HardWater"];
                customer.WW_HardWaterCount = Globals.SafeSqlString(sqlDataReader["WW_HardWaterCount"]);
                customer.WW_FrenchWindows = (bool)sqlDataReader["WW_FrenchWindows"];
                customer.WW_FrenchWindowCount = Globals.SafeSqlString(sqlDataReader["WW_FrenchWindowCount"]);
                customer.WW_StormWindows = (bool)sqlDataReader["WW_StormWindows"];
                customer.WW_StormWindowCount = Globals.SafeSqlString(sqlDataReader["WW_StormWindowCount"]);
                customer.WW_Screens = (bool)sqlDataReader["WW_Screens"];
                customer.WW_ScreensCount = Globals.SafeSqlString(sqlDataReader["WW_ScreensCount"]);
                customer.WW_Tracks = (bool)sqlDataReader["WW_Tracks"];
                customer.WW_TracksCount = Globals.SafeSqlString(sqlDataReader["WW_TracksCount"]);
                customer.WW_Wells = (bool)sqlDataReader["WW_Wells"];
                customer.WW_WellsCount = Globals.SafeSqlString(sqlDataReader["WW_WellsCount"]);
                customer.WW_Gutters = (bool)sqlDataReader["WW_Gutters"];
                customer.WW_GuttersFeet = Globals.SafeSqlString(sqlDataReader["WW_GuttersFeet"]);
                customer.WW_Details = Globals.SafeSqlString(sqlDataReader["WW_Details"]);
                customer.HW_Frequency = Globals.SafeSqlString(sqlDataReader["HW_Frequency"]);
                customer.HW_StartDate = Globals.SafeSqlString(sqlDataReader["HW_StartDate"]);
                customer.HW_EndDate = Globals.SafeSqlString(sqlDataReader["HW_EndDate"]);
                customer.HW_GarbageCans = (bool)sqlDataReader["HW_GarbageCans"];
                customer.HW_GarbageDay = Globals.SafeSqlString(sqlDataReader["HW_GarbageDay"]);
                customer.HW_PlantsWatered = (bool)sqlDataReader["HW_PlantsWatered"];
                customer.HW_PlantsWateredFrequency = Globals.SafeSqlString(sqlDataReader["HW_PlantsWateredFrequency"]);
                customer.HW_Thermostat = (bool)sqlDataReader["HW_Thermostat"];
                customer.HW_ThermostatTemperature = Globals.SafeSqlString(sqlDataReader["HW_ThermostatTemperature"]);
                customer.HW_Breakers = (bool)sqlDataReader["HW_Breakers"];
                customer.HW_BreakersLocation = Globals.SafeSqlString(sqlDataReader["HW_BreakersLocation"]);
                customer.HW_CleanBeforeReturn = (bool)sqlDataReader["HW_CleanBeforeReturn"];
                customer.HW_Details = Globals.SafeSqlString(sqlDataReader["HW_Details"]);
                customer.TakePic = (bool)sqlDataReader["TakePic"];
                return null;
            }
            catch (Exception ex)
            {
                return "SQL GetCustomerByID EX: " + ex.Message;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetCustomerAverageHoursPerContractor
        public static TimeSpan GetCustomerAverageHoursPerContractor(int customerID)
        {
            Dictionary<TimeSpan, int> timeList = new Dictionary<TimeSpan, int>();
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
	                (endTime - startTime) AS time,
	                COUNT(*) as count
                FROM
	                Appointments
                WHERE
	                customerID = @customerID AND
	                appointmentDate > DATEADD(day, -90, GETUTCDATE()) 
                GROUP BY
	                (endTime - startTime)
                ORDER BY
	                COUNT(*) DESC";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    DateTime dateTime = (DateTime)sqlDataReader["time"];
                    TimeSpan span = new TimeSpan(dateTime.Hour, dateTime.Minute, 0);
                    if (span > TimeSpan.Zero && span < TimeSpan.FromHours(8))
                        return span;
                }

            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return TimeSpan.Zero;
        }
        #endregion

        #region GetCustomerAverageHoursPerDay
        public static decimal GetCustomerAverageHoursPerDay(int customerID)
        {
            Dictionary<TimeSpan, int> timeList = new Dictionary<TimeSpan, int>();
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
	                SUM(customerHours) as hours
                FROM
	                Appointments
                WHERE
	                customerID = @customerID AND
	                appointmentDate > DATEADD(day, -90, GETUTCDATE()) 
                GROUP BY
	                appointmentDate
                ORDER BY
	                COUNT(*) DESC";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    return (decimal)sqlDataReader["hours"];
                }

            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return 0;
        }
        #endregion

        #region GetCustomerAverageCount
        public static int GetCustomerAverageCount(int customerID)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
	                COUNT(*) as count
                FROM
	                Appointments
                WHERE
	                customerID = @customerID AND
	                appointmentDate > DATEADD(day, -90, GETUTCDATE()) 
                GROUP BY
	                appointmentDate
                ORDER BY
	                COUNT(*) DESC";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    int count = (int)sqlDataReader["count"];
                    if (!dict.ContainsKey(count)) dict.Add(count, 0);
                    dict[count]++;
                }

                int ret = 0;
                int best = 0;
                foreach (int key in dict.Keys)
                {
                    if (dict[key] > best)
                    {
                        ret = key;
                        best = dict[key];
                    }
                }
                return ret;
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return 0;
        }
        #endregion

        #region SetCustomer
        public static string SetCustomer(ref CustomerStruct customer)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                if (!customer.accountStatus.ToLower().Contains("quote"))
                {
                    if (string.IsNullOrEmpty(customer.firstName) || string.IsNullOrEmpty(customer.lastName))
                        return "Required fields First Name and Last Name.";

                    if (string.IsNullOrEmpty(customer.locationAddress) || string.IsNullOrEmpty(customer.locationCity) || string.IsNullOrEmpty(customer.locationState) || string.IsNullOrEmpty(customer.locationZip))
                        return "Required fields Address, City, State, and Zip";

                    if (string.IsNullOrEmpty(customer.bestPhone))
                        return "Required field Phone Number";

                    if (!string.IsNullOrEmpty(customer.email) && !Globals.ValidEmail(customer.email))
                        return "Invalid Customer Email";

                    if (customer.sectionMask == 0)
                        return "You must select at least one Service";
                }

                if (customer.franchiseMask <= 0)
                    return "Invalid Franchise Selected";

                if (string.IsNullOrEmpty(customer.paymentType))
                    customer.paymentType = "Credit Card";

                if (customer.customerID > 0 && (customer.sectionMask & 1) == 0)
                {
                    CustomerStruct currCustomer;
                    string error = GetCustomerByID(-1, customer.customerID, out currCustomer);
                    if (error == null && currCustomer.customerID > 0 && (currCustomer.sectionMask & 1) != 0 && !string.IsNullOrWhiteSpace(currCustomer.NC_CleaningType))
                    {
                        return "Cleaning type must be cleared first before you can uncheck housekeeping.";
                    }
                }

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                #region CMD_TEXT
                string cmdText = @"
                    IF @customerID > 0
                    BEGIN
	                    UPDATE 
		                    Customers
	                    SET
                            franchiseMask = @franchiseMask,
                            referredBy = @referredBy,
                            bookedBy = @bookedBy,
                            lastUpdate = (getutcdate()),
                            accountStatus = @accountStatus,
                            accountType = @accountType,
                            newBuilding = @newBuilding,
                            sectionMask = @sectionMask,
                            customNote = @customNote,
                            businessName = @businessName,
                            companyContact = @companyContact,
                            firstName = @firstName,
                            lastName = @lastName,
                            spouseName = @spouseName,
                            locationAddress = @locationAddress,
                            locationCity = @locationCity,
                            locationState = @locationState,
                            locationZip = @locationZip,
                            billingName = @billingName,
                            billingSame = @billingSame,
                            billingAddress = @billingAddress,
                            billingCity = @billingCity,
                            billingState = @billingState,
                            billingZip = @billingZip,
                            bestPhone = @bestPhone,
                            bestPhoneCell = @bestPhoneCell,
                            alternatePhoneOne = @alternatePhoneOne,
                            alternatePhoneOneCell = @alternatePhoneOneCell,
                            alternatePhoneTwo = @alternatePhoneTwo,
                            alternatePhoneTwoCell = @alternatePhoneTwoCell,
                            email = @email,
                            advertisement = @advertisement,
                            preferredContact = @preferredContact,
                            sendPromotions = @sendPromotions,
                            paymentType = @paymentType,
                            creditCardNumber = @creditCardNumber,
                            creditCardExpMonth = @creditCardExpMonth,
                            creditCardExpYear = @creditCardExpYear,
                            creditCardCCV = @creditCardCCV,
                            serviceFee = @serviceFee,
                            ratePerHour = @ratePerHour,
                            discountPercent = @discountPercent,
                            discountAmount = @discountAmount,
                            estimatedHours = @estimatedHours,
                            estimatedCC = @estimatedCC,
                            estimatedWW = @estimatedWW,
                            estimatedHW = @estimatedHW,
                            estimatedCL = @estimatedCL,
                            estimatedPrice = @estimatedPrice,
                            NC_Notes = (CASE WHEN LEN(@NC_Notes) > 0 THEN @NC_Notes ELSE NC_Notes END),
                            NC_Special = @NC_Special,
                            NC_Details = (CASE WHEN LEN(@NC_Details) > 0 THEN @NC_Details ELSE NC_Details END),
                            NC_Frequency = @NC_Frequency,
                            NC_DayOfWeek = @NC_DayOfWeek,
                            NC_TimeOfDayPrefix = @NC_TimeOfDayPrefix,
                            NC_TimeOfDay = @NC_TimeOfDay,
                            NC_Bathrooms = @NC_Bathrooms,
                            NC_Bedrooms = @NC_Bedrooms,
                            NC_SquareFootage = @NC_SquareFootage,
                            NC_Vacuum = @NC_Vacuum,
                            NC_DoDishes = @NC_DoDishes,
                            NC_ChangeBed = @NC_ChangeBed,
                            NC_Pets = @NC_Pets,
                            NC_FlooringCarpet = @NC_FlooringCarpet,
                            NC_FlooringHardwood = @NC_FlooringHardwood,
                            NC_FlooringTile = @NC_FlooringTile,
                            NC_FlooringLinoleum = @NC_FlooringLinoleum,
                            NC_FlooringSlate = @NC_FlooringSlate,
                            NC_FlooringMarble = @NC_FlooringMarble,
                            NC_EnterHome = @NC_EnterHome,
                            NC_RequiresKeys = @NC_RequiresKeys,
                            NC_Organize = @NC_Organize,
                            NC_CleanRating = @NC_CleanRating,
                            NC_CleaningType = @NC_CleaningType,
                            NC_GateCode = @NC_GateCode,
                            NC_GarageCode = @NC_GarageCode,
                            NC_DoorCode = @NC_DoorCode,
                            NC_RequestEcoCleaners = @NC_RequestEcoCleaners,
                            DC_Blinds = @DC_Blinds,
                            DC_BlindsAmount = @DC_BlindsAmount,
                            DC_BlindsCondition = @DC_BlindsCondition,
                            DC_Windows = @DC_Windows,
                            DC_WindowsAmount = @DC_WindowsAmount,
                            DC_WindowsSills = @DC_WindowsSills,
                            DC_Walls = @DC_Walls,
                            DC_WallsDetail = @DC_WallsDetail,
                            DC_Baseboards = @DC_Baseboards,
                            DC_DoorFrames = @DC_DoorFrames,
                            DC_LightSwitches = @DC_LightSwitches,
                            DC_VentCovers = @DC_VentCovers,
                            DC_InsideVents = @DC_InsideVents,
                            DC_Pantry = @DC_Pantry,
                            DC_LaundryRoom = @DC_LaundryRoom,
                            DC_CeilingFans = @DC_CeilingFans,
                            DC_CeilingFansAmount = @DC_CeilingFansAmount,
                            DC_LightFixtures = @DC_LightFixtures,
                            DC_KitchenCuboards = @DC_KitchenCuboards,
                            DC_KitchenCuboardsDetail = @DC_KitchenCuboardsDetail,
                            DC_BathroomCuboards = @DC_BathroomCuboards,
                            DC_BathroomCuboardsDetail = @DC_BathroomCuboardsDetail,
                            DC_Oven = @DC_Oven,
                            DC_Refrigerator = @DC_Refrigerator,
                            DC_OtherOne = @DC_OtherOne,
                            DC_OtherTwo = @DC_OtherTwo,
                            CC_SquareFootage = @CC_SquareFootage,
                            CC_RoomCountSmall = @CC_RoomCountSmall,
                            CC_RoomCountLarge = @CC_RoomCountLarge,
                            CC_PetOdorAdditive = @CC_PetOdorAdditive,
                            CC_Details = (CASE WHEN LEN(@CC_Details) > 0 THEN @CC_Details ELSE CC_Details END),
                            WW_BuildingStyle = @WW_BuildingStyle,
                            WW_BuildingLevels = @WW_BuildingLevels,
                            WW_VaultedCeilings = @WW_VaultedCeilings,
                            WW_PostConstruction = @WW_PostConstruction,
                            WW_WindowCount = @WW_WindowCount,
                            WW_WindowType = @WW_WindowType,
                            WW_InsidesOutsides = @WW_InsidesOutsides,
                            WW_Razor = @WW_Razor,
                            WW_RazorCount = @WW_RazorCount,
                            WW_HardWater = @WW_HardWater,
                            WW_HardWaterCount = @WW_HardWaterCount,
                            WW_FrenchWindows = @WW_FrenchWindows,
                            WW_FrenchWindowCount = @WW_FrenchWindowCount,
                            WW_StormWindows = @WW_StormWindows,
                            WW_StormWindowCount = @WW_StormWindowCount,
                            WW_Screens = @WW_Screens,
                            WW_ScreensCount = @WW_ScreensCount,
                            WW_Tracks = @WW_Tracks,
                            WW_TracksCount = @WW_TracksCount,
                            WW_Wells = @WW_Wells,
                            WW_WellsCount = @WW_WellsCount,
                            WW_Gutters = @WW_Gutters,
                            WW_GuttersFeet = @WW_GuttersFeet,
                            WW_Details = (CASE WHEN LEN(@WW_Details) > 0 THEN @WW_Details ELSE WW_Details END),
                            HW_Frequency = @HW_Frequency,
                            HW_StartDate = @HW_StartDate,
                            HW_EndDate = @HW_EndDate,
                            HW_GarbageCans = @HW_GarbageCans,
                            HW_GarbageDay = @HW_GarbageDay,
                            HW_PlantsWatered = @HW_PlantsWatered,
                            HW_PlantsWateredFrequency = @HW_PlantsWateredFrequency,
                            HW_Thermostat = @HW_Thermostat,
                            HW_ThermostatTemperature = @HW_ThermostatTemperature,
                            HW_Breakers = @HW_Breakers,
                            HW_BreakersLocation = @HW_BreakersLocation,
                            HW_CleanBeforeReturn = @HW_CleanBeforeReturn,
                            HW_Details = (CASE WHEN LEN(@HW_Details) > 0 THEN @HW_Details ELSE HW_Details END),
TakePic = @TakePic

	                    WHERE
		                    customerID = @customerID;
                         SELECT @customerID AS customerID;
                    END
                    ELSE
                    BEGIN 
                        DECLARE @newCustomerID int;
                        SET @newCustomerID = (dbo.InlineMax((SELECT MAX(customerID) + 1 FROM Customers), 1000000));
	                    INSERT INTO Customers 
		                    (franchiseMask,
                            customerID,
                            referredBy,
                            bookedBy,
                            accountStatus,
                            accountType,
                            newBuilding,
                            sectionMask,
                            customNote,
                            businessName,
                            companyContact,
                            firstName,
                            lastName,
                            spouseName,
                            locationAddress,
                            locationCity,
                            locationState,
                            locationZip,
                            billingSame,
                            billingName,
                            billingAddress,
                            billingCity,
                            billingState,
                            billingZip,
                            bestPhone,
                            bestPhoneCell,
                            alternatePhoneOne,
                            alternatePhoneOneCell,
                            alternatePhoneTwo,
                            alternatePhoneTwoCell,
                            email,
                            advertisement,
                            preferredContact,
                            sendPromotions,
                            paymentType,
                            creditCardNumber,
                            creditCardExpMonth,
                            creditCardExpYear,
                            creditCardCCV,
                            serviceFee,
                            ratePerHour,
                            discountPercent,
                            discountAmount,
                            estimatedHours,
                            estimatedCC,
                            estimatedWW,
                            estimatedHW,
                            estimatedCL,
                            estimatedPrice,
                            NC_Notes,
                            NC_Special,
                            NC_Details,
                            NC_Frequency,
                            NC_DayOfWeek,
                            NC_TimeOfDayPrefix,
                            NC_TimeOfDay,
                            NC_Bathrooms,
                            NC_Bedrooms,
                            NC_SquareFootage,
                            NC_Vacuum,
                            NC_DoDishes,
                            NC_ChangeBed,
                            NC_Pets,
                            NC_FlooringCarpet,
                            NC_FlooringHardwood,
                            NC_FlooringTile,
                            NC_FlooringLinoleum,
                            NC_FlooringSlate,
                            NC_FlooringMarble,
                            NC_EnterHome,
                            NC_RequiresKeys,
                            NC_Organize,
                            NC_CleanRating,
                            NC_CleaningType,
                            NC_GateCode,
                            NC_GarageCode,
                            NC_DoorCode,
                            NC_RequestEcoCleaners,
                            DC_Blinds,
                            DC_BlindsAmount,
                            DC_BlindsCondition,
                            DC_Windows,
                            DC_WindowsAmount,
                            DC_WindowsSills,
                            DC_Walls,
                            DC_WallsDetail,
                            DC_Baseboards,
                            DC_DoorFrames,
                            DC_LightSwitches,
                            DC_VentCovers,
                            DC_InsideVents,
                            DC_Pantry,
                            DC_LaundryRoom,
                            DC_CeilingFans,
                            DC_CeilingFansAmount,
                            DC_LightFixtures,
                            DC_KitchenCuboards,
                            DC_KitchenCuboardsDetail,
                            DC_BathroomCuboards,
                            DC_BathroomCuboardsDetail,
                            DC_Oven,
                            DC_Refrigerator,
                            DC_OtherOne,
                            DC_OtherTwo,
                            CC_SquareFootage,
                            CC_RoomCountSmall,
                            CC_RoomCountLarge,
                            CC_PetOdorAdditive,
                            CC_Details,
                            WW_BuildingStyle,
                            WW_BuildingLevels,
                            WW_VaultedCeilings,
                            WW_PostConstruction,
                            WW_WindowCount,
                            WW_WindowType,
                            WW_InsidesOutsides,
                            WW_Razor,
                            WW_RazorCount,
                            WW_HardWater,
                            WW_HardWaterCount,
                            WW_FrenchWindows,
                            WW_FrenchWindowCount,
                            WW_StormWindows,
                            WW_StormWindowCount,
                            WW_Screens,
                            WW_ScreensCount,
                            WW_Tracks,
                            WW_TracksCount,
                            WW_Wells,
                            WW_WellsCount,
                            WW_Gutters,
                            WW_GuttersFeet,
                            WW_Details,
                            HW_Frequency,
                            HW_StartDate,
                            HW_EndDate,
                            HW_GarbageCans,
                            HW_GarbageDay,
                            HW_PlantsWatered,
                            HW_PlantsWateredFrequency,
                            HW_Thermostat,
                            HW_ThermostatTemperature,
                            HW_Breakers,
                            HW_BreakersLocation,
                            HW_CleanBeforeReturn,
                            HW_Details,
TakePic)
	                    VALUES
		                    (@franchiseMask,
                            @newCustomerID,
                            @referredBy,
                            @bookedBy,
                            @accountStatus,
                            @accountType,
                            @newBuilding,
                            @sectionMask,
                            @customNote,
                            @businessName,
                            @companyContact,
                            @firstName,
                            @lastName,
                            @spouseName,
                            @locationAddress,
                            @locationCity,
                            @locationState,
                            @locationZip,
                            @billingSame,
                            @billingName,
                            @billingAddress,
                            @billingCity,
                            @billingState,
                            @billingZip,
                            @bestPhone,
                            @bestPhoneCell,
                            @alternatePhoneOne,
                            @alternatePhoneOneCell,
                            @alternatePhoneTwo,
                            @alternatePhoneTwoCell,
                            @email,
                            @advertisement,
                            @preferredContact,
                            @sendPromotions,
                            @paymentType,
                            @creditCardNumber,
                            @creditCardExpMonth,
                            @creditCardExpYear,
                            @creditCardCCV,
                            @serviceFee,
                            @ratePerHour,
                            @discountPercent,
                            @discountAmount,
                            @estimatedHours,
                            @estimatedCC,
                            @estimatedWW,
                            @estimatedHW,
                            @estimatedCL,
                            @estimatedPrice,
                            @NC_Notes,
                            @NC_Special,
                            @NC_Details,
                            @NC_Frequency,
                            @NC_DayOfWeek,
                            @NC_TimeOfDayPrefix,
                            @NC_TimeOfDay,
                            @NC_Bathrooms,
                            @NC_Bedrooms,
                            @NC_SquareFootage,
                            @NC_Vacuum,
                            @NC_DoDishes,
                            @NC_ChangeBed,
                            @NC_Pets,
                            @NC_FlooringCarpet,
                            @NC_FlooringHardwood,
                            @NC_FlooringTile,
                            @NC_FlooringLinoleum,
                            @NC_FlooringSlate,
                            @NC_FlooringMarble,
                            @NC_EnterHome,
                            @NC_RequiresKeys,
                            @NC_Organize,
                            @NC_CleanRating,
                            @NC_CleaningType,
                            @NC_GateCode,
                            @NC_GarageCode,
                            @NC_DoorCode,
                            @NC_RequestEcoCleaners,
                            @DC_Blinds,
                            @DC_BlindsAmount,
                            @DC_BlindsCondition,
                            @DC_Windows,
                            @DC_WindowsAmount,
                            @DC_WindowsSills,
                            @DC_Walls,
                            @DC_WallsDetail,
                            @DC_Baseboards,
                            @DC_DoorFrames,
                            @DC_LightSwitches,
                            @DC_VentCovers,
                            @DC_InsideVents,
                            @DC_Pantry,
                            @DC_LaundryRoom,
                            @DC_CeilingFans,
                            @DC_CeilingFansAmount,
                            @DC_LightFixtures,
                            @DC_KitchenCuboards,
                            @DC_KitchenCuboardsDetail,
                            @DC_BathroomCuboards,
                            @DC_BathroomCuboardsDetail,
                            @DC_Oven,
                            @DC_Refrigerator,
                            @DC_OtherOne,
                            @DC_OtherTwo,
                            @CC_SquareFootage,
                            @CC_RoomCountSmall,
                            @CC_RoomCountLarge,
                            @CC_PetOdorAdditive,
                            @CC_Details,
                            @WW_BuildingStyle,
                            @WW_BuildingLevels,
                            @WW_VaultedCeilings,
                            @WW_PostConstruction,
                            @WW_WindowCount,
                            @WW_WindowType,
                            @WW_InsidesOutsides,
                            @WW_Razor,
                            @WW_RazorCount,
                            @WW_HardWater,
                            @WW_HardWaterCount,
                            @WW_FrenchWindows,
                            @WW_FrenchWindowCount,
                            @WW_StormWindows,
                            @WW_StormWindowCount,
                            @WW_Screens,
                            @WW_ScreensCount,
                            @WW_Tracks,
                            @WW_TracksCount,
                            @WW_Wells,
                            @WW_WellsCount,
                            @WW_Gutters,
                            @WW_GuttersFeet,
                            @WW_Details,
                            @HW_Frequency,
                            @HW_StartDate,
                            @HW_EndDate,
                            @HW_GarbageCans,
                            @HW_GarbageDay,
                            @HW_PlantsWatered,
                            @HW_PlantsWateredFrequency,
                            @HW_Thermostat,
                            @HW_ThermostatTemperature,
                            @HW_Breakers,
                            @HW_BreakersLocation,
                            @HW_CleanBeforeReturn,
                            @HW_Details,
@TakePic);
                        SELECT @newCustomerID AS customerID;
                    END";
                #endregion

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"franchiseMask", customer.franchiseMask));
                cmd.Parameters.Add(new SqlParameter(@"customerID", customer.customerID));
                cmd.Parameters.Add(new SqlParameter(@"referredBy", customer.referredBy));
                cmd.Parameters.Add(new SqlParameter(@"bookedBy", customer.bookedBy == 0 ? DBNull.Value : (object)customer.bookedBy));
                cmd.Parameters.Add(new SqlParameter(@"accountStatus", (object)customer.accountStatus ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"accountType", (object)customer.accountType ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"newBuilding", customer.newBuilding));
                cmd.Parameters.Add(new SqlParameter(@"sectionMask", customer.sectionMask));
                cmd.Parameters.Add(new SqlParameter(@"customNote", (object)customer.customNote ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"businessName", (object)customer.businessName ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"companyContact", (object)customer.companyContact ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"firstName", (object)customer.firstName ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"lastName", (object)customer.lastName ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"spouseName", (object)customer.spouseName ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"locationAddress", (object)customer.locationAddress ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"locationCity", (object)customer.locationCity ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"locationState", (object)customer.locationState ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"locationZip", (object)customer.locationZip ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"billingSame", customer.billingSame));
                cmd.Parameters.Add(new SqlParameter(@"billingName", (object)customer.billingName ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"billingAddress", (object)customer.billingAddress ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"billingCity", (object)customer.billingCity ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"billingState", (object)customer.billingState ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"billingZip", (object)customer.billingZip ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"bestPhone", (object)customer.bestPhone ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"bestPhoneCell", customer.bestPhoneCell));
                cmd.Parameters.Add(new SqlParameter(@"alternatePhoneOne", (object)customer.alternatePhoneOne ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"alternatePhoneOneCell", customer.alternatePhoneOneCell));
                cmd.Parameters.Add(new SqlParameter(@"alternatePhoneTwo", (object)customer.alternatePhoneTwo ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"alternatePhoneTwoCell", customer.alternatePhoneTwoCell));
                cmd.Parameters.Add(new SqlParameter(@"email", (object)customer.email ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"advertisement", (object)customer.advertisement ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"preferredContact", (object)customer.preferredContact ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"sendPromotions", customer.sendPromotions));
                cmd.Parameters.Add(new SqlParameter(@"paymentType", (object)customer.paymentType ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"creditCardNumber", Globals.Encrypt(customer.creditCardNumber)));
                cmd.Parameters.Add(new SqlParameter(@"creditCardExpMonth", (object)customer.creditCardExpMonth ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"creditCardExpYear", (object)customer.creditCardExpYear ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"creditCardCCV", (object)customer.creditCardCCV ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"serviceFee", customer.serviceFee));
                cmd.Parameters.Add(new SqlParameter(@"ratePerHour", customer.ratePerHour));
                cmd.Parameters.Add(new SqlParameter(@"discountPercent", customer.discountPercent));
                cmd.Parameters.Add(new SqlParameter(@"discountAmount", customer.discountAmount));
                cmd.Parameters.Add(new SqlParameter(@"estimatedHours", (object)customer.estimatedHours ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"estimatedCC", (object)customer.estimatedCC ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"estimatedWW", (object)customer.estimatedWW ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"estimatedHW", (object)customer.estimatedHW ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"estimatedCL", (object)customer.estimatedCL ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"estimatedPrice", (object)customer.estimatedPrice ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_Notes", (object)customer.NC_Notes ?? ""));
                cmd.Parameters.Add(new SqlParameter(@"NC_Special", (object)customer.NC_Special ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_Details", (object)customer.NC_Details ?? ""));
                cmd.Parameters.Add(new SqlParameter(@"NC_Frequency", (object)customer.NC_Frequency ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_DayOfWeek", (object)customer.NC_DayOfWeek ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_TimeOfDayPrefix", (object)customer.NC_TimeOfDayPrefix ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_TimeOfDay", (object)customer.NC_TimeOfDay ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_Bathrooms", (object)customer.NC_Bathrooms ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_Bedrooms", (object)customer.NC_Bedrooms ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_SquareFootage", (object)customer.NC_SquareFootage ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_Vacuum", customer.NC_Vacuum));
                cmd.Parameters.Add(new SqlParameter(@"NC_DoDishes", customer.NC_DoDishes));
                cmd.Parameters.Add(new SqlParameter(@"NC_ChangeBed", customer.NC_ChangeBed));
                cmd.Parameters.Add(new SqlParameter(@"NC_Pets", (object)customer.NC_Pets ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_FlooringCarpet", customer.NC_FlooringCarpet));
                cmd.Parameters.Add(new SqlParameter(@"NC_FlooringHardwood", customer.NC_FlooringHardwood));
                cmd.Parameters.Add(new SqlParameter(@"NC_FlooringTile", customer.NC_FlooringTile));
                cmd.Parameters.Add(new SqlParameter(@"NC_FlooringLinoleum", customer.NC_FlooringLinoleum));
                cmd.Parameters.Add(new SqlParameter(@"NC_FlooringSlate", customer.NC_FlooringSlate));
                cmd.Parameters.Add(new SqlParameter(@"NC_FlooringMarble", customer.NC_FlooringMarble));
                cmd.Parameters.Add(new SqlParameter(@"NC_EnterHome", (object)customer.NC_EnterHome ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_RequiresKeys", customer.NC_RequiresKeys));
                cmd.Parameters.Add(new SqlParameter(@"NC_Organize", customer.NC_Organize));
                cmd.Parameters.Add(new SqlParameter(@"NC_CleanRating", (object)customer.NC_CleanRating ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_CleaningType", (object)customer.NC_CleaningType ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_GateCode", (object)customer.NC_GateCode ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_GarageCode", (object)customer.NC_GarageCode ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_DoorCode", (object)customer.NC_DoorCode ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"NC_RequestEcoCleaners", customer.NC_RequestEcoCleaners));
                cmd.Parameters.Add(new SqlParameter(@"DC_Blinds", customer.DC_Blinds));
                cmd.Parameters.Add(new SqlParameter(@"DC_BlindsAmount", (object)customer.DC_BlindsAmount ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"DC_BlindsCondition", (object)customer.DC_BlindsCondition ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"DC_Windows", customer.DC_Windows));
                cmd.Parameters.Add(new SqlParameter(@"DC_WindowsAmount", (object)customer.DC_WindowsAmount ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"DC_WindowsSills", (object)customer.DC_WindowsSills ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"DC_Walls", customer.DC_Walls));
                cmd.Parameters.Add(new SqlParameter(@"DC_WallsDetail", (object)customer.DC_WallsDetail ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"DC_Baseboards", (object)customer.DC_Baseboards ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"DC_DoorFrames", customer.DC_DoorFrames));
                cmd.Parameters.Add(new SqlParameter(@"DC_LightSwitches", customer.DC_LightSwitches));
                cmd.Parameters.Add(new SqlParameter(@"DC_VentCovers", customer.DC_VentCovers));
                cmd.Parameters.Add(new SqlParameter(@"DC_InsideVents", customer.DC_InsideVents));
                cmd.Parameters.Add(new SqlParameter(@"DC_Pantry", customer.DC_Pantry));
                cmd.Parameters.Add(new SqlParameter(@"DC_LaundryRoom", customer.DC_LaundryRoom));
                cmd.Parameters.Add(new SqlParameter(@"DC_CeilingFans", customer.DC_CeilingFans));
                cmd.Parameters.Add(new SqlParameter(@"DC_CeilingFansAmount", (object)customer.DC_CeilingFansAmount ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"DC_LightFixtures", customer.DC_LightFixtures));
                cmd.Parameters.Add(new SqlParameter(@"DC_KitchenCuboards", customer.DC_KitchenCuboards));
                cmd.Parameters.Add(new SqlParameter(@"DC_KitchenCuboardsDetail", (object)customer.DC_KitchenCuboardsDetail ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"DC_BathroomCuboards", customer.DC_BathroomCuboards));
                cmd.Parameters.Add(new SqlParameter(@"DC_BathroomCuboardsDetail", (object)customer.DC_BathroomCuboardsDetail ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"DC_Oven", customer.DC_Oven));
                cmd.Parameters.Add(new SqlParameter(@"DC_Refrigerator", customer.DC_Refrigerator));
                cmd.Parameters.Add(new SqlParameter(@"DC_OtherOne", (object)customer.DC_OtherOne ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"DC_OtherTwo", (object)customer.DC_OtherTwo ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"CC_SquareFootage", (object)customer.CC_SquareFootage ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"CC_RoomCountSmall", (object)customer.CC_RoomCountSmall ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"CC_RoomCountLarge", (object)customer.CC_RoomCountLarge ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"CC_PetOdorAdditive", customer.CC_PetOdorAdditive));
                cmd.Parameters.Add(new SqlParameter(@"CC_Details", (object)customer.CC_Details ?? ""));
                cmd.Parameters.Add(new SqlParameter(@"WW_BuildingStyle", (object)customer.WW_BuildingStyle ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"WW_BuildingLevels", (object)customer.WW_BuildingLevels ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"WW_VaultedCeilings", customer.WW_VaultedCeilings));
                cmd.Parameters.Add(new SqlParameter(@"WW_PostConstruction", customer.WW_PostConstruction));
                cmd.Parameters.Add(new SqlParameter(@"WW_WindowCount", (object)customer.WW_WindowCount ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"WW_WindowType", (object)customer.WW_WindowType ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"WW_InsidesOutsides", (object)customer.WW_InsidesOutsides ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"WW_Razor", customer.WW_Razor));
                cmd.Parameters.Add(new SqlParameter(@"WW_RazorCount", (object)customer.WW_RazorCount ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"WW_HardWater", customer.WW_HardWater));
                cmd.Parameters.Add(new SqlParameter(@"WW_HardWaterCount", (object)customer.WW_HardWaterCount ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"WW_FrenchWindows", customer.WW_FrenchWindows));
                cmd.Parameters.Add(new SqlParameter(@"WW_FrenchWindowCount", (object)customer.WW_FrenchWindowCount ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"WW_StormWindows", customer.WW_StormWindows));
                cmd.Parameters.Add(new SqlParameter(@"WW_StormWindowCount", (object)customer.WW_StormWindowCount ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"WW_Screens", customer.WW_Screens));
                cmd.Parameters.Add(new SqlParameter(@"WW_ScreensCount", (object)customer.WW_ScreensCount ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"WW_Tracks", customer.WW_Tracks));
                cmd.Parameters.Add(new SqlParameter(@"WW_TracksCount", (object)customer.WW_TracksCount ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"WW_Wells", customer.WW_Wells));
                cmd.Parameters.Add(new SqlParameter(@"WW_WellsCount", (object)customer.WW_WellsCount ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"WW_Gutters", customer.WW_Gutters));
                cmd.Parameters.Add(new SqlParameter(@"WW_GuttersFeet", (object)customer.WW_GuttersFeet ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"WW_Details", (object)customer.WW_Details ?? ""));
                cmd.Parameters.Add(new SqlParameter(@"HW_Frequency", (object)customer.HW_Frequency ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"HW_StartDate", (object)customer.HW_StartDate ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"HW_EndDate", (object)customer.HW_EndDate ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"HW_GarbageCans", customer.HW_GarbageCans));
                cmd.Parameters.Add(new SqlParameter(@"HW_GarbageDay", (object)customer.HW_GarbageDay ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"HW_PlantsWatered", customer.HW_PlantsWatered));
                cmd.Parameters.Add(new SqlParameter(@"HW_PlantsWateredFrequency", (object)customer.HW_PlantsWateredFrequency ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"HW_Thermostat", customer.HW_Thermostat));
                cmd.Parameters.Add(new SqlParameter(@"HW_ThermostatTemperature", (object)customer.HW_ThermostatTemperature ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"HW_Breakers", customer.HW_Breakers));
                cmd.Parameters.Add(new SqlParameter(@"HW_BreakersLocation", (object)customer.HW_BreakersLocation ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"HW_CleanBeforeReturn", customer.HW_CleanBeforeReturn));
                cmd.Parameters.Add(new SqlParameter(@"HW_Details", (object)customer.HW_Details ?? ""));
                cmd.Parameters.Add(new SqlParameter(@"TakePic", (bool)customer.TakePic));
                sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    customer.customerID = (int)sqlDataReader["customerID"];
                }

                return null;
            }
            catch (Exception ex)
            {
                return "SQL SetCustomer EX: " + ex.Message;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region SetCustomerWelcomeLetter
        public static string SetCustomerWelcomeLetter(int customerID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE
                    Customers
                SET
                    welcomeLetter = 1
	            WHERE
		            customerID = @customerID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"customerID", customerID));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL SetCustomerWelcomeLetter EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region DeleteCustomer
        public static string DeleteCustomer(int customerID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                if (customerID <= 0)
                    return "Cannot Delete New Customer";

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                IF NOT EXISTS (SELECT * FROM Appointments WHERE customerID = @customerID)
                BEGIN
                    DELETE FROM 
                        Customers 
                    WHERE 
                        customerID = @customerID;
                END
                ELSE
                BEGIN
                    THROW 50000, 'Cannot delete a customer with appointments', 1;
                END";
                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"customerID", customerID));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_Appointments_Customers")) return "Cannot Delete Customer with Attached Appointments";
                return "SQL DeleteCustomer EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetAppsByCustomerID
        public static List<AppStruct> GetAppsByCustomerID(int customerID)
        {
            List<AppStruct> ret = new List<AppStruct>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        A.appointmentID,
                        A.appStatus,
                        A.appType,
                        A.dateCreated,
                        A.dateUpdated,
                        A.appointmentDate,
                        A.startTime,
                        A.endTime,
                        A.customerID,
                        A.customerHours,
                        A.customerRate,
                        A.customerServiceFee,
                        A.customerSubContractor,
                        A.customerDiscountAmount,
                        A.customerDiscountPercent,
                        A.customerDiscountReferral,
                        A.contractorID,
                        A.contractorHours,
                        A.contractorRate,
                        A.contractorTips,
                        A.contractorAdjustAmount,
                        A.contractorAdjustType,
                        A.amountPaid,
                        A.paymentFinished,
                        A.recurrenceID,
                        A.recurrenceType,
                        A.weeklyFrequency,
                        A.monthlyWeek,
                        A.monthlyDay,
                        A.followUpSent,
                        A.salesTax,
                        CO.contractorType,
                        CO.firstName,
                        CO.lastName,
                        CO.businessName,
                        A.notes,
                        A.jobStartTime,
                        A.jobEndTime,
                        A.ShareLocation,
                        A.JobCompleted

                    FROM
                        Appointments A LEFT JOIN Contractors CO ON A.contractorID = CO.contractorID
                    WHERE
                        A.customerID = @customerID
                    ORDER BY
                        A.appointmentDate DESC, A.startTime DESC, A.endTime DESC, CO.firstName, CO.lastName";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    AppStruct app = new AppStruct();
                    app.appointmentID = (int)sqlDataReader["appointmentID"];
                    app.appStatus = (int)sqlDataReader["appStatus"];
                    app.appType = (int)sqlDataReader["appType"];
                    app.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                    app.dateUpdated = (DateTime)sqlDataReader["dateUpdated"];
                    app.appointmentDate = ((DateTime)sqlDataReader["appointmentDate"]).Date;
                    app.startTime = Globals.TimeOnly((DateTime)sqlDataReader["startTime"]);
                    app.endTime = Globals.TimeOnly((DateTime)sqlDataReader["endTime"]);
                    app.customerID = (int)sqlDataReader["customerID"];
                    app.customerHours = (decimal)sqlDataReader["customerHours"];
                    app.customerRate = (decimal)sqlDataReader["customerRate"];
                    app.customerServiceFee = (decimal)sqlDataReader["customerServiceFee"];
                    app.customerSubContractor = (decimal)sqlDataReader["customerSubContractor"];
                    app.customerDiscountAmount = (decimal)sqlDataReader["customerDiscountAmount"];
                    app.customerDiscountPercent = (decimal)sqlDataReader["customerDiscountPercent"];
                    app.customerDiscountReferral = (decimal)sqlDataReader["customerDiscountReferral"];
                    app.contractorTitle = Globals.FormatContractorTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                    app.contractorID = sqlDataReader["contractorID"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorID"];
                    app.contractorType = sqlDataReader["contractorType"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorType"];
                    app.contractorHours = (decimal)sqlDataReader["contractorHours"];
                    app.contractorRate = (decimal)sqlDataReader["contractorRate"];
                    app.contractorTips = (decimal)sqlDataReader["contractorTips"];
                    app.contractorAdjustAmount = (decimal)sqlDataReader["contractorAdjustAmount"];
                    app.contractorAdjustType = sqlDataReader["contractorAdjustType"] == DBNull.Value ? null : (string)sqlDataReader["contractorAdjustType"];
                    app.amountPaid = (decimal)sqlDataReader["amountPaid"];
                    app.paymentFinished = (bool)sqlDataReader["paymentFinished"];
                    app.followUpSent = (bool)sqlDataReader["followUpSent"];
                    app.recurrenceID = (int)sqlDataReader["recurrenceID"];
                    app.recurrenceType = (int)sqlDataReader["recurrenceType"];
                    app.weeklyFrequency = (int)sqlDataReader["weeklyFrequency"];
                    app.monthlyWeek = (int)sqlDataReader["monthlyWeek"];
                    app.monthlyDay = (int)sqlDataReader["monthlyDay"];
                    app.salesTax = (decimal)sqlDataReader["salesTax"];
                    app.JobCompleted = (bool)sqlDataReader["JobCompleted"];

                    app.ShareLocation = (bool)sqlDataReader["ShareLocation"];
                    app.Notes = sqlDataReader["notes"] == DBNull.Value ? null : (string)sqlDataReader["notes"];
                    app.jobStartTime = sqlDataReader["jobStartTime"] != DBNull.Value ? (DateTime?)sqlDataReader["jobStartTime"] : null;
                    app.jobEndTime = sqlDataReader["jobEndTime"] != DBNull.Value ? (DateTime?)sqlDataReader["jobEndTime"] : null;
                    ret.Add(app);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

            return ret;
        }
        #endregion

        #region GetAppsByDateRange
        public static List<AppStruct> GetAppsByDateRange(int franchiseMask, DateTime startDate, DateTime endDate, string orderBy, bool showIgnored)
        {
            List<AppStruct> ret = new List<AppStruct>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                Globals.FormatDateRange(ref startDate, ref endDate);

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        A.appointmentID,
                        A.appStatus,
                        A.appType,
                        A.appointmentDate,
                        A.startTime,
                        A.endTime,
                        A.customerID,
                        A.customerHours,
                        A.customerRate,
                        A.customerServiceFee,
                        A.customerSubContractor,
                        A.customerDiscountAmount,
                        A.customerDiscountPercent,
                        A.customerDiscountReferral,
                        A.contractorID,
                        A.contractorHours,
                        A.contractorRate,
                        A.contractorTips,
                        A.contractorAdjustAmount,
                        A.contractorAdjustType,
                        A.amountPaid,
                        A.paymentFinished,
                        A.confirmed,
                        A.leftMessage,
                        A.sentSMS,
                        A.sentWeekSMS,
                        A.sentEmail,
                        A.keysReturned,
                        A.followUpSent,
                        A.salesTax,
                        CO.contractorType,
                        CO.firstName AS coFirstName,
                        CO.lastName AS coLastName,
                        CO.businessName AS coBusinessName,
                        CO.bestPhone AS coBestPhone,
                        CU.franchiseMask,
                        CU.firstName AS cuFirstName,
                        CU.lastName AS cuLastName,
                        CU.customNote,
                        CU.businessName,
                        CU.accountStatus,
                        CU.paymentType,
                        CU.creditCardExpMonth,
                        CU.creditCardExpYear,
                        CU.locationAddress,
                        CU.locationCity,
                        CU.locationState,
                        CU.locationZip,
                        CU.email,
                        CU.bestPhone,
                        CU.bestPhoneCell,
                        CU.alternatePhoneOne,
                        CU.alternatePhoneOneCell,
                        CU.alternatePhoneTwo,
                        CU.alternatePhoneTwoCell,
                        CU.preferredContact,
                        CU.NC_Special,
                        CU.NC_TimeOfDayPrefix,
                        CU.NC_TimeOfDay,
                        CU.NC_DayOfWeek,
                        CU.NC_RequiresKeys,
                        A.notes,
                        A.jobStartTime,
                        A.jobEndTime,
                        A.ShareLocation,
                        A.JobCompleted

                    FROM
                        Appointments A,
                        Customers CU,
                        Contractors CO
                    WHERE
                        CU.franchiseMask & @franchiseMask > 0 AND
                        A.appointmentDate >= @startDate AND
                        A.appointmentDate <= @endDate AND
                        A.customerID = CU.customerID AND
                        A.contractorID = CO.contractorID
                    ORDER BY
                        " + orderBy;

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("startDate", startDate));
                cmd.Parameters.Add(new SqlParameter("endDate", endDate));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    if (!showIgnored && Globals.SafeSqlString(sqlDataReader["accountStatus"]) == "Ignored")
                        continue;

                    AppStruct app = new AppStruct();
                    app.appointmentID = (int)sqlDataReader["appointmentID"];
                    app.appStatus = (int)sqlDataReader["appStatus"];
                    app.appType = (int)sqlDataReader["appType"];
                    app.franchiseMask = (int)sqlDataReader["franchiseMask"];
                    app.appointmentDate = ((DateTime)sqlDataReader["appointmentDate"]).Date;
                    app.startTime = Globals.TimeOnly((DateTime)sqlDataReader["startTime"]);
                    app.endTime = Globals.TimeOnly((DateTime)sqlDataReader["endTime"]);

                    app.customerTitle = Globals.FormatCustomerTitle(sqlDataReader["cuFirstName"], sqlDataReader["cuLastName"], sqlDataReader["businessName"]);
                    app.customerTitleCustomNote = Globals.FormatCustomerTitle(sqlDataReader["cuFirstName"], sqlDataReader["cuLastName"], sqlDataReader["customNote"]);
                    app.customerID = (int)sqlDataReader["customerID"];
                    app.customerAccountStatus = Globals.SafeSqlString(sqlDataReader["accountStatus"]);
                    app.customerHours = (decimal)sqlDataReader["customerHours"];
                    app.customerRate = (decimal)sqlDataReader["customerRate"];
                    app.customerServiceFee = (decimal)sqlDataReader["customerServiceFee"];
                    app.customerSubContractor = (decimal)sqlDataReader["customerSubContractor"];
                    app.customerDiscountAmount = (decimal)sqlDataReader["customerDiscountAmount"];
                    app.customerDiscountPercent = (decimal)sqlDataReader["customerDiscountPercent"];
                    app.customerDiscountReferral = (decimal)sqlDataReader["customerDiscountReferral"];
                    app.customerPaymentType = Globals.SafeSqlString(sqlDataReader["paymentType"]);
                    app.customerCardExpMonth = Globals.SafeSqlString(sqlDataReader["creditCardExpMonth"]);
                    app.customerCardExpYear = Globals.SafeSqlString(sqlDataReader["creditCardExpYear"]);
                    app.customerAddress = Globals.SafeSqlString(sqlDataReader["locationAddress"]);
                    app.customerCity = Globals.SafeSqlString(sqlDataReader["locationCity"]);
                    app.customerState = Globals.SafeSqlString(sqlDataReader["locationState"]);
                    app.customerZip = Globals.SafeSqlString(sqlDataReader["locationZip"]);
                    app.customerEmail = Globals.SafeSqlString(sqlDataReader["email"]);
                    app.customerPhoneOne = Globals.SafeSqlString(sqlDataReader["bestPhone"]);
                    app.customerPhoneOneCell = (bool)sqlDataReader["bestPhoneCell"];
                    app.customerPhoneTwo = Globals.SafeSqlString(sqlDataReader["alternatePhoneOne"]);
                    app.customerPhoneTwoCell = (bool)sqlDataReader["alternatePhoneOneCell"];
                    app.customerPhoneThree = Globals.SafeSqlString(sqlDataReader["alternatePhoneTwo"]);
                    app.customerPhoneThreeCell = (bool)sqlDataReader["alternatePhoneTwoCell"];
                    app.customerPreferredContact = Globals.SafeSqlString(sqlDataReader["preferredContact"]);
                    app.customerSpecial = Globals.SafeSqlString(sqlDataReader["NC_Special"]);
                    app.customerTimeOfDayPrefix = Globals.SafeSqlString(sqlDataReader["NC_TimeOfDayPrefix"]);
                    app.customerTimeOfDay = Globals.SafeSqlString(sqlDataReader["NC_TimeOfDay"]);
                    app.customerDayOfWeek = Globals.SafeSqlString(sqlDataReader["NC_DayOfWeek"]);
                    app.customerKeys = (bool)sqlDataReader["NC_RequiresKeys"];

                    app.contractorTitle = Globals.FormatContractorTitle(sqlDataReader["coFirstName"], sqlDataReader["coLastName"], sqlDataReader["coBusinessName"]);
                    app.contractorID = sqlDataReader["contractorID"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorID"];
                    app.contractorType = sqlDataReader["contractorType"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorType"];
                    app.contractorPhone = Globals.SafeSqlString(sqlDataReader["coBestPhone"]);
                    app.contractorHours = (decimal)sqlDataReader["contractorHours"];
                    app.contractorRate = (decimal)sqlDataReader["contractorRate"];
                    app.contractorTips = (decimal)sqlDataReader["contractorTips"];
                    app.contractorAdjustAmount = (decimal)sqlDataReader["contractorAdjustAmount"];
                    app.contractorAdjustType = Globals.SafeSqlString(sqlDataReader["contractorAdjustType"]);
                    app.amountPaid = (decimal)sqlDataReader["amountPaid"];
                    app.paymentFinished = (bool)sqlDataReader["paymentFinished"];
                    app.confirmed = (bool)sqlDataReader["confirmed"];
                    app.leftMessage = (bool)sqlDataReader["leftMessage"];
                    app.sentSMS = (bool)sqlDataReader["sentSMS"];
                    app.sentWeekSMS = (bool)sqlDataReader["sentWeekSMS"];
                    app.sentEmail = (bool)sqlDataReader["sentEmail"];
                    app.keysReturned = (bool)sqlDataReader["keysReturned"];
                    app.followUpSent = (bool)sqlDataReader["followUpSent"];
                    app.ShareLocation = (bool)sqlDataReader["ShareLocation"];
                    app.JobCompleted = (bool)sqlDataReader["JobCompleted"];
                    app.salesTax = (decimal)sqlDataReader["salesTax"];
                    app.Notes = sqlDataReader["notes"] == DBNull.Value ? null : (string)sqlDataReader["notes"];
                    app.jobStartTime = sqlDataReader["jobStartTime"] != DBNull.Value ? (DateTime?)sqlDataReader["jobStartTime"] : null;
                    app.jobEndTime = sqlDataReader["jobEndTime"] != DBNull.Value ? (DateTime?)sqlDataReader["jobEndTime"] : null;

                    ret.Add(app);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region GetAppsByBooking
        public static List<AppStruct> GetAppsByBooking(int franchiseMask, DateTime startDate, DateTime endDate)
        {
            List<AppStruct> ret = new List<AppStruct>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                Globals.FormatDateRange(ref startDate, ref endDate);

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        A.appointmentID,
                        A.appStatus,
                        A.appType,
                        A.dateCreated,
                        A.appointmentDate,
                        A.startTime,
                        A.endTime,
                        A.customerID,
                        A.customerHours,
                        A.contractorID,
                        A.username,
                        A.usernameBooked,
                        CO.contractorType,
                        CO.firstName AS coFirstName,
                        CO.lastName AS coLastName,
                        CO.businessName AS coBusinessName,
                        CU.franchiseMask,
                        CU.firstName AS cuFirstName,
                        CU.lastName AS cuLastName,
                        CU.businessName,
                        CU.customNote,
                        CU.accountStatus
                    FROM
                        Appointments A,
                        Customers CU,
                        Contractors CO
                    WHERE
                        CU.franchiseMask & @franchiseMask > 0 AND
                        A.username IS NOT NULL AND
                        CU.accountStatus != 'Ignored' AND
                        A.dateCreated >= @startDate AND
                        A.dateCreated <= @endDate AND
                        A.customerID = CU.customerID AND
                        A.contractorID = CO.contractorID
                    ORDER BY
                        CONVERT(date, DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), A.dateCreated)) DESC, A.username, DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), A.dateCreated) DESC";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("startDate", startDate.ToUniversalTime()));
                cmd.Parameters.Add(new SqlParameter("endDate", endDate.ToUniversalTime()));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    AppStruct app = new AppStruct();
                    app.appointmentID = (int)sqlDataReader["appointmentID"];
                    app.appStatus = (int)sqlDataReader["appStatus"];
                    app.appType = (int)sqlDataReader["appType"];
                    app.franchiseMask = (int)sqlDataReader["franchiseMask"];
                    app.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                    app.appointmentDate = ((DateTime)sqlDataReader["appointmentDate"]).Date;
                    app.startTime = Globals.TimeOnly((DateTime)sqlDataReader["startTime"]);
                    app.endTime = Globals.TimeOnly((DateTime)sqlDataReader["endTime"]);
                    app.username = Globals.SafeSqlString(sqlDataReader["username"]);
                    app.usernameBooked = (bool)sqlDataReader["usernameBooked"];

                    app.customerTitle = Globals.FormatCustomerTitle(sqlDataReader["cuFirstName"], sqlDataReader["cuLastName"], sqlDataReader["businessName"]);
                    app.customerTitleCustomNote = Globals.FormatCustomerTitle(sqlDataReader["cuFirstName"], sqlDataReader["cuLastName"], sqlDataReader["customNote"]);
                    app.customerID = (int)sqlDataReader["customerID"];
                    app.customerAccountStatus = Globals.SafeSqlString(sqlDataReader["accountStatus"]);
                    app.customerHours = (decimal)sqlDataReader["customerHours"];

                    app.contractorTitle = Globals.FormatContractorTitle(sqlDataReader["coFirstName"], sqlDataReader["coLastName"], sqlDataReader["coBusinessName"]);
                    app.contractorID = sqlDataReader["contractorID"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorID"];
                    app.contractorType = sqlDataReader["contractorType"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorType"];
                    ret.Add(app);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

            return ret;
        }
        #endregion

        #region GetAppsByEarnedHours
        public static List<AppStruct> GetAppsByEarnedHours(int franchiseMask, DateTime startDate, DateTime endDate)
        {
            List<AppStruct> ret = new List<AppStruct>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                Globals.FormatDateRange(ref startDate, ref endDate);

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        A.appointmentID,
                        A.appStatus,
                        A.appType,
                        A.dateCreated,
                        A.appointmentDate,
                        A.startTime,
                        A.endTime,
                        A.customerID,
                        A.customerHours,
                        A.contractorID,
                        A.username,
                        A.usernameBooked,
                        CO.contractorType,
                        CO.firstName AS coFirstName,
                        CO.lastName AS coLastName,
                        CO.businessName AS coBusinessName,
                        CU.franchiseMask,
                        CU.firstName AS cuFirstName,
                        CU.lastName AS cuLastName,
                        CU.businessName,
                        CU.customNote,
                        CU.accountStatus
                    FROM
                        Appointments A,
                        Customers CU,
                        Contractors CO
                    WHERE
                        CU.franchiseMask & @franchiseMask > 0 AND
                        A.username IS NOT NULL AND
                        A.appStatus = 0 AND
                        CU.accountStatus != 'Ignored' AND
                        A.appointmentDate >= @startDate AND
                        A.appointmentDate <= @endDate AND
                        A.customerID = CU.customerID AND
                        A.contractorID = CO.contractorID
                    ORDER BY
                        A.appointmentDate, A.customerID, A.dateCreated";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("startDate", startDate));
                cmd.Parameters.Add(new SqlParameter("endDate", endDate));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    AppStruct app = new AppStruct();
                    app.appointmentID = (int)sqlDataReader["appointmentID"];
                    app.appStatus = (int)sqlDataReader["appStatus"];
                    app.appType = (int)sqlDataReader["appType"];
                    app.franchiseMask = (int)sqlDataReader["franchiseMask"];
                    app.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                    app.appointmentDate = ((DateTime)sqlDataReader["appointmentDate"]).Date;
                    app.startTime = Globals.TimeOnly((DateTime)sqlDataReader["startTime"]);
                    app.endTime = Globals.TimeOnly((DateTime)sqlDataReader["endTime"]);
                    app.username = Globals.SafeSqlString(sqlDataReader["username"]);
                    app.usernameBooked = (bool)sqlDataReader["usernameBooked"];

                    app.customerTitle = Globals.FormatCustomerTitle(sqlDataReader["cuFirstName"], sqlDataReader["cuLastName"], sqlDataReader["businessName"]);
                    app.customerTitleCustomNote = Globals.FormatCustomerTitle(sqlDataReader["cuFirstName"], sqlDataReader["cuLastName"], sqlDataReader["customNote"]);
                    app.customerID = (int)sqlDataReader["customerID"];
                    app.customerAccountStatus = Globals.SafeSqlString(sqlDataReader["accountStatus"]);
                    app.customerHours = (decimal)sqlDataReader["customerHours"];

                    app.contractorTitle = Globals.FormatContractorTitle(sqlDataReader["coFirstName"], sqlDataReader["coLastName"], sqlDataReader["coBusinessName"]);
                    app.contractorID = sqlDataReader["contractorID"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorID"];
                    app.contractorType = sqlDataReader["contractorType"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorType"];
                    ret.Add(app);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

            return ret;
        }
        #endregion

        #region GetCleaningProjects
        public static List<ConAppStruct> GetCleaningProjects(int franchiseMask, DateTime startDate, DateTime endDate)
        {
            List<ConAppStruct> ret = new List<ConAppStruct>();
            Dictionary<KeyValuePair<DateTime, int>, decimal> collectDict = new Dictionary<KeyValuePair<DateTime, int>, decimal>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                Globals.FormatDateRange(ref startDate, ref endDate);

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        A.appointmentID,
                        A.appType,
                        A.appointmentDate,
                        A.startTime,
                        A.endTime,
                        A.customerID,
                        A.customerHours,
                        A.customerRate,
                        A.customerServiceFee,
                        A.customerSubContractor,
                        A.customerDiscountAmount,
                        A.customerDiscountPercent,
                        A.customerDiscountReferral,
                        A.contractorID,
                        A.contractorTips,
                        A.salesTax,
                        CO.contractorID,
                        CO.firstName AS coFirstName,
                        CO.lastName AS coLastName,
                        CO.businessName AS coBusinessName,
                        CO.address AS contractorAddress,
                        CO.city AS contractorCity,
                        CO.state AS contractorState,
                        CO.zip AS contractorZip,
                        CO.bestPhone AS contractorPhone,
                        CU.firstName AS cuFirstName,
                        CU.lastName AS cuLastName,
                        CU.accountStatus,
                        CU.newBuilding,
                        CU.businessName,
                        CU.paymentType,
                        CU.locationAddress,
                        CU.locationCity,
                        CU.locationState,
                        CU.locationZip,
                        CU.bestPhone,
                        CU.alternatePhoneOne,
                        CU.alternatePhoneTwo,
                        CU.serviceFee,
                        CU.ratePerHour,
                        CU.discountPercent,
                        CU.discountAmount,
                        CU.NC_SquareFootage,
                        CU.NC_Details,
                        CU.NC_Bathrooms,
                        CU.NC_Bedrooms,
                        CU.NC_vacuum,
                        CU.NC_DoDishes,
                        CU.NC_ChangeBed,
                        CU.NC_Pets,
                        CU.NC_FlooringType,
                        CU.NC_FlooringCarpet,
                        CU.NC_FlooringHardwood,
                        CU.NC_FlooringTile,
                        CU.NC_FlooringLinoleum,
                        CU.NC_FlooringSlate,
                        CU.NC_FlooringMarble,
                        CU.NC_EnterHome,
                        CU.NC_RequiresKeys,
                        CU.NC_Organize,
                        CU.NC_CleanRating,
                        CU.NC_CleaningType,
                        CU.NC_GateCode,
                        CU.NC_GarageCode,
                        CU.NC_DoorCode,
                        CU.NC_RequestEcoCleaners,
                        CU.DC_Blinds,
                        CU.DC_BlindsAmount,
                        CU.DC_BlindsCondition,
                        CU.DC_Windows,
                        CU.DC_WindowsAmount,
                        CU.DC_WindowsSills,
                        CU.DC_Walls,
                        CU.DC_WallsDetail, 
                        CU.DC_Baseboards,
                        CU.DC_DoorFrames,
                        CU.DC_CeilingFans,
                        CU.DC_CeilingFansAmount,
                        CU.DC_LightFixtures,
                        CU.DC_LightSwitches,
                        CU.DC_VentCovers,
                        CU.DC_InsideVents,
                        CU.DC_Pantry,
                        CU.DC_LaundryRoom,
                        CU.DC_KitchenCuboards,
                        CU.DC_KitchenCuboardsDetail,
                        CU.DC_BathroomCuboards,
                        CU.DC_BathroomCuboardsDetail,
                        CU.DC_Oven,
                        CU.DC_Refrigerator,
                        CU.DC_OtherOne,
                        CU.DC_OtherTwo,
                        CU.CC_SquareFootage,
                        CU.CC_RoomCountSmall,
                        CU.CC_RoomCountLarge,
                        CU.CC_PetOdorAdditive,
                        CU.CC_Details,
                        CU.WW_BuildingStyle,
                        CU.WW_BuildingLevels,
                        CU.WW_VaultedCeilings,
                        CU.WW_PostConstruction,
                        CU.WW_WindowCount,
                        CU.WW_WindowType,
                        CU.WW_InsidesOutsides,
                        CU.WW_Razor,
                        CU.WW_RazorCount,
                        CU.WW_HardWater,
                        CU.WW_HardWaterCount,
                        CU.WW_FrenchWindows,
                        CU.WW_FrenchWindowCount,
                        CU.WW_StormWindows,
                        CU.WW_StormWindowCount,
                        CU.WW_Screens,
                        CU.WW_ScreensCount,
                        CU.WW_Tracks,
                        CU.WW_TracksCount,
                        CU.WW_Wells,
                        CU.WW_WellsCount,
                        CU.WW_Gutters,
                        CU.WW_GuttersFeet,
                        CU.WW_Details,
                        CU.HW_Frequency,
                        CU.HW_StartDate,
                        CU.HW_EndDate,
                        CU.HW_GarbageCans,
                        CU.HW_GarbageDay,
                        CU.HW_PlantsWatered,
                        CU.HW_PlantsWateredFrequency,
                        CU.HW_Thermostat,
                        CU.HW_ThermostatTemperature,
                        CU.HW_Breakers,
                        CU.HW_BreakersLocation,
                        CU.HW_CleanBeforeReturn,
                        CU.HW_Details
                    FROM
                        Appointments A,
                        Customers CU,
                        Contractors CO
                    WHERE
                        CU.franchiseMask & @franchiseMask > 0 AND
                        A.appointmentDate >= @startDate AND
                        A.appointmentDate <= @endDate AND
                        A.customerID = CU.customerID AND
                        A.contractorID = CO.contractorID AND
                        A.appStatus = 0
                    ORDER BY
                        A.appointmentDate, A.startTime";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("startDate", startDate));
                cmd.Parameters.Add(new SqlParameter("endDate", endDate));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    ConAppStruct conApp = new ConAppStruct();
                    conApp.appointmentID = (int)sqlDataReader["appointmentID"];
                    conApp.appType = (int)sqlDataReader["appType"];
                    conApp.appointmentDate = ((DateTime)sqlDataReader["appointmentDate"]).Date;
                    conApp.startTime = (DateTime)sqlDataReader["startTime"];
                    conApp.endTime = (DateTime)sqlDataReader["endTime"];
                    conApp.contractorID = (int)sqlDataReader["contractorID"];
                    conApp.contractorTitle = Globals.FormatContractorTitle(sqlDataReader["coFirstName"], sqlDataReader["coLastName"], sqlDataReader["coBusinessName"]);
                    conApp.contractorAddress = Globals.SafeSqlString(sqlDataReader["contractorAddress"]);
                    conApp.contractorCity = Globals.SafeSqlString(sqlDataReader["contractorCity"]);
                    conApp.contractorState = Globals.SafeSqlString(sqlDataReader["contractorState"]);
                    conApp.contractorZip = Globals.SafeSqlString(sqlDataReader["contractorZip"]);
                    conApp.contractorPhone = Globals.SafeSqlString(sqlDataReader["contractorPhone"]);
                    conApp.customerID = (int)sqlDataReader["customerID"];
                    conApp.customerServiceFee = (decimal)sqlDataReader["serviceFee"];
                    conApp.customerRate = (decimal)sqlDataReader["ratePerHour"];
                    conApp.customerTitle = Globals.FormatCustomerTitle(sqlDataReader["cuFirstName"], sqlDataReader["cuLastName"], sqlDataReader["businessName"]);
                    conApp.customerAccountStatus = Globals.SafeSqlString(sqlDataReader["accountStatus"]);
                    conApp.paymentType = Globals.SafeSqlString(sqlDataReader["paymentType"]);
                    conApp.address = Globals.SafeSqlString(sqlDataReader["locationAddress"]);
                    conApp.city = Globals.SafeSqlString(sqlDataReader["locationCity"]);
                    conApp.state = Globals.SafeSqlString(sqlDataReader["locationState"]);
                    conApp.zip = Globals.SafeSqlString(sqlDataReader["locationZip"]);
                    conApp.bestPhone = Globals.SafeSqlString(sqlDataReader["bestPhone"]);
                    conApp.alternatePhoneOne = Globals.SafeSqlString(sqlDataReader["alternatePhoneOne"]);
                    conApp.alternatePhoneTwo = Globals.SafeSqlString(sqlDataReader["alternatePhoneTwo"]);
                    conApp.keys = (bool)sqlDataReader["NC_RequiresKeys"];

                    switch (conApp.appType)
                    {
                        case 1: conApp.details = Globals.SafeSqlString(sqlDataReader["NC_Details"]); break;
                        case 2: conApp.details = Globals.SafeSqlString(sqlDataReader["CC_Details"]); break;
                        case 3: conApp.details = Globals.SafeSqlString(sqlDataReader["WW_Details"]); break;
                        case 4: conApp.details = Globals.SafeSqlString(sqlDataReader["HW_Details"]); break;
                    }

                    AppStruct app = new AppStruct();
                    app.customerHours = (decimal)sqlDataReader["customerHours"];
                    app.customerRate = (decimal)sqlDataReader["ratePerHour"];
                    app.customerServiceFee = (decimal)sqlDataReader["customerServiceFee"];
                    app.customerSubContractor = (decimal)sqlDataReader["customerSubContractor"];
                    app.customerDiscountPercent = (decimal)sqlDataReader["customerDiscountPercent"];
                    app.customerDiscountAmount = (decimal)sqlDataReader["customerDiscountAmount"];
                    app.customerDiscountReferral = (decimal)sqlDataReader["customerDiscountReferral"];
                    app.salesTax = (decimal)sqlDataReader["salesTax"];
                    decimal collect = Globals.CalculateAppointmentTotal(app);

                    KeyValuePair<DateTime, int> keyValuePair = new KeyValuePair<DateTime, int>(conApp.appointmentDate, conApp.customerID);
                    if (!collectDict.ContainsKey(keyValuePair)) collectDict.Add(keyValuePair, collect);
                    else collectDict[keyValuePair] += collect;

                    List<string> generalList = new List<string>();

                    string enterHome = Globals.SafeSqlString(sqlDataReader["NC_EnterHome"]);
                    if (enterHome.ToLower().Contains("code")) enterHome += "(Call office for code)";
                    if (enterHome != "") generalList.Add(enterHome);
                    if ((bool)sqlDataReader["newBuilding"]) generalList.Add("Newly Constructed");
                    string squareFootage = Globals.SafeSqlString(sqlDataReader["NC_SquareFootage"]);
                    if (squareFootage != "") generalList.Add("SQ.FT(" + squareFootage + ")");
                    string pets = Globals.SafeSqlString(sqlDataReader["NC_Pets"]);
                    if (pets != "" && pets != "None") generalList.Add("Pets(" + pets + ")");


                    List<string> instructionList = new List<string>();

                    if (conApp.appType == 1)
                    {
                        //Housekeeping
                        if ((bool)sqlDataReader["NC_RequiresKeys"]) instructionList.Add("Take Keys");
                        string cleaningType = Globals.SafeSqlString(sqlDataReader["NC_CleaningType"]);
                        if (cleaningType != "") instructionList.Add(cleaningType);
                        string cleanRating = Globals.SafeSqlString(sqlDataReader["NC_CleanRating"]);
                        if (cleanRating != "" && cleanRating != "N/A") instructionList.Add("CleanRating(" + cleanRating + ")");
                        if ((bool)sqlDataReader["NC_Organize"]) instructionList.Add("Organize");
                        if ((bool)sqlDataReader["NC_DoDishes"]) instructionList.Add("Do Dishes");
                        if ((bool)sqlDataReader["NC_ChangeBed"]) instructionList.Add("Change Bed Linens");

                        List<string> flooringList = new List<string>();
                        string flooringType = Globals.SafeSqlString(sqlDataReader["NC_FlooringType"]);
                        if (flooringType != "") flooringList.Add(flooringType);
                        if ((bool)sqlDataReader["NC_FlooringCarpet"]) flooringList.Add("Carpet");
                        if ((bool)sqlDataReader["NC_FlooringHardwood"]) flooringList.Add("Hardwood");
                        if ((bool)sqlDataReader["NC_FlooringTile"]) flooringList.Add("Tile");
                        if ((bool)sqlDataReader["NC_FlooringLinoleum"]) flooringList.Add("Linoleum");
                        if ((bool)sqlDataReader["NC_FlooringSlate"]) flooringList.Add("Slate");
                        if ((bool)sqlDataReader["NC_FlooringMarble"]) flooringList.Add("Marble");
                        if (flooringList.Count > 0) instructionList.Add("Flooring(" + string.Join(", ", flooringList.ToArray()) + ")");
                        if ((bool)sqlDataReader["NC_vacuum"]) instructionList.Add("Take Vacuum");
                        if ((bool)sqlDataReader["NC_RequestEcoCleaners"]) instructionList.Add("Eco Cleaners Requested");

                        string bdrms = Globals.SafeSqlString(sqlDataReader["NC_Bedrooms"]);
                        if (bdrms != "" && bdrms != "0") instructionList.Add("Bdrms(" + bdrms + ")");
                        string baths = Globals.SafeSqlString(sqlDataReader["NC_Bathrooms"]);
                        if (baths != "" && baths != "0") instructionList.Add("Baths(" + baths + ")");

                        if (cleaningType.ToLower().Contains("deep") || cleaningType.ToLower().Contains("plus") || cleaningType.ToLower().Contains("construction"))
                        {
                            if ((bool)sqlDataReader["DC_Blinds"]) instructionList.Add("Blinds(" + Globals.SafeSqlString(sqlDataReader["DC_BlindsAmount"]) + ", " + Globals.SafeSqlString(sqlDataReader["DC_BlindsCondition"]) + ")");
                            if ((bool)sqlDataReader["DC_Windows"]) instructionList.Add("Windows(" + Globals.SafeSqlString(sqlDataReader["DC_WindowsAmount"]) + ")");
                            if ((bool)sqlDataReader["DC_WindowsSills"]) instructionList.Add("Tracks & Sills");
                            if ((bool)sqlDataReader["DC_Walls"]) instructionList.Add("Walls(" + Globals.SafeSqlString(sqlDataReader["DC_WallsDetail"]) + ")");
                            if ((bool)sqlDataReader["DC_CeilingFans"]) instructionList.Add("Ceiling Fans(" + Globals.SafeSqlString(sqlDataReader["DC_CeilingFansAmount"]) + ")");
                            if ((bool)sqlDataReader["DC_KitchenCuboards"]) instructionList.Add("Kitch Cupboards(" + Globals.SafeSqlString(sqlDataReader["DC_KitchenCuboardsDetail"]) + ")");
                            if ((bool)sqlDataReader["DC_BathroomCuboards"]) instructionList.Add("Bath Cupboards(" + Globals.SafeSqlString(sqlDataReader["DC_BathroomCuboardsDetail"]) + ")");
                            if ((bool)sqlDataReader["DC_Baseboards"]) instructionList.Add("Baseboards");
                            if ((bool)sqlDataReader["DC_DoorFrames"]) instructionList.Add("Doors/DoorFrames");
                            if ((bool)sqlDataReader["DC_LightFixtures"]) instructionList.Add("Light Fixtures");
                            if ((bool)sqlDataReader["DC_LightSwitches"]) instructionList.Add("Light Switches");
                            if ((bool)sqlDataReader["DC_VentCovers"]) instructionList.Add("Vent Covers");
                            if ((bool)sqlDataReader["DC_InsideVents"]) instructionList.Add("Inside Vents");
                            if ((bool)sqlDataReader["DC_Pantry"]) instructionList.Add("Pantry");
                            if ((bool)sqlDataReader["DC_LaundryRoom"]) instructionList.Add("Laundry Room");
                            if ((bool)sqlDataReader["DC_Oven"]) instructionList.Add("Oven");
                            if ((bool)sqlDataReader["DC_Refrigerator"]) instructionList.Add("Refrigerator");
                            string otherOne = Globals.SafeSqlString(sqlDataReader["DC_OtherOne"]);
                            if (otherOne != "") instructionList.Add(otherOne);
                            string otherTwo = Globals.SafeSqlString(sqlDataReader["DC_OtherTwo"]);
                            if (otherTwo != "") instructionList.Add(otherTwo);
                        }
                    }

                    if (conApp.appType == 2)
                    {
                        //Carpet Cleaning
                        string CC_SquareFootage = Globals.SafeSqlString(sqlDataReader["CC_SquareFootage"]);
                        if (!string.IsNullOrEmpty(CC_SquareFootage)) instructionList.Add("Carpet SQ.FT(" + CC_SquareFootage + ")");
                        string CC_RoomCountSmall = Globals.SafeSqlString(sqlDataReader["CC_RoomCountSmall"]);
                        if (!string.IsNullOrEmpty(CC_RoomCountSmall)) instructionList.Add("Areas Under 200 sq/ft(" + CC_RoomCountSmall + ")");
                        string CC_RoomCountLarge = Globals.SafeSqlString(sqlDataReader["CC_RoomCountLarge"]);
                        if (!string.IsNullOrEmpty(CC_RoomCountLarge)) instructionList.Add("Areas Over 200 sq/ft(" + CC_RoomCountLarge + ")");
                        if ((bool)sqlDataReader["CC_PetOdorAdditive"]) instructionList.Add("Pet Odor Additive");
                    }

                    if (conApp.appType == 3)
                    {
                        //Window Washing
                        string WW_BuildingStyle = Globals.SafeSqlString(sqlDataReader["WW_BuildingStyle"]);
                        if (!string.IsNullOrEmpty(WW_BuildingStyle)) instructionList.Add("Building Style(" + WW_BuildingStyle + ")");
                        string WW_BuildingLevels = Globals.SafeSqlString(sqlDataReader["WW_BuildingLevels"]);
                        if (!string.IsNullOrEmpty(WW_BuildingLevels)) instructionList.Add("Levels(" + WW_BuildingLevels + ")");
                        if ((bool)sqlDataReader["WW_VaultedCeilings"]) instructionList.Add("Vaulted Ceilings");
                        if ((bool)sqlDataReader["WW_PostConstruction"]) instructionList.Add("Post Construction");
                        string WW_WindowCount = Globals.SafeSqlString(sqlDataReader["WW_WindowCount"]);
                        if (!string.IsNullOrEmpty(WW_WindowCount)) instructionList.Add("Window Count(" + WW_WindowCount + ")");
                        string WW_WindowType = Globals.SafeSqlString(sqlDataReader["WW_WindowType"]);
                        if (!string.IsNullOrEmpty(WW_WindowType)) instructionList.Add("Window Type(" + WW_WindowType + ")");
                        string WW_InsidesOutsides = Globals.SafeSqlString(sqlDataReader["WW_InsidesOutsides"]);
                        if (!string.IsNullOrEmpty(WW_InsidesOutsides)) instructionList.Add("Insides/Outsides(" + WW_InsidesOutsides + ")");
                        if ((bool)sqlDataReader["WW_Razor"]) instructionList.Add("Use Razor(" + Globals.SafeSqlString(sqlDataReader["WW_RazorCount"]) + ")");
                        if ((bool)sqlDataReader["WW_HardWater"]) instructionList.Add("Hard Water(" + Globals.SafeSqlString(sqlDataReader["WW_HardWaterCount"]) + ")");
                        if ((bool)sqlDataReader["WW_FrenchWindows"]) instructionList.Add("French Windows(" + Globals.SafeSqlString(sqlDataReader["WW_FrenchWindowCount"]) + ")");
                        if ((bool)sqlDataReader["WW_StormWindows"]) instructionList.Add("Storm Windows(" + Globals.SafeSqlString(sqlDataReader["WW_StormWindowCount"]) + ")");
                        if ((bool)sqlDataReader["WW_Screens"]) instructionList.Add("Screens(" + Globals.SafeSqlString(sqlDataReader["WW_ScreensCount"]) + ")");
                        if ((bool)sqlDataReader["WW_Tracks"]) instructionList.Add("Tracks(" + Globals.SafeSqlString(sqlDataReader["WW_TracksCount"]) + ")");
                        if ((bool)sqlDataReader["WW_Wells"]) instructionList.Add("Wells(" + Globals.SafeSqlString(sqlDataReader["WW_WellsCount"]) + ")");
                        if ((bool)sqlDataReader["WW_Gutters"]) instructionList.Add("Gutters(" + Globals.SafeSqlString(sqlDataReader["WW_GuttersFeet"]) + ")");
                    }

                    if (conApp.appType == 4)
                    {
                        //Home Guard
                        string HW_Frequency = Globals.SafeSqlString(sqlDataReader["HW_Frequency"]);
                        if (!string.IsNullOrEmpty(HW_Frequency)) instructionList.Add("Frequency(" + HW_Frequency + ")");
                        string HW_StartDate = Globals.SafeSqlString(sqlDataReader["HW_StartDate"]);
                        if (!string.IsNullOrEmpty(HW_StartDate)) instructionList.Add("Start Date(" + HW_StartDate + ")");
                        string HW_EndDate = Globals.SafeSqlString(sqlDataReader["HW_EndDate"]);
                        if (!string.IsNullOrEmpty(HW_EndDate)) instructionList.Add("End Date(" + HW_EndDate + ")");
                        if ((bool)sqlDataReader["HW_GarbageCans"]) instructionList.Add("Garbage Cans(" + Globals.SafeSqlString(sqlDataReader["HW_GarbageDay"]) + ")");
                        if ((bool)sqlDataReader["HW_PlantsWatered"]) instructionList.Add("Plants Watered(" + Globals.SafeSqlString(sqlDataReader["HW_PlantsWateredFrequency"]) + ")");
                        if ((bool)sqlDataReader["HW_Thermostat"]) instructionList.Add("Thermostat(" + Globals.SafeSqlString(sqlDataReader["HW_ThermostatTemperature"]) + ")");
                        if ((bool)sqlDataReader["HW_Breakers"]) instructionList.Add("Check Breakers(" + Globals.SafeSqlString(sqlDataReader["HW_BreakersLocation"]) + ")");
                        if ((bool)sqlDataReader["HW_CleanBeforeReturn"]) instructionList.Add("Clean Before Return");
                    }

                    conApp.general = string.Join(", ", generalList.ToArray());
                    conApp.instructions = string.Join(", ", instructionList.ToArray());

                    ret.Add(conApp);
                }

                for (int i = 0; i < ret.Count; i++)
                {
                    ConAppStruct conApp = ret[i];
                    conApp.customerCollect = collectDict[new KeyValuePair<DateTime, int>(conApp.appointmentDate, conApp.customerID)];
                    ret[i] = conApp;
                }

            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

            return ret;
        }
        #endregion

        #region GetApointmentpByID
        public static string GetApointmentpByID(int franchiseMask, int appID, out AppStruct app)
        {
            app = new AppStruct();
            if (appID == 0) return null;
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        A.appointmentID,
                        A.appStatus,
                        A.appType,
                        A.dateCreated,
                        A.dateUpdated,
                        A.appointmentDate,
                        A.startTime,
                        A.endTime,
                        A.customerID,
                        A.customerHours,
                        A.customerRate,
                        A.customerServiceFee,
                        A.customerSubContractor,
                        A.customerDiscountAmount,
                        A.customerDiscountPercent,
                        A.customerDiscountReferral,
                        A.contractorID,
                        A.contractorHours,
                        A.contractorRate,
                        A.contractorTips,
                        A.contractorAdjustAmount,
                        A.contractorAdjustType,
                        A.amountPaid,
                        A.paymentFinished,
                        A.recurrenceID,
                        A.recurrenceType,
                        A.weeklyFrequency,
                        A.monthlyWeek,
                        A.monthlyDay,
                        A.confirmed,
                        A.leftMessage,
                        A.sentSMS,
                        A.sentWeekSMS,
                        A.sentEmail,
                        A.followUpSent,
                        A.username,
                        A.salesTax,
A.notes,
A.jobStartTime,
A.jobEndTime,
A.ShareLocation,
A.JobCompleted,

                        CU.franchiseMask,
                        CU.firstName AS cuFirstName,
                        CU.lastName AS cuLastName,
                        CU.businessName,
                        CU.customNote,
                        CU.serviceFee AS combinedFee,
                        CO.contractorType,
                        CO.firstName AS coFirstName,
                        CO.lastName AS coLastName,
                        CO.businessName AS coBusinessName
                    FROM
                        Appointments A,
                        Customers CU,
                        Contractors CO
                    WHERE
                        A.appointmentID = @appointmentID AND
                        A.customerID = CU.customerID AND
                        A.contractorID = CO.contractorID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("appointmentID", appID));
                sqlDataReader = cmd.ExecuteReader();

                if (!sqlDataReader.Read())
                    return "SQL GetApointmentpByID: Record (AppointmentID=" + appID + ") does not exist.";

                if (((int)sqlDataReader["franchiseMask"] & franchiseMask) == 0)
                    return "Cannot access AppointmentID (" + appID + ") it does not belong to your assigned Franchise";

                app.appointmentID = (int)sqlDataReader["appointmentID"];
                app.appStatus = (int)sqlDataReader["appStatus"];
                app.appType = (int)sqlDataReader["appType"];
                app.franchiseMask = (int)sqlDataReader["franchiseMask"];
                app.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                app.dateUpdated = (DateTime)sqlDataReader["dateUpdated"];
                app.appointmentDate = ((DateTime)sqlDataReader["appointmentDate"]).Date;
                app.startTime = Globals.TimeOnly((DateTime)sqlDataReader["startTime"]);
                app.endTime = Globals.TimeOnly((DateTime)sqlDataReader["endTime"]);
                app.customerTitle = Globals.FormatCustomerTitle(sqlDataReader["cuFirstName"], sqlDataReader["cuLastName"], sqlDataReader["businessName"]);
                app.customerTitleCustomNote = Globals.FormatCustomerTitle(sqlDataReader["cuFirstName"], sqlDataReader["cuLastName"], sqlDataReader["customNote"]);
                app.customerID = (int)sqlDataReader["customerID"];
                app.customerHours = (decimal)sqlDataReader["customerHours"];
                app.customerRate = (decimal)sqlDataReader["customerRate"];
                app.customerServiceFee = (decimal)sqlDataReader["customerServiceFee"];
                app.customerSubContractor = (decimal)sqlDataReader["customerSubContractor"];
                app.customerDiscountAmount = (decimal)sqlDataReader["customerDiscountAmount"];
                app.customerDiscountPercent = (decimal)sqlDataReader["customerDiscountPercent"];
                app.customerDiscountReferral = (decimal)sqlDataReader["customerDiscountReferral"];
                app.contractorTitle = Globals.FormatContractorTitle(sqlDataReader["coFirstName"], sqlDataReader["coLastName"], sqlDataReader["coBusinessName"]);
                app.contractorID = sqlDataReader["contractorID"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorID"];
                app.contractorType = sqlDataReader["contractorType"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorType"];
                app.contractorHours = (decimal)sqlDataReader["contractorHours"];
                app.contractorRate = (decimal)sqlDataReader["contractorRate"];
                app.contractorTips = (decimal)sqlDataReader["contractorTips"];
                app.contractorAdjustAmount = (decimal)sqlDataReader["contractorAdjustAmount"];
                app.contractorAdjustType = sqlDataReader["contractorAdjustType"] == DBNull.Value ? null : (string)sqlDataReader["contractorAdjustType"];
                app.amountPaid = (decimal)sqlDataReader["amountPaid"];
                app.paymentFinished = (bool)sqlDataReader["paymentFinished"];
                app.recurrenceID = (int)sqlDataReader["recurrenceID"];
                app.recurrenceType = (int)sqlDataReader["recurrenceType"];
                app.weeklyFrequency = (int)sqlDataReader["weeklyFrequency"];
                app.monthlyWeek = (int)sqlDataReader["monthlyWeek"];
                app.monthlyDay = (int)sqlDataReader["monthlyDay"];
                app.combinedFee = (decimal)sqlDataReader["combinedFee"];
                app.confirmed = (bool)sqlDataReader["confirmed"];
                app.leftMessage = (bool)sqlDataReader["leftMessage"];
                app.sentSMS = (bool)sqlDataReader["sentSMS"];
                app.sentWeekSMS = (bool)sqlDataReader["sentWeekSMS"];
                app.sentEmail = (bool)sqlDataReader["sentEmail"];
                app.followUpSent = (bool)sqlDataReader["followUpSent"];
                app.ShareLocation = (bool)sqlDataReader["ShareLocation"];
                app.JobCompleted = (bool)sqlDataReader["JobCompleted"];
                app.username = Globals.SafeSqlString(sqlDataReader["username"]);
                app.salesTax = (decimal)sqlDataReader["salesTax"];
                app.Notes = sqlDataReader["notes"] == DBNull.Value ? null : (string)sqlDataReader["notes"];
                app.jobStartTime = sqlDataReader["jobStartTime"] != DBNull.Value ? (DateTime?)sqlDataReader["jobStartTime"] : null;
                app.jobEndTime = sqlDataReader["jobEndTime"] != DBNull.Value ? (DateTime?)sqlDataReader["jobEndTime"] : null;

                return null;
            }
            catch (Exception ex)
            {
                return "SQL GetApointmentpByID EX: " + ex.Message;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetSameApointments
        public static List<AppStruct> GetSameApointments(AppStruct app)
        {
            List<AppStruct> ret = new List<AppStruct>();
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        A.appointmentID,
                        A.appStatus,
                        A.appType,
                        A.dateCreated,
                        A.dateUpdated,
                        A.appointmentDate,
                        A.startTime,
                        A.endTime,
                        A.customerID,
                        A.customerHours,
                        A.customerRate,
                        A.customerServiceFee,
                        A.customerSubContractor,
                        A.customerDiscountAmount,
                        A.customerDiscountPercent,
                        A.customerDiscountReferral,
                        A.contractorID,
                        A.contractorHours,
                        A.contractorRate,
                        A.contractorTips,
                        A.contractorAdjustAmount,
                        A.contractorAdjustType,
                        A.amountPaid,
                        A.paymentFinished,
                        A.recurrenceID,
                        A.recurrenceType,
                        A.weeklyFrequency,
                        A.monthlyWeek,
                        A.monthlyDay,
                        A.confirmed,
                        A.leftMessage,
                        A.sentSMS,
                        A.sentWeekSMS,
                        A.sentEmail,
                        A.followUpSent,
                        A.salesTax,
                        CO.contractorType,
                        CO.firstName AS coFirstName,
                        CO.lastName AS coLastName,
                        CO.businessName AS coBusinessName,
A.notes,
A.jobStartTime,
A.jobEndTime,
A.ShareLocation
                    FROM
                        Appointments A,
                        Contractors CO
                    WHERE
                        A.appointmentID != @appointmentID AND
                        A.appointmentDate = @appointmentDate AND
                        A.appStatus = @appStatus AND
                        A.customerID = @customerID AND
                        A.customerRate = @customerRate AND
                        A.customerDiscountPercent = @customerDiscountPercent AND
                        A.customerDiscountReferral = @customerDiscountReferral AND
                        A.salesTax = @salesTax AND
                        A.contractorID = CO.contractorID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("appointmentID", app.appointmentID));
                cmd.Parameters.Add(new SqlParameter("appointmentDate", app.appointmentDate));
                cmd.Parameters.Add(new SqlParameter("appStatus", app.appStatus));
                cmd.Parameters.Add(new SqlParameter("customerID", app.customerID));
                cmd.Parameters.Add(new SqlParameter("customerRate", app.customerRate));
                cmd.Parameters.Add(new SqlParameter("customerDiscountPercent", app.customerDiscountPercent));
                cmd.Parameters.Add(new SqlParameter("customerDiscountReferral", app.customerDiscountReferral));
                cmd.Parameters.Add(new SqlParameter("salesTax", app.salesTax));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    AppStruct value = new AppStruct();
                    value.appointmentID = (int)sqlDataReader["appointmentID"];
                    value.appStatus = (int)sqlDataReader["appStatus"];
                    value.appType = (int)sqlDataReader["appType"];
                    value.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                    value.dateUpdated = (DateTime)sqlDataReader["dateUpdated"];
                    value.appointmentDate = ((DateTime)sqlDataReader["appointmentDate"]).Date;
                    value.startTime = Globals.TimeOnly((DateTime)sqlDataReader["startTime"]);
                    value.endTime = Globals.TimeOnly((DateTime)sqlDataReader["endTime"]);
                    value.customerID = (int)sqlDataReader["customerID"];
                    value.customerHours = (decimal)sqlDataReader["customerHours"];
                    value.customerRate = (decimal)sqlDataReader["customerRate"];
                    value.customerServiceFee = (decimal)sqlDataReader["customerServiceFee"];
                    value.customerSubContractor = (decimal)sqlDataReader["customerSubContractor"];
                    value.customerDiscountAmount = (decimal)sqlDataReader["customerDiscountAmount"];
                    value.customerDiscountPercent = (decimal)sqlDataReader["customerDiscountPercent"];
                    value.customerDiscountReferral = (decimal)sqlDataReader["customerDiscountReferral"];
                    value.contractorID = sqlDataReader["contractorID"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorID"];
                    value.contractorType = sqlDataReader["contractorType"] == DBNull.Value ? 0 : (int)sqlDataReader["contractorType"];
                    value.contractorTitle = Globals.FormatContractorTitle(sqlDataReader["coFirstName"], sqlDataReader["coLastName"], sqlDataReader["coBusinessName"]);
                    value.contractorHours = (decimal)sqlDataReader["contractorHours"];
                    value.contractorRate = (decimal)sqlDataReader["contractorRate"];
                    value.contractorTips = (decimal)sqlDataReader["contractorTips"];
                    value.contractorAdjustAmount = (decimal)sqlDataReader["contractorAdjustAmount"];
                    value.contractorAdjustType = sqlDataReader["contractorAdjustType"] == DBNull.Value ? null : (string)sqlDataReader["contractorAdjustType"];
                    value.amountPaid = (decimal)sqlDataReader["amountPaid"];
                    value.paymentFinished = (bool)sqlDataReader["paymentFinished"];
                    value.recurrenceID = (int)sqlDataReader["recurrenceID"];
                    value.recurrenceType = (int)sqlDataReader["recurrenceType"];
                    value.weeklyFrequency = (int)sqlDataReader["weeklyFrequency"];
                    value.monthlyWeek = (int)sqlDataReader["monthlyWeek"];
                    value.monthlyDay = (int)sqlDataReader["monthlyDay"];
                    value.confirmed = (bool)sqlDataReader["confirmed"];
                    value.leftMessage = (bool)sqlDataReader["leftMessage"];
                    value.sentSMS = (bool)sqlDataReader["sentSMS"];
                    value.sentWeekSMS = (bool)sqlDataReader["sentWeekSMS"];
                    value.sentEmail = (bool)sqlDataReader["sentEmail"];
                    value.followUpSent = (bool)sqlDataReader["followUpSent"];
                    value.ShareLocation = (bool)sqlDataReader["ShareLocation"];
                    value.salesTax = (decimal)sqlDataReader["salesTax"];
                    value.Notes = (string)sqlDataReader["notes"];
                    value.jobStartTime = (DateTime)sqlDataReader["jobStartTime"];
                    value.jobEndTime = (DateTime)sqlDataReader["jobEndTime"];
                    ret.Add(value);
                }
            }
            catch
            {
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region SetAppointment
        public static string SetAppointment(ref AppStruct app, bool deleteFuture)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                if (app.appointmentDate < new DateTime(1980, 1, 1) || app.appointmentDate > new DateTime(2100, 1, 1))
                    return "Invalid Appointment Date";

                if (deleteFuture && app.appointmentDate.Date < Globals.UtcToMst(DateTime.UtcNow).Date)
                    return "(Re-Make Future Appointments) feature cannot be used on past appoinments";

                if (app.endTime < app.startTime)
                    return "End time must be greater than start time";

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    DECLARE @RetTable table(appointmentID int, customerID int, recurrenceID int);

                    DECLARE @contractorRate money;
                    SET @contractorRate = (CASE WHEN @contractorID IS NULL THEN 0 ELSE (SELECT hourlyRate FROM Contractors WHERE contractorID = @contractorID) END);

                    DECLARE @lastHoursBooked money;
                    SELECT @lastHoursBooked = SUM(customerHours) FROM Appointments WHERE customerID = @customerID AND appointmentDate < CONVERT(date, getdate()) AND appointmentDate > DATEADD(day, -90, CONVERT(date, getdate()));

                    DECLARE @usernameBooked bit;
                    SET @usernameBooked = (CASE WHEN @appointmentDate < DATEADD(day, 30, CONVERT(date, getdate())) AND (@lastHoursBooked IS NULL OR @lastHoursBooked < 1) THEN 1 ELSE 0 END);
                    
                    IF @appointmentID > 0
                    BEGIN
                        DECLARE @oldAppDate datetime;
                        SET @oldAppDate = (SELECT appointmentDate FROM Appointments WHERE appointmentID = @appointmentID);
                        SET @customerID = (SELECT customerID FROM Appointments WHERE appointmentID = @appointmentID);

                        UPDATE
                            Appointments
                        SET
                            dateUpdated = getdate(),
                            appointmentID = appointmentID,
                            appointmentDate = @appointmentDate,
                            appStatus = @appStatus,
                            appType = @appType,
                            startTime = @startTime,
                            endTime = @endTime,
                            customerHours = @customerHours,
                            customerRate = @customerRate,
                            customerServiceFee = @customerServiceFee,
                            customerSubContractor = @customerSubContractor,
                            customerDiscountAmount = @customerDiscountAmount,
                            customerDiscountPercent = @customerDiscountPercent,
                            contractorID = @contractorID,
                            contractorHours = @contractorHours,
                            contractorRate = @contractorRate,
                            contractorTips = @contractorTips,
                            contractorAdjustAmount = @contractorAdjustAmount,
                            contractorAdjustType = @contractorAdjustType,
                            amountPaid = @amountPaid,
                            paymentFinished = @paymentFinished,
                            recurrenceID = (CASE WHEN recurrenceID = 0 AND @recurrenceType != 0 THEN @appointmentID ELSE recurrenceID END),
                            recurrenceType = @recurrenceType,
                            weeklyFrequency = @weeklyFrequency,
                            monthlyWeek = @monthlyWeek,
                            monthlyDay = @monthlyDay,
                            usernameBooked = (CASE WHEN @appointmentDate < DATEADD(day, 30, CONVERT(date, getdate())) THEN usernameBooked ELSE 0 END),
                            salesTax = @salesTax
                        OUTPUT INSERTED.appointmentID, INSERTED.customerID, INSERTED.recurrenceID INTO @RetTable
	                    WHERE
		                    appointmentID = @appointmentID;

                        UPDATE 
                            [Transaction]
                        SET
                            dateApply = @appointmentDate
                        WHERE
                            customerID = @customerID AND
                            dateApply = @oldAppDate;
                    END
                    ELSE
                    BEGIN
                        DECLARE @newAppointmentID int;
                        SET @newAppointmentID = dbo.InlineMax((SELECT MAX(appointmentID) + 1 FROM Appointments), 100000000);

                        DECLARE @recID int;
                        SET @recID = (CASE WHEN @recurrenceType != 0 THEN @newAppointmentID ELSE 0 END);

                        INSERT INTO Appointments
                            (appointmentID,
                            appStatus,
                            appType,
                            appointmentDate,
                            startTime,
                            endTime,
                            customerID,
                            customerHours,
                            customerRate,
                            customerServiceFee,
                            customerSubContractor,
                            customerDiscountAmount,
                            customerDiscountPercent,
                            contractorID,
                            contractorHours,
                            contractorRate,
                            contractorTips,
                            contractorAdjustAmount,
                            contractorAdjustType,
                            amountPaid,
                            paymentFinished,
                            recurrenceID,
                            recurrenceType,
                            weeklyFrequency,
                            monthlyWeek,
                            monthlyDay,
                            username,
                            usernameBooked,
                            salesTax)
                        OUTPUT INSERTED.appointmentID, INSERTED.customerID, INSERTED.recurrenceID INTO @RetTable
                        VALUES
                            (@newAppointmentID,
                            @appStatus,
                            @appType,
                            @appointmentDate,
                            @startTime,
                            @endTime,
                            @customerID,
                            @customerHours,
                            @customerRate,
                            @customerServiceFee,
                            @customerSubContractor,
                            @customerDiscountAmount,
                            @customerDiscountPercent,
                            @contractorID,
                            @contractorHours,
                            @contractorRate,
                            @contractorTips,
                            @contractorAdjustAmount,
                            @contractorAdjustType,
                            @amountPaid,
                            @paymentFinished,
                            @recID,
                            @recurrenceType,
                            @weeklyFrequency,
                            @monthlyWeek,
                            @monthlyDay,
                            @username,
                            @usernameBooked,
                            @salesTax);
                    END

                    DECLARE @recurrenceID int;
                    SET @recurrenceID = (SELECT TOP 1 recurrenceID FROM @RetTable);

                    IF @deleteFuture = 1
                    BEGIN
                        DELETE FROM
                            Appointments
                        WHERE
                            (appointmentID < 100000000 AND recurrenceID = 0 AND appointmentDate > @appointmentDate AND customerID = (SELECT customerID FROM @RetTable)) OR 
                            (recurrenceID > 0 AND recurrenceID = @recurrenceID AND appointmentDate > @appointmentDate);
                    END

                    UPDATE
                        Appointments
                    SET
                        recurrenceType = @recurrenceType,
                        weeklyFrequency = @weeklyFrequency,
                        monthlyWeek = @monthlyWeek,
                        monthlyDay = @monthlyDay
                    WHERE
                        recurrenceID > 0 AND
                        recurrenceID = @recurrenceID;

                    EXEC dbo.CreateRecurringApps @recurrenceID;
                    SELECT TOP 1 appointmentID FROM @RetTable";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"appointmentID", app.appointmentID));
                cmd.Parameters.Add(new SqlParameter(@"appStatus", app.appStatus));
                cmd.Parameters.Add(new SqlParameter(@"appType", app.appType));
                cmd.Parameters.Add(new SqlParameter(@"appointmentDate", app.appointmentDate.Date));
                cmd.Parameters.Add(new SqlParameter(@"startTime", Globals.TimeOnly(app.startTime)));
                cmd.Parameters.Add(new SqlParameter(@"endTime", Globals.TimeOnly(app.endTime)));
                cmd.Parameters.Add(new SqlParameter(@"customerID", app.customerID));
                cmd.Parameters.Add(new SqlParameter(@"customerHours", app.customerHours));
                cmd.Parameters.Add(new SqlParameter(@"customerRate", app.customerRate));
                cmd.Parameters.Add(new SqlParameter(@"customerServiceFee", app.customerServiceFee));
                cmd.Parameters.Add(new SqlParameter(@"customerSubContractor", app.customerSubContractor));
                cmd.Parameters.Add(new SqlParameter(@"customerDiscountAmount", app.customerDiscountAmount));
                cmd.Parameters.Add(new SqlParameter(@"customerDiscountPercent", app.customerDiscountPercent));
                cmd.Parameters.Add(new SqlParameter(@"contractorID", app.contractorID > 0 ? (object)app.contractorID : DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"contractorHours", app.contractorHours));
                cmd.Parameters.Add(new SqlParameter(@"contractorTips", app.contractorTips));
                cmd.Parameters.Add(new SqlParameter(@"contractorAdjustAmount", app.contractorAdjustAmount));
                cmd.Parameters.Add(new SqlParameter(@"contractorAdjustType", app.contractorAdjustType));
                cmd.Parameters.Add(new SqlParameter(@"amountPaid", app.amountPaid));
                cmd.Parameters.Add(new SqlParameter(@"paymentFinished", app.paymentFinished));
                cmd.Parameters.Add(new SqlParameter(@"recurrenceType", app.recurrenceType));
                cmd.Parameters.Add(new SqlParameter(@"weeklyFrequency", app.weeklyFrequency));
                cmd.Parameters.Add(new SqlParameter(@"monthlyWeek", app.monthlyWeek));
                cmd.Parameters.Add(new SqlParameter(@"monthlyDay", app.monthlyDay));
                cmd.Parameters.Add(new SqlParameter(@"salesTax", app.salesTax));
                cmd.Parameters.Add(new SqlParameter(@"username", (object)app.username ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"deleteFuture", deleteFuture));
                sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                    app.appointmentID = (int)sqlDataReader["appointmentID"];

                return null;
            }
            catch (Exception ex)
            {
                return "SQL SetAppointment EX: " + ex.Message;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region SetAppointmentBits
        public static string SetAppointmentBits(int appointmentID, bool verified, bool leftMessage, bool sentDaySMS, bool sentWeekSMS, bool sentEmail, bool keysReturned)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE
                    Appointments
                SET
                    confirmed = (CASE WHEN confirmed = 0 THEN @verified ELSE 1 END),
                    leftMessage = (CASE WHEN leftMessage = 0 THEN @leftMessage ELSE 1 END),
                    sentSMS = (CASE WHEN sentSMS = 0 THEN @sentSMS ELSE 1 END),
                    sentWeekSMS = (CASE WHEN sentWeekSMS = 0 THEN @sentWeekSMS ELSE 1 END),
                    sentEmail = (CASE WHEN sentEmail = 0 THEN @sentEmail ELSE 1 END),
                    keysReturned = (CASE WHEN keysReturned = 0 THEN @keysReturned ELSE 1 END)
                FROM
                    Appointments JOIN (SELECT appointmentDate, customerID FROM Appointments WHERE appointmentID = @appointmentID) AS Q ON Appointments.appointmentDate = Q.appointmentDate AND Appointments.customerID = Q.customerID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"appointmentID", appointmentID));
                cmd.Parameters.Add(new SqlParameter(@"verified", verified));
                cmd.Parameters.Add(new SqlParameter(@"leftMessage", leftMessage));
                cmd.Parameters.Add(new SqlParameter(@"sentSMS", sentDaySMS));
                cmd.Parameters.Add(new SqlParameter(@"sentWeekSMS", sentWeekSMS));
                cmd.Parameters.Add(new SqlParameter(@"sentEmail", sentEmail));
                cmd.Parameters.Add(new SqlParameter(@"keysReturned", keysReturned));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL SetAppointmentBits EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region AddAppointmentTip
        public static string AddAppointmentTip(int appointmentID, int customerID, decimal tip)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE
                    Appointments
                SET
                    contractorTips = contractorTips + @tip
	            WHERE
		            appointmentID = @appointmentID AND
                    customerID = @customerID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"appointmentID", appointmentID));
                cmd.Parameters.Add(new SqlParameter(@"customerID", customerID));
                cmd.Parameters.Add(new SqlParameter(@"tip", tip));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL AddAppointmentTip EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region DeleteAppointment
        public static string DeleteAppointment(int appointmentID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                if (appointmentID > 0)
                {
                    sqlConnection = new SqlConnection(connString);
                    sqlConnection.Open();

                    string cmdText = @"
                    DECLARE @customerID int;
                    DECLARE @appointmentDate datetime;

                    SELECT 
                        @customerID = customerID, 
                        @appointmentDate = appointmentDate
                    FROM
                        Appointments
                    WHERE
                        appointmentID = @appointmentID;

                    DELETE FROM 
                        Appointments
	                WHERE
		                appointmentID = @appointmentID;";

                    SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                    cmd.Parameters.Add(new SqlParameter(@"appointmentID", appointmentID));
                    cmd.ExecuteNonQuery();
                }
                return null;
            }
            catch (Exception ex)
            {
                return "SQL DeleteAppointment EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region DeleteAppointmentRange
        public static string DeleteAppointmentRange(int franchiseMask, int customerID, DateTime startDate, DateTime endDate)
        {
            SqlConnection sqlConnection = null;
            try
            {
                if (endDate == DateTime.MinValue) endDate = DateTime.MaxValue;
                if (startDate.Date < Globals.UtcToMst(DateTime.UtcNow).Date) return "Invalid Begin Date";
                if (endDate < startDate) return "Invalid End Date";

                Globals.FormatDateRange(ref startDate, ref endDate);

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    IF (SELECT franchiseMask FROM Customers WHERE customerID = @customerID) & @franchiseMask > 0
                    BEGIN
                        IF EXISTS (SELECT transID FROM [Transaction] T WHERE customerID = @customerID AND dateApply >= @startDate AND dateApply <= @endDate)
                        BEGIN
                            THROW 51000, 'Existing Transactions for Appointment Range.', 1;
                        END

                        IF @endDate > '1/1/2500'
                        BEGIN
                            UPDATE
                                Appointments
                            SET
                                recurrenceID = 0,
                                recurrenceType = 0
                            WHERE
                                customerID = @customerID AND
                                recurrenceID > 0 AND
                                recurrenceID IN (SELECT recurrenceID FROM Appointments WHERE customerID = @customerID AND appointmentDate >= @startDate AND appointmentDate <= @endDate);
                        END

                        DELETE FROM 
                            Appointments
	                    WHERE
                            customerID = @customerID AND
                            appointmentDate >= @startDate AND
                            appointmentDate <= @endDate
                    END";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter(@"customerID", customerID));
                cmd.Parameters.Add(new SqlParameter(@"startDate", startDate));
                cmd.Parameters.Add(new SqlParameter(@"endDate", endDate));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL DeleteAppointmentRange EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GenerateRecurringApps
        public static void GenerateRecurringApps()
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        DISTINCT(recurrenceID)
                    FROM
                        Appointments
                    WHERE
                        recurrenceID > 0";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                sqlDataReader = cmd.ExecuteReader();

                List<int> appList = new List<int>();
                while (sqlDataReader.Read())
                    appList.Add((int)sqlDataReader["recurrenceID"]);
                sqlDataReader.Close();

                foreach (int i in appList)
                {
                    cmd = new SqlCommand("CreateRecurringApps", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("recurrenceID", i));
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetUnavailableByDateRange
        public static List<UnavailableStruct> GetUnavailableByDateRange(int franchiseMask, int contractorType, int contractorID, DateTime startDate, DateTime endDate, string orderBy)
        {
            List<UnavailableStruct> ret = new List<UnavailableStruct>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                Globals.FormatDateRange(ref startDate, ref endDate);

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        U.unavailableID,
                        U.contractorID,
                        U.dateCreated,
                        U.dateRequested,
                        U.startTime,
                        U.endTime,
                        U.recurrenceID,
                        U.recurrenceType,
                        C.firstName,
                        C.lastName,
                        C.businessName,
                        C.active,
                        C.scheduled
                    FROM
                        Unavailable U,
                        Contractors C
                    WHERE
                        (@contractorID = -1 OR U.contractorID = @contractorID) AND
                        C.franchiseMask & @franchiseMask > 0 AND
                        contractorType & @contractorType > 0 AND
                        C.contractorID = U.contractorID AND
                        U.dateRequested >= @startDate AND
                        U.dateRequested <= @endDate
                    ORDER BY
                        " + orderBy;

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("contractorType", contractorType));
                cmd.Parameters.Add(new SqlParameter("contractorID", contractorID));
                cmd.Parameters.Add(new SqlParameter("startDate", startDate));
                cmd.Parameters.Add(new SqlParameter("endDate", endDate));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    UnavailableStruct unavailable = new UnavailableStruct();
                    unavailable.unavailableID = (int)sqlDataReader["unavailableID"];
                    unavailable.contractorID = (int)sqlDataReader["contractorID"];
                    unavailable.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                    unavailable.dateRequested = (DateTime)sqlDataReader["dateRequested"];
                    unavailable.startTime = (DateTime)sqlDataReader["startTime"];
                    unavailable.endTime = (DateTime)sqlDataReader["endTime"];
                    unavailable.recurrenceID = (int)sqlDataReader["recurrenceID"];
                    unavailable.recurrenceType = (int)sqlDataReader["recurrenceType"];
                    unavailable.contractorTitle = Globals.FormatContractorTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                    unavailable.contractorActive = (bool)sqlDataReader["active"];
                    unavailable.contractorScheduled = (bool)sqlDataReader["scheduled"];
                    ret.Add(unavailable);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

            return ret;
        }
        #endregion

        #region GetUnavailableByID
        public static string GetUnavailableByID(int franchiseMask, int unavailableID, out UnavailableStruct unavailable)
        {
            unavailable = new UnavailableStruct();
            if (unavailableID == 0) return null;
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        U.unavailableID,
                        U.contractorID,
                        U.dateCreated,
                        U.dateRequested,
                        U.startTime,
                        U.endTime,
                        U.recurrenceID,
                        U.recurrenceType,
                        C.firstName,
                        C.lastName,
                        C.businessName
                    FROM
                        Unavailable U,
                        Contractors C
                    WHERE
                        U.unavailableID = @unavailableID AND
                        C.contractorID = U.contractorID AND
                        C.franchiseMask & @franchiseMask > 0";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("unavailableID", unavailableID));
                sqlDataReader = cmd.ExecuteReader();

                if (!sqlDataReader.Read())
                    return null;

                unavailable.unavailableID = (int)sqlDataReader["unavailableID"];
                unavailable.contractorID = (int)sqlDataReader["contractorID"];
                unavailable.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                unavailable.dateRequested = (DateTime)sqlDataReader["dateRequested"];
                unavailable.startTime = (DateTime)sqlDataReader["startTime"];
                unavailable.endTime = (DateTime)sqlDataReader["endTime"];
                unavailable.recurrenceID = (int)sqlDataReader["recurrenceID"];
                unavailable.recurrenceType = (int)sqlDataReader["recurrenceType"];
                unavailable.contractorTitle = Globals.FormatContractorTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);

                return null;
            }
            catch (Exception ex)
            {
                return "SQL GetUnavailableByID EX: " + ex.Message;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region SetUnavailable
        public static string SetUnavailable(UnavailableStruct unavailable)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                if (unavailable.dateRequested > new DateTime(2100, 1, 1))
                    return "Invalid Date Requested";

                if (unavailable.endTime < unavailable.startTime)
                    return "End time must be greater than start time";

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    DECLARE @RetTable table(recurrenceID int);

                    IF @unavailableID > 0
                    BEGIN
                      UPDATE
                            Unavailable
                        SET
                            dateRequested = @dateRequested,
                            startTime = @startTime,
                            endTime = @endTime,
                            recurrenceID = (CASE WHEN recurrenceID = 0 AND @recurrenceType != 0 THEN @unavailableID ELSE recurrenceID END),
                            recurrenceType = @recurrenceType
                        OUTPUT INSERTED.recurrenceID INTO @RetTable
	                    WHERE
		                    unavailableID = @unavailableID;
                    END
                    ELSE
                    BEGIN
                        DECLARE @newUnavailableID int;
                        SET @newUnavailableID = (SELECT COALESCE(MAX(unavailableID),0) + 1 FROM Unavailable);

                        DECLARE @recID int;
                        SET @recID = (CASE WHEN @recurrenceType != 0 THEN @newUnavailableID ELSE 0 END);

                        INSERT INTO Unavailable
                            (unavailableID,
                            contractorID,
                            dateRequested,
                            startTime,
                            endTime,
                            recurrenceID,
                            recurrenceType)
                        OUTPUT INSERTED.recurrenceID INTO @RetTable
                        VALUES
                            (@newUnavailableID,
                            @contractorID,
                            @dateRequested,
                            @startTime,
                            @endTime,
                            @recID,
                            @recurrenceType)
                    END

                    DECLARE @recurrenceID int;
                    SET @recurrenceID = (SELECT TOP 1 recurrenceID FROM @RetTable);

                    DELETE FROM
                        Unavailable
                    WHERE
                        recurrenceID > 0 AND 
                        recurrenceID = @recurrenceID AND 
                        dateRequested > @dateRequested;

                    UPDATE
                        Unavailable
                    SET
                        recurrenceType = @recurrenceType
                    WHERE
                        recurrenceID > 0 AND
                        recurrenceID = @recurrenceID;

                    EXEC dbo.CreateRecurringUnavailable @recurrenceID;";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"unavailableID", unavailable.unavailableID));
                cmd.Parameters.Add(new SqlParameter(@"contractorID", unavailable.contractorID));
                cmd.Parameters.Add(new SqlParameter(@"dateRequested", unavailable.dateRequested.Date));
                cmd.Parameters.Add(new SqlParameter(@"startTime", Globals.TimeOnly(unavailable.startTime)));
                cmd.Parameters.Add(new SqlParameter(@"endTime", Globals.TimeOnly(unavailable.endTime)));
                cmd.Parameters.Add(new SqlParameter(@"recurrenceType", unavailable.recurrenceType));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL SetUnavailable EX: " + ex.Message;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region DeleteUnavailable
        public static string DeleteUnavailable(int unavailableID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                if (unavailableID == 0) return null;
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                /*string cmdText = @"
                    DECLARE @recurrenceID int;
                    SET @recurrenceID = (SELECT TOP 1 recurrenceID FROM Unavailable WHERE unavailableID = @unavailableID);

                    DECLARE @dateRequested datetime;
                    SET @dateRequested = (SELECT TOP 1 dateRequested FROM Unavailable WHERE unavailableID = @unavailableID);

                    DELETE FROM
                        Unavailable
	                WHERE
                        dateRequested >= @dateRequested AND
                        recurrenceID > 0 AND
		                recurrenceID = @recurrenceID;
                       
                    UPDATE 
                        Unavailable
                    SET
                        recurrenceType = 0
                    WHERE
                        recurrenceID > 0 AND
                        recurrenceID = @recurrenceID;

                    DELETE FROM
                        Unavailable
	                WHERE
		                unavailableID = @unavailableID;";*/

                string cmdText = @"
                    DELETE FROM
                        Unavailable
	                WHERE
		                unavailableID = @unavailableID;";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"unavailableID", unavailableID));
                cmd.ExecuteNonQuery();
                return null;
            }
            catch (Exception ex)
            {
                return "SQL DeleteUnavailable EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GenerateRecurringUnavialble
        public static void GenerateRecurringUnavailable()
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        DISTINCT(recurrenceID)
                    FROM
                        Unavailable
                    WHERE
                        recurrenceID > 0";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                sqlDataReader = cmd.ExecuteReader();

                List<int> appList = new List<int>();
                while (sqlDataReader.Read())
                    appList.Add((int)sqlDataReader["recurrenceID"]);
                sqlDataReader.Close();

                foreach (int i in appList)
                {
                    cmd = new SqlCommand("CreateRecurringUnavailable", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("recurrenceID", i));
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region SetQuoteTemplate
        public static string SetQuoteTemplate(QuoteTemplate template)
        {
            SqlConnection sqlConnection = null;
            try
            {
                if (template.franchiseMask == 0)
                    return "Invalid Franchise Mask";

                if (string.IsNullOrEmpty(template.templateName))
                    return "Template Name Cannot be Blank";

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    IF EXISTS (SELECT * FROM QuoteTemplate WHERE (franchiseMask & @franchiseMask > 0) AND templateName = @templateName)
                    BEGIN
	                    UPDATE 
	                        QuoteTemplate
	                    SET
                            templateValue = @templateValue
	                    WHERE
                            franchiseMask & @franchiseMask > 0 AND
                            templateName = @templateName;
                    END
                    ELSE
                    BEGIN 
	                    INSERT INTO QuoteTemplate 
		                    (franchiseMask,
                            templateName,
                            templateValue)
	                    VALUES
		                    (@franchiseMask,
                            @templateName,
                            @templateValue);
                    END";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"franchiseMask", template.franchiseMask));
                cmd.Parameters.Add(new SqlParameter(@"templateName", template.templateName));
                cmd.Parameters.Add(new SqlParameter(@"templateValue", template.templateValue));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL SetQuoteTemplate EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region DeleteQuoteTemplate
        public static string DeleteQuoteTemplate(int franchiseMask, string templateName)
        {
            SqlConnection sqlConnection = null;
            try
            {
                if (franchiseMask == 0)
                    return "Invalid Franchise Mask";

                if (string.IsNullOrEmpty(templateName))
                    return "Template Name Cannot be Blank";

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    DELETE FROM
                        QuoteTemplate
                    WHERE
                        franchiseMask & @franchiseMask > 0 AND
                        templateName = @templateName";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter(@"templateName", templateName));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL DeleteQuoteTemplate EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetQuoteTemplates
        public static List<QuoteTemplate> GetQuoteTemplates(int franchiseMask)
        {
            List<QuoteTemplate> ret = new List<QuoteTemplate>();
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        franchiseMask,
                        templateName,
                        templateValue
                    FROM
                        QuoteTemplate
                    WHERE
                        franchiseMask & @franchiseMask > 0
                    ORDER BY 
                        templateName";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    QuoteTemplate template = new QuoteTemplate();
                    template.franchiseMask = (int)sqlDataReader["franchiseMask"];
                    template.templateName = Globals.SafeSqlString(sqlDataReader["templateName"]);
                    template.templateValue = Globals.SafeSqlString(sqlDataReader["templateValue"]);
                    ret.Add(template);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region SetCustomerQuoteReply
        public static string SetCustomerQuoteReply(int customerID, string quoteValue)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
	            UPDATE 
	                Customers
	            SET
                    quoteReply = 1,
                    quoteValue = @quoteValue
	            WHERE
                    customerID = @customerID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"customerID", customerID));
                cmd.Parameters.Add(new SqlParameter(@"quoteValue", quoteValue));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL SetCustomerQuoteReply EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetDailyTotalsByDateRange
        public static SortedList<DateTime, decimal> GetDailyTotalsByDateRange(int franchiseMask, string dataType, DateTime startDate, DateTime endDate, int offset, string columnName = "total")
        {
            SortedList<DateTime, decimal> ret = new SortedList<DateTime, decimal>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                Globals.FormatDateRange(ref startDate, ref endDate);

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        dataTime,
	                    SUM(dataValue) AS total,
                        AVG(dataValue) AS average
                    FROM
                        DailyTotals
                    WHERE
                        franchiseMask & @franchiseMask > 0 AND
                        dataType = @dataType AND
                        dataTime >= @startDate AND
                        dataTime <= @endDate
                    GROUP BY
	                    dataTime
                    ORDER BY
                        dataTime DESC";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("dataType", dataType));
                cmd.Parameters.Add(new SqlParameter("startDate", startDate));
                cmd.Parameters.Add(new SqlParameter("endDate", endDate));
                sqlDataReader = cmd.ExecuteReader();

                for (int i = 1; sqlDataReader.Read(); i++)
                {
                    if ((i % offset) == 0 || i == 1)
                    {
                        DateTime dataTime = (DateTime)sqlDataReader["dataTime"];
                        decimal dataValue = (decimal)sqlDataReader[columnName];
                        if (!ret.ContainsKey(dataTime)) ret.Add(dataTime, dataValue);
                    }
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region GetChartDataAppHours
        public static SortedList<DateTime, decimal> GetChartDataAppHours(int franchiseMask, DateTime startDate, DateTime endDate)
        {
            SortedList<DateTime, decimal> ret = new SortedList<DateTime, decimal>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                Globals.FormatDateRange(ref startDate, ref endDate);

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
	                A.appointmentDate,
	                SUM(A.customerHours) AS total
                FROM 
	                Appointments A,
	                Customers C
                WHERE
	                A.customerID = C.customerID AND
	                C.franchiseMask & @franchiseMask > 0 AND
	                A.appointmentDate >= @startDate AND
                    A.appointmentDate <= @endDate AND
	                C.accountStatus != 'Ignored'
                GROUP BY
	                A.appointmentDate
                ORDER BY
	                A.appointmentDate DESC";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("startDate", startDate));
                cmd.Parameters.Add(new SqlParameter("endDate", endDate));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    DateTime dataTime = (DateTime)sqlDataReader["appointmentDate"];
                    dataTime = Globals.StartOfWeek(dataTime);
                    decimal dataValue = (decimal)sqlDataReader["total"];
                    if (!ret.ContainsKey(dataTime)) ret.Add(dataTime, dataValue);
                    else ret[dataTime] += dataValue;
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region RunDailyTotals
        public static string RunDailyTotals(int franchiseMask)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                DECLARE @dataValue money;
                SET @dataTime = dbo.DateOnly(@dataTime);

                IF NOT EXISTS (SELECT * FROM DailyTotals WHERE franchiseMask = @franchiseMask AND dataType = 'Active Customers' AND dataTime = @dataTime)
                BEGIN
		             SET @dataValue = (SELECT COUNT(*) FROM Customers WHERE franchiseMask & @franchiseMask != 0 AND accountStatus = 'Active');

	                INSERT INTO DailyTotals
		                (franchiseMask,
		                dataType,
		                dataTime,
		                dataValue)
	                VALUES
		                (@franchiseMask,
		                'Active Customers',
		                @dataTime,
		                @dataValue);
                END
                
                IF NOT EXISTS (SELECT * FROM DailyTotals WHERE franchiseMask = @franchiseMask AND dataType = 'Appointment Hours' AND dataTime = @dataTime)
                BEGIN

		            SET @dataValue = 
		            (SELECT 
			            SUM(A.customerHours) 
		            FROM 
			            Appointments A,
			            Customers C 
		            WHERE 
			            A.customerID = C.customerID AND
			            C.franchiseMask & @franchiseMask > 0 AND 
			            dbo.DateOnly(A.appointmentDate) = @dataTime);

	                INSERT INTO DailyTotals
		                (franchiseMask,
		                dataType,
		                dataTime,
		                dataValue)
	                VALUES
		                (@franchiseMask,
		                'Appointment Hours',
		                @dataTime,
		                @dataValue);
                END

                IF NOT EXISTS (SELECT * FROM DailyTotals WHERE franchiseMask = @franchiseMask AND dataType = 'Scheduling Satisfaction' AND dataTime = @dataTime)
                BEGIN

		            SET @dataValue = 
		            (SELECT
	                    CONVERT(money, SUM(schedulingSatisfaction)) / COUNT(schedulingSatisfaction)
                    FROM
	                    FollowUp F,
	                    Appointments A,
                        Customers C 
                    WHERE
	                    F.appointmentID = A.appointmentID AND
                        A.customerID = C.customerID AND
                        C.franchiseMask & @franchiseMask > 0 AND
	                    A.appointmentDate > DATEADD(day, -90, GETUTCDATE()) AND
                        F.schedulingSatisfaction > 0);

	                INSERT INTO DailyTotals
		                (franchiseMask,
		                dataType,
		                dataTime,
		                dataValue)
	                VALUES
		                (@franchiseMask,
		                'Scheduling Satisfaction',
		                @dataTime,
		                @dataValue);
                END";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("@franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("@dataTime", Globals.UtcToMst(DateTime.UtcNow)));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL RunDailyTotals EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region InsertDailyTotal
        public static string InsertDailyTotal(int franchiseMask, string dataType, DateTime dataTime, decimal dataValue)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
	                INSERT INTO DailyTotals
		                (franchiseMask,
		                dataType,
		                dataTime,
		                dataValue)
	                VALUES
		                (@franchiseMask,
		                @dataType,
		                @dataTime,
		                @dataValue);";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("@franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("@dataType", dataType));
                cmd.Parameters.Add(new SqlParameter("@dataTime", dataTime));
                cmd.Parameters.Add(new SqlParameter("@dataValue", dataValue));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL InsertDailyTotal EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region InsertTransaction
        public static string InsertTransaction(ref TransactionStruct trans)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"INSERT INTO [Transaction] 
                    (transType,
                    paymentID,
                    paymentType,
                    lastFourCard,
                    auth,
                    customerID,
                    dateCreated,
                    dateApply,
                    total,
                    hoursBilled,
                    hourlyRate,
                    serviceFee,
                    subContractorCC,
                    subContractorWW,
                    subContractorHW,
                    subContractorCL,
                    discountAmount,
                    discountPercent,
                    discountReferral,
                    tips,
                    salesTax,
                    email,
                    notes,
                    username)
                OUTPUT INSERTED.transID
	            VALUES
                    (@transType,
                    @paymentID,
                    @paymentType,
                    @lastFourCard,
                    @auth,
                    @customerID,
                    @dateCreated,
                    @dateApply,
                    @total,
                    @hoursBilled,
                    @hourlyRate,
                    @serviceFee,
                    @subContractorCC,
                    @subContractorWW,
                    @subContractorHW,
                    @subContractorCL,
                    @discountAmount,
                    @discountPercent,
                    @discountReferral,
                    @tips,
                    @salesTax,
                    @email,
                    @notes,
                    @username)";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);

                cmd.Parameters.Add(new SqlParameter(@"transType", trans.transType));
                cmd.Parameters.Add(new SqlParameter(@"paymentID", (object)trans.paymentID ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"paymentType", trans.paymentType));
                cmd.Parameters.Add(new SqlParameter(@"lastFourCard", (object)trans.lastFourCard ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"auth", trans.auth));
                cmd.Parameters.Add(new SqlParameter(@"customerID", trans.customerID));
                cmd.Parameters.Add(new SqlParameter(@"dateCreated", trans.dateCreated));
                cmd.Parameters.Add(new SqlParameter(@"dateApply", trans.dateApply));
                cmd.Parameters.Add(new SqlParameter(@"total", trans.total));
                cmd.Parameters.Add(new SqlParameter(@"hoursBilled", trans.hoursBilled));
                cmd.Parameters.Add(new SqlParameter(@"hourlyRate", trans.hourlyRate));
                cmd.Parameters.Add(new SqlParameter(@"serviceFee", trans.serviceFee));
                cmd.Parameters.Add(new SqlParameter(@"subContractorCC", trans.subContractorCC));
                cmd.Parameters.Add(new SqlParameter(@"subContractorWW", trans.subContractorWW));
                cmd.Parameters.Add(new SqlParameter(@"subContractorHW", trans.subContractorHW));
                cmd.Parameters.Add(new SqlParameter(@"subContractorcL", trans.subContractorCL));
                cmd.Parameters.Add(new SqlParameter(@"discountAmount", trans.discountAmount));
                cmd.Parameters.Add(new SqlParameter(@"discountPercent", trans.discountPercent));
                cmd.Parameters.Add(new SqlParameter(@"discountReferral", trans.discountReferral));
                cmd.Parameters.Add(new SqlParameter(@"tips", trans.tips));
                cmd.Parameters.Add(new SqlParameter(@"salesTax", trans.salesTax));
                cmd.Parameters.Add(new SqlParameter(@"email", (object)trans.email ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"notes", (object)trans.notes ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter(@"username", (object)trans.username ?? DBNull.Value));

                sqlDataReader = cmd.ExecuteReader();
                if (sqlDataReader.Read()) trans.transID = (int)sqlDataReader["transID"];
                return null;
            }
            catch (Exception ex)
            {
                return "SQL InsertTransaction EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
                if (sqlDataReader != null)
                    sqlDataReader.Close();
            }
        }
        #endregion

        #region TransactionEmailSent
        public static string TransactionEmailSent(int transID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE 
	                [Transaction]
	            SET
                    emailSent = 1
	            WHERE
                    transID = @transID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"transID", transID));

                cmd.ExecuteNonQuery();
                return null;
            }
            catch (Exception ex)
            {
                return "SQL TransactionEmailSent EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region VoidTransaction
        public static string VoidTransaction(int transID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE 
	                [Transaction]
	            SET
                    isVoid = 1,
                    emailSent = 0
	            WHERE
                    transID = @transID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"transID", transID));

                cmd.ExecuteNonQuery();
                return null;
            }
            catch (Exception ex)
            {
                return "SQL VoidTransaction EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region SetTransactionPaymentID
        public static string SetTransactionPaymentID(int transID, string paymentID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE 
	                [Transaction]
	            SET
                    paymentID = @paymentID
	            WHERE
                    transID = @transID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"transID", transID));
                cmd.Parameters.Add(new SqlParameter(@"paymentID", paymentID));

                cmd.ExecuteNonQuery();
                return null;
            }
            catch (Exception ex)
            {
                return "SQL SetTransactionPaymentID EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetTransactions
        public static List<TransactionStruct> GetTransactions(int franchiseMask, int customerID, DateTime startDate, DateTime endDate, string orderBy)
        {
            List<TransactionStruct> ret = new List<TransactionStruct>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                Globals.FormatDateRange(ref startDate, ref endDate);

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    *
                FROM 
                    [Transaction] T,
                    Customers C
                WHERE
                    (@customerID = 0 OR T.customerID = @customerID) AND
	                T.customerID = C.customerID AND
                    C.franchiseMask & @franchiseMask > 0 AND
                    T.dateApply >= @startDate AND
                    T.dateApply <= @endDate
                ORDER BY
	                " + orderBy;

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                cmd.Parameters.Add(new SqlParameter("startDate", startDate));
                cmd.Parameters.Add(new SqlParameter("endDate", endDate));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    TransactionStruct tran = new TransactionStruct();
                    tran.transID = (int)sqlDataReader["transID"];
                    tran.transType = Globals.SafeSqlString(sqlDataReader["transType"]);
                    tran.paymentID = Globals.SafeSqlString(sqlDataReader["paymentID"]);
                    tran.paymentType = Globals.SafeSqlString(sqlDataReader["paymentType"]);
                    tran.lastFourCard = Globals.SafeSqlString(sqlDataReader["lastFourCard"]);
                    tran.auth = (int)sqlDataReader["auth"];
                    tran.batched = (bool)sqlDataReader["batched"];
                    tran.customerID = (int)sqlDataReader["customerID"];
                    tran.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                    tran.dateApply = (DateTime)sqlDataReader["dateApply"];
                    tran.isVoid = (bool)sqlDataReader["isVoid"];
                    tran.total = (decimal)sqlDataReader["total"];
                    tran.hoursBilled = (decimal)sqlDataReader["hoursBilled"];
                    tran.hourlyRate = (decimal)sqlDataReader["hourlyRate"];
                    tran.serviceFee = (decimal)sqlDataReader["serviceFee"];
                    tran.subContractorCC = (decimal)sqlDataReader["subContractorCC"];
                    tran.subContractorWW = (decimal)sqlDataReader["subContractorWW"];
                    tran.subContractorHW = (decimal)sqlDataReader["subContractorHW"];
                    tran.subContractorCL = (decimal)sqlDataReader["subContractorCL"];
                    tran.discountAmount = (decimal)sqlDataReader["discountAmount"];
                    tran.discountPercent = (decimal)sqlDataReader["discountPercent"];
                    tran.discountReferral = (decimal)sqlDataReader["discountReferral"];
                    tran.tips = (decimal)sqlDataReader["tips"];
                    tran.salesTax = (decimal)sqlDataReader["salesTax"];
                    tran.email = Globals.SafeSqlString(sqlDataReader["email"]);
                    tran.emailSent = (bool)sqlDataReader["emailSent"];
                    tran.notes = Globals.SafeSqlString(sqlDataReader["notes"]);
                    tran.username = Globals.SafeSqlString(sqlDataReader["username"]);
                    tran.customerTitle = Globals.FormatCustomerTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                    tran.customerFirstName = Globals.SafeSqlString(sqlDataReader["firstName"]);
                    tran.customerLastName = Globals.SafeSqlString(sqlDataReader["locationAddress"]);
                    tran.customerBusinessName = Globals.SafeSqlString(sqlDataReader["businessName"]);
                    tran.customerAddress = Globals.SafeSqlString(sqlDataReader["locationAddress"]);
                    tran.customerCity = Globals.SafeSqlString(sqlDataReader["locationCity"]);
                    tran.customerState = Globals.SafeSqlString(sqlDataReader["locationState"]);
                    tran.customerZip = Globals.SafeSqlString(sqlDataReader["locationZip"]);
                    tran.customerAccountStatus = Globals.SafeSqlString(sqlDataReader["accountStatus"]);
                    ret.Add(tran);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region CloseAuthTransaction
        public static bool CloseAuthTransaction(int transID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"UPDATE [Transaction] SET auth = 3 WHERE transID = @transID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("transID", transID));
                cmd.ExecuteNonQuery();
                return true;
            }
            catch { }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return false;
        }
        #endregion

        #region GetTransactionByID
        public static TransactionStruct GetTransactionByID(int franchiseMask, int transID)
        {
            TransactionStruct ret = new TransactionStruct();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    *
                FROM 
                    [Transaction] T,
                    Customers C
                WHERE
                    T.transID = @transID AND
	                T.customerID = C.customerID AND
	                C.franchiseMask & @franchiseMask > 0
                ORDER BY
	                T.dateApply";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("transID", transID));
                sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    ret.transID = (int)sqlDataReader["transID"];
                    ret.transType = Globals.SafeSqlString(sqlDataReader["transType"]);
                    ret.paymentID = Globals.SafeSqlString(sqlDataReader["paymentID"]);
                    ret.paymentType = Globals.SafeSqlString(sqlDataReader["paymentType"]);
                    ret.lastFourCard = Globals.SafeSqlString(sqlDataReader["lastFourCard"]);
                    ret.auth = (int)sqlDataReader["auth"];
                    ret.batched = (bool)sqlDataReader["batched"];
                    ret.customerID = (int)sqlDataReader["customerID"];
                    ret.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                    ret.dateApply = (DateTime)sqlDataReader["dateApply"];
                    ret.isVoid = (bool)sqlDataReader["isVoid"];
                    ret.total = (decimal)sqlDataReader["total"];
                    ret.hoursBilled = (decimal)sqlDataReader["hoursBilled"];
                    ret.hourlyRate = (decimal)sqlDataReader["hourlyRate"];
                    ret.serviceFee = (decimal)sqlDataReader["serviceFee"];
                    ret.subContractorCC = (decimal)sqlDataReader["subContractorCC"];
                    ret.subContractorWW = (decimal)sqlDataReader["subContractorWW"];
                    ret.subContractorHW = (decimal)sqlDataReader["subContractorHW"];
                    ret.subContractorCL = (decimal)sqlDataReader["subContractorCL"];
                    ret.discountAmount = (decimal)sqlDataReader["discountAmount"];
                    ret.discountPercent = (decimal)sqlDataReader["discountPercent"];
                    ret.discountReferral = (decimal)sqlDataReader["discountReferral"];
                    ret.tips = (decimal)sqlDataReader["tips"];
                    ret.salesTax = (decimal)sqlDataReader["salesTax"];
                    ret.email = Globals.SafeSqlString(sqlDataReader["email"]);
                    ret.emailSent = (bool)sqlDataReader["emailSent"];
                    ret.notes = Globals.SafeSqlString(sqlDataReader["notes"]);
                    ret.username = Globals.SafeSqlString(sqlDataReader["username"]);
                    ret.customerTitle = Globals.FormatCustomerTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                    ret.customerFirstName = Globals.SafeSqlString(sqlDataReader["firstName"]);
                    ret.customerLastName = Globals.SafeSqlString(sqlDataReader["lastName"]);
                    ret.customerBusinessName = Globals.SafeSqlString(sqlDataReader["businessName"]);
                    ret.customerAddress = Globals.SafeSqlString(sqlDataReader["locationAddress"]);
                    ret.customerCity = Globals.SafeSqlString(sqlDataReader["locationCity"]);
                    ret.customerState = Globals.SafeSqlString(sqlDataReader["locationState"]);
                    ret.customerZip = Globals.SafeSqlString(sqlDataReader["locationZip"]);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region InsertServiceRequest
        public static string InsertServiceRequest(ServiceRequestStruct serviceRequest)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
	                INSERT INTO ServiceRequest
		                (customerID,
		                requestDate,
		                timePrefix,
		                timeSuffix,
                        notes)
	                VALUES
		                (@customerID,
		                @requestDate,
		                @timePrefix,
		                @timeSuffix,
                        @notes);";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("@customerID", serviceRequest.customerID));
                cmd.Parameters.Add(new SqlParameter("@requestDate", serviceRequest.requestDate));
                cmd.Parameters.Add(new SqlParameter("@timePrefix", serviceRequest.timePrefix));
                cmd.Parameters.Add(new SqlParameter("@timeSuffix", serviceRequest.timeSuffix));
                cmd.Parameters.Add(new SqlParameter("@notes", serviceRequest.notes));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL InsertServiceRequest EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region DeleteServiceRequest
        public static string DeleteServiceRequest(int serviceRequestID, int customerID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    DELETE FROM
                        ServiceRequest
                    WHERE
                        serviceRequestID = @serviceRequestID AND
                        customerID = @customerID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"serviceRequestID", serviceRequestID));
                cmd.Parameters.Add(new SqlParameter(@"customerID", customerID));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL DeleteServiceRequest EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetServiceRequests
        public static List<ServiceRequestStruct> GetServiceRequests(int customerID)
        {
            List<ServiceRequestStruct> ret = new List<ServiceRequestStruct>();
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        ServiceRequest
                    WHERE
                        customerID = @customerID
                    ORDER BY
                        requestDate DESC, dateCreated DESC";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    ServiceRequestStruct value = new ServiceRequestStruct();
                    value.serviceRequestID = (int)sqlDataReader["serviceRequestID"];
                    value.customerID = (int)sqlDataReader["customerID"];
                    value.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                    value.requestDate = ((DateTime)sqlDataReader["requestDate"]).Date;
                    value.timePrefix = (string)sqlDataReader["timePrefix"];
                    value.timeSuffix = (string)sqlDataReader["timeSuffix"];
                    value.notes = (string)sqlDataReader["notes"];
                    ret.Add(value);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region GetServiceRequestsByDateRange
        public static List<DBRow> GetServiceRequestsByDateRange(int franchiseMask, DateTime startDate, DateTime endDate)
        {
            Globals.FormatDateRange(ref startDate, ref endDate);

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    *
                FROM 
                    ServiceRequest R,
	                Customers C
                WHERE
	                R.customerID = C.customerID AND
	                C.franchiseMask & @franchiseMask > 0 AND
                    R.dateCreated >= @startDate AND
                    R.dateCreated <= @endDate
                ORDER BY
                    C.firstName, C.lastName, C.businessName, R.requestDate, R.timePrefix, R.timeSuffix";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("startDate", startDate));
                cmd.Parameters.Add(new SqlParameter("endDate", endDate));
                sqlDataReader = cmd.ExecuteReader();

                return GetDBRows(sqlDataReader);
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return new List<DBRow>();
        }
        #endregion

        #region SetFollowUp
        public static string SetFollowUp(FollowUpStruct followUp)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    IF EXISTS (SELECT * FROM FollowUp WHERE appointmentID = @appointmentID)
                    BEGIN
	                    UPDATE 
	                        FollowUp
	                    SET
                            schedulingSatisfaction = @schedulingSatisfaction,
                            timeManagement = @timeManagement,
                            professionalism = @professionalism,
                            cleaningQuality = @cleaningQuality,
                            notes = @notes
	                    WHERE
                            appointmentID = @appointmentID;
                    END
                    ELSE
                    BEGIN 
	                    INSERT INTO FollowUp 
		                    (appointmentID,
                            schedulingSatisfaction,
                            timeManagement,
                            professionalism,
                            cleaningQuality,
                            notes)
	                    VALUES
		                    (@appointmentID,
                            @schedulingSatisfaction,
                            @timeManagement,
                            @professionalism,
                            @cleaningQuality,
                            @notes);
                    END
                    
                    UPDATE
                        Appointments
                    SET
                        followUpSent = 1
                    WHERE
                        appointmentID = @appointmentID;";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"appointmentID", followUp.appointmentID));
                cmd.Parameters.Add(new SqlParameter(@"schedulingSatisfaction", followUp.schedulingSatisfaction));
                cmd.Parameters.Add(new SqlParameter(@"timeManagement", followUp.timeManagement));
                cmd.Parameters.Add(new SqlParameter(@"professionalism", followUp.professionalism));
                cmd.Parameters.Add(new SqlParameter(@"cleaningQuality", followUp.cleaningQuality));
                cmd.Parameters.Add(new SqlParameter(@"notes", followUp.notes));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL SetFollowUp EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetFollowUpByID
        public static FollowUpStruct GetFollowUpByID(int appointmentID)
        {
            FollowUpStruct ret = new FollowUpStruct();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    *
                FROM 
                    FollowUp
                WHERE
                    appointmentID = @appointmentID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("appointmentID", appointmentID));
                sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    ret.appointmentID = (int)sqlDataReader["appointmentID"];
                    ret.createdOn = (DateTime)sqlDataReader["createdOn"];
                    ret.schedulingSatisfaction = (int)sqlDataReader["schedulingSatisfaction"];
                    ret.timeManagement = (int)sqlDataReader["timeManagement"];
                    ret.professionalism = (int)sqlDataReader["professionalism"];
                    ret.cleaningQuality = (int)sqlDataReader["cleaningQuality"];
                    ret.notes = (string)sqlDataReader["notes"];
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region GetFollowUpScoresByDateRange
        public static List<DBRow> GetFollowUpScoresByDateRange(int franchiseMask, int contractorType, DateTime startDate, DateTime endDate)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;

            try
            {
                Globals.FormatDateRange(ref startDate, ref endDate);

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    CO.firstName AS coFirstName,
	                CO.lastName AS coLastName,
	                CO.contractorID,
	                A.appointmentDate,
                    A.customerID,
	                A.startTime,
	                A.endTime,
	                CU.firstName AS cuFirstName,
	                CU.lastName AS cuLastName,
	                F.createdOn,
	                F.schedulingSatisfaction,
	                F.timeManagement,
	                F.professionalism,
	                F.cleaningQuality,
	                F.notes
                FROM
                    FollowUp F,
                    Appointments A,
                    Contractors CO,
                    Customers CU
                WHERE
                    CO.franchiseMask & @franchiseMask > 0 AND
                    CO.contractorType & @contractorType > 0 AND
                    A.appointmentDate >= @startDate AND
                    A.appointmentDate <= @endDate AND
                    F.appointmentID = A.appointmentID AND
                    A.contractorID = CO.contractorID AND
                    A.customerID = CU.customerID AND
                    (F.schedulingSatisfaction != 0 OR F.timeManagement != 0 OR F.professionalism != 0 OR F.cleaningQuality != 0)
                ORDER BY
                    CO.firstName, CO.lastName, A.contractorID, A.appointmentDate, A.startTime";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("contractorType", contractorType));
                cmd.Parameters.Add(new SqlParameter("startDate", startDate));
                cmd.Parameters.Add(new SqlParameter("endDate", endDate));
                sqlDataReader = cmd.ExecuteReader();

                return GetDBRows(sqlDataReader);
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return new List<DBRow>();
        }
        #endregion

        #region GetFollowUpScoresByCustomerID
        public static List<DBRow> GetFollowUpScoresByCustomerID(int customerID)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;

            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
	                F.schedulingSatisfaction,
	                F.timeManagement,
	                F.professionalism,
	                F.cleaningQuality
                FROM
                    FollowUp F,
                    Appointments A
                WHERE
                    F.appointmentID = A.appointmentID AND
                    A.appointmentDate >= DATEADD(day, -90, GETUTCDATE()) AND
                    A.customerID = @customerID AND
                    (F.schedulingSatisfaction != 0 OR F.timeManagement != 0 OR F.professionalism != 0 OR F.cleaningQuality != 0)
                ORDER BY
                    A.appointmentDate, A.startTime";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                sqlDataReader = cmd.ExecuteReader();

                return GetDBRows(sqlDataReader);
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return new List<DBRow>();
        }
        #endregion

        #region InsertGiftCard
        public static string InsertGiftCard(GiftCardStruct giftCard)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
	                INSERT INTO GiftCards
		                (giftCardID,
		                customerID,
                        paymentType,
		                paymentID,
		                amount,
                        lastFourCard,
                        giverName,
                        recipientName,
                        recipientEmail,
                        billingEmail,
                        message,
                        username)
	                VALUES
		                (@giftCardID,
		                @customerID,
                        @paymentType,
		                @paymentID,
		                @amount,
                        @lastFourCard,
                        @giverName,
                        @recipientName,
                        @recipientEmail,
                        @billingEmail,
                        @message,
                        @username);";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("@giftCardID", giftCard.giftCardID));
                cmd.Parameters.Add(new SqlParameter("@customerID", giftCard.customerID));
                cmd.Parameters.Add(new SqlParameter("@paymentType", giftCard.paymentType));
                cmd.Parameters.Add(new SqlParameter("@paymentID", (object)giftCard.paymentID ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@amount", giftCard.amount));
                cmd.Parameters.Add(new SqlParameter("@lastFourCard", (object)giftCard.lastFourCard ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@giverName", giftCard.giverName));
                cmd.Parameters.Add(new SqlParameter("@recipientName", giftCard.recipientName));
                cmd.Parameters.Add(new SqlParameter("@recipientEmail", giftCard.recipientEmail));
                cmd.Parameters.Add(new SqlParameter("@billingEmail", giftCard.billingEmail));
                cmd.Parameters.Add(new SqlParameter("@message", giftCard.message));
                cmd.Parameters.Add(new SqlParameter("@username", (object)giftCard.username ?? DBNull.Value));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL InsertGiftCard EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region VoidGiftCard
        public static string VoidGiftCard(int giftCardID)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE
                    GiftCards
                SET
                    isVoid = 1
	            WHERE
		            giftCardID = @giftCardID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("@giftCardID", giftCardID));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL VoidGiftCard EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetGiftCardByID
        public static GiftCardStruct GetGiftCardByID(int franchiseMask, int giftCardID)
        {
            GiftCardStruct ret = new GiftCardStruct();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    *
                FROM 
                    GiftCards G,
                    Customers C
                WHERE
                    G.giftCardID = @giftCardID AND
	                G.customerID = C.customerID AND
	                C.franchiseMask & @franchiseMask > 0";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("giftCardID", giftCardID));
                sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    ret.giftCardID = (int)sqlDataReader["giftCardID"];
                    ret.customerID = (int)sqlDataReader["customerID"];
                    ret.paymentType = (string)sqlDataReader["paymentType"];
                    ret.paymentID = Globals.SafeSqlString(sqlDataReader["paymentID"]);
                    ret.lastFourCard = Globals.SafeSqlString(sqlDataReader["lastFourCard"]);
                    ret.isVoid = (bool)sqlDataReader["isVoid"];
                    ret.batched = (bool)sqlDataReader["batched"];
                    ret.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                    ret.amount = (decimal)sqlDataReader["amount"];
                    ret.redeemed = (int)sqlDataReader["redeemed"];
                    ret.giverName = (string)sqlDataReader["giverName"];
                    ret.recipientName = (string)sqlDataReader["recipientName"];
                    ret.recipientEmail = (string)sqlDataReader["recipientEmail"];
                    ret.billingEmail = (string)sqlDataReader["billingEmail"];
                    ret.message = (string)sqlDataReader["message"];
                    ret.username = Globals.SafeSqlString(sqlDataReader["username"]);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region GetGiftCardsByDateRange
        public static List<GiftCardStruct> GetGiftCardsByDateRange(int franchiseMask, DateTime startDate, DateTime endDate)
        {
            List<GiftCardStruct> ret = new List<GiftCardStruct>();

            Globals.FormatDateRange(ref startDate, ref endDate);

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    GiftCards.giftCardID,
	                GiftCards.customerID,
                    GiftCards.paymentType,
	                GiftCards.paymentID,
	                GiftCards.isVoid,
	                GiftCards.batched,
	                GiftCards.dateCreated,
	                GiftCards.amount,
	                GiftCards.redeemed,
	                GiftCards.lastFourCard,
	                GiftCards.giverName,
	                GiftCards.recipientName,
	                GiftCards.recipientEmail,
	                GiftCards.billingEmail,
	                GiftCards.message,
                    GiftCards.username,
	                C.firstName,
	                C.lastName,
	                C.businessName,
	                Customers.firstName as redeemedFirstName,
	                Customers.lastName as redeemedLastName,
	                Customers.businessName as redeemedBusinessName
                FROM 
                    GiftCards LEFT OUTER JOIN Customers ON GiftCards.redeemed = Customers.customerID,
	                Customers C
                WHERE
	                GiftCards.customerID = C.customerID AND
	                C.franchiseMask & @franchiseMask > 0 AND
                    GiftCards.dateCreated >= @startDate AND
                    GiftCards.dateCreated <= @endDate
                ORDER BY
                    GiftCards.dateCreated DESC";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("startDate", startDate));
                cmd.Parameters.Add(new SqlParameter("endDate", endDate));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    GiftCardStruct giftCard = new GiftCardStruct();
                    giftCard.giftCardID = (int)sqlDataReader["giftCardID"];
                    giftCard.customerID = (int)sqlDataReader["customerID"];
                    giftCard.paymentType = (string)sqlDataReader["paymentType"];
                    giftCard.paymentID = Globals.SafeSqlString(sqlDataReader["paymentID"]);
                    giftCard.isVoid = (bool)sqlDataReader["isVoid"];
                    giftCard.batched = (bool)sqlDataReader["batched"];
                    giftCard.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                    giftCard.amount = (decimal)sqlDataReader["amount"];
                    giftCard.redeemed = (int)sqlDataReader["redeemed"];
                    giftCard.lastFourCard = Globals.SafeSqlString(sqlDataReader["lastFourCard"]);
                    giftCard.giverName = (string)sqlDataReader["giverName"];
                    giftCard.recipientName = (string)sqlDataReader["recipientName"];
                    giftCard.recipientEmail = (string)sqlDataReader["recipientEmail"];
                    giftCard.billingEmail = (string)sqlDataReader["billingEmail"];
                    giftCard.message = (string)sqlDataReader["message"];
                    giftCard.username = Globals.SafeSqlString(sqlDataReader["username"]);
                    giftCard.customerTitle = Globals.FormatCustomerTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                    giftCard.redeemedTitle = Globals.FormatCustomerTitle(sqlDataReader["redeemedFirstName"], sqlDataReader["redeemedLastName"], sqlDataReader["redeemedBusinessName"]);
                    ret.Add(giftCard);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region GetGiftCardsByCustomerID
        public static List<GiftCardStruct> GetGiftCardsByCustomerID(int franchiseMask, int customerID)
        {
            List<GiftCardStruct> ret = new List<GiftCardStruct>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    GiftCards.giftCardID,
	                GiftCards.customerID,
                    GiftCards.paymentType,
                    GiftCards.paymentID,
	                GiftCards.isVoid,
	                GiftCards.batched,
	                GiftCards.dateCreated,
	                GiftCards.amount,
	                GiftCards.redeemed,
	                GiftCards.lastFourCard,
	                GiftCards.giverName,
	                GiftCards.recipientName,
	                GiftCards.recipientEmail,
	                GiftCards.billingEmail,
	                GiftCards.message,
                    GiftCards.username,
	                C.firstName,
	                C.lastName,
	                C.businessName,
	                Customers.firstName as redeemedFirstName,
	                Customers.lastName as redeemedLastName,
	                Customers.businessName as redeemedBusinessName
                FROM 
                    GiftCards LEFT OUTER JOIN Customers ON GiftCards.redeemed = Customers.customerID,
	                Customers C
                WHERE
	                GiftCards.customerID = C.customerID AND
	                C.franchiseMask & @franchiseMask > 0 AND
                    GiftCards.customerID = @customerID
                ORDER BY
                    GiftCards.dateCreated DESC";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    GiftCardStruct giftCard = new GiftCardStruct();
                    giftCard.giftCardID = (int)sqlDataReader["giftCardID"];
                    giftCard.customerID = (int)sqlDataReader["customerID"];
                    giftCard.paymentType = (string)sqlDataReader["paymentType"];
                    giftCard.paymentID = Globals.SafeSqlString(sqlDataReader["paymentID"]);
                    giftCard.isVoid = (bool)sqlDataReader["isVoid"];
                    giftCard.batched = (bool)sqlDataReader["batched"];
                    giftCard.dateCreated = (DateTime)sqlDataReader["dateCreated"];
                    giftCard.amount = (decimal)sqlDataReader["amount"];
                    giftCard.redeemed = (int)sqlDataReader["redeemed"];
                    giftCard.lastFourCard = Globals.SafeSqlString(sqlDataReader["lastFourCard"]);
                    giftCard.giverName = (string)sqlDataReader["giverName"];
                    giftCard.recipientName = (string)sqlDataReader["recipientName"];
                    giftCard.recipientEmail = (string)sqlDataReader["recipientEmail"];
                    giftCard.billingEmail = (string)sqlDataReader["billingEmail"];
                    giftCard.message = (string)sqlDataReader["message"];
                    giftCard.username = Globals.SafeSqlString(sqlDataReader["username"]);
                    giftCard.customerTitle = Globals.FormatCustomerTitle(sqlDataReader["firstName"], sqlDataReader["lastName"], sqlDataReader["businessName"]);
                    giftCard.redeemedTitle = Globals.FormatCustomerTitle(sqlDataReader["redeemedFirstName"], sqlDataReader["redeemedLastName"], sqlDataReader["redeemedBusinessName"]);
                    ret.Add(giftCard);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region GetCustomerPoints
        public static string GetCustomerPoints(int franchiseMask, int customerID, out decimal points, out decimal ratePerHour)
        {
            points = 0;
            ratePerHour = 0;
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    points,
                    ratePerHour
                FROM 
                    Customers
                WHERE
	                customerID = @customerID AND
	                franchiseMask & @franchiseMask > 0";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                sqlDataReader = cmd.ExecuteReader();

                sqlDataReader.Read();
                points = (decimal)sqlDataReader["points"];
                ratePerHour = (decimal)sqlDataReader["ratePerHour"];
                return null;
            }
            catch (Exception ex)
            {
                return "SQL GetCustomerPoints EX: " + ex.Message;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region AddCustomerPoints
        public static string AddCustomerPoints(int franchiseMask, int customerID, decimal points)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE
                    Customers
                SET
                    points = (points + @points)
	            WHERE
		            customerID = @customerID AND
                    franchiseMask & @franchiseMask > 0";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                cmd.Parameters.Add(new SqlParameter("points", points));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL SetCustomerPoints EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetPartnerByCompany
        public static DBRow GetPartnerByCompany(string companyName)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        Partners
                    WHERE
                        companyName = @companyName";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("companyName", companyName));
                sqlDataReader = cmd.ExecuteReader();

                return GetDBRow(sqlDataReader);
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return null;
        }
        #endregion

        #region GetPartnersByCategory
        public static List<DBRow> GetPartnersByCategory(string category, int franchiseMask, string orderBy)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;

            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    *
                FROM
                    Partners
                WHERE
                    franchiseMask & @franchiseMask > 0 AND
                    (category = @category OR @category IS NULL)
                ORDER BY
                    " + orderBy;

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("franchiseMask", franchiseMask));
                cmd.Parameters.Add(new SqlParameter("category", (object)category ?? DBNull.Value));
                sqlDataReader = cmd.ExecuteReader();

                return GetDBRows(sqlDataReader);
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return new List<DBRow>();
        }
        #endregion

        #region AddDrivingRoute
        public static string AddDrivingRoute(string lookupKey, decimal distance, decimal travelTime)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                INSERT INTO DrivingRoutes (lookupKey, distance, travelTime) VALUES (@lookupKey, @distance, @travelTime)";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("lookupKey", lookupKey));
                cmd.Parameters.Add(new SqlParameter("distance", distance));
                cmd.Parameters.Add(new SqlParameter("travelTime", travelTime));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL AddDrivingRoute EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetDrivingRoutes
        public static Dictionary<string, DrivingRoute> GetDrivingRoutes()
        {
            Dictionary<string, DrivingRoute> ret = new Dictionary<string, DrivingRoute>();
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        lookupKey,
                        distance,
                        travelTime
                    FROM
                        DrivingRoutes
                    ORDER BY 
                        lookupKey";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    DrivingRoute route = new DrivingRoute();
                    string lookupKey = (string)sqlDataReader["lookupKey"];
                    route.distance = (decimal)sqlDataReader["distance"];
                    route.travelTime = (decimal)sqlDataReader["travelTime"];
                    ret.Add(lookupKey, route);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return ret;
        }
        #endregion

        #region GetCleaningPackByID
        public static DBRow GetCleaningPackByID(int cleaningPackID)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        CleaningPacks
                    WHERE
                        cleaningPackID = @cleaningPackID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("cleaningPackID", cleaningPackID));
                sqlDataReader = cmd.ExecuteReader();

                return GetDBRow(sqlDataReader);
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return null;
        }
        #endregion

        #region GetCleaningPackByCustomerID
        public static List<DBRow> GetCleaningPackByCustomerID(int customerID)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        CleaningPacks
                    WHERE
                        customerID = @customerID
                    ORDER BY
                        dateCreated DESC";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                sqlDataReader = cmd.ExecuteReader();

                return GetDBRows(sqlDataReader);
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return new List<DBRow>();
        }
        #endregion

        #region GetInvoiceRangeByCustomerID
        public static List<DBRow> GetInvoiceRangeByCustomerID(int customerID)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        InvoiceRange
                    WHERE
                        customerID = @customerID
                    ORDER BY
                        dateCreated DESC";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                sqlDataReader = cmd.ExecuteReader();

                return GetDBRows(sqlDataReader);
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return new List<DBRow>();
        }
        #endregion

        #region GetInvoiceRangeByDateRange
        public static List<DBRow> GetInvoiceRangeByDateRange(DateTime start, DateTime end)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        InvoiceRange
                    WHERE
                        endDate >= @start AND
                        startDate <= @end
                    ORDER BY
                        customerID, dateCreated";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("start", start));
                cmd.Parameters.Add(new SqlParameter("end", end));
                sqlDataReader = cmd.ExecuteReader();

                return GetDBRows(sqlDataReader);
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return new List<DBRow>();
        }
        #endregion

        #region GetMassEmailsToSend
        public static List<DBRow> GetMassEmailsToSend()
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT TOP 250
                        *
                    FROM
                        MassEmail
                    WHERE
                        status != 255
                    ORDER BY
                        dateCreated";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                sqlDataReader = cmd.ExecuteReader();

                return GetDBRows(sqlDataReader);
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return new List<DBRow>();
        }
        #endregion

        #region GetMassEmailsSentToday
        public static List<DBRow> GetMassEmailsSentToday()
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        MassEmail
                    WHERE
                        status = 255 AND
                        CONVERT(date, dateCreated) = CONVERT(date, GETUTCDATE())
                    ORDER BY
                        dateCreated";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                sqlDataReader = cmd.ExecuteReader();

                return GetDBRows(sqlDataReader);
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return new List<DBRow>();
        }
        #endregion

        #region UpdateInactiveCustomers
        public static string UpdateInactiveCustomers()
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE
	                Customers
                SET 
	                accountStatus = 'Inactive'
                FROM
	                (SELECT
		                Customers.customerID,
		                MAX(Appointments.appointmentDate) as lastApp
	                FROM
		                Customers LEFT JOIN Appointments ON Customers.customerID = Appointments.customerID
	                WHERE
		                Customers.accountStatus = 'Active'
	                GROUP BY
		                Customers.customerID) AS Q
                WHERE
	                Q.customerID = Customers.customerID AND
	                (Q.lastApp < DATEADD(day, -2, GETUTCDATE()) OR Q.lastApp IS NULL);

                UPDATE
	                Customers
                SET 
	                accountStatus = 'Active'
                FROM
	                (SELECT
		                Customers.customerID,
		                MAX(Appointments.appointmentDate) as lastApp
	                FROM
		                Customers LEFT JOIN Appointments ON Customers.customerID = Appointments.customerID
	                WHERE
		                (Customers.accountStatus = 'Inactive' OR Customers.accountStatus = 'As Needed')
	                GROUP BY
		                Customers.customerID) AS Q
                WHERE
	                Q.customerID = Customers.customerID AND
	                Q.lastApp > DATEADD(day, -2, GETUTCDATE());

                UPDATE
	                Customers
                SET 
	                accountStatus = 'New'
                FROM
	                (SELECT
		                Customers.customerID,
		                MAX(Appointments.appointmentDate) as lastApp
	                FROM
		                Customers LEFT JOIN Appointments ON Customers.customerID = Appointments.customerID
	                WHERE
		                (Customers.accountStatus = 'Quote' OR Customers.accountStatus = 'Web Quote')
	                GROUP BY
		                Customers.customerID) AS Q
                WHERE
	                Q.customerID = Customers.customerID AND
	                Q.lastApp > DATEADD(day, -2, GETUTCDATE());

                UPDATE
	                Appointments
                SET
	                customerDiscountReferral = (CASE WHEN Q.refCount IS NULL THEN 0 ELSE (CASE WHEN Q.refCount > 5 THEN 30 ELSE (Q.refCount + 1) * 5 END) END)
                FROM
	                Appointments LEFT JOIN
	                (SELECT
		                referredBy AS refID,
		                COUNT(*) AS refCount
	                FROM 
		                Customers
	                WHERE
		                referredBy > 0 AND
		                accountStatus = 'Active'
	                GROUP BY
		                referredBy) AS Q ON Appointments.customerID = Q.refID
                WHERE
	                appointmentDate >= CONVERT(date, DATEADD(day, -1, GETUTCDATE()));";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL UpdateInactiveCustomers EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region UpdateContractorScores
        public static string UpdateContractorScores()
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE
	                Contractors
                SET
	                score = (CASE WHEN Q.newScore IS NULL THEN 0 ELSE Q.newScore END)
                FROM
	                Contractors LEFT JOIN
	                (SELECT 
		                Appointments.contractorID,
		                (SUM(timeManagement) + SUM(professionalism) + SUM(cleaningQuality)) / (COUNT(*) * 3.0) as newScore
	                FROM
		                FollowUp,
		                Appointments
	                WHERE
		                FollowUp.appointmentID = Appointments.appointmentID AND
		                Appointments.appointmentDate > DATEADD(day, -90, GETUTCDATE()) AND
                        (FollowUp.schedulingSatisfaction != 0 OR FollowUp.timeManagement != 0 OR FollowUp.professionalism != 0 OR FollowUp.cleaningQuality != 0)
	                GROUP BY
		                Appointments.contractorID) AS Q ON Contractors.contractorID = Q.contractorID;";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL UpdateContractorScores EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
        #endregion

        #region GetHomeGuardContract
        public static DBRow GetHomeGuardContract(int customerID)
        {
            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                SELECT
                    *
                FROM
                    HomeGuardContract
                WHERE
	                customerID = @customerID";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("customerID", customerID));
                sqlDataReader = cmd.ExecuteReader();

                return GetDBRow(sqlDataReader);
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
            return null;
        }
        #endregion


        #region UpdateAppointmentFromAPI

        public static string UpdateAppointmentFromAPI(int appId, List<string> images, string notes)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE 
		            Appointments
	            SET
                    Notes = @notes
	            WHERE
		            appointmentID = @appointmentID;";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"appointmentID", appId));
                cmd.Parameters.Add(new SqlParameter(@"notes", notes ?? ""));
                cmd.ExecuteNonQuery();


                if (images.Count > 0)
                {
                    string cmdText1 = @"
                Delete from 
		            AppointmentAttachments
	            WHERE
		            AppointmentId = @appointmentID;";

                    SqlCommand cmd1 = new SqlCommand(cmdText1, sqlConnection);
                    cmd1.Parameters.Add(new SqlParameter(@"appointmentID", appId));
                    cmd1.ExecuteNonQuery();


                    foreach (var item in images)
                    {

                        string cmdText2 = @"
                insert into 
		            AppointmentAttachments (AppointmentId,ImageURL)
	       VALUES
(@appointmentId,  @imgURL  )";

                        SqlCommand cmd2 = new SqlCommand(cmdText2, sqlConnection);
                        cmd2.Parameters.Add(new SqlParameter(@"appointmentId", appId));
                        cmd2.Parameters.Add(new SqlParameter(@"imgURL", item));
                        cmd2.ExecuteNonQuery();


                    }

                }

                return null;
            }
            catch (Exception ex)
            {
                return "SQL UpdateFranchiseNotes EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }



        #endregion


        #region Get Appointment Attachments

        public static List<AppAttachments> GetAppointmentAttachments(int appId)
        {
            List<AppAttachments> ret = new List<AppAttachments>();

            SqlConnection sqlConnection = null;
            SqlDataReader sqlDataReader = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                    SELECT
                        *
                    FROM
                        AppointmentAttachments
                    WHERE
AppointmentId = @appID
";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter("appID", appId));
                sqlDataReader = cmd.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    AppAttachments contractor = new AppAttachments();
                    contractor.id = (int)sqlDataReader["id"];
                    contractor.AppointmentId = (int)sqlDataReader["appointmentId"];
                    contractor.ImageURL = (string)sqlDataReader["imageURL"];
                    ret.Add(contractor);
                }
            }
            catch { }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Close();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

            return ret;
        }



        #endregion



        public static string StartJobByAppId(int appId)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                if (appId > 0)
                {
                    string cmdText1 = @"
                select  jobStartTime from
		            Appointments
	            WHERE
		            appointmentID = @appointmentID;";

                    SqlCommand cmd1 = new SqlCommand(cmdText1, sqlConnection);
                    cmd1.Parameters.Add(new SqlParameter(@"appointmentID", appId));
                    var sqlReader = cmd1.ExecuteReader();

                    if (!sqlReader.Read())
                        return "SQL GetApointmentpByID: Record (AppointmentID=" + appId + ") does not exist.";

                    if (sqlReader != null)
                    {
                        try
                        {
                            var time = (DateTime)sqlReader["jobStartTime"];
                            DateTime.Parse(time.ToString());
                            return "Job has already started.";

                        }
                        catch (Exception)
                        {


                        }
                        finally
                        {
                            if (sqlConnection != null)
                                sqlConnection.Close();
                        }
                    }
                }

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE 
		            Appointments
	            SET
                    jobStartTime = GetDate()
	            WHERE
		            appointmentID = @appointmentID;";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"appointmentID", appId));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL UpdateFranchiseNotes EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }

        public static string EndJobByAppId(int appId)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                if (appId > 0)
                {
                    string cmdText1 = @"
                select  jobEndTime,jobStartTime from
		            Appointments
	            WHERE
		            appointmentID = @appointmentID;";

                    SqlCommand cmd1 = new SqlCommand(cmdText1, sqlConnection);
                    cmd1.Parameters.Add(new SqlParameter(@"appointmentID", appId));
                    var sqlReader = cmd1.ExecuteReader();

                    if (!sqlReader.Read())
                        return "SQL GetApointmentpByID: Record (AppointmentID=" + appId + ") does not exist.";

                    if (sqlReader != null)
                    {
                        try
                        {
                            if (sqlReader["jobStartTime"] == DBNull.Value)
                            {
                                return "Job is not started yet. Please start job first.";
                            }

                            var time = (DateTime)sqlReader["jobEndTime"];
                            DateTime.Parse(time.ToString());
                            return "Job has already ended.";

                        }
                        catch (Exception)
                        {


                        }
                        finally
                        {
                            if (sqlConnection != null)
                                sqlConnection.Close();
                        }
                    }
                }

                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE 
		            Appointments
	            SET
                    jobEndTime = GetDate(),
JobCompleted = 1
	            WHERE
		            appointmentID = @appointmentID;";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"appointmentID", appId));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL UpdateFranchiseNotes EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

        }
        public static string JobCoordinatesByAppId(int appId, string latitude, string longitude)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(connString);
                sqlConnection.Open();

                string cmdText = @"
                UPDATE 
		            Contractors
	            SET
                    latitude = @latitude,
longitude = @longitude,
ShareLocation = 1
	            WHERE
		            contractorID = @appointmentID;";

                SqlCommand cmd = new SqlCommand(cmdText, sqlConnection);
                cmd.Parameters.Add(new SqlParameter(@"appointmentID", appId));
                cmd.Parameters.Add(new SqlParameter(@"longitude", longitude));
                cmd.Parameters.Add(new SqlParameter(@"latitude", latitude));
                cmd.ExecuteNonQuery();

                return null;
            }
            catch (Exception ex)
            {
                return "SQL Update coordinates EX: " + ex.Message;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

        }


    }
}