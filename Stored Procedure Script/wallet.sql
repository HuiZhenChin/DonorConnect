USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[wallet]    Script Date: 8/11/2024 4:01:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[wallet]
	@otp varchar(10) = NULL,
	@riderEmail varchar(500) = NULL,
	@action varchar(50) = NULL,
	@riderUsername varchar (500)= NULL
	
AS
BEGIN 
    SET NOCOUNT ON;

    DECLARE 
        @EMAIL_TITLE varchar(100) = NULL,
        @CONTENT varchar(max) = NULL,
        @EMAIL varchar(300) = NULL

    
    IF (@riderUsername IS NOT NULL AND @riderUsername <> '')
    BEGIN
        IF (@action = 'OTP')
        BEGIN
            
            SET @EMAIL_TITLE = 'DonorConnect: Wallet Cash Out Verification OTP - ' + CONVERT(varchar(300), GETDATE(), 112);

            SET @CONTENT = 'Hi ' + @riderUsername + ', ' + '<br>' +
                           'You are receiving this email because of the auto-generated OTP for your cash out verification.' + '<br>' +
                           'Here is the OTP generated for you: <br>' +
                           @otp + '<br><br>' +
                           'Do not share this OTP with anyone.' + '<br>' +
                           'Please note that the OTP will expire after 2 minutes.' + '<br>' +
                           '********************************(DONORCONNECT)********************************' + '<br>' +
                           'This mail is sent by the server automatically, please do not reply directly.' + '<br>' +
                           '***********************************************************************' + '<br>';

            -- Send email with OTP 
            EXEC msdb.dbo.sp_send_dbmail
                @profile_name = 'Notification',
                @recipients = @riderEmail,
                @subject = @EMAIL_TITLE,
                @body = @CONTENT,
                @body_format = 'HTML';
        END
        
    END
END;