﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyTrade.Models
{
    public class AdminReports :Common
    {
       public List<AdminReports> lsttopupreport { get; set; }

        public string isBlocked { get; set; }

        public string Email { get;  set; }
        public string FromDate { get;  set; }
        public bool IsDownline { get;  set; }
        public string JoiningDate { get;  set; }
        public string LoginId { get;  set; }
        public List<AdminReports> lstassociate { get; set; }
        public List<AdminReports> lstPinTransfer { get; set; }
        public List<AdminReports> lstDirect { get; set; }
        public string Mobile { get;  set; }
        public string Name { get;  set; }
        public string Password { get;  set; }
        public string SponsorId { get;  set; }
        public string SponsorName { get;  set; }
        public string Status { get;  set; }
        public string ToDate { get;  set; }
        public string ToLoginID { get;  set; }
        public string UpgradtionDate { get;  set; }
        public string Amount { get;  set; }
        public string TopupBy { get;  set; }
        public string PrintingDate { get;  set; }
        public string Description { get;  set; }
        public string PaymentMode { get;  set; }
        public string BusinessType { get;  set; }
        public string ReceiptNo { get;  set; }

        public string FromLoginID { get; set; }
        public string ePinNo { get; set; }
        public string FromId { get; set; }
        public string FromName { get; set; }
        public string ToId { get; set; }
        public string ToName { get; set; }
        public string TransferDate { get; set; }
        public string FromActivationDate { get; set; }
        public string ToActivationDate { get; set; }
        public string PermanentDate { get; set; }

        #region associatelist
        public DataSet GetAssociateList()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@Name", Name),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),
                                    new SqlParameter("@SponsorID", SponsorId),
                                    new SqlParameter("@SponsorName", SponsorName),
                                    new SqlParameter("@Status", Status),
                                    new SqlParameter("@IsDownline", IsDownline),
                                    new SqlParameter("@Leg", Leg)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetAssociateList", para);
            return ds;
        }
        #endregion
        #region topupreport
        public DataSet GetTopupReport()
        {
            SqlParameter[] para = {   new SqlParameter("@LoginID", LoginId),
                                      new SqlParameter("@Name", Name),
                                      new SqlParameter("@FromDate", FromDate),
                                      new SqlParameter("@ToDate", ToDate),
                                      new SqlParameter("@Package", Package),
                                      new SqlParameter("@ClaculationStatus", Status),
                                      new SqlParameter("@Fk_BusinessId", BusinessType)
                                  };

            DataSet ds = DBHelper.ExecuteQuery("GetTopupreport", para);
            return ds;
        }
        #endregion

        public DataSet GetTransferPinReport()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@FromLoginId",LoginId),
                new SqlParameter("@ToLoginId",ToLoginID),
                new SqlParameter("@EpinNo",ePinNo)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetTransferPinReportForAdmin", para);
            return ds;
        }

        public DataSet GetDirectList()
        {
            SqlParameter[] para = { new SqlParameter("@LoginId", LoginId),
                                    new SqlParameter("@Name", Name),
                                    new SqlParameter("@FromDate", FromDate),
                                    new SqlParameter("@ToDate", ToDate),
                                    new SqlParameter("@FromActivationDate", FromActivationDate),
                                    new SqlParameter("@ToActivationDate", ToActivationDate),
                                    new SqlParameter("@Leg", Leg),
                                    new SqlParameter("@Status", Status),
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetDirectListForAdmin", para);
            return ds;
        }




    }
}