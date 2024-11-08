USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[check_all_organizations_expiry]    Script Date: 8/11/2024 3:52:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[check_all_organizations_expiry]
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @orgId NVARCHAR(50), @reminderDays INT, @item NVARCHAR(100), @expiryDate DATE;
    DECLARE @EMAIL NVARCHAR(300), @CC_EMAIL NVARCHAR(300), @orgName NVARCHAR(200);
    DECLARE @EMAIL_TITLE NVARCHAR(100), @content NVARCHAR(MAX);

    -- loop through all organizations
    DECLARE org_cursor CURSOR FOR
    SELECT orgId, expiryDateReminder 
    FROM organization;  

    OPEN org_cursor;

    FETCH NEXT FROM org_cursor INTO @orgId, @reminderDays;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @content = 'Greetings,<br>The following items in your inventory are nearing their expiry date:<br><ul>';

        DECLARE item_cursor CURSOR FOR
        SELECT item, expiryDate
        FROM inventory
        WHERE orgId = @orgId
        AND DATEDIFF(day, GETDATE(), expiryDate) <= @reminderDays
        AND expiryDate >= GETDATE(); 

        OPEN item_cursor;

        FETCH NEXT FROM item_cursor INTO @item, @expiryDate;

        -- process each expiring item
        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- set each item to the email content
            SET @content = @content + '<li>' + @item + ' (Expiry Date: ' + CONVERT(VARCHAR, @expiryDate, 23) + ')</li>';

            -- insert into temp_expiry_reminder table (if needed)
            INSERT INTO temp_expiry_reminder (orgId, content, created_on)
            VALUES (@orgId, 'Item ' + @item + ' expiring on ' + CONVERT(VARCHAR, @expiryDate, 23), GETDATE());

            FETCH NEXT FROM item_cursor INTO @item, @expiryDate;
        END;

        CLOSE item_cursor;
        DEALLOCATE item_cursor;

        IF @content LIKE '%<li>%'
        BEGIN
            SET @content = @content + '</ul><br><br>' + 
                           '********************************(DONORCONNECT)********************************<br>' +
                           'This mail is sent by server automatically, please do not reply directly.<br>' +
                           '******************************************************************************<br>';

            SELECT @orgName = orgName, @EMAIL = orgEmail, @CC_EMAIL = picEmail 
            FROM organization 
            WHERE orgId = @orgId;

            SET @EMAIL_TITLE = 'REMINDER: Expiring Items Notification for ' + @orgName + ' - ' + CONVERT(VARCHAR, GETDATE(), 120);

            EXEC msdb.dbo.sp_send_dbmail
                @profile_name = 'Notification',
                @recipients = @EMAIL,
                @copy_recipients = @CC_EMAIL,
                @subject = @EMAIL_TITLE,
                @body = @content,
                @body_format = 'HTML';
        END


        FETCH NEXT FROM org_cursor INTO @orgId, @reminderDays;
    END;

    CLOSE org_cursor;
    DEALLOCATE org_cursor;
END
