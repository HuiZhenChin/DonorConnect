USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[update_password]    Script Date: 8/11/2024 4:01:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[update_password]
	
    @email VARCHAR(500)=NULL,
	@method VARCHAR(300)=NULL,
	@password VARCHAR(max)=NULL,
	@role VARCHAR(100)= NULL,
	@token VARCHAR(MAX)=NULL,
	@username VARCHAR(MAX)= NULL

AS
BEGIN
    IF (@method = 'UPDATE')
    BEGIN
        DECLARE @expire DATETIME;
        SET @expire = DATEADD(MINUTE, 3, (SELECT created_on FROM reset_password WHERE password_token = @token))
        
        IF EXISTS (SELECT * FROM reset_password WHERE password_token = @token)
        BEGIN
            IF GETDATE() <= @expire
            BEGIN
                -- Update based on role
                IF @role = 'donor'
                BEGIN
                    IF EXISTS (SELECT * FROM donor d
                               INNER JOIN reset_password r 
                               ON d.donorEmail = r.email AND d.donorUsername = r.username
                               WHERE r.password_token = @token)
                    BEGIN
                        UPDATE donor
                        SET donorHashPassword = @password
                        FROM donor d
                        INNER JOIN reset_password r 
                        ON d.donorEmail = r.email AND d.donorUsername = r.username
                        WHERE r.password_token = @token;

                        -- Mark the token as used
                        UPDATE reset_password
                        SET used = 1
                        WHERE password_token = @token;

                        SELECT 'SUCCESSFUL! Your password has been reset!' AS MESSAGE;
                    END
                    ELSE
                    BEGIN
                        SELECT 'ERROR! No matching donor found.' AS MESSAGE;
                    END
                END
                ELSE IF @role = 'organization'
                BEGIN
                    IF EXISTS (SELECT * FROM organization o
                               INNER JOIN reset_password r 
                               ON o.orgEmail = r.email AND o.orgName = r.username
                               WHERE r.password_token = @token)
                    BEGIN
                        UPDATE organization
                        SET orgHashPassword = @password 
                        FROM organization o
                        INNER JOIN reset_password r 
                        ON o.orgEmail = r.email AND o.orgName = r.username
                        WHERE r.password_token = @token;

                        -- Mark the token as used
                        UPDATE reset_password
                        SET used = 1
                        WHERE password_token = @token;

                        SELECT 'SUCCESSFUL! Your password has been reset!' AS MESSAGE;
                    END
                    ELSE
                    BEGIN
                        SELECT 'ERROR! No matching organization found.' AS MESSAGE;
                    END
                END
                ELSE IF @role = 'rider'
                BEGIN
                    IF EXISTS (SELECT * FROM delivery_rider dr
                               INNER JOIN reset_password r 
                               ON dr.riderEmail = r.email AND dr.riderUsername = r.username
                               WHERE r.password_token = @token)
                    BEGIN
                        UPDATE delivery_rider
                        SET riderHashPassword = @password 
                        FROM delivery_rider dr
                        INNER JOIN reset_password r 
                        ON dr.riderEmail = r.email AND dr.riderUsername = r.username
                        WHERE r.password_token = @token;

                        -- Mark the token as used
                        UPDATE reset_password
                        SET used = 1
                        WHERE password_token = @token;

                        SELECT 'SUCCESSFUL! Your password has been reset!' AS MESSAGE;
                    END
                    ELSE
                    BEGIN
                        SELECT 'ERROR! No matching rider found.' AS MESSAGE;
                    END
                END
				ELSE IF @role = 'admin'
                BEGIN
                    IF EXISTS (SELECT * FROM [admin] dr
                               INNER JOIN reset_password r 
                               ON dr.adminEmail = r.email AND dr.adminUsername = r.username
                               WHERE r.password_token = @token)
                    BEGIN
                        UPDATE [admin]
                        SET adminHashPassword = @password 
                        FROM [admin] dr
                        INNER JOIN reset_password r 
                        ON dr.adminEmail = r.email AND dr.adminUsername = r.username
                        WHERE r.password_token = @token;

                        -- Mark the token as used
                        UPDATE reset_password
                        SET used = 1
                        WHERE password_token = @token;

                        SELECT 'SUCCESSFUL! Your password has been reset!' AS MESSAGE;
                    END
                    ELSE
                    BEGIN
                        SELECT 'ERROR! No matching rider found.' AS MESSAGE;
                    END
                END
                ELSE
                BEGIN
                    SELECT 'ERROR! Invalid role provided.'  AS MESSAGE;
                END
            END
            ELSE IF GETDATE() > @expire
            BEGIN
                SELECT 'ERROR! Your password reset link has expired.' AS MESSAGE;
            END
        END
        ELSE
        BEGIN
            SELECT 'ERROR! Your link has expired.' AS MESSAGE;
        END
    END

    ELSE IF (@method = 'INSERT')
    BEGIN
        IF NOT EXISTS (SELECT * FROM reset_password WHERE email = @email AND username = @username)
        BEGIN
            INSERT INTO reset_password (email, username, password_token, userRole, created_on, used)
            VALUES (@email, @username, @token, @role, GETDATE(), 0);
        END
        ELSE
        BEGIN
            UPDATE reset_password
            SET password_token = @token, created_on = GETDATE(), used = 0
            WHERE email = @email AND username = @username;
        END
    END
END;




