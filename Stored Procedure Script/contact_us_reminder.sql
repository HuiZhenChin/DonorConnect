USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[contact_us_reminder]    Script Date: 8/11/2024 3:53:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[contact_us_reminder]
    @username NVARCHAR(50),
    @SENDER_EMAIL NVARCHAR(100),
	@fullName NVARCHAR(300),
	@phoneNumber NVARCHAR(500),
	@orgName NVARCHAR(500) = NULL,
	@message NVARCHAR(max),
	@attch NVARCHAR(max) = NULL

AS
BEGIN

DECLARE 
        @EMAIL_TITLE varchar(100) = NULL,
        @CONTENT varchar(max) = NULL,
        @RECIPIENT_EMAIL varchar(300) = 'mailserver8877@gmail.com';
    
		BEGIN
				SET @EMAIL_TITLE = 'Contact Us Message ' + CONVERT(varchar(20), GETDATE(), 120);

				SET @CONTENT = 'You have received a new message from: ' + @username + '.'  + '<br><br>' +    
							   'Phone Number: ' + @phoneNumber + '<br><br>' +  
							   'Message: ' + @message + '<br><br>' +    
							   '<br><br>' +      
							   '********************************(DONORCONNECT)********************************<br>' +
							   'This mail is sent by server automatically, please do not reply directly.<br>' +
							   '******************************************************************************<br>';

	   EXEC msdb.dbo.sp_send_dbmail
					@profile_name = 'Notification',
					@recipients = @RECIPIENT_EMAIL,           
					@subject = @EMAIL_TITLE,
					@body = @CONTENT,
					@from_address = @SENDER_EMAIL,
					@reply_to = @SENDER_EMAIL,  
					@body_format = 'HTML',
					@file_attachments = @attch;
	   END

	BEGIN
            SET @EMAIL_TITLE = 'DonorConnect- Contact Us Submission Copy ' + CONVERT(varchar(20), GETDATE(), 120);

            SET @CONTENT = 'Greetings, ' + @username + '<br><br>' +	  
							'Thank you for reaching out to us!' + '<br><br>' +	  
							'Here is a copy of submission that you have submitted just now in DonorConnect.'  + '<br><br>' +	  
							'Full Name: ' + @fullName +'<br><br>' +	  
							'Email Address: '+ @SENDER_EMAIL +'<br><br>' +	  
							'Phone Number: ' + @phoneNumber +'<br><br>' +	  
							'Organization Name: ' + @orgName + '<br><br>' +	  
							'Message: ' + @message +'<br><br>' +	  									
                           '<br><br>' +	  
						   'Our team will respond you within working hours, please check your email inbox and be patient for our replies.' + '<br><br>' +	  
						   'Thank you for coorporation!' + '<br><br>' +	 
						   '<br><br>' +	  
						   'Regards,' + '<br><br>' +	  
						   'DonorConnect' + '<br><br>' +	  
                           '********************************(DONORCONNECT)********************************<br>' +
                           'This mail is sent by server automatically, please do not reply directly.<br>' +
                           '******************************************************************************<br>';

   EXEC msdb.dbo.sp_send_dbmail
                @profile_name = 'Notification',
                @recipients = @SENDER_EMAIL,           
                @subject = @EMAIL_TITLE,
                @body = @CONTENT,
                @body_format = 'HTML',
				@file_attachments = @attch;
	END

   
END

