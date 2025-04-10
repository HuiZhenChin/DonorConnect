USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[expiry_date_reminder]    Script Date: 8/11/2024 3:57:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[expiry_date_reminder]
    @reminderDays INT,  
    @orgId NVARCHAR(50) 
AS
BEGIN
    SET NOCOUNT ON;

    -- Variables
    DECLARE @item NVARCHAR(100), @expiryDate DATE, @content NVARCHAR(MAX);
    DECLARE @EMAIL_TITLE NVARCHAR(100), @EMAIL NVARCHAR(300), @CC_EMAIL NVARCHAR(300), @orgName NVARCHAR(MAX);
    DECLARE @historyContent NVARCHAR(MAX);

    BEGIN TRY
        -- Initialize email content
        SET @content = 'Greetings,<br>' +
                       'The following items in your inventory are nearing their expiry date:<br>' +
                       '<ul>';

        -- Cursor to loop through expiring items for this organization
        DECLARE item_cursor CURSOR FOR
        SELECT item, expiryDate
        FROM inventory
        WHERE orgId = @orgId
        AND DATEDIFF(day, GETDATE(), expiryDate) = @reminderDays;

        OPEN item_cursor;

        FETCH NEXT FROM item_cursor INTO @item, @expiryDate;

        -- Process each expiring item for the organization
        IF @@FETCH_STATUS = 0
        BEGIN
            RAISERROR('No expiring items found for orgId: %s', 0, 1, @orgId) WITH NOWAIT;
        END

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Add each item to the email content
            SET @content = @content + '<li>' + @item + ' (Expiry Date: ' + CONVERT(VARCHAR, @expiryDate, 23) + ')</li>';
            
            -- Prepare content for inventory_history
            SET @historyContent = 'Item ' + @item + ' expiring on ' + CONVERT(VARCHAR, @expiryDate, 23);

            -- Insert into inventory_history
            INSERT INTO inventory_history (content, orgId, created_on)
            VALUES (@historyContent, @orgId, GETDATE());

            RAISERROR('Inserted into inventory_history: %s', 0, 1, @historyContent) WITH NOWAIT;

            FETCH NEXT FROM item_cursor INTO @item, @expiryDate;
        END;

        CLOSE item_cursor;
        DEALLOCATE item_cursor;

        IF @content LIKE '%<li>%'
        BEGIN
            SET @content = @content + '</ul>' +
                           '<br><br>' +	  
                           '********************************(DONORCONNECT)********************************<br>' +
                           'This mail is sent by server automatically, please do not reply directly.<br>' +
                           '******************************************************************************<br>';

            SELECT 
                @orgName = orgName,
                @EMAIL = orgEmail,
                @CC_EMAIL = picEmail  
            FROM organization 
            WHERE orgId = @orgId;

            SET @EMAIL_TITLE = 'REMINDER: Expiring Items Notification for ' + @orgName + ' - ' + CONVERT(varchar(20), GETDATE(), 120);

            EXEC msdb.dbo.sp_send_dbmail
                @profile_name = 'Notification',
                @recipients = @EMAIL,
                @copy_recipients = @CC_EMAIL,
                @subject = @EMAIL_TITLE,
                @body = @content,
                @body_format = 'HTML';

            RAISERROR('Email sent to orgId: %s', 0, 1, @orgId) WITH NOWAIT;
        END
        ELSE
        BEGIN
            RAISERROR('No expiring items to send email for orgId: %s', 0, 1, @orgId) WITH NOWAIT;
        END
    END TRY
    BEGIN CATCH
   
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR('Error occurred: %s', 16, 1, @ErrorMessage) WITH NOWAIT;
    END CATCH
END
