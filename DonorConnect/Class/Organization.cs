using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DonorConnect
{
    public class Organization
    {

        public string _orgName, _orgId, _orgProfilePic, _orgAddress, _orgStatus;

        public Organization(string orgName, string orgId, string orgProfilePic, string orgAddress, string orgStatus)
        {
            _orgName = orgName;
            _orgId = orgId;
            _orgProfilePic = orgProfilePic;
            _orgAddress = orgAddress;
            _orgStatus = orgStatus;
        }

        public string GetOrgId()
        {
            string sql = "SELECT orgId FROM [organization] WHERE orgName = @orgName";
            QRY _Qry = new QRY();
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgName", _orgName }
            };
            DataTable _dt = _Qry.GetData(sql, parameters);

            if (_dt.Rows.Count > 0)
            {
                return _dt.Rows[0]["orgId"].ToString();
            }

            return string.Empty;
        }

        public string GetOrgAddress()
        {
            string sql = "SELECT orgAddress FROM [organization] WHERE orgName = @orgName";
            QRY _Qry = new QRY();
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgName", _orgName }
            };
            DataTable _dt = _Qry.GetData(sql, parameters);

            if (_dt.Rows.Count > 0)
            {
                return _dt.Rows[0]["orgAddress"].ToString();
            }

            return string.Empty;
        }

        public string GetOrgProfilePic()
        {
            string sql;
            string profilepic = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [organization] WHERE orgId = '" + _orgId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                profilepic = _dt.Rows[0]["orgProfilePicBase64"].ToString();
            }

            return profilepic;
        }

        public string GetOrgName()
        {
            string sql;
            string name = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [organization] WHERE orgId = '" + _orgId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                name = _dt.Rows[0]["orgName"].ToString();
            }

            return name;
        }

        public string GetOrgStatus()
        {
            string sql;
            string status = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [organization] WHERE orgId = '" + _orgId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                status = _dt.Rows[0]["orgStatus"].ToString();
            }

            return status;
        }

    }
}