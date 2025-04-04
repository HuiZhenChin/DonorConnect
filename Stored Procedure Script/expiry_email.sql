USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[expiry_email]    Script Date: 8/11/2024 3:58:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[expiry_email]
    @reminderDays INT,   
    @orgId NVARCHAR(50)  
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @item NVARCHAR(100), @expiryDate DATE, @content NVARCHAR(MAX);
    DECLARE @EMAIL_TITLE NVARCHAR(100), @EMAIL NVARCHAR(300), @CC_EMAIL NVARCHAR(300), @orgName NVARCHAR(MAX);
    DECLARE @historyContent NVARCHAR(MAX);

    SET @content = 'Greetings,<br>' +
                   'The following items in your inventory are nearing their expiry date:<br>' +
                   '<ul>';


    DECLARE item_cursor CURSOR FOR
    SELECT item, expiryDate
    FROM inventory
    WHERE orgId = @orgId
    AND DATEDIFF(day, GETDATE(), expiryDate) <= @reminderDays
    AND expiryDate >= GETDATE();  

    OPEN item_cursor;

    FETCH NEXT FROM item_cursor INTO @item, @expiryDate;


    WHILE @@FETCH_STATUS = 0
    BEGIN
    
        SET @content = @content + '<li>' + @item + ' (Expiry Date: ' + CONVERT(VARCHAR, @expiryDate, 23) + ')</li>';
        

        SET @historyContent = 'Item ' + @item + ' expiring on ' + CONVERT(VARCHAR, @expiryDate, 23);


        INSERT INTO inventory_history (content, orgId, created_on)
        VALUES (@historyContent, @orgId, GETDATE());

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
    END
    ELSE
    BEGIN
     
        PRINT 'No items found that are expiring within the reminder period for orgId ' + @orgId;
    END
END
