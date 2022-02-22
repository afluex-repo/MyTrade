﻿using MyTrade.Filter;
using MyTrade.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mytrade.Models;
namespace MyTrade.Controllers
{
    public class AdminController : AdminBaseController
    {
        // GET: Admin
        public ActionResult AdminDashboard()
        {
            Dashboard newdata = new Dashboard();
            DataSet Ds = newdata.GetDashBoardDetails();

            ViewBag.TotalUsers = Ds.Tables[1].Rows[0]["TotalUsers"].ToString();
            ViewBag.BlockedUsers = Ds.Tables[1].Rows[0]["BlockedUsers"].ToString();
            ViewBag.InactiveUsers = Ds.Tables[1].Rows[0]["InactiveUsers"].ToString();
            ViewBag.ActiveUsers = Ds.Tables[1].Rows[0]["ActiveUsers"].ToString();
            if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[2].Rows.Count > 0)
            {
                ViewBag.Tr1Business = Ds.Tables[2].Rows[0]["Tr1Business"].ToString();
                ViewBag.Tr2Business = Ds.Tables[2].Rows[0]["Tr2Business"].ToString();
            }
            return View(newdata);
        }
        #region GeneratePin
        public ActionResult Generate_EPin()
        {
            #region Product Bind
            Common objcomm = new Common();
            List<SelectListItem> ddlProduct = new List<SelectListItem>();
            DataSet ds1 = objcomm.BindProduct();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlProduct.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlProduct.Add(new SelectListItem { Text = r["ProductName"].ToString(), Value = r["Pk_ProductId"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlProduct = ddlProduct;

            #endregion
            #region PaymentMode
            Common com = new Common();
            List<SelectListItem> ddlPayment = new List<SelectListItem>();
            DataSet ds = com.PaymentList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int paycount = 0;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (paycount == 0)
                    {
                        ddlPayment.Add(new SelectListItem { Text = "Select Payment", Value = "0" });
                    }
                    ddlPayment.Add(new SelectListItem { Text = r["PaymentMode"].ToString(), Value = r["PK_paymentID"].ToString() });
                    paycount++;
                }
            }

            ViewBag.ddlPayment = ddlPayment;

            #endregion
            return View();
        }
        [HttpPost]
        [ActionName("Generate_EPin")]
        [OnAction(ButtonName = "btn_Pin")]
        public ActionResult CreatePinAction(Admin obj)
        {
            try
            {
                obj.AddedBy = Session["Pk_AdminId"].ToString();

                DataSet ds = obj.CreatePin();
                if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["createpin"] = "Pin Created Successfully";
                    }
                    else
                    {
                        TempData["createpin"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }

                }
                else { }

            }
            catch (Exception ex)
            {
                TempData["createpin"] = ex.Message;
            }
            return RedirectToAction("Generate_EPin", "Admin");
        }
        public ActionResult UnUsedPin(Admin obj)
        {

            List<Admin> lst = new List<Admin>();
            obj.Package = obj.Package == "0" ? null : obj.Package;
            DataSet ds = obj.GetUsedUnUsedPins();
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Admin Objload = new Admin();
                    Objload.ePinNo = dr["ePinNo"].ToString();
                    Objload.Package = dr["Package"].ToString();
                    Objload.DisplayName = dr["PinUser"].ToString();
                    Objload.AddedOn = dr["CreatedDate"].ToString();
                    Objload.RegisteredTo = dr["RegisteredTo"].ToString();
                    Objload.Status = dr["PinStaus"].ToString();
                    lst.Add(Objload);
                }
                obj.lstunusedpins = lst;
            }
            #region Product Bind

            Common objcomm = new Common();
            List<SelectListItem> ddlProduct = new List<SelectListItem>();
            DataSet ds1 = objcomm.BindProduct();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlProduct.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlProduct.Add(new SelectListItem { Text = r["ProductName"].ToString(), Value = r["Pk_ProductId"].ToString() });
                    count++;
                }
            }
            ViewBag.ddlProduct = ddlProduct;
            #endregion
            return View(obj);
        }
        public ActionResult FillAmount(string ProductId)
        {
            Admin obj = new Admin();
            obj.Package = ProductId;
            DataSet ds = obj.BindPriceByProduct();
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                obj.Amount = ds.Tables[0].Rows[0]["ProductPrice"].ToString();
            }
            else { }
            return Json(obj, JsonRequestBehavior.AllowGet);

        }
        #endregion
        public ActionResult GetMemberName(string LoginId)
        {
            Common obj = new Common();
            obj.ReferBy = LoginId;
            DataSet ds = obj.GetMemberDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                obj.DisplayName = ds.Tables[0].Rows[0]["FullName"].ToString();
                obj.Result = "Yes";
            }
            else { obj.Result = "Invalid LoginId"; }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ChangePassword")]
        public ActionResult ChangePassword(Admin model)
        {
            try
            {
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.ChangePassword();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Password Changed  Successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("ChangePassword", "Admin");
        }


        public ActionResult PinTransferReportForAdmin()
        {
            AdminReports model = new AdminReports();
            List<AdminReports> lst = new List<AdminReports>();
            DataSet ds = model.GetTransferPinReport();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    AdminReports obj = new AdminReports();
                    obj.ePinNo = r["EpinNo"].ToString();
                    obj.FromId = r["FromId"].ToString();
                    obj.FromName = r["FromName"].ToString();
                    obj.ToId = r["ToId"].ToString();
                    obj.ToName = r["ToName"].ToString();
                    obj.TransferDate = r["TransferDate"].ToString();
                    lst.Add(obj);
                }
                model.lstPinTransfer = lst;
            }
            return View(model);
        }

        [HttpPost]
        [OnAction(ButtonName = "GetDetails")]
        [ActionName("PinTransferReportForAdmin")]
        public ActionResult PinTransferReportForAdmin(AdminReports model)
        {
            List<AdminReports> lst = new List<AdminReports>();
            model.ePinNo = model.ePinNo == "0" ? null : model.ePinNo;
            model.LoginId = model.LoginId == "0" ? null : model.LoginId;
            model.ToLoginID = model.ToLoginID == "0" ? null : model.ToLoginID;
            DataSet ds = model.GetTransferPinReport();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    AdminReports obj = new AdminReports();
                    obj.ePinNo = r["EpinNo"].ToString();
                    obj.FromId = r["FromId"].ToString();
                    obj.FromName = r["FromName"].ToString();
                    obj.ToId = r["ToId"].ToString();
                    obj.ToName = r["ToName"].ToString();
                    obj.TransferDate = r["TransferDate"].ToString();
                    lst.Add(obj);
                }
                model.lstPinTransfer = lst;
            }
            return View(model);
        }



        public ActionResult WalletList()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetEwalletRequestDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.RequestID = r["PK_RequestID"].ToString();
                    obj.UserId = r["FK_UserId"].ToString();
                    obj.RequestCode = r["RequestCode"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.PaymentMode = r["PaymentMode"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.BankName = r["BankName"].ToString();
                    obj.TransactionDate = r["RequestedDate"].ToString();
                    obj.BankBranch = r["BankBranch"].ToString();
                    obj.ChequeDDNo = r["ChequeDDNo"].ToString();
                    obj.ChequeDDDate = r["ChequeDDDate"].ToString();
                    obj.WalletId = r["WalletId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.DisplayName = r["Name"].ToString();
                    lst.Add(obj);
                }
                model.lstWallet = lst;
            }
            return View(model);
        }

        [HttpPost]
        [OnAction(ButtonName = "GetDetails")]
        [ActionName("WalletList")]
        public ActionResult WalletList(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.GetEwalletRequestDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.RequestID = r["PK_RequestID"].ToString();
                    obj.UserId = r["FK_UserId"].ToString();
                    obj.RequestCode = r["RequestCode"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.PaymentMode = r["PaymentMode"].ToString();
                    obj.Status = r["Status"].ToString();
                    obj.BankName = r["BankName"].ToString();
                    obj.BankBranch = r["BankBranch"].ToString();
                    obj.ChequeDDNo = r["ChequeDDNo"].ToString();
                    obj.ChequeDDDate = r["ChequeDDDate"].ToString();
                    obj.WalletId = r["WalletId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.DisplayName = r["Name"].ToString();
                    lst.Add(obj);
                }
                model.lstWallet = lst;
            }
            return View(model);
        }



        public ActionResult Approve(string id)
        {
            try
            {
                Admin model = new Admin();
                model.RequestID = id;
                model.Status = (model.Status = "Approved");
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.ApproveDeclineEwalletRequest();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Ewallet Request Approved Successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("WalletList", "Admin");
        }

        public ActionResult DeClined(string id)
        {
            try
            {
                Admin model = new Admin();
                model.RequestID = id;
                model.Status = (model.Status = "Declined");
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.ApproveDeclineEwalletRequest();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Ewallet Request Declined Successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("WalletList", "Admin");
        }

        public ActionResult PaymentTypeMaster()
        {

            #region ddlSites
            Admin obj = new Admin();
            int count = 0;
            List<Admin> lst = new List<Admin>();
            List<SelectListItem> ddlPaymentRype = new List<SelectListItem>();
            DataSet ds1 = obj.GetPaymentType();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlPaymentRype.Add(new SelectListItem { Text = "-Select-", Value = "" });
                    }
                    ddlPaymentRype.Add(new SelectListItem { Text = r["PaymentType"].ToString(), Value = r["PK_PaymentTypeId"].ToString() });
                    count = count + 1;
                }
            }

            ViewBag.ddlPaymentRype = ddlPaymentRype;
            #endregion
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                Admin model = new Admin();
                model.PaymentType = dr["PaymentType"].ToString();
                if (dr["IsActive"].ToString() == "True")
                {
                    model.Status = "Active";
                }
                else
                {
                    model.Status = "Inactive";
                }
                lst.Add(model);
            }
            obj.lstWallet = lst;
            return View(obj);
        }

        [HttpPost]
        [OnAction(ButtonName = "Update")]
        [ActionName("PaymentTypeMaster")]
        public ActionResult PaymentTypeMaster(Admin model)
        {
            try
            {
                model.AddedBy = Session["Pk_AdminId"].ToString();
                //model.PaymentTypeId = model.PaymentTypeId;
                DataSet ds = model.UpdatePaymentType();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Payment type update successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("PaymentTypeMaster", "Admin");
        }

        public ActionResult EPinRequestList()
        {
            Admin model = new Admin();
            List<Admin> list = new List<Admin>();
            DataSet dss = model.GetEPinRequestDetails();
            if (dss != null && dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dss.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_RequestID = r["PK_RequestID"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.ProductName = r["ProductName"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Fk_Paymentid = r["PaymentMode"].ToString();
                    obj.BankName = r["BankName"].ToString();
                    obj.BankBranch = r["BankBranch"].ToString();
                    obj.TransactionNo = r["ChequeDDNo"].ToString();
                    obj.TransactionDate = r["ChequeDDDate"].ToString();
                    obj.Status = r["Status"].ToString();
                    list.Add(obj);
                }
                model.lstEpinRequest = list;
            }
            return View();
        }
        [HttpPost]
        [OnAction(ButtonName = "GetDetails")]
        [ActionName("EPinRequestList")]
        public ActionResult EPinRequestList(Admin model)
        {
            List<Admin> list = new List<Admin>();
            model.Name = model.Name == "0" ? null : model.Name;
            model.LoginId = model.LoginId == "0" ? null : model.LoginId;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");

            DataSet dss = model.GetEPinRequestDetails();
            if (dss != null && dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dss.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_RequestID = r["PK_RequestID"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.ProductName = r["ProductName"].ToString();
                    obj.Amount = r["Amount"].ToString();
                    obj.Fk_Paymentid = r["PaymentMode"].ToString();
                    obj.BankName = r["BankName"].ToString();
                    obj.BankBranch = r["BankBranch"].ToString();
                    obj.TransactionNo = r["ChequeDDNo"].ToString();
                    obj.TransactionDate = r["ChequeDDDate"].ToString();
                    obj.Status = r["Status"].ToString();
                    list.Add(obj);
                }
                model.lstEpinRequest = list;
            }
            return View(model);
        }


        public ActionResult AcceptedEPinRequest(string id)
        {
            try
            {
                Admin model = new Admin();
                model.RequestID = id;
                model.Status = (model.Status = "Accepted");
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.AcceptRejectEPinRequest();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "EPin Request Accepted Successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("EPinRequestList", "Admin");
        }

        public ActionResult RejectedEPinRequest(string id)
        {
            try
            {
                Admin model = new Admin();
                model.RequestID = id;
                model.Status = (model.Status = "Rejected");
                model.UpdatedBy = Session["Pk_AdminId"].ToString();
                DataSet ds = model.AcceptRejectEPinRequest();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "EPin Request Rejected Successfully";
                    }
                    else
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("EPinRequestList", "Admin");
        }


        public ActionResult ROIWalletForAdmin()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetROIWalletDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.RoiWalletId = r["Pk_ROIWalletId"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lstTps = lst;
            }
            return View(model);
        }


        [HttpPost]
        [ActionName("ROIWalletForAdmin")]
        [OnAction(ButtonName = "Search")]
        public ActionResult ROIWalletForAdmin(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            model.LoginId = model.LoginId == "0" ? null : model.LoginId;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.GetROIWalletDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.RoiWalletId = r["Pk_ROIWalletId"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lstTps = lst;
            }
            return View(model);

        }

        public ActionResult ROIIncomeReportsForAdmin()
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetROIIncomeReportsDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.ROIId = r["Pk_ROIId"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.Name = r["Name"].ToString();
                    obj.TopUpAmount = r["TopUpAmount"].ToString();
                    obj.Date = r["TopUpDate"].ToString();
                    lst.Add(obj);
                }
                model.lstROIIncome = lst;
            }
            return View(model);
        }

        public ActionResult ViewROIForAdmin(string Id)
        {
            Admin model = new Admin();
            List<Admin> lst = new List<Admin>();
            DataSet ds = model.GetROIDetails();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.ROI = r["Pk_ROIId"].ToString();
                    obj.ROI = r["ROI"].ToString();
                    obj.Date = r["ROIDate"].ToString();
                    lst.Add(obj);
                }
                model.lstROI = lst;
            }
            return View(model);
        }


        public ActionResult PayoutWalletLedgerForAdmin()
        {
            List<Admin> lst = new List<Admin>();
            Admin model = new Admin();
            DataSet ds = model.PayoutWalletLedger();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_PayoutWalletId = r["PK_PayoutWalletId"].ToString();
                    obj.Fk_UserId = r["FK_UserId"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult PayoutWalletLedgerForAdmin(Admin model)
        {
            List<Admin> lst = new List<Admin>();
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            DataSet ds = model.PayoutWalletLedger();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Admin obj = new Admin();
                    obj.PK_PayoutWalletId = r["PK_PayoutWalletId"].ToString();
                    obj.Fk_UserId = r["FK_UserId"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.Narration = r["Narration"].ToString();
                    obj.TransactionDate = r["TransactionDate"].ToString();
                    lst.Add(obj);
                }
                model.lst = lst;
            }
            return View(model);
        }

    }
}