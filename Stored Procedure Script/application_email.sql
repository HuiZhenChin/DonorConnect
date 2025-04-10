USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[application_email]    Script Date: 8/11/2024 3:51:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[application_email]
    @action varchar(500) = NULL,
    @orgId varchar(500) = NULL,
    @riderId varchar(500) = NULL,
	@donorId varchar(500) = NULL,
	@adminId varchar(500) = NULL,
    @role varchar(100),
    @reason varchar(max) = NULL,
    @resubmitLink varchar(max) = NULL,
	@donationPublishId varchar(100)= NULL,
	@warningCount varchar(50) = NULL,
	@path varchar(max) = NULL

AS
BEGIN 
    SET NOCOUNT ON;

    DECLARE 
        @EMAIL_TITLE varchar(100) = NULL,
        @CONTENT varchar(max) = NULL,
        @EMAIL varchar(300) = NULL,
        @CC_EMAIL varchar(300) = NULL,
        @orgName varchar(max) = NULL, 
        @riderFullName varchar(500)= NULL,
        @riderEmail varchar(max)= NULL,
		@adminEmail varchar(max)= NULL,
        @riderUsername varchar(500)= NULL,
		@donorUsername varchar(500)= NULL,
		@adminUsername varchar(500)= NULL,
		@title varchar(500)= NULL;

    -- Handling organization role
    IF (@role = 'organization')
    BEGIN
        -- Retrieve organization information
        SELECT 
        @orgName = orgName,
        @EMAIL = orgEmail,
        @CC_EMAIL = picEmail  
        FROM organization 
        WHERE orgId = @orgId;

		SELECT 
        @title = title
		FROM donation_publish 
		WHERE donationPublishId = @donationPublishId;

        IF (@action = 'APPROVED')
        BEGIN
            SET @EMAIL_TITLE = 'APPROVAL: DonorConnect Registration Approval for - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);

            SET @CONTENT = 'Greetings,<br>' +
                           'Your application has been approved by our team.' + '<br>' +
                           'Please login to your account using the login details below: <br>' +
                           'Your username: ' + @orgName + '<br>' +
                           'Please use the password you set during registration.<br>' +
                           'If you have forgotten your password, you may reset it at the Login Page.<br>' +
                           '<br><br>' +	  
                           '********************************(DONORCONNECT)********************************<br>' +
                           'This mail is sent by server automatically, please do not reply directly.<br>' +
                           '******************************************************************************<br>';

            EXEC msdb.dbo.sp_send_dbmail
                @profile_name = 'Notification',
                @recipients = @EMAIL,
                @copy_recipients = @CC_EMAIL,
                @subject = @EMAIL_TITLE,
                @body = @CONTENT,
                @body_format = 'HTML';
        END
        ELSE IF (@action = 'REJECTED')
        BEGIN
            SET @EMAIL_TITLE = 'REJECTION: DonorConnect Registration Rejection for - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);

            SET @CONTENT = 'Greetings,<br>' +
                           'Your application has been REJECTED by our team.' + '<br>' +
                           'This is the reason provided: ' + @reason + '<br>' +
                           'Please resubmit again using the link below. <br>' + 
                            @resubmitLink + '<br>' + '<br>' +
                           'If you have any enquiry, you may contact us at mailserver8877@gmail.com. <br>' +
                           'Please do not send email to the current email address. <br>' +
                           'Regards,' + '<br>' +
                           'DonorConnect Team'  + '<br>' +
                           '<br><br>' +	  
                           '********************************(DONORCONNECT)********************************<br>' +
                           'This mail is sent by server automatically, please do not reply directly.<br>' +
                           '******************************************************************************<br>';

            EXEC msdb.dbo.sp_send_dbmail
                @profile_name = 'Notification',
                @recipients = @EMAIL,
                @copy_recipients = @CC_EMAIL,
                @subject = @EMAIL_TITLE,
                @body = @CONTENT,
                @body_format = 'HTML';
        END

		ELSE IF (@action = 'DONATION APPROVED')
        BEGIN
            SET @EMAIL_TITLE = 'APPROVAL: DonorConnect Donation Publish Approval for - ' + @title + ' ' + CONVERT(varchar(20), GETDATE(), 120);

            SET @CONTENT = 'Greetings,<br>' +
                           'Your donation has been approved by our team.' + '<br>' +
                           'You may view it at the public Donations page on the navigation bar. <br>' +
						   'You will receive notifications if there is any donation requests from the donors. <br>' +
                           '<br><br>' +	  
                           '********************************(DONORCONNECT)********************************<br>' +
                           'This mail is sent by server automatically, please do not reply directly.<br>' +
                           '******************************************************************************<br>';

            EXEC msdb.dbo.sp_send_dbmail
                @profile_name = 'Notification',
                @recipients = @EMAIL,
                @copy_recipients = @CC_EMAIL,
                @subject = @EMAIL_TITLE,
                @body = @CONTENT,
                @body_format = 'HTML';
        END

		ELSE IF (@action = 'DONATION REJECTED')
        BEGIN
             SET @EMAIL_TITLE = 'REJECTION: DonorConnect Donation Publish Rejection for - ' + @title + ' ' + CONVERT(varchar(20), GETDATE(), 120);

            SET @CONTENT = 'Greetings,<br>' +
                           'Your donation has been REJECTED by our team.' + '<br>' +
                           'This is the reason provided: ' + @reason + '<br>' +
                           'Please resubmit again through your dashboard. <br>' +  
                           'If you have any enquiry, you may contact us at mailserver8877@gmail.com. <br>' +
                           'Please do not send email to the current email address. <br>' +
                           'Regards,' + '<br>' +
                           'DonorConnect Team'  + '<br>' +
                           '<br><br>' +	  
                           '********************************(DONORCONNECT)********************************<br>' +
                           'This mail is sent by server automatically, please do not reply directly.<br>' +
                           '******************************************************************************<br>';

            EXEC msdb.dbo.sp_send_dbmail
                @profile_name = 'Notification',
                @recipients = @EMAIL,
                @copy_recipients = @CC_EMAIL,
                @subject = @EMAIL_TITLE,
                @body = @CONTENT,
                @body_format = 'HTML';
        END
		ELSE IF (@action = 'ORGANIZATION TERMINATED')
		BEGIN
			SET @EMAIL_TITLE = 'Account Terminated: DonorConnect - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);

			SET @CONTENT = 'Dear ' + @orgName + ', ' + '<br>' +
						   'We regret to inform you that your account has been terminated by our team.' + '.' + '<br>' +
						   'The termination was based on the following reason(s): ' + @reason + '.' + '<br>' + 
						   'If you believe this is a mistake, you may contact our support team for further clarification.' + '<br>' +
						   '<br>' +
						   'We sincerely thank you for your contributions and hope to assist you again in the future.' + '<br>' +
						   'For any inquiries, please feel free to reach out to our support team.' + '<br>' +
						   '<br>' +
						   'Regards,' + '<br>' +
						   'DonorConnect Team'  + '<br>' +
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
		ELSE IF (@action = 'NEW DONATION REQUEST')
		BEGIN
			SET @EMAIL_TITLE = 'NEW Donation Request: DonorConnect - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);

			SET @CONTENT = 'Dear ' + @orgName + ', ' + '<br>' +
						   'You have 1 donation request from our donors' + '.' + '<br>' +
						   'Here is the link for you: ' + @resubmitLink + '.' + '<br>' + 
						   'Or you may login to your account dashboard to view the request.' + '<br>' +
						   '<br>' +						  
						   'For any inquiries, please feel free to reach out to our support team.' + '<br>' +
						   '<br>' +
						   'Regards,' + '<br>' +
						   'DonorConnect Team'  + '<br>' +
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
		ELSE IF (@action = 'URGENT DONATION')

		SELECT 
        @title = title
		FROM donation_publish 
		WHERE donationPublishId = @donationPublishId;

		BEGIN
			SET @EMAIL_TITLE = 'Urgent Donation Ended: DonorConnect - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);

			SET @CONTENT = 'Dear ' + @orgName + ', ' + '<br>' +
						   'We would like to inform you that your urgent donation countdown timer is already ended for ' + @title + '.' + '<br>' +						  
						   'Please log in to your account and decide whether you wish to republish the donations again or not.' + '<br>' +
						   'You can ignore it if you already have enough items.' + '<br>' +
						   'Note that the republish of this donation does not need to undergo approval from our team.' + '<br>' +
						   '<br>' +
						   'We sincerely thank you for your participations and hope to assist you again in the future.' + '<br>' +
						   'For any inquiries, please feel free to reach out to our support team.' + '<br>' +
						   '<br>' +
						   'Regards,' + '<br>' +
						   'DonorConnect Team'  + '<br>' +
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

	
    END
    -- Handling rider role
    ELSE IF (@role = 'rider')
    BEGIN
        -- Retrieve rider information
        SELECT 
            @riderFullName = riderFullName,
            @EMAIL = riderEmail,
            @riderUsername = riderUsername
        FROM delivery_rider
        WHERE riderId = @riderId;

        IF (@action = 'APPROVED')
        BEGIN
            SET @EMAIL_TITLE = 'APPROVAL: DonorConnect Registration Approval for - ' + @riderFullName + ' ' + CONVERT(varchar(20), GETDATE(), 120);

            SET @CONTENT = 'Greetings,<br>' +
                           'Your application has been approved by our team.' + '<br>' +
                           'Please login to your account using the login details below: <br>' +
                           'Your username: ' + @riderUsername + '<br>' +
                           'Please use the password you set during registration.<br>' +
                           'If you have forgotten your password, you may reset it at the Login Page.<br>' +
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
        ELSE IF (@action = 'REJECTED')
        BEGIN
            SET @EMAIL_TITLE = 'REJECTION: DonorConnect Registration Rejection for - ' + @riderFullName + ' ' + CONVERT(varchar(20), GETDATE(), 120);

            SET @CONTENT = 'Greetings,<br>' +
                           'Your application has been REJECTED by our team.' + '<br>' +
                           'This is the reason provided: ' + @reason + '<br>' +
                            'Please resubmit again using the link below. <br>' + 
                            @resubmitLink + '<br>' + '<br>' +
                           'If you have any enquiry, you may contact us at mailserver8877@gmail.com. <br>' +
                           'Please do not send email to the current email address. <br>' +
                           'Regards,' + '<br>' +
                           'DonorConnect Team'  + '<br>' +
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

		ELSE IF (@action = 'RIDER TERMINATED')
		BEGIN
			SET @EMAIL_TITLE = 'Account Terminated: DonorConnect - ' + @riderFullName + ' ' + CONVERT(varchar(20), GETDATE(), 120);

			SET @CONTENT = 'Dear ' + @riderFullName + ', ' + '<br>' +
						   'We regret to inform you that your account has been terminated by our team.' + '.' + '<br>' +
						   'The termination was based on the following reason(s): ' + @reason + '.' + '<br>' + 
						   'If you believe this is a mistake, you may contact our support team for further clarification.' + '<br>' +
						   '<br>' +
						   'We sincerely thank you for your contributions and hope to assist you again in the future.' + '<br>' +
						   'For any inquiries, please feel free to reach out to our support team.' + '<br>' +
						   '<br>' +
						   'Regards,' + '<br>' +
						   'DonorConnect Team'  + '<br>' +
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
		ELSE IF (@action = 'RIDER WARNING')
		BEGIN
			SET @EMAIL_TITLE = 'Account Terminated: DonorConnect - ' + @riderFullName + ' ' + CONVERT(varchar(20), GETDATE(), 120);

			SET @CONTENT = 'Dear ' + @riderFullName + ', ' + '<br>' +
						   'We regret to inform you that your account has been WARNED by our team.' + '.' + '<br>' +
						   'The warning was based on the following reason(s): ' + @reason + '.' + '<br>' + 		
						   'This is your current warning count: ' + @warningCount + '.' + '<br>' + 	
						   'Please note that the maximum warning count is 3. The account will be terminated when you have more than 3 warnings.' + '<br>' +
						   'For any inquiries, please feel free to reach out to our support team.' + '<br>' +
						   '<br>' +
						   'Regards,' + '<br>' +
						   'DonorConnect Team'  + '<br>' +
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
		ELSE IF (@action = 'EARNINGS STATEMENT')
		BEGIN
			SET @EMAIL_TITLE = 'Earnings Statement: DonorConnect - ' + @riderFullName + ' ' + CONVERT(varchar(20), GETDATE(), 120);

			SET @CONTENT = 'Dear ' + @riderFullName + ', ' + '<br>' +
						   'Your earnings statement for delivery with Id ' + @donationPublishId + ' is here.' + '<br>' +
						   '<br>' +
						   'Regards,' + '<br>' +
						   'DonorConnect Team'  + '<br>' +
						   '********************************(DONORCONNECT)********************************<br>' +
						   'This mail is sent by server automatically, please do not reply directly.<br>' +
						   '******************************************************************************<br>';

			EXEC msdb.dbo.sp_send_dbmail
				@profile_name = 'Notification',
				@recipients = @EMAIL,
				@subject = @EMAIL_TITLE,
				@body = @CONTENT,
				@body_format = 'HTML',
				@file_attachments = @path;  
		END
		
    END

	ELSE IF (@role = 'donor')
    BEGIN
        -- Retrieve organization information
        SELECT 
		@donorUsername= donorUsername,
        @EMAIL = donorEmail
        FROM donor 
        WHERE donorId = @donorId;

		SELECT 
        @orgName = orgName
        FROM organization 

		SELECT 
        @title = title
		FROM donation_publish 
		WHERE donationPublishId = @donationPublishId;

        IF (@action = 'DONATION REQUEST REJECTED')
        BEGIN
            SET @EMAIL_TITLE = 'REJECTION: DonorConnect Donation Request by - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);

            SET @CONTENT = 'Greetings' + @donorUsername + ', ' + '<br>' +
                           'Your application has been REJECTED by the organization.' + '<br>' +
                           'This is the reason provided: ' + @reason + '<br>' +
						   'Please resubmit again using the link below. <br>' + 
                            @resubmitLink + '<br>' + '<br>' +
                           'Or you may login to your account to check your notifications for the link.' + '<br>' +
						   'If not, you can ignore this email if you are not required to resubmit again.' + '<br>' +
						   'Apologies for any inconvenience.' + '<br>' +
                           '<br><br>' +	  
						   'Regards,' + '<br>' +
                           'DonorConnect Team'  + '<br>' +
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
		ELSE IF (@action = 'DONATION REQUEST APPROVED')
        BEGIN
            SET @EMAIL_TITLE = 'APPROVAL: DonorConnect Donation Request by - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);

            SET @CONTENT = 'Greetings' + @donorUsername + ', ' + '<br>' +
                           'Your application has been APPROVED by the organization.' + '<br>' +                           
						   'Please proceed the delivery pickup and payment using the link below. <br>' + 
                            @resubmitLink + '<br>' + '<br>' +
                           'Or you may login to your account to check your notifications for the link.' + '<br>' +
						   'If you want to cancel the donation, you can do so by clicking the "Cancel Donations". ' + '<br>' +
						   'Thank you for donating!.' + '<br>' +
                           '<br><br>' +	  
						   'Regards,' + '<br>' +
                           'DonorConnect Team'  + '<br>' +
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
		ELSE IF (@action = 'DONOR TERMINATED')
		BEGIN
			SET @EMAIL_TITLE = 'Account Terminated: DonorConnect - ' + @donorUsername + ' ' + CONVERT(varchar(20), GETDATE(), 120);

			SET @CONTENT = 'Dear ' + @donorUsername + ', ' + '<br>' +
						   'We regret to inform you that your account has been terminated by our team.' + '.' + '<br>' +
						   'The termination was based on the following reason(s): ' + @reason + '.' + '<br>' + 
						   'If you believe this is a mistake, you may contact our support team for further clarification.' + '<br>' +
						   '<br>' +
						   'We sincerely thank you for your contributions and hope to assist you again in the future.' + '<br>' +
						   'For any inquiries, please feel free to reach out to our support team.' + '<br>' +
						   '<br>' +
						   'Regards,' + '<br>' +
						   'DonorConnect Team'  + '<br>' +
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
		ELSE IF (@action = 'AUTO-REJECT DONATION REQUEST')
		BEGIN
			SET @EMAIL_TITLE = 'Donation Request Rejected: DonorConnect - ' + @donorUsername + ' ' + CONVERT(varchar(20), GETDATE(), 120);

			SET @CONTENT = 'Dear ' + @donorUsername + ', ' + '<br>' +
						   'We regret to inform you that your recent donation request has been auto-rejected by our system because it has been closed by the organization.' + '<br>' +
						   'The request was closed based on the following reason(s): ' + @reason + '.' + '<br>' +
						   '<br>' +
						   'Thank you for your interest in supporting our need. We appreciate your understanding and hope you will continue to participate in future donation opportunities.' + '<br>' +
						   '<br>' +
						   'Best regards,' + '<br>' +
						   'DonorConnect Team'  + '<br>' +
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
		ELSE IF (@action = 'DONOR WARNING')
		BEGIN
			SET @EMAIL_TITLE = 'Account Terminated: DonorConnect - ' + @donorUsername + ' ' + CONVERT(varchar(20), GETDATE(), 120);

			SET @CONTENT = 'Dear ' + @donorUsername + ', ' + '<br>' +
						   'We regret to inform you that your account has been WARNED by our team.' + '.' + '<br>' +
						   'The warning was based on the following reason(s): ' + @reason + '.' + '<br>' + 		
						   'This is your current warning count: ' + @warningCount + '.' + '<br>' + 	
						   'Please note that the maximum warning count is 3. The account will be terminated when you have more than 3 warnings.' + '<br>' +
						   'For any inquiries, please feel free to reach out to our support team.' + '<br>' +
						   '<br>' +
						   'Regards,' + '<br>' +
						   'DonorConnect Team'  + '<br>' +
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
		ELSE IF (@action = 'DONATION DELIVERY UPDATE ACCEPTED')
		BEGIN
			SET @EMAIL_TITLE = 'Donation Pickup Accepted: DonorConnect - ' + @donorUsername + ' ' + CONVERT(varchar(20), GETDATE(), 120);

			SET @CONTENT = 'Dear ' + @donorUsername + ', ' + '<br>' +
						   'Your donation with Id' + @donationPublishId + ' has been accepted by our delivery rider.' + '<br>' +
						   'Here is the link to check the status: ' + @resubmitLink + '<br>' +
						   'Please note that the rider will come to the pickup address based on the scheduled date. If you have any special requirements, please contact the rider.' + '<br>' +
						   'For any inquiries, please feel free to reach out to our support team.' + '<br>' +
						   '<br>' +
						   'Regards,' + '<br>' +
						   'DonorConnect Team'  + '<br>' +
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
		ELSE IF (@action = 'DONATION DELIVERY UPDATE COMPLETED')
		BEGIN
			SET @EMAIL_TITLE = 'Donation Pickup Completed: DonorConnect - ' + @donorUsername + ' ' + CONVERT(varchar(20), GETDATE(), 120);

			SET @CONTENT = 'Dear ' + @donorUsername + ', ' + '<br>' +
						   'Your donation with Id ' + @donationPublishId + ' has reached the destination.' + '<br>' +
						   'Here is the link to check the status: ' + @resubmitLink + '<br>' +
						   'You will receive the acknowledgement receipt once the organization approved the item arrival. It may take a few days.' + '<br>' +
						   'For any inquiries, please feel free to reach out to our support team.' + '<br>' +
						   '<br>' +
						   'Regards,' + '<br>' +
						   'DonorConnect Team'  + '<br>' +
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

	END

	ELSE IF (@role = 'admin')
	BEGIN
       
        SELECT 
		@adminUsername= adminUsername,
        @EMAIL = adminEmail
        FROM [admin] 
        WHERE adminId = @adminId;

		IF (@action = 'ADMIN TERMINATED')
			BEGIN
				SET @EMAIL_TITLE = 'Account Terminated: DonorConnect ' + ' ' + CONVERT(varchar(20), GETDATE(), 120);

				SET @CONTENT = 'Dear ' + @adminUsername + ', ' + '<br>' +
							   'We regret to inform you that your account has been terminated by our team.' + '.' + '<br>' +
							   'The termination was based on the following reason(s): ' + @reason + '.' + '<br>' + 
							   'If you believe this is a mistake, you may contact our support team for further clarification.' + '<br>' +
							   '<br>' +
							   'We sincerely thank you for your contributions and hope to assist you again in the future.' + '<br>' +
							   'For any inquiries, please feel free to reach out to our support team.' + '<br>' +
							   '<br>' +
							   'Regards,' + '<br>' +
							   'DonorConnect Team'  + '<br>' +
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
	END
END;

