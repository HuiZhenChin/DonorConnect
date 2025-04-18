USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[login]    Script Date: 8/11/2024 4:00:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[login]
    @username varchar(500),
    @password varchar(max)  
AS
BEGIN
    SET NOCOUNT ON;

    IF @username = 'DCAdmin$$'
    BEGIN
        SELECT 'Admin Registration' AS MESSAGE;
        RETURN;
    END

    DECLARE @role varchar(50);
    DECLARE @hashedPassword varchar(max);
    DECLARE @status varchar(50);
    DECLARE @valid bit = 0;

    -- Check if user exists and get role
    SELECT @role = role FROM [user] WHERE username = @username;

    IF @role IS NOT NULL
    BEGIN
        IF @role = 'donor'
        BEGIN
            -- Check password and status for donor
            SELECT @hashedPassword = donorHashPassword, @status = status FROM donor WHERE donorUsername = @username;
            IF @status = 'Terminated'
            BEGIN
                SELECT 'Your account has been terminated' AS MESSAGE;
                RETURN;
            END
            IF @hashedPassword = @password
            BEGIN
                SET @valid = 1;
            END
        END
        ELSE IF @role = 'organization'
        BEGIN
            -- Check password and status for organization
            SELECT @hashedPassword = orgHashPassword, @status = orgStatus FROM organization WHERE orgName = @username;
            IF @status = 'Terminated'
            BEGIN
                SELECT 'Your account has been terminated' AS MESSAGE;
                RETURN;
            END
            IF @hashedPassword = @password
            BEGIN
                IF @status = 'Pending Approval'
                BEGIN
                    SELECT 'Your application is still pending for approval' AS MESSAGE;
                    RETURN;
                END
                ELSE
                BEGIN
                    SET @valid = 1;
                END
            END
        END
        ELSE IF @role = 'rider'
        BEGIN
            -- Check password and status for rider
            SELECT @hashedPassword = riderHashPassword, @status = riderStatus FROM delivery_rider WHERE riderUsername = @username;
            IF @status = 'Terminated'
            BEGIN
                SELECT 'Your account has been terminated' AS MESSAGE;
                RETURN;
            END
            IF @hashedPassword = @password
            BEGIN
                IF @status = 'Pending Approval'
                BEGIN
                    SELECT 'Your application is still pending for approval' AS MESSAGE;
                    RETURN;
                END
                ELSE
                BEGIN
                    SET @valid = 1;
                END
            END
        END
        ELSE IF @role = 'admin'
        BEGIN
            -- Check password and status for admin
            SELECT @hashedPassword = adminHashPassword, @status = status FROM [admin] WHERE adminUsername = @username;
            IF @status = 'Terminated'
            BEGIN
                SELECT 'Your account has been terminated' AS MESSAGE;
                RETURN;
            END
            IF @hashedPassword = @password
            BEGIN
                SET @valid = 1;
            END
        END
    END

    IF @valid = 1
    BEGIN
        SELECT 'Login Successful!' AS MESSAGE;
    END
    ELSE
    BEGIN
        IF @role IS NULL
        BEGIN
            SELECT 'Account does not exist' AS MESSAGE;
        END
        ELSE
        BEGIN
            SELECT 'Incorrect Password!' AS MESSAGE;
        END
    END
END
