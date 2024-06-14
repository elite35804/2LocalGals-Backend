using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexus
{
    public class CreditCard
    {
        #region Charge
        public static string Charge(string ePNAccount, string restrictKey, string cardNumber, string expMo, string expYear, string address, string zip, string ccv, decimal amount, out string invoice, out string transID)
        {
            if (Retriever.IsRetrieverAccount(ePNAccount))
            {
                return Retriever.Charge(ePNAccount.Substring(4), restrictKey, cardNumber, expMo, expYear, address, zip, ccv, amount, false, false, out invoice, out transID);
            }
            else if (PayTrace.IsPayTraceAccount(ePNAccount))
            {
                return PayTrace.Charge(ePNAccount, restrictKey, cardNumber, expMo, expYear, address, zip, ccv, amount, out invoice, out transID);
            }
            else
            {
                return ECreditCard.Charge(ePNAccount, restrictKey, cardNumber, expMo, expYear, address, zip, amount, out invoice, out transID);
            }
        }
        #endregion

        #region Authorize
        public static string Authorize(string ePNAccount, string restrictKey, string cardNumber, string expMo, string expYear, string address, string zip, string ccv, decimal amount, out string invoice, out string transID)
        {
            invoice = null;
            transID = null;
            if (Retriever.IsRetrieverAccount(ePNAccount))
            {
                return Retriever.Charge(ePNAccount.Substring(4), restrictKey, cardNumber, expMo, expYear, address, zip, ccv, amount, false, true, out invoice, out transID);
            }
            return "Only Available to Retriever Account";
        }
        #endregion

        #region Capture
        public static string Capture(string ePNAccount, string restrictKey, string authTransID, decimal amount, out string transID)
        {
            transID = null;
            if (Retriever.IsRetrieverAccount(ePNAccount))
            {
                return Retriever.Capture(ePNAccount.Substring(4), restrictKey, authTransID, amount, out transID);
            }
            return "Only Available to Retriever Account";
        }
        #endregion

        #region Void
        public static string Void(string ePNAccount, string restrictKey, string transID, out string voidTransID)
        {
            if (Retriever.IsRetrieverAccount(ePNAccount))
            {
                return Retriever.Void(ePNAccount.Substring(4), restrictKey, transID, out voidTransID);
            }
            else if (PayTrace.IsPayTraceAccount(ePNAccount))
            {
                return PayTrace.Void(ePNAccount, restrictKey, transID, out voidTransID);
            }
            else
            {
                return ECreditCard.Void(ePNAccount, restrictKey, transID, out voidTransID);
            }
        }
        #endregion

        #region Refund
        public static string Refund(string ePNAccount, string restrictKey, string cardNumber, string expMo, string expYear, string address, string zip, string ccv, decimal amount, out string invoice, out string transID)
        {
            if (Retriever.IsRetrieverAccount(ePNAccount))
            {
                return Retriever.Charge(ePNAccount.Substring(4), restrictKey, cardNumber, expMo, expYear, address, zip, ccv, amount, true, false, out invoice, out transID);
            }
            else if (PayTrace.IsPayTraceAccount(ePNAccount))
            {
                return PayTrace.Refund(ePNAccount, restrictKey, cardNumber, expMo, expYear, address, zip, ccv, amount, out invoice, out transID);
            }
            else
            {
                return ECreditCard.Refund(ePNAccount, restrictKey, cardNumber, expMo, expYear, address, zip, amount, out invoice, out transID);
            }
        }
        #endregion

        #region CloseBatch
        public static string CloseBatch(string ePNAccount, string restrictKey)
        {
            if (Retriever.IsRetrieverAccount(ePNAccount) || PayTrace.IsPayTraceAccount(ePNAccount))
            {
                return null;
            }
            else
            {
                return ECreditCard.CloseBatch(ePNAccount, restrictKey);
            }
        }
        #endregion
    }
}