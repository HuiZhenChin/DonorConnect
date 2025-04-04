USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[send_payment]    Script Date: 8/11/2024 4:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[send_payment]
    @donorId varchar(500) = NULL,
	@otp varchar(10) = NULL,
	@donorEmail varchar(500) = NULL,
	@pdfFilePath varchar(500) = NULL,
	@action varchar(50) = NULL 
	
AS
BEGIN 
    SET NOCOUNT ON;

    DECLARE 
        @EMAIL_TITLE varchar(100) = NULL,
        @CONTENT varchar(max) = NULL,
        @EMAIL varchar(300) = NULL,
        @donorUsername varchar(500) = NULL;

    -- Retrieve donor information
    SELECT 
        @donorUsername = donorUsername      
    FROM donor 
    WHERE donorId = @donorId;

    IF (@donorUsername IS NOT NULL AND @donorUsername <> '')
    BEGIN
        -- Check if action is 'OTP' or 'Receipt'
        IF (@action = 'OTP')
        BEGIN
            -- Set email title and content for OTP
            SET @EMAIL_TITLE = 'DonorConnect: Payment Verification OTP - ' + CONVERT(varchar(300), GETDATE(), 112);

            SET @CONTENT = 'Hi ' + @donorUsername + ', ' + '<br>' +
                           'You are receiving this email because of the auto-generated OTP for your payment verification.' + '<br>' +
                           'Here is the OTP generated for you: <br>' +
                           @otp + '<br><br>' +
                           'Do not share this OTP with anyone.' + '<br>' +
                           'Please note that the OTP will expire after 2 minutes.' + '<br>' +
                           '********************************(DONORCONNECT)********************************' + '<br>' +
                           'This mail is sent by the server automatically, please do not reply directly.' + '<br>' +
                           '***********************************************************************' + '<br>';

            -- Send email with OTP (no attachment)
            EXEC msdb.dbo.sp_send_dbmail
                @profile_name = 'Notification',
                @recipients = @donorEmail,
                @subject = @EMAIL_TITLE,
                @body = @CONTENT,
                @body_format = 'HTML';
        END
        ELSE IF (@action = 'RECEIPT')
        BEGIN
            -- Set email title and content for payment receipt
            SET @EMAIL_TITLE = 'DonorConnect: Payment Receipt - ' + CONVERT(varchar(300), GETDATE(), 112);

            SET @CONTENT = 'Hi ' + @donorUsername + ', ' + '<br>' +
                           'Thank you for your donation! Please find attached your payment receipt. You may check your DonorConnect wallet for the money-in.' + '<br><br>' +
                           '********************************(DONORCONNECT)********************************' + '<br>' +
                           'This mail is sent by the server automatically, please do not reply directly.' + '<br>' +
                           '***********************************************************************' + '<br>';

            -- Send email with receipt attachment (PDF)
            EXEC msdb.dbo.sp_send_dbmail
                @profile_name = 'Notification',
                @recipients = @donorEmail,
                @subject = @EMAIL_TITLE,
                @body = @CONTENT,
                @body_format = 'HTML',
                @file_attachments = @pdfFilePath;  
        END
    END
END;