USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[admin_reminder_email]    Script Date: 8/11/2024 3:51:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[admin_reminder_email]
    @action varchar(max) = NULL,
    @orgName varchar(max) = NULL,
    @reason varchar(max) = NULL,
    @username varchar(500) = NULL
AS
BEGIN 
    SET NOCOUNT ON;

    DECLARE 
        @EMAIL_TITLE varchar(100) = NULL,
        @CONTENT varchar(max) = NULL,
        @EMAIL varchar(300) = 'mailserver8877@gmail.com',
        @EMAILLIST varchar(max) = NULL,
        @CC_EMAILS varchar(max) = NULL;

    SELECT @CC_EMAILS = STRING_AGG(adminEmail, ';')
    FROM [admin]
    WHERE [status] = 'Active';

    IF (@action = 'NEW NORMAL APPLICATION')
    BEGIN
        SET @EMAIL_TITLE = 'NEW Item Donation Application from - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);
        SET @CONTENT = 'Greetings admin,<br>' +
                       'Here is an item donation application from ' + @orgName + '<br>' +
                       'Please login to your dashboard to view the application.<br>' +
                       '<br><br>' +
                       '********************************(DONORCONNECT)********************************<br>' +
                       'This mail is sent by server automatically, please do not reply directly.<br>' +
                       '******************************************************************************<br>';

        EXEC msdb.dbo.sp_send_dbmail
            @profile_name = 'Notification',
            @recipients = @EMAIL,
            @copy_recipients = @CC_EMAILS,  
            @subject = @EMAIL_TITLE,
            @body = @CONTENT,
            @body_format = 'HTML';
    END

    ELSE IF (@action = 'NEW URGENT APPLICATION')
    BEGIN
        SET @EMAIL_TITLE = 'IMPORTANT! NEW URGENT Item Donation Application from - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);
        SET @CONTENT = 'Greetings admin,<br>' +
                       'Here is an urgent item donation application from ' + @orgName + '<br>' +
                       'Please login to your dashboard to view the application.<br>' +
                       '<br><br>' +
                       '********************************(DONORCONNECT)********************************<br>' +
                       'This mail is sent by server automatically, please do not reply directly.<br>' +
                       '******************************************************************************<br>';

        EXEC msdb.dbo.sp_send_dbmail
            @profile_name = 'Notification',
            @recipients = @EMAIL,
			@copy_recipients = @CC_EMAILS, 
            @subject = @EMAIL_TITLE,
            @body = @CONTENT,
            @body_format = 'HTML';
    END

	ELSE IF (@action = 'REGISTRATION APPLICATION')
    BEGIN
        SET @EMAIL_TITLE = 'NEW Registration Application from - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);
        SET @CONTENT = 'Greetings admin,<br>' +
                       'Here is a registration application from ' + @orgName + '<br>' +
                       'Please login to your dashboard to view the application.<br>' +
                       '<br><br>' +
                       '********************************(DONORCONNECT)********************************<br>' +
                       'This mail is sent by server automatically, please do not reply directly.<br>' +
                       '******************************************************************************<br>';

        EXEC msdb.dbo.sp_send_dbmail
            @profile_name = 'Notification',
            @recipients = @EMAIL,
            @copy_recipients = @CC_EMAILS,  
            @subject = @EMAIL_TITLE,
            @body = @CONTENT,
            @body_format = 'HTML';
    END

	ELSE IF (@action = 'REGISTRATION APPLICATION RIDER')
    BEGIN
        SET @EMAIL_TITLE = 'NEW Delivery Rider Registration Application from - ' + @username + ' ' + CONVERT(varchar(20), GETDATE(), 120);
        SET @CONTENT = 'Greetings admin,<br>' +
                       'Here is a delivery rider registration application from ' + @username + '<br>' +
                       'Please login to your dashboard to view the application.<br>' +
                       '<br><br>' +
                       '********************************(DONORCONNECT)********************************<br>' +
                       'This mail is sent by server automatically, please do not reply directly.<br>' +
                       '******************************************************************************<br>';

        EXEC msdb.dbo.sp_send_dbmail
            @profile_name = 'Notification',
            @recipients = @EMAIL,
            @copy_recipients = @CC_EMAILS,  
            @subject = @EMAIL_TITLE,
            @body = @CONTENT,
            @body_format = 'HTML';
    END

    ELSE IF (@action = 'CLOSE')
    BEGIN
        SET @EMAIL_TITLE = 'CLOSURE of Item Donation from - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);
        SET @CONTENT = 'Greetings admin,<br>' +
                       'An item donation is closed from ' + @orgName + ' with reason: ' + @reason + '<br>' +
                       'You may login to your dashboard to view the donation closure.<br>' +
                       '<br><br>' +
                       '********************************(DONORCONNECT)********************************<br>' +
                       'This mail is sent by server automatically, please do not reply directly.<br>' +
                       '******************************************************************************<br>';

        EXEC msdb.dbo.sp_send_dbmail
            @profile_name = 'Notification',
            @recipients = @EMAIL,
			@copy_recipients = @CC_EMAILS, 
            @subject = @EMAIL_TITLE,
            @body = @CONTENT,
            @body_format = 'HTML';
    END

	ELSE IF (@action = 'CANCEL')
    BEGIN
        SET @EMAIL_TITLE = 'CANCEL of Item Donation Application from - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);
        SET @CONTENT = 'Greetings admin,<br>' +
                       'An item donation application is cancelled from ' + @orgName + '<br>' +
                       '<br><br>' +
                       '********************************(DONORCONNECT)********************************<br>' +
                       'This mail is sent by server automatically, please do not reply directly.<br>' +
                       '******************************************************************************<br>';

        EXEC msdb.dbo.sp_send_dbmail
            @profile_name = 'Notification',
            @recipients = @EMAIL,
			@copy_recipients = @CC_EMAILS, 
            @subject = @EMAIL_TITLE,
            @body = @CONTENT,
            @body_format = 'HTML';
    END

    ELSE IF (@action = 'RESUBMIT')
    BEGIN
        SET @EMAIL_TITLE = 'RESUBMISSION of Item Donation from - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);
        SET @CONTENT = 'Greetings admin,<br>' +
                       'An item donation application is resubmitted from ' + @orgName + '<br>' +
                       'You may login to your dashboard to view the details.<br>' +
                       '<br><br>' +
                       '********************************(DONORCONNECT)********************************<br>' +
                       'This mail is sent by server automatically, please do not reply directly.<br>' +
                       '******************************************************************************<br>';

        EXEC msdb.dbo.sp_send_dbmail
            @profile_name = 'Notification',
            @recipients = @EMAIL,
			@copy_recipients = @CC_EMAILS, 
            @subject = @EMAIL_TITLE,
            @body = @CONTENT,
            @body_format = 'HTML';
    END

	ELSE IF (@action = 'REFUND')
    BEGIN
        SET @EMAIL_TITLE = 'REFUND for Donation Delivery Fee by - ' + @username + ' ' + CONVERT(varchar(20), GETDATE(), 120);
        SET @CONTENT = 'Greetings admin,<br>' +
                       'Here is the refund application from donor ' + @username + ' with reason: ' + @reason +'<br>' +
                       'You may login to your dashboard to view the details and approve the refund as soon as possible.<br>' +
                       '<br><br>' +
                       '********************************(DONORCONNECT)********************************<br>' +
                       'This mail is sent by server automatically, please do not reply directly.<br>' +
                       '******************************************************************************<br>';

        EXEC msdb.dbo.sp_send_dbmail
            @profile_name = 'Notification',
            @recipients = @EMAIL,
            @subject = @EMAIL_TITLE,
            @body = @CONTENT,
            @body_format = 'HTML';
    END
END;
