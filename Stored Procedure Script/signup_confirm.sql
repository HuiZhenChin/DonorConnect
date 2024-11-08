USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[signup_confirm]    Script Date: 8/11/2024 4:01:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		//
-- Create date: //
-- Description:	//
-- =============================================
ALTER PROCEDURE [dbo].[signup_confirm]
    @email varchar(max),
	@role varchar(100),
	@username varchar(max)
AS
BEGIN 

    SET NOCOUNT ON;

    DECLARE 
    @EMAIL_TITLE varchar(100),
    @CONTENT varchar(max),
    @EMAILLIST varchar(max);

    IF (@email IS NOT NULL)
    BEGIN

	IF (@role = 'donor')
	BEGIN
        SET @EMAIL_TITLE = 'DonorConnect: Sign Up Confirmation Email -' + CONVERT(varchar(300), GETDATE(), 112);

        SET @CONTENT = 'Hi' + ' ' + @username + ' , ' + '<br>' +
						'You have registered as a donor successfully in DonorConnect.' + '<br>' +
                        'Thank you for supporting us and you may start your item donation journey now!' + '<br>' +
						'From: DonorConnect' + '<br>' +
                        '********************************(DONORCONNECT)********************************' + '<br>' +
                        'The mail is sent by server automatically, please do not reply directly.' + '<br>' +
                        '******************************************************************************<br>';

        EXEC msdb.dbo.sp_send_dbmail
          @profile_name = 'Notification',
          @recipients = @email,
          @subject = @EMAIL_TITLE,
          @body = @CONTENT,
          @body_format = 'HTML'
         
	END;

	
	IF (@role = 'organization' or @role = 'rider')
	BEGIN
        SET @EMAIL_TITLE = 'DonorConnect: Sign Up Confirmation Email -' + CONVERT(varchar(300), GETDATE(), 112);

        SET @CONTENT = 'Hi' + ' ' + @username + ' , ' + '<br>' +
						'You have registered as ' + @role + ' successfully in DonorConnect.' + '<br>' +
                        'Thank you for joining us and your request is now pending for approval.' + '<br>' +
						'By submitting all required information, you will receive an email notifications when your application is approved.' + '<br>' +
						'Or else, you will receive an email if your application is rejected and it will guide you on how to resubmit a new application.' + '<br>' +
						'From: DonorConnect' + '<br>' +
                        '********************************(DONORCONNECT)********************************' + '<br>' +
                        'The mail is sent by server automatically, please do not reply directly.' + '<br>' +
                        '******************************************************************************<br>';

        EXEC msdb.dbo.sp_send_dbmail
          @profile_name = 'Notification',
          @recipients = @email,
          @subject = @EMAIL_TITLE,
          @body = @CONTENT,
          @body_format = 'HTML'
         
	END;
    END;

END;