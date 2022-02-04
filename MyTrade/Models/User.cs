﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyTrade.Models
{
    public class User:Common
    {
        #region property
        public string EPin { get; set; }
        public string PinStatus { get;  set; }
        #endregion
        public DataSet ValidateEpin()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@EPin", EPin),
                                      new SqlParameter("@Fk_UserId",Fk_UserId)

                                  };
            DataSet ds = DBHelper.ExecuteQuery("ValidatePin", para);

            return ds;
        }
        public DataSet ActivateUser()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@EPinNo", EPin),
                                      new SqlParameter("@Fk_UserId",Fk_UserId)

                                  };
            DataSet ds = DBHelper.ExecuteQuery("ActivateUser", para);

            return ds;
        }
    }


    public class Pin
    {
        public string FK_UserId { get; set; }
        public string LoginId { get; set; }
        public string ParentLoginId { get; set; }
        public string Name { get; set; }
        public string PK_PinId { get; set; }
        public string PK_EPinDetailsId { get; set; }
        public string ePinNo { get; set; }
        public string PinAmount { get; set; }
        public string PinStatus { get; set; }
        public string IsRegistered { get; set; }
        public string RegisteredTo { get; set; }
        public List<Pin> lst { get; set; }
        public string ProductName { get; set; }
        public string FromId { get;  set; }
        public string FromName { get;  set; }
        public string ToId { get;  set; }
        public string ToName { get;  set; }
        public string TransferDate { get;  set; }
        public string ToLoginId { get; set; }
        public DataSet GetPinList()
        {
            SqlParameter[] para = {
                                      new SqlParameter("@FK_UserId", FK_UserId)

                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetPin", para);

            return ds;
        }
        public DataSet ePinTransfer()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Epino",ePinNo),
                new SqlParameter("@LoginId",LoginId)
            };
            DataSet ds = DBHelper.ExecuteQuery("ePinTransfer", para);
            return ds;
        }
        public DataSet GetTransferPinReport()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@FromLoginId",LoginId),
                new SqlParameter("@ToLoginId",ToLoginId),
                new SqlParameter("@EpinNo",ePinNo)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetTransferPinReport", para);
            return ds;
        }

    }
}