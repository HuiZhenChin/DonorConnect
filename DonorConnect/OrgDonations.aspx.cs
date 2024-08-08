﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace DonorConnect
{
    public partial class PublishDonations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        private void BindGridView(string status = "All", string urgency = "All")
        {
            QRY _Qry = new QRY();
            DataTable _dt = new DataTable();
            string username = Session["username"].ToString();
            string id = GetOrgId(username);
            string strSQL = @"SELECT donationPublishId, urgentStatus, title, peopleNeeded, description, restriction, itemCategory, 
                             specificItemsForCategory, specificQtyForCategory, address, donationState, created_on, status, donationImage, donationAttch
                      FROM [donation_publish] 
                      WHERE orgId = '" + id + "'";

            if (status != "All")
            {
                strSQL += " AND status = '" + status + "'";
            }

            if (urgency != "All")
            {
                strSQL += " AND urgentStatus = '" + urgency + "'";
            }

            _dt = _Qry.GetData(strSQL);
            if (_dt.Rows.Count > 0)
            {
                DataTable processedTable = new DataTable();
                processedTable.Columns.Add("donationPublishId");
                processedTable.Columns.Add("urgentStatus");
                processedTable.Columns.Add("title");
                processedTable.Columns.Add("peopleNeeded");
                processedTable.Columns.Add("description");
                processedTable.Columns.Add("restriction");
                processedTable.Columns.Add("address");
                processedTable.Columns.Add("donationState");
                processedTable.Columns.Add("created_on");
                processedTable.Columns.Add("status");
                processedTable.Columns.Add("itemDetails");
                processedTable.Columns.Add("donationImages");
                processedTable.Columns.Add("donationFiles");

                foreach (DataRow row in _dt.Rows)
                {
                    string[] itemCategories = row["itemCategory"].ToString().Trim('[', ']').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] specificItems = SplitItems(row["specificItemsForCategory"].ToString().Trim('[', ']'));
                    string[] specificQty = SplitItems(row["specificQtyForCategory"].ToString().Trim('[', ']'));

                    StringBuilder itemDetailsBuilder = new StringBuilder();

                    for (int i = 0; i < itemCategories.Length; i++)
                    {
                        itemDetailsBuilder.Append("Item Category " + (i + 1) + " (" + itemCategories[i].Trim() + "):<br />");

                        if (i < specificItems.Length && !string.IsNullOrWhiteSpace(specificItems[i]))
                        {
                            itemDetailsBuilder.Append("Specific Items Needed: " + specificItems[i].Trim('(', ')') + "<br />");
                        }
                        else
                        {
                            itemDetailsBuilder.Append("Specific Items Needed: Any<br />");
                        }

                        if (i < specificQty.Length && !string.IsNullOrWhiteSpace(specificQty[i]))
                        {
                            itemDetailsBuilder.Append("Specific Quantity Needed: " + specificQty[i].Trim('(', ')') + "<br />");
                        }
                        else
                        {
                            itemDetailsBuilder.Append("Specific Quantity Needed: Not stated<br />");
                        }

                        itemDetailsBuilder.Append("<br />");
                    }

                    DataRow newRow = processedTable.NewRow();
                    newRow["donationPublishId"] = row["donationPublishId"];
                    newRow["urgentStatus"] = row["urgentStatus"];
                    newRow["title"] = row["title"];
                    newRow["peopleNeeded"] = row["peopleNeeded"];
                    newRow["description"] = row["description"];
                    newRow["restriction"] = row["restriction"];
                    newRow["address"] = row["address"];
                    newRow["donationState"] = row["donationState"];
                    newRow["created_on"] = row["created_on"];
                    newRow["status"] = row["status"];
                    newRow["itemDetails"] = itemDetailsBuilder.ToString();
                    newRow["donationImages"] = ProcessImages(row["donationImage"].ToString());
                    newRow["donationFiles"] = ProcessFiles(row["donationAttch"].ToString()); 


                    processedTable.Rows.Add(newRow);
                }

                gvDonations.DataSource = processedTable;
                gvDonations.DataBind();
                lblNoResults.Visible = false;
            }
            else
            {
                gvDonations.DataSource = null;
                gvDonations.DataBind();
                lblNoResults.Visible = true;
            }
        }

        private string[] SplitItems(string input)
        {
            var items = new List<string>();
            var sb = new StringBuilder();
            int openBracketCount = 0;

            foreach (char c in input)
            {
                if (c == ',' && openBracketCount == 0)
                {
                    items.Add(sb.ToString().Trim());
                    sb.Clear();
                }
                else
                {
                    if (c == '(')
                        openBracketCount++;
                    if (c == ')')
                        openBracketCount--;

                    sb.Append(c);
                }
            }

            if (sb.Length > 0)
                items.Add(sb.ToString().Trim());

            return items.ToArray();
        }

        private string ProcessImages(string base64Images)
        {
            if (string.IsNullOrEmpty(base64Images))
            {
                return string.Empty;
            }

            string[] base64ImageArray = base64Images.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder imagesBuilder = new StringBuilder();
            imagesBuilder.AppendLine("<div class='image-grid'>");

            foreach (string base64Image in base64ImageArray)
            {
                imagesBuilder.AppendLine($"<div class='image-item'><img src='data:image/png;base64,{base64Image}' alt='Image' class='img-fluid' /></div>");
            }

            imagesBuilder.AppendLine("</div>");
            return imagesBuilder.ToString();
        }


        private string ProcessFiles(string base64Files)
        {
            if (string.IsNullOrEmpty(base64Files))
            {
                return string.Empty;
            }

            string[] base64FileArray = base64Files.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder filesBuilder = new StringBuilder();

            foreach (string base64File in base64FileArray)
            {
                string[] fileParts = base64File.Split(new char[] { ':' }, 2);
                if (fileParts.Length == 2)
                {
                    string fileName = fileParts[0]; // get original filename
                    string base64Content = fileParts[1];
                    string fileExtension = fileName.Split('.').Last().ToLower();  // get file extension
                    string fileDataUrl = $"data:application/{fileExtension};base64,{base64Content}"; // get url to download from browser
                    filesBuilder.AppendLine($"<a href='{fileDataUrl}' download='{fileName}'>Download {fileName}</a><br />");
                }
            }

            return filesBuilder.ToString();
        }



        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus= ddlStatus.SelectedValue;
            string selectedUrgency = ddlUrgency.SelectedValue;
            BindGridView(selectedStatus, selectedUrgency);
        }

        protected void ddlUrgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus = ddlStatus.SelectedValue;
            string selectedUrgency = ddlUrgency.SelectedValue;
            BindGridView(selectedStatus, selectedUrgency);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string selectedStatus = ddlStatus.SelectedValue;
            string selectedUrgency = ddlUrgency.SelectedValue;
            BindGridView(selectedStatus, selectedUrgency);
        }

        //protected void btnFilter_Click(object sender, EventArgs e)
        //{
        //    string selectedStatus = ddlStatus.SelectedValue;
        //    BindGridView(selectedStatus);
        //}

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string donationPublishId = btn.CommandArgument;

           
            string deleteQuery = $"DELETE FROM [donation_publish] WHERE donationPublishId = @donationPublishId";
            // Execute the delete query with the donationPublishId parameter

            BindGridView(); 
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string donationPublishId = btn.CommandArgument;
            string username = Session["username"].ToString();
            string orgId = GetOrgId(username);

            // redirect to edit page 
            Response.Redirect($"EditOrgDonations.aspx?donationPublishId={donationPublishId}&orgId={orgId}");
        }


        protected void gvDonations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Customize row data if needed
            }
        }

        protected void btnCreateDonation_Click(object sender, EventArgs e)
        {
            // redirect to a page to create a new donation
            Response.Redirect("~/PublishDonations.aspx");
        }

        private string GetOrgId(string username)
        {
            string sql;
            string id = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [organization] WHERE orgName = '" + username + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                id = _dt.Rows[0]["orgId"].ToString();
            }

            return id;
        }
    }
}