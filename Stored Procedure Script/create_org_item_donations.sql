USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[create_org_item_donations]    Script Date: 8/11/2024 3:56:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[create_org_item_donations] 
	@donationPublishId varchar(500),
	@title varchar(500),
	@peopleNeeded varchar(500),
	@address varchar(MAX), 
	@desc varchar(MAX),
	@itemCategory varchar(500), 
	@specificItems varchar(500)= NULL,
	@specificQty varchar(500)= NULL,
	@timeRange varchar(500),
	@urgentStatus varchar(500),
	@status varchar(500),
	@donationImage varchar(MAX)= NULL, 
	@donationAttch varchar(MAX)= NULL,
	@orgId varchar(500),
	@adminId varchar(500),
	@created_on varchar(500),
	@restriction varchar(500)= NULL,
	@state varchar(500),
	@closureReason varchar(500)= NULL,
	@resubmit varchar(50)= NULL,
	@method varchar(300),
	@name varchar(100),
	@phone varchar(100)
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @MESSAGE VARCHAR(500);

    IF (@method = 'INSERT')
    BEGIN
       
        BEGIN

            -- Generate new item donation publish ID
            SELECT @donationPublishId = 'ID' + CAST(ISNULL(MAX(CAST(SUBSTRING(donationPublishId, 3, LEN(donationPublishId) - 1) AS INT)), 0) + 1 AS varchar)
            FROM donation_publish;

            -- Insert new item donation
            INSERT INTO donation_publish (donationPublishId, title, peopleNeeded, [address], [description], itemCategory, specificItemsForCategory, specificQtyForCategory, timeRange, urgentStatus, [status], donationImage, donationAttch, orgId, adminId, created_on, restriction, donationState, closureReason, resubmit, recipientName, recipientPhoneNumber)
            VALUES (@donationPublishId, @title, @peopleNeeded, @address, @desc, @itemCategory, @specificItems, @specificQty, @timeRange, @urgentStatus, @status, @donationImage, @donationAttch, @orgId, @adminId ,GETDATE(), @restriction, @state, @closureReason, @resubmit, @name, @phone);

           SET @MESSAGE = 'Successful! You have submitted a new item donation application. Your request is now pending for approval.';
        END
        

        SELECT @MESSAGE AS MESSAGE;
    END
END
