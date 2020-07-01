using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GarbagePresentation.Models
{
    public class UserModel {
        public string UserId { get; set; }
        public string Name{get;set;}
        public string Email{get;set;}
        public string Password{get;set;}
        public string Status{get;set;}
        public string RoleId{get;set;}
        public string RoleName{get;set;}
        public string ContactNo{get;set;}
        public string Available{get;set;}
    }
    public class CleintRequestModel
    {
        public string RequestId { get; set; }
        public string UserId { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Address { get; set; }
        public string GarbageType { get; set; }
        public string TotalAmount { get; set; }
        public string RequestStatus { get; set; }
        public string CreatedDate { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
    }
    public class CleintRequestModel1
    {
        public string RequestId { get; set; }
        public string UserId { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Address { get; set; }
        public string GarbageType { get; set; }
        public string TotalAmount { get; set; }
        public string RequestStatus { get; set; }
        public string CreatedDate { get; set; }
        public string Name { get; set; }
        public string EmployeeName { get; set; }
        public string ContactNo { get; set; }
    }
    public class CleintRequestDetailModel
    {
        public string RequestId { get; set; }
        public string UserId { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Address { get; set; }
        public string GarbageType { get; set; }
        public string TotalAmount { get; set; }
        public string RequestStatus { get; set; }
        public string CreatedDate { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string Date { get; set; }
        public string RequestDetailId { get; set; }
        public string RequestDetailStatus { get; set; }
        public string Time { get; set; }
    }
    public class CleintRequestDetailModel2
    {
        public string RequestId { get; set; }
        public string UserId { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Address { get; set; }
        public string GarbageType { get; set; }
        public string TotalAmount { get; set; }
        public string RequestStatus { get; set; }
        public string CreatedDate { get; set; }
        public string Name { get; set; }
        public string EmployeeName { get; set; }
        public string ContactNo { get; set; }
        public string Date { get; set; }
        public string RequestDetailId { get; set; }
        public string RequestDetailStatus { get; set; }
        public string Time { get; set; }
    }
    public class CleintRequestDetailModel1
    {
        public string UserId { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Address { get; set; }
        public string GarbageType { get; set; }
        public string TotalAmount { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
    public class RateModel
    {
        public string RateId { get; set; }
        public string Rate { get; set; }
        public string Title { get; set; }
        public string CreatedOn { get; set; }
    }
    public class FeedbackModel
    {
        public string FeedbackId { get; set; }
        public string Feedback { get; set; }
        public string FeedbackBy { get; set; }
        public string CreatedDate { get; set; }
    }
 public class UserMessageModel
    {
        public string MessageId { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
     public string Name { get; set; }
        public string ContactNo { get; set; }
        public string CreatedDate { get; set; }
    }
    public class AuctionModel
    {
        public string AuctionId { get; set; } 
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string DateFrom1 { get; set; }
        public string DateTo1 { get; set; }
        public string Address { get; set; }
        public string Detail { get; set; }
        public string EstimatedRate { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
    }
    public class AuctionBiddingModel
    {
        public string AuctionId { get; set; } 
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Address { get; set; }
        public string Detail { get; set; }
        public string EstimatedRate { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string BidingAmount { get; set; }
        public string BidingId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string BiddingCreatedDate { get; set; }
        public string BiddingContactNo { get; set; }
    }

    class UtilityClass{
        public DataTable RequestDetailType(List<CleintRequestDetailModel1> CleintRequestDetailList)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Date]", typeof(string));
            if (CleintRequestDetailList != null && CleintRequestDetailList.ToList().Count != 0)
            {

                foreach (var items in CleintRequestDetailList)
                    dt.Rows.Add(items.Time, items.Date);
            }
            return dt;
        }
    }
}
