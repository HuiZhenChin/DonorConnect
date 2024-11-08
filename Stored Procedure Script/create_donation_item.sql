USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[create_donation_item]    Script Date: 8/11/2024 3:54:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[create_donation_item]
    @method NVARCHAR(10),                
    @donationId varchar(100) = NULL,          
    @itemImage VARCHAR(MAX) = NULL,      
    @requestStatus VARCHAR(50) = NULL,   
    @itemCategory VARCHAR(255) = NULL, 
    @item VARCHAR(MAX) = NULL,           
    @quantityDonated VARCHAR(MAX) = NULL, 
    @reasonOfRejection VARCHAR(MAX) = NULL, 
    @description VARCHAR(MAX) = NULL,   
    @donorId VARCHAR(50) = NULL,         
    @orgId VARCHAR(50) = NULL,           
    @donationPublishId VARCHAR(50) = NULL,
    @totalQuantity varchar(100)  = NULL,          
    @quantityWithSameExpiryDate varchar(100)  = NULL,
    @expiryDate varchar(200) = NULL,    
    @created_on DATETIME = NULL,       
	@donorFullName varchar(500) = NULL, 
	@donorEmail varchar(500) = NULL, 
	@donorPhoneNumber varchar(200) = NULL, 
	@pickUpAddress varchar(500) = NULL, 
	@state varchar(100) = NULL

AS
BEGIN
    SET NOCOUNT ON;

    ---- Generate the new donationId if it is NULL
    --IF @donationId IS NULL
    --BEGIN
    --    -- Create the new donationId using the simplified logic
    --    SELECT @donationId = 'DR' + CAST(ISNULL(MAX(CAST(SUBSTRING(donationId, 3, LEN(donationId) - 2) AS INT)), 0) + 1 AS VARCHAR)
    --    FROM donation_item_request;
    --END


    IF (@method = 'INSERT')
    BEGIN
	BEGIN
        -- insert into donation_item_request based on NULL checks for requestStatus and donorFullName
        IF (@requestStatus IS NOT NULL AND @donorFullName IS NOT NULL)
        -- Insert into donation_item_request
        INSERT INTO donation_item_request (donationId, itemImage, requestStatus, itemCategory, item, quantityDonated, reasonOfRejection, [description], donorId, orgId, donationPublishId, created_on, donorFullName, donorEmail, donorPhone, pickUpAddress, [state])
        VALUES (@donationId, @itemImage, @requestStatus, @itemCategory, @item, @quantityDonated, @reasonOfRejection, @description, @donorId, @orgId, @donationPublishId, GETDATE(), @donorFullName, @donorEmail, @donorPhoneNumber, @pickUpAddress, @state);

        -- Insert into donation_item_expiry_date with the same donationId
        IF @expiryDate IS NOT NULL
        BEGIN
            INSERT INTO donation_item_expiry_date (donationId, itemCategory, item2, totalQuantity, quantityWithSameExpiryDate, expiryDate, donorId, orgId, donationPublishId, created_on)
            VALUES (@donationId, @itemCategory, @item, @totalQuantity, @quantityWithSameExpiryDate, @expiryDate, @donorId, @orgId, @donationPublishId, GETDATE());
        END
    END
	END
   
END