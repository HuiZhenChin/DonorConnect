USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[create_delivery_payment]    Script Date: 8/11/2024 3:53:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[create_delivery_payment]
	@method NVARCHAR(10),                
    @deliveryId varchar(500) = NULL,         
    @date VARCHAR(500) = NULL,      
    @time VARCHAR(500) = NULL,   
    @status VARCHAR(100) = NULL,
	@vehicleType varchar(50)= NULL,
	@pickupAddress varchar(max)= NULL,
	@destinationAddress varchar(max)= NULL,
	@donorId varchar(500)= NULL,
	@orgId varchar(500)= NULL,
	@donationId varchar(500)= NULL,
	@noteRider varchar(500)= NULL,
	@noteOrg varchar(500)= NULL,
	@fee varchar(100)= NULL,
	@paymentId varchar(500) = NULL, 
	@paymentStatus varchar(50) = NULL,
	@link varchar(max)= NULL


AS
BEGIN
    SET NOCOUNT ON;

    ---- Generate the new deliveryId if it is NULL
    IF @deliveryId IS NULL
    BEGIN
        -- Create the new deliveryId 
        SELECT @deliveryId = 'DLV' + CAST(ISNULL(MAX(CAST(SUBSTRING(deliveryId, 4, LEN(deliveryId) - 3) AS INT)), 0) + 1 AS VARCHAR)
        FROM delivery;
    END

	IF @paymentId IS NULL
    BEGIN
        -- Create the new paymentId
        SELECT @paymentId = 'P' + CAST(ISNULL(MAX(CAST(SUBSTRING(paymentId, 2, LEN(paymentId) - 1) AS INT)), 0) + 1 AS VARCHAR)
        FROM payment;
    END

    IF (@method = 'INSERT')
    BEGIN
	 
        -- Insert into delivery
        INSERT INTO delivery (deliveryId, pickupDate, pickupTime, deliveryStatus, vehicleType, pickupAddress, destinationAddress, donorId, orgId, riderId, donationId, noteRider, noteOrg, created_on, paymentAmount)
        VALUES (@deliveryId, @date, @time, @status, @vehicleType, @pickupAddress, @destinationAddress, @donorId, @orgId, NULL, @donationId, @noteRider, @noteOrg, GETDATE(), @fee);

		INSERT INTO payment (paymentId, paymentDateTime, paymentAmount, paymentStatus, donorId, donationId)
		VALUES (@paymentId, GETDATE(), @fee, @paymentStatus, @donorId, @donationId)
        
		UPDATE donation_item_request
		SET paymentStatus= @paymentStatus
		where donationId= @donationId
   
	END

	
	
    
END

