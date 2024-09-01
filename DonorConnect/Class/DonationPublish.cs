using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DonorConnect
{
    public class DonationPublish
    {
        private string _donationPublishId, _title, _status, _urgency, _orgId, _createdOn, _address;

        public DonationPublish(string donationPublishId, string title, string status, string urgency, string orgId, string createdOn, string address)
        {
            _donationPublishId = donationPublishId;
            _title = title;
            _status = status;
            _urgency = urgency;
            _orgId = orgId;
            _createdOn = createdOn;
            _address = address;
        }

        public string GetStatus()
        {
            string sql;
            string status = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [donation_publish] WHERE donationPublishId = '" + _donationPublishId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                status = _dt.Rows[0]["status"].ToString();
            }

            return status;
        }

        public string GetUrgency()
        {
            string sql;
            string urgency = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [donation_publish] WHERE donationPublishId = '" + _donationPublishId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                urgency = _dt.Rows[0]["urgentStatus"].ToString();
            }

            return urgency;
        }

        public string GetId()
        {
            string sql;
            string id = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [donation_publish] WHERE donationPublishId = '" + _donationPublishId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                id = _dt.Rows[0]["orgId"].ToString();
            }

            return id;
        }

        public string GetCreatedOn()
        {
            string sql;
            string created_on = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [donation_publish] WHERE donationPublishId = '" + _donationPublishId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                created_on = _dt.Rows[0]["created_on"].ToString();
            }

            return created_on;
        }

        private string GetAddress()
        {
            string sql;
            string address = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [donation_publish] WHERE donationPublishId = '" + _donationPublishId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                address = _dt.Rows[0]["orgAddress"].ToString();
            }

            return address;
        }
    }
}