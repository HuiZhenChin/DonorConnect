USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[signup_otp]    Script Date: 8/11/2024 4:01:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		//
-- Create date: //
-- Description:	//
-- =============================================
ALTER PROCEDURE [dbo].[signup_otp]
    @email varchar(max),
	@otp varchar(50),
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

        SET @EMAIL_TITLE = 'DonorConnect: Sign Up OTP -' + CONVERT(varchar(300), GETDATE(), 112);

        SET @CONTENT = 'Hi' + ' ' + @username + ' , ' + '<br>' +
						'You are receiving this email because for the auto-generated OTP for your registration.' + '<br>' +
                        'Here is the OTP generated for you.' + '<br>' +
                        @otp + '<br>' + '<br>'+
						'Please note that the OTP will be expired after 2 minutes.' + '<br>' +
                        '********************************(DONORCONNECT)********************************' + '<br>' +
                        'The mail is sent by server automatically, please do not reply directly.' + '<br>' +
                        '***********************************************************************' + '<br>';

        EXEC msdb.dbo.sp_send_dbmail
          @profile_name = 'Notification',
          @recipients = @email,
          @subject = @EMAIL_TITLE,
          @body = @CONTENT,
          @body_format = 'HTML'
          --@file_attachments = @filenames;

    END;

END;