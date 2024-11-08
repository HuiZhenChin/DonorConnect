using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDonorsGridView();
                LoadDonors();
            }

            if (IsPostBack)
            {
                string eventTarget = Request["__EVENTTARGET"];
                string eventArgument = Request["__EVENTARGUMENT"];

                if (eventTarget == btnGiveWarning.UniqueID)
                {
                    btnGiveWarning_Click(null, EventArgs.Empty);
                }

                if (eventTarget == btnGiveWarningRider.UniqueID)
                {
                    btnGiveWarningRider_Click(null, EventArgs.Empty);
                }

                if (eventTarget == "TerminateDonor" && !string.IsNullOrEmpty(eventArgument))
                {
                    TerminateDonor(eventArgument);
                }

                if (eventTarget == "TerminateRider" && !string.IsNullOrEmpty(eventArgument))
                {
                    TerminateRider(eventArgument);
                }

            }
        }

        protected void gvDonors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string warningReason = DataBinder.Eval(e.Row.DataItem, "warningReason")?.ToString();

                if (!string.IsNullOrEmpty(warningReason))
                {
                    e.Row.ToolTip = "Warning Reason: " + warningReason;
                }

                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                LinkButton btnTerminateDonor = (LinkButton)e.Row.FindControl("btnTerminateDonor");

                string warningValue = DataBinder.Eval(e.Row.DataItem, "warning")?.ToString();

                // change row color based on the warning value
                if (!string.IsNullOrEmpty(warningValue) && int.TryParse(warningValue, out int warningCount))
                {
                    switch (warningCount)
                    {
                        case 1:
                            e.Row.BackColor = System.Drawing.Color.LightYellow;
                            break;
                        case 2:
                            e.Row.BackColor = System.Drawing.Color.LightSalmon;
                            break;
                        case 3:
                            e.Row.BackColor = System.Drawing.Color.LightCoral;
                            break;
                    }
                }

                // set the status label color
                if (lblStatus != null)
                {
                    string status = lblStatus.Text;
                    if (status == "Active")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (status == "Terminated")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        btnTerminateDonor.Visible = false; 
                    }
                }
            }
        }

        protected void gvAdmin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvAdmin.EditIndex == e.Row.RowIndex)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    foreach (Control control in cell.Controls)
                    {
                        if (control is TextBox txtBox && txtBox.ID != "ddlIsMain")
                        {
                            txtBox.ReadOnly = true;  
                        }
                    }
                }

                DropDownList ddlIsMain = (DropDownList)e.Row.FindControl("ddlIsMain");
                if (ddlIsMain != null)
                {
                    string isMainValue = DataBinder.Eval(e.Row.DataItem, "isMain").ToString();
                    ddlIsMain.SelectedValue = isMainValue.Equals("Yes", StringComparison.OrdinalIgnoreCase) ? "Yes" : "No";
                }
            }

            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            LinkButton btnTerminateAdmin = (LinkButton)e.Row.FindControl("btnTerminateAdmin");

            if (lblStatus != null)
            {
                string status = lblStatus.Text;
                if (status == "Active")
                {
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                }
                else if (status == "Terminated")
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    if (btnTerminateAdmin != null)
                        btnTerminateAdmin.Visible = false;
                }
            }
        }


        protected void gvAdmin_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // set the row to edit mode
            gvAdmin.EditIndex = e.NewEditIndex;
            BindAdminGridView();
        }

        protected void gvAdmin_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // cancel the editing
            gvAdmin.EditIndex = -1;
            BindAdminGridView();
        }

        protected void gvAdmin_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvAdmin.Rows[e.RowIndex];
            DropDownList ddlIsMain = (DropDownList)row.FindControl("ddlIsMain");

            if (ddlIsMain != null)
            {
                string isMainValue = ddlIsMain.SelectedValue;
                string adminId = gvAdmin.DataKeys[e.RowIndex].Value.ToString();  

                QRY _Qry = new QRY();
                string sql = "UPDATE admin SET isMain = @isMain WHERE adminId = @adminId";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@isMain", isMainValue },
                    { "@adminId", adminId }
                };

                _Qry.ExecuteQuery(sql, parameters);
            }

            gvAdmin.EditIndex = -1;
            BindAdminGridView();
        }



        protected void gvOrg_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {            
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                LinkButton btnTerminateOrg = (LinkButton)e.Row.FindControl("btnTerminateOrg");

                if (lblStatus != null)
                {
                    string status = lblStatus.Text;
                    if (status == "Active")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (status == "Terminated")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        btnTerminateOrg.Visible = false;
                    }
                }
            }
        }

        protected void gvRider_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string warningReason = DataBinder.Eval(e.Row.DataItem, "warningReason")?.ToString();

                if (!string.IsNullOrEmpty(warningReason))
                {
                    e.Row.ToolTip = "Warning Reason: " + warningReason;
                }

                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                LinkButton btnTerminateRider = (LinkButton)e.Row.FindControl("btnTerminateRider");

                string warningValue = DataBinder.Eval(e.Row.DataItem, "warning")?.ToString();

                // change row color based on the warning value
                if (!string.IsNullOrEmpty(warningValue) && int.TryParse(warningValue, out int warningCount))
                {
                    switch (warningCount)
                    {
                        case 1:
                            e.Row.BackColor = System.Drawing.Color.LightYellow;
                            break;
                        case 2:
                            e.Row.BackColor = System.Drawing.Color.LightSalmon;
                            break;
                        case 3:
                            e.Row.BackColor = System.Drawing.Color.LightCoral;
                            break;
                    }
                }

                if (lblStatus != null)
                {
                    string status = lblStatus.Text;
                    if (status == "Active")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (status == "Terminated")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        btnTerminateRider.Visible = false;
                    }
                }
            }
        }

        protected void btnShowDonor_click(object sender, EventArgs e)
        {
            BindDonorsGridView();
            LoadDonors();
        }

        protected void btnShowOrg_click(object sender, EventArgs e)
        {
            BindOrgGridView();
        }

        protected void btnShowRider_click(object sender, EventArgs e)
        {
            BindRiderGridView();
            LoadRider();
        }

        protected void btnShowAdmin_click(object sender, EventArgs e)
        {
            BindAdminGridView();
        }

        private void BindDonorsGridView()
        {
          
            string sql = "SELECT donorId, donorUsername, donorName, donorEmail, donorContactNumber, donorAddress1, createdOn, terminateReason, status, warning, warningReason FROM donor";

            QRY _Qry = new QRY();
            DataTable _dt;
            _dt = _Qry.GetData(sql);

            gvDonors.DataSource = _dt;
            gvDonors.DataBind();
            donor.Style["display"] = "block";
            org.Style["display"] = "none";
            rider.Style["display"] = "none";
            admin.Style["display"] = "none";
        }

        private void BindOrgGridView()
        {

            string sql = "SELECT orgId, orgName, orgEmail, orgContactNumber, orgAddress, picName, picEmail, picContactNumber, " +
              "orgDescription, orgRegion, createdOn, orgStatus, terminateReason, adminId FROM organization " +
              "WHERE orgStatus NOT IN ('Pending Approval', 'Rejected')"; 

            QRY _Qry = new QRY();
            DataTable _dt;
            _dt = _Qry.GetData(sql);

            gvOrg.DataSource = _dt;
            gvOrg.DataBind();
            donor.Style["display"] = "none";
            org.Style["display"] = "block";
            rider.Style["display"] = "none";
            admin.Style["display"] = "none";


        }

        private void BindRiderGridView()
        {

            string sql = "SELECT riderId, riderUsername, riderFullName, riderEmail, riderContactNumber, vehicleType, vehiclePlateNumber, registerDate, riderStatus, adminId, terminateReason, warning, warningReason FROM delivery_rider WHERE riderStatus NOT IN('Pending Approval', 'Rejected')"; 


            QRY _Qry = new QRY();
            DataTable _dt;
            _dt = _Qry.GetData(sql);

            gvRider.DataSource = _dt;
            gvRider.DataBind();
            donor.Style["display"] = "none";
            org.Style["display"] = "none";
            rider.Style["display"] = "block";
            admin.Style["display"] = "none";

        }

        private void BindAdminGridView()
        {

            string sql = "SELECT adminId, adminUsername, adminEmail, status, created_on, terminateReason, isMain FROM admin ";
           

            QRY _Qry = new QRY();
            DataTable _dt;
            _dt = _Qry.GetData(sql);

            gvAdmin.DataSource = _dt;
            gvAdmin.DataBind();
            donor.Style["display"] = "none";
            org.Style["display"] = "none";
            rider.Style["display"] = "none";
            admin.Style["display"] = "block";

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                SearchAll(keyword);
            }
            else
            {
                BindDonorsGridView();
                BindOrgGridView();
                BindRiderGridView();
                BindAdminGridView();
            }
        }

        private void SearchAll(string keyword)
        {
            bool donorIsFound = SearchDonor(keyword);
            bool orgIsFound = SearchOrg(keyword);
            bool riderIsFound = SearchRider(keyword);
            bool adminIsFound = SearchAdmin(keyword);

            donor.Style["display"] = donorIsFound ? "block" : "none";
            org.Style["display"] = orgIsFound ? "block" : "none";
            rider.Style["display"] = riderIsFound ? "block" : "none";
            admin.Style["display"] = adminIsFound ? "block" : "none";
        }

        private bool SearchDonor(string keyword)
        {
            string sql = "SELECT donorId, donorUsername, donorName, donorEmail, donorContactNumber, donorAddress1, createdOn, status, terminateReason, warningReason, warning " +
                         "FROM donor WHERE donorUsername LIKE '%" + keyword + "%' OR donorName LIKE '%" + keyword + "%' OR donorEmail LIKE '%" + keyword + "%' OR donorAddress1 LIKE '%" + keyword + "%'";

            QRY _Qry = new QRY();
            DataTable _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                gvDonors.DataSource = _dt;
                gvDonors.DataBind();
                return true; // results found
            }
            return false; // no results
        }

        private bool SearchOrg(string keyword)
        {
            string sql = "SELECT orgId, orgName, orgEmail, orgContactNumber, orgAddress, picName, picEmail, picContactNumber, orgDescription, orgRegion, createdOn, orgStatus, adminId, terminateReason " +
                         "FROM organization WHERE orgName LIKE '%" + keyword + "%' OR orgEmail LIKE '%" + keyword + "%' OR picName LIKE '%" + keyword + "%' OR orgAddress LIKE '%" + keyword + "%'";

            QRY _Qry = new QRY();
            DataTable _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                gvOrg.DataSource = _dt;
                gvOrg.DataBind();
                return true; 
            }
            return false; 
        }

        private bool SearchRider(string keyword)
        {
            string sql = "SELECT riderId, riderUsername, riderFullName, riderEmail, riderContactNumber, vehicleType, vehiclePlateNumber, registerDate, riderStatus, adminId, terminateReason, warningReason, warning " +
                         "FROM delivery_rider WHERE riderUsername LIKE '%" + keyword + "%' OR riderFullName LIKE '%" + keyword + "%' OR riderEmail LIKE '%" + keyword + "%' OR riderContactNumber LIKE '%" + keyword + "%' OR vehiclePlateNumber LIKE '%" + keyword + "%'";

            QRY _Qry = new QRY();
            DataTable _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                gvRider.DataSource = _dt;
                gvRider.DataBind();
                return true; 
            }
            return false; 
        }

        private bool SearchAdmin(string keyword)
        {
            string sql = "SELECT adminId, adminUsername, adminEmail, status, created_on, isMain, terminateReason " +
                         "FROM admin WHERE adminUsername LIKE '%" + keyword + "%' OR adminEmail LIKE '%" + keyword + "%'";

            QRY _Qry = new QRY();
            DataTable _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                gvAdmin.DataSource = _dt;
                gvAdmin.DataBind();
                return true; 
            }
            return false; 
        }

        protected void btnTerminateDonor_Click(object sender, EventArgs e)
        {
            string adminUsername = Session["username"]?.ToString();

            // check if the current admin has permission to terminate donors
            string sqlCheckAdmin = "SELECT isMain FROM admin WHERE adminUsername = @adminUsername";
            Dictionary<string, object> checkParams = new Dictionary<string, object>
            {
                { "@adminUsername", adminUsername }
            };

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sqlCheckAdmin, checkParams);

            // if the admin's isMain value is "No", show error and prevent termination
            if (dt.Rows.Count > 0 && dt.Rows[0]["isMain"].ToString() == "No")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('You do not have permission to terminate the user. Only super admins can do this.');", true);
                return;
            }

            // if the check passes, proceed with termination
            string donorId = hfDonorId.Value.ToString();
            string terminationReason = ddlReason.SelectedValue == "Others" ? txtOtherReason.Text : ddlReason.SelectedItem.Text;

            string sql = "UPDATE donor SET status = @status, terminateReason = @reason WHERE donorId = @donorId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@donorId", donorId },
                { "@status", "Terminated" },
                { "@reason", terminationReason }
            };

            bool success = _Qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                string sqlemail = "EXEC [application_email] " +
                                  "@action = 'DONOR TERMINATED', " +
                                  "@reason = @reason, " +
                                  "@role = 'donor', " +
                                  "@donorId = @donorId";

                var emailParameter = new Dictionary<string, object>
                {
                    { "@reason", terminationReason },
                    { "@donorId", donorId }
                };

                bool success2 = _Qry.ExecuteNonQuery(sqlemail, emailParameter);

                if (success2)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Successful! An email is sent to inform donor about this.');", true);
                    BindDonorsGridView();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Error is found. Please try again!');", true);
            }
        }


        protected void ddlReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReason.SelectedValue == "Others")
            {
                txtOtherReason.Visible = true;
            }
            else
            {
                txtOtherReason.Visible = false;
            }
        }

        protected void btnViewDonor_Click(object sender, EventArgs e)
        {
            LinkButton btnViewDonor = (LinkButton)sender;

            string[] commandArgs = btnViewDonor.CommandArgument.Split('|');
            string donorId = commandArgs[0];
            string donorUsername = commandArgs[1];

            Response.Redirect("PreviewPublicInfo.aspx?role=donor&username=" + donorUsername);
        }

        protected void btnTerminateOrg_Click(object sender, EventArgs e)
        {
            string adminUsername = Session["username"]?.ToString();

            string sqlCheckAdmin = "SELECT isMain FROM admin WHERE adminUsername = @adminUsername";
            Dictionary<string, object> checkParams = new Dictionary<string, object>
            {
                { "@adminUsername", adminUsername }
            };

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sqlCheckAdmin, checkParams);

            if (dt.Rows.Count > 0 && dt.Rows[0]["isMain"].ToString() == "No")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "showError('You do not have permission to terminate this organization. Only super admins can do this.');", true);
                return;
            }

            string orgId = hfOrgId.Value.ToString();
            string terminationReason = ddlReasonOrg.SelectedValue == "Others" ? txtOtherReasonOrg.Text : ddlReasonOrg.SelectedItem.Text;

            string sql = "UPDATE organization SET orgStatus = @status, terminateReason = @reason WHERE orgId = @orgId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId },
                { "@status", "Terminated" },
                { "@reason", terminationReason }
            };

            bool success = _Qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                // send termination email notification
                string sqlemail = "EXEC [application_email] " +
                                  "@action = 'ORGANIZATION TERMINATED', " +
                                  "@reason = @reason, " +
                                  "@role = 'organization', " +
                                  "@orgId = @orgId";

                var emailParameter = new Dictionary<string, object>
                {
                    { "@reason", terminationReason },
                    { "@orgId", orgId }
                };

                _Qry.ExecuteNonQuery(sqlemail, emailParameter);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "showSuccess('Successful! An email is sent to inform the organization about this.');", true);
                BindOrgGridView();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "showError('An error occurred. Please try again!');", true);
            }
        }


        protected void btnViewOrg_Click(object sender, EventArgs e)
        {
            LinkButton btnViewOrg = (LinkButton)sender;

            string[] commandArgs = btnViewOrg.CommandArgument.Split('|');
            string orgId = commandArgs[0];
            string orgName = commandArgs[1];

            Response.Redirect("PreviewPublicInfo.aspx?role=organization&orgName=" + orgName);
        }

        protected void btnViewRider_Click(object sender, EventArgs e)
        {
            LinkButton btnViewRider = (LinkButton)sender;

            string[] commandArgs = btnViewRider.CommandArgument.Split('|');
            string riderId = commandArgs[0];
            string riderFullName = commandArgs[1];

            Response.Redirect("PreviewPublicInfo.aspx?role=rider&username=" + riderFullName);
        }

        protected void btnTerminateRider_Click(object sender, EventArgs e)
        {
            string adminUsername = Session["username"]?.ToString();

            string sqlCheckAdmin = "SELECT isMain FROM admin WHERE adminUsername = @adminUsername";
            Dictionary<string, object> checkParams = new Dictionary<string, object>
            {
                { "@adminUsername", adminUsername }
            };

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sqlCheckAdmin, checkParams);

            if (dt.Rows.Count > 0 && dt.Rows[0]["isMain"].ToString() == "No")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "showError('You do not have permission to terminate this rider. Only super admins can do this.');", true);
                return;
            }

            string riderId = hfRiderId.Value.ToString();
            string terminationReason = ddlReasonRider.SelectedValue == "Others" ? txtOtherReasonRider.Text : ddlReasonRider.SelectedItem.Text;

            string sql = "UPDATE delivery_rider SET riderStatus = @status, terminateReason = @reason WHERE riderId = @riderId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@riderId", riderId },
                { "@status", "Terminated" },
                { "@reason", terminationReason }
            };

            bool success = _Qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                // send termination email notification
                string sqlemail = "EXEC [application_email] " +
                                  "@action = 'RIDER TERMINATED', " +
                                  "@reason = @reason, " +
                                  "@role = 'rider', " +
                                  "@riderId = @riderId";

                var emailParameter = new Dictionary<string, object>
                {
                    { "@reason", terminationReason },
                    { "@riderId", riderId }
                };

                _Qry.ExecuteNonQuery(sqlemail, emailParameter);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "showSuccess('Successful! An email is sent to inform the delivery rider about this.');", true);
                BindRiderGridView();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "showError('An error occurred. Please try again!');", true);
            }
        }


        private void TerminateDonor(string donorId)
        {
            string terminationReason = "Already 3 warnings provided.";

            string sql = "UPDATE donor SET status = @status, terminateReason = @reason WHERE donorId = @donorId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@donorId", donorId },
                { "@status", "Terminated" },
                { "@reason", terminationReason }
            };

            QRY _Qry = new QRY();

            bool success = _Qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                string sqlemail = "EXEC [application_email] " +
                                 "@action = 'DONOR TERMINATED', " +
                                 "@reason = @reason, " +
                                 "@role = 'donor', " +
                                 "@donorId = @donorId ";

                var emailParameter = new Dictionary<string, object>
                {
                    { "@reason", terminationReason },
                    { "@donorId", donorId },

                };

                bool success2 = _Qry.ExecuteNonQuery(sqlemail, emailParameter);

                if (success2)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showSuccess('Successful! An email is sent to inform donor about this.');", true);
                    BindDonorsGridView();
                    
                }


            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showError('Error is found. Please try again!');", true);
            }
        }

        private void TerminateRider(string riderId)
        {
            string terminationReason = "Already 3 warnings provided.";

            string sql = "UPDATE delivery_rider SET riderStatus = @status, terminateReason = @reason WHERE donorId = @donorId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@donorId", riderId },
                { "@status", "Terminated" },
                { "@reason", terminationReason }
            };

            QRY _Qry = new QRY();

            bool success = _Qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                string sqlemail = "EXEC [application_email] " +
                                 "@action = 'RIDER TERMINATED', " +
                                 "@reason = @reason, " +
                                 "@role = 'rider', " +
                                 "@riderId = @riderId ";

                var emailParameter = new Dictionary<string, object>
                {
                    { "@reason", terminationReason },
                    { "@riderId", riderId },

                };

                bool success2 = _Qry.ExecuteNonQuery(sqlemail, emailParameter);

                if (success2)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showSuccess('Successful! An email is sent to inform rider about this.');", true);
                    BindRiderGridView();

                }


            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showError('Error is found. Please try again!');", true);
            }
        }
        protected void btnTerminateAdmin_Click(object sender, EventArgs e)
        {
            string adminUsername = Session["username"]?.ToString();

            string sqlCheckAdmin = "SELECT isMain FROM admin WHERE adminUsername = @adminUsername";
            Dictionary<string, object> checkParams = new Dictionary<string, object>
            {
                { "@adminUsername", adminUsername }
            };

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sqlCheckAdmin, checkParams);

            if (dt.Rows.Count > 0 && dt.Rows[0]["isMain"].ToString() == "No")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "showError('You do not have permission to terminate another admin. Only super admins can do this.');", true);
                return;
            }

            string adminId = hfAdminId.Value.ToString();
            string terminationReason = ddlReasonAdmin.SelectedValue == "Others" ? txtOtherReasonAdmin.Text : ddlReasonAdmin.SelectedItem.Text;

            string sql = "UPDATE admin SET status = @status, terminateReason = @reason WHERE adminId = @adminId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@adminId", adminId },
                { "@status", "Terminated" },
                { "@reason", terminationReason }
            };

            bool success = _Qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                // send termination email notification
                string sqlemail = "EXEC [application_email] " +
                                  "@action = 'ADMIN TERMINATED', " +
                                  "@reason = @reason, " +
                                  "@role = 'admin', " +
                                  "@adminId = @adminId";

                var emailParameter = new Dictionary<string, object>
                {
                    { "@reason", terminationReason },
                    { "@adminId", adminId }
                };

                _Qry.ExecuteNonQuery(sqlemail, emailParameter);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "showSuccess('Successful! An email is sent to inform about this.');", true);
                BindAdminGridView();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "showError('An error occurred. Please try again!');", true);
            }
        }

        [WebMethod]
        public static string GetBusinessLicense(string orgId)
        {
            string licenseDetail = "";

            string sql = "SELECT businessLicenseImageBase64 FROM organization WHERE orgId = @orgId";

            var parameter = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            QRY _Qry = new QRY();

            DataTable dataTable = _Qry.GetData(sql, parameter);

            if (dataTable.Rows.Count > 0)
            {

                string encryptedImage = dataTable.Rows[0]["businessLicenseImageBase64"].ToString();
             
                licenseDetail = ImageFileProcessing.ProcessImages(encryptedImage);
            }

            return licenseDetail;
        }

        [WebMethod]
        public static string GetDrivingLicense(string riderId)
        {
            string licenseDetail = "";

            string sql = "SELECT drivingLicenseImageBase64 FROM delivery_rider WHERE riderId = @riderId";

            var parameter = new Dictionary<string, object>
            {
                { "@riderId", riderId }
            };

            QRY _Qry = new QRY();

            DataTable dataTable = _Qry.GetData(sql, parameter);

            if (dataTable.Rows.Count > 0)
            {

                string encryptedImage = dataTable.Rows[0]["drivingLicenseImageBase64"].ToString();

                licenseDetail = ImageFileProcessing.ProcessImages(encryptedImage);
            }

            return licenseDetail;
        }

        private void LoadDonors()
        {
            string sql = "SELECT DISTINCT donorId, donorUsername FROM donor WHERE status <> 'Terminated'";

            QRY _Qry = new QRY();
            DataTable donorsTable = _Qry.GetData(sql);

            ddlDonors.Items.Clear();
            ddlDonors.DataSource = donorsTable;
            ddlDonors.DataTextField = "donorUsername";
            ddlDonors.DataValueField = "donorId";
            ddlDonors.DataBind();

           
        }

        private void LoadRider()
        {
            string sql = "SELECT DISTINCT riderId, riderUsername FROM delivery_rider WHERE riderStatus <> 'Terminated'";
            QRY _Qry = new QRY();
            DataTable rider = _Qry.GetData(sql);

            ddlDonors.Items.Clear();
            ddlRiders.DataSource = rider;
            ddlRiders.DataTextField = "riderUsername";
            ddlRiders.DataValueField = "riderId";
            ddlRiders.DataBind();


        }

        protected void btnGiveWarning_Click(object sender, EventArgs e)
        {
            string donorId = ddlDonors.SelectedValue;
            string warningReason = txtWarningReason.Text.Trim(); 

            if (string.IsNullOrEmpty(donorId))
            {
                lblWarningStatus.Text = "Please select a donor.";
                lblWarningStatus.CssClass = "text-danger";
                lblWarningStatus.Style["display"] = "block";
                return;
            }

            string sql = "SELECT warning FROM donor WHERE donorId = @donorId";
            QRY _Qry = new QRY();
            var parameters = new Dictionary<string, object> { { "@donorId", donorId } };
            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                string warningStatus = dt.Rows[0]["warning"].ToString();

                int currentWarningCount = 0;
                int.TryParse(warningStatus, out currentWarningCount);

                if (currentWarningCount >= 3)
                {
                    // show confirmation message that the account will be terminated
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                        "Swal.fire({ " +
                        "title: 'Terminate Account?', " +
                        "text: 'This donor has reached the maximum number of warnings. Do you want to terminate the account?', " +
                        "icon: 'warning', " +
                        "showCancelButton: true, " +
                        "confirmButtonColor: '#3085d6', " +
                        "cancelButtonColor: '#d33', " +
                        "confirmButtonText: 'Yes, terminate' " +
                        "}).then((result) => { " +
                        "if (result.isConfirmed) { " +
                        "__doPostBack('TerminateDonor', '" + donorId + "'); " +
                        "} " +
                        "});", true);
                    return;
                }
                else
                {
                    // increment the warning count
                    currentWarningCount++;
                    string updateSql = "UPDATE donor SET warning = @warningCount, warningReason = @reason WHERE donorId = @donorId";
                    var updateParameters = new Dictionary<string, object>
                    {
                        { "@warningCount", currentWarningCount },
                        { "@reason", warningReason },
                        { "@donorId", donorId }
                    };

                    bool success = _Qry.ExecuteNonQuery(updateSql, updateParameters);

                    if (success)
                    {
                        string sqlemail = "EXEC [application_email] " +
                                "@action = 'DONOR WARNING', " +
                                "@reason = @reason, " +
                                "@role = 'donor', " +
                                "@donorId = @donorId, " +
                                "@warningCount = @count ";

                        var emailParameter = new Dictionary<string, object>
                        {
                            { "@reason", warningReason },
                            { "@donorId", donorId },
                            { "@count", currentWarningCount }

                        };

                        bool success2 = _Qry.ExecuteNonQuery(sqlemail, emailParameter);

                        if (success2)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                           $"showSuccess('Successful! Warning is given. Current warning count: {currentWarningCount}');", true);
                            BindDonorsGridView();
                            gvDonors.DataBind();
                        }
                        
                       
                    }
                    else
                    {
                        // show error message
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                            $"showError('There is an error giving the warning. Please try again!');", true);
                    }
                }
            }
        }

        protected void btnGiveWarningRider_Click(object sender, EventArgs e)
        {
            string riderId = ddlRiders.SelectedValue;
            string warningReason = txtWarningReasonRider.Text.Trim(); 

            if (string.IsNullOrEmpty(riderId))
            {
                lblWarningStatus.Text = "Please select a rider.";
                lblWarningStatus.CssClass = "text-danger";
                lblWarningStatus.Style["display"] = "block";
                return;
            }

            string sql = "SELECT warning FROM delivery_rider WHERE riderId = @riderId";
            QRY _Qry = new QRY();
            var parameters = new Dictionary<string, object> { { "@riderId", riderId } };
            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                string warningStatus = dt.Rows[0]["warning"].ToString();

                int currentWarningCount = 0;
                int.TryParse(warningStatus, out currentWarningCount);

                if (currentWarningCount >= 3)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                        "Swal.fire({ " +
                        "title: 'Terminate Account?', " +
                        "text: 'This rider has reached the maximum number of warnings. Do you want to terminate the account?', " +
                        "icon: 'warning', " +
                        "showCancelButton: true, " +
                        "confirmButtonColor: '#3085d6', " +
                        "cancelButtonColor: '#d33', " +
                        "confirmButtonText: 'Yes, terminate' " +
                        "}).then((result) => { " +
                        "if (result.isConfirmed) { " +
                        "__doPostBack('TerminateRider', '" + riderId + "'); " +
                        "} " +
                        "});", true);
                    return;
                }
                else
                {
                    // increment the warning count
                    currentWarningCount++;
                    string updateSql = "UPDATE delivery_rider SET warning = @warningCount, warningReason = @reason WHERE riderId = @riderId";
                    var updateParameters = new Dictionary<string, object>
                    {
                        { "@warningCount", currentWarningCount },
                        { "@reason", warningReason },
                        { "@riderId", riderId }
                    };

                    bool success = _Qry.ExecuteNonQuery(updateSql, updateParameters);

                    if (success)
                    {
                        string sqlemail = "EXEC [application_email] " +
                                "@action = 'RIDER WARNING', " +
                                "@reason = @reason, " +
                                "@role = 'rider', " +
                                "@riderId = @riderId, " +
                                "@warningCount = @count ";

                        var emailParameter = new Dictionary<string, object>
                        {
                            { "@reason", warningReason },
                            { "@riderId", riderId },
                            { "@count", currentWarningCount }

                        };

                        bool success2 = _Qry.ExecuteNonQuery(sqlemail, emailParameter);

                        if (success2)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                           $"showSuccess('Successful! Warning is given. Current warning count: {currentWarningCount}');", true);
                            BindRiderGridView();
                            gvRider.DataBind();
                        }


                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                            $"showError('There is an error giving the warning. Please try again!');", true);
                    }
                }
            }
        }


    }
}