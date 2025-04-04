USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[create_rider]    Script Date: 8/11/2024 3:57:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[create_rider]
    @method varchar(300),
    @riderId varchar(500) OUTPUT,
	@riderUsername varchar (500), 
	@riderFullName varchar (500), 
	@riderEmail varchar (max), 
	@riderContactNumber varchar (50), 
	@riderHashPassword varchar (max), 
	@riderNewHashPassword varchar (max)= NULL, 
	@drivingLicenseImageBase64 varchar (max), 
	@vehicleType varchar(50),
	@riderFacePicBase64 varchar (max), 
	@riderRegion varchar(100), 
	@noOfDeliveryMade varchar(500)= NULL,
	@walletAmount varchar(500)= NULL,
	@vehiclePlateNumber varchar(500),
	@registerDate datetime,
	@riderStatus varchar(100),
	@adminId varchar(100)= NULL

AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @MESSAGE VARCHAR(500);

    IF (@method = 'INSERT')
    BEGIN
        -- Check if the rider already exists 
        IF NOT EXISTS(SELECT * FROM delivery_rider WHERE LOWER(riderFullName) = LOWER(@riderFullName) and riderEmail= @riderEmail)
        BEGIN
            -- Generate new rider ID
            SELECT @riderId = 'r' + CAST(ISNULL(MAX(CAST(SUBSTRING(riderId, 2, LEN(riderId) - 1) AS INT)), 0) + 1 AS varchar)
            FROM delivery_rider;

            -- Insert new rider
            INSERT INTO delivery_rider(riderId ,riderUsername ,riderFullName ,riderEmail ,riderContactNumber ,riderHashPassword, riderNewHashPassword ,drivingLicenseImageBase64 ,vehicleType ,riderFacePicBase64 ,noOfDeliveryMade ,walletAmount ,vehiclePlateNumber ,registerDate ,riderStatus ,adminId, riderRegion )
            VALUES (@riderId ,@riderUsername ,@riderFullName ,@riderEmail ,@riderContactNumber ,@riderHashPassword, @riderNewHashPassword ,@drivingLicenseImageBase64 ,@vehicleType ,@riderFacePicBase64 ,@noOfDeliveryMade ,@walletAmount ,@vehiclePlateNumber ,GETDATE() ,@riderStatus ,@adminId, @riderRegion );

			INSERT INTO [user] (username, email, role)
			VALUES (@riderUsername, @riderEmail, 'rider');

            SET @MESSAGE = 'Successful! You have registered as a delivery rider! Your application is pending for approval now.';
        END
        ELSE
        BEGIN
            SET @MESSAGE = 'Rider already exists';
        END

        SELECT @MESSAGE AS MESSAGE;
    END
END

