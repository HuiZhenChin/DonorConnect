USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[reset_password_email]    Script Date: 8/11/2024 4:00:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[reset_password_email]
    @email varchar(300) = NULL,
	@username varchar(300) = NULL,
	@resetLink varchar(max)= NULL
AS
BEGIN 

    SET NOCOUNT ON;

    DECLARE 
    @EMAIL_TITLE varchar(100) = NULL,
    @CONTENT varchar(max) = NULL,
    @EMAILLIST varchar(max) = NULL;


    IF (@username IS NOT NULL OR @username <> '')
    BEGIN

        SET @EMAIL_TITLE = 'Reset Password for DonorConnect -' + CONVERT(varchar(300), GETDATE(), 112);

        SET @CONTENT = 'Hi' + ' ' + @username + ' , ' + '<br>' +
						'You are receiving this email because we received a password reset request for your DonorConnect account.' + '<br>' +
                        'Here is the link for you to reset your password.' + '<br>' +
                        '<< Please click the link below >>' + '<br>' +
                        @resetLink + '<br>' + '<br>'+
						'Please note that the link will be expired within 3 minutes.' + '<br>' +
                        '********************************(DONORCONNECT)********************************' + '<br>' +
                        'The mail is sent by server automatically, please do not reply directly.' + '<br>' +
                        '******************************************************************************<br>';

        EXEC msdb.dbo.sp_send_dbmail
          @profile_name = 'Notification',
          @recipients = @email,
          @subject = @EMAIL_TITLE,
          @body = @CONTENT,
          @body_format = 'HTML'
          --@file_attachments = @filenames;

    END;

END;