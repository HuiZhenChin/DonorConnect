USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[create_admin]    Script Date: 8/11/2024 3:53:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[create_admin]
    @method varchar(300),
    @adminId varchar(500) OUTPUT, 
    @adminUsername varchar(500),
    @adminEmail varchar(max),
    @adminHashPassword varchar(max),
	@adminPassword varchar(max),
    @status varchar(200)

AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @MESSAGE VARCHAR(500);

    IF (@method = 'INSERT')
    BEGIN
        -- Check if the admin already exists 
        IF NOT EXISTS(SELECT * FROM [admin] WHERE LOWER(adminEmail) = LOWER(@adminEmail))
        BEGIN
            -- Generate new organization ID
            SELECT @adminId = 'A' + CAST(ISNULL(MAX(CAST(SUBSTRING(adminId, 2, LEN(adminId) - 1) AS INT)), 0) + 1 AS varchar)
            FROM [admin];

			-- Generate new admin username (DCAdmin + increment number)
            SELECT @adminUsername = 'DCAdmin' + CAST(ISNULL(MAX(CAST(SUBSTRING(adminUsername, 8, LEN(adminUsername) - 7) AS INT)), 0) + 1 AS VARCHAR)
            FROM [admin];

            -- Determine if this is the first admin (set isMain = 'Yes')
            DECLARE @isMain NVARCHAR(3) = 'No';
            IF NOT EXISTS(SELECT 1 FROM [admin])
            BEGIN
                SET @isMain = 'Yes';
            END

            -- Insert new organization
            INSERT INTO [admin](adminId, adminUsername, adminEmail, adminHashPassword, [status], isMain, created_on)
            VALUES (@adminId, @adminUsername, @adminEmail, @adminHashPassword, @status, @isMain ,GETDATE());

			INSERT INTO [user] (username, email, role)
			VALUES (@adminUsername, @adminEmail, 'admin');

            SET @MESSAGE = 'Successful! You have registered as an admin! The login details are sent in DonorConnect official email address. You may login and manage the application now.';
			
			DECLARE 
			@EMAIL_TITLE varchar(100) = NULL,
			@CONTENT varchar(max) = NULL

			 SET @EMAIL_TITLE = 'Admin Registration Confirmation for DonorConnect- ' + CONVERT(varchar(300), GETDATE(), 112);
             SET @CONTENT = 'Dear ' + ' ' + @adminEmail + ',' + '<br>' +
                        'Congratulations! You have successfully registered as an admin on DonorConnect.' + '<br>' +
                        'Your admin username: ' + @adminUsername + '<br>' +
                        'Your password: ' + @adminPassword + '<br>' +
                        'Please use these credentials to log in and manage the application.' + '<br>' +
						'Please note that you may change your password once you logged in to your account.' + '<br>' +
                        'Regards,' + '<br>' +
                        'DonorConnect Team';

			EXEC msdb.dbo.sp_send_dbmail
                @profile_name = 'Notification',
                @recipients = 'mailserver8877@gmail.com',
                @subject = @EMAIL_TITLE,
                @body = @CONTENT,
				@body_format = 'HTML'

			DECLARE 
			@EMAIL_TITLE2 varchar(100) = NULL,
			@CONTENT2 varchar(max) = NULL

			 SET @EMAIL_TITLE2 = 'Admin Registration Confirmation for DonorConnect- ' + CONVERT(varchar(300), GETDATE(), 112);
             SET @CONTENT2 = 'Dear ' + ' ' + @adminEmail + ',' + '<br>' +
                        'Congratulations! You have successfully registered as an admin on DonorConnect.' + '<br>' +    
                        'Your login credentials has been delivered to DonorConnect official email, please access that email to get your login details.' + '<br>'+
                        'Regards,' + '<br>' +
                        'DonorConnect Team';

			EXEC msdb.dbo.sp_send_dbmail
                @profile_name = 'Notification',
                @recipients = @adminEmail,
                @subject = @EMAIL_TITLE2,
                @body = @CONTENT2,
				@body_format = 'HTML'
		
		END
        ELSE
        BEGIN
            SET @MESSAGE = 'Admin already exists';
        END

        SELECT @MESSAGE AS MESSAGE;
    END
END