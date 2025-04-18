USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[create_donor]    Script Date: 8/11/2024 3:55:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[create_donor] 
	@donorId varchar(500),
    @donorName varchar(500),
    @donorUsername varchar(500),
    @donorEmail varchar(500),
    @donorContactNumber varchar(500),
    @method varchar(300),
    @donorHashPassword varchar(MAX),
    @donorNewHashPassword varchar(MAX) = NULL,
    @donorAddress1 varchar(MAX) = NULL,
    @donorProfilePicBase64 varchar(MAX) = NULL,
	@status varchar(200)
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @MESSAGE VARCHAR(500);

    IF (@method = 'INSERT')
    BEGIN
        -- Check if the donor already exists
        IF NOT EXISTS(SELECT * FROM donor WHERE donorUsername = @donorUsername)
        BEGIN

            -- Generate new donor ID
            SELECT @donorId = 'd' + CAST(ISNULL(MAX(CAST(SUBSTRING(donorId, 2, LEN(donorId) - 1) AS INT)), 0) + 1 AS varchar)
            FROM donor;

            -- Insert new donor
            INSERT INTO donor (donorId, donorName, donorUsername, donorEmail, donorContactNumber, donorHashPassword, donorNewHashPassword, donorAddress1, donorProfilePicBase64, createdOn, [status])
            VALUES (@donorId, @donorName, @donorUsername, @donorEmail, @donorContactNumber, @donorHashPassword, @donorNewHashPassword, @donorAddress1, @donorProfilePicBase64, GETDATE(), @status);

			INSERT INTO [user] (username, email, role)
			VALUES (@donorUsername, @donorEmail, 'donor');

           SET @MESSAGE = 'Successful! You have registered as a donor. You may start your item donation journey now!';
        END
        ELSE
        BEGIN
            SET @MESSAGE = 'Donor with the same username already exists';
        END

        SELECT @MESSAGE AS MESSAGE;
    END
END
