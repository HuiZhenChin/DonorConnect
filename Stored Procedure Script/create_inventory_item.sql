USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[create_inventory_item]    Script Date: 8/11/2024 3:55:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[create_inventory_item]
	@method NVARCHAR(10),     
	@id varchar(500)= NULL,
    @itemCategory varchar(500)= NULL,
	@item varchar(500) = NULL,
	@quantity varchar(500) = NULL,
	@expiryDate varchar(500) = NULL,
	@orgId varchar(500) = NULL,
	@donationId varchar(500)= NULL

AS
BEGIN
    SET NOCOUNT ON;

    ---- Generate the new Id if it is NULL
    IF @id IS NULL
    BEGIN
        
        SELECT @id = 'INV' + CAST(ISNULL(MAX(CAST(SUBSTRING(inventoryId, 4, LEN(inventoryId) - 3) AS INT)), 0) + 1 AS VARCHAR)
        FROM inventory;
    END

    IF (@method = 'INSERT')
    BEGIN
	BEGIN
        
        -- Insert into inventory
        INSERT INTO inventory (inventoryId, itemCategory, item, quantity, expiryDate, orgId, itemUsage, donationId, created_on)
        VALUES (@id, @itemCategory, @item, @quantity, @expiryDate, @orgId, @quantity, @donationId, GETDATE());

		INSERT INTO inventory_item_usage(itemCategory, item, quantityIn, quantityOut, created_on, inventoryId, orgId)
		VALUES (@itemCategory, @item, @quantity, NULL, GETDATE(), @id, @orgId);

		INSERT INTO inventory_history (content, orgId, created_on)
		VALUES (
			CONCAT(
				@item, ' is added to ', @itemCategory, ' with quantity of ', @quantity, 
				CASE 
					WHEN @expiryDate IS NULL THEN ''
					ELSE CONCAT(' and with expiry date of ', CONVERT(VARCHAR, @expiryDate, 120))
				END,
				' on ', CONVERT(VARCHAR, GETDATE(), 23)
			),
			@orgId,
			GETDATE()
		);
        
    END
	END
    
END


