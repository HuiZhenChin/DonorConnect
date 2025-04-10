USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[filter_donations]    Script Date: 8/11/2024 3:59:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[filter_donations]
    @categoryName VARCHAR(MAX),
    @specificItems VARCHAR(MAX),
    @state VARCHAR(255) 
AS
BEGIN
    -- Clean up input values
    SET @categoryName = REPLACE(@categoryName, '[', '');
    SET @categoryName = REPLACE(@categoryName, ']', '');
    SET @categoryName = REPLACE(@categoryName, '(', '');
    SET @categoryName = REPLACE(@categoryName, ')', '');
    SET @categoryName = LTRIM(RTRIM(@categoryName));

	IF @specificItems = 'null'
    BEGIN
        SET @specificItems = '';
    END

    ELSE
    BEGIN
        SET @specificItems = REPLACE(@specificItems, '[', '');
        SET @specificItems = REPLACE(@specificItems, ']', '');
        SET @specificItems = REPLACE(@specificItems, '(', '');
        SET @specificItems = REPLACE(@specificItems, ')', '');
        SET @specificItems = LTRIM(RTRIM(@specificItems));
    END
    -- Declare table variables
    DECLARE @Categories TABLE (Category VARCHAR(255));
    DECLARE @Items TABLE (Category VARCHAR(255), Item VARCHAR(255));
    DECLARE @States TABLE (State VARCHAR(255));
    DECLARE @ExistingCategories TABLE (Category VARCHAR(255));
    DECLARE @ItemList TABLE (Item VARCHAR(255));

    -- Check if categoryName is 'Others'
    IF @categoryName = 'Others'
    BEGIN
        -- Get all categories from the itemCategory table
        INSERT INTO @ExistingCategories (Category)
        SELECT categoryName
        FROM itemCategory
        WHERE categoryName <> 'Others';

        -- Iterate through each row in donation_publish
        DECLARE @itemCategory NVARCHAR(MAX);
        DECLARE @item NVARCHAR(255);

        DECLARE category_cursor CURSOR FOR
        SELECT DISTINCT dp.itemCategory
        FROM donation_publish dp;

        OPEN category_cursor;

        FETCH NEXT FROM category_cursor INTO @itemCategory;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Clean the itemCategory value by removing brackets
            SET @itemCategory = REPLACE(REPLACE(REPLACE(@itemCategory, '[', ''), ']', ''), ',', ', '); 
        
            -- Insert individual items into the @ItemList table variable
            INSERT INTO @ItemList (Item)
            SELECT TRIM(value) AS Item
            FROM STRING_SPLIT(@itemCategory, ',')
            WHERE TRIM(value) <> ''; -- Remove empty items

            -- Insert missing items into @Categories
            INSERT INTO @Categories (Category)
            SELECT DISTINCT Item
            FROM @ItemList
            WHERE Item NOT IN (SELECT Category FROM @ExistingCategories);

            -- Clear the @ItemList for the next iteration
            DELETE FROM @ItemList;

            FETCH NEXT FROM category_cursor INTO @itemCategory;
        END

        CLOSE category_cursor;
        DEALLOCATE category_cursor;

        -- Step 1: Filter donations with itemCategory not in @Categories (considered as "Others") and store the original rows in a temporary table
        DECLARE @OriginalFilteredDonations TABLE (
            donationPublishId varchar(500),
            title varchar(500),
            peopleNeeded INT,
            [address] VARCHAR(MAX),
            [description] VARCHAR(MAX),
            itemCategory VARCHAR(MAX),
            specificItemsForCategory VARCHAR(MAX) NULL,
            specificQtyForCategory VARCHAR(MAX) NULL,
            timeRange VARCHAR(MAX) NULL,
            urgentStatus VARCHAR(50),
            [status] VARCHAR(50),
            donationImage VARCHAR(MAX) NULL,
            donationAttch VARCHAR(MAX) NULL,
            orgId varchar(500),
            adminId varchar(500) NULL,
            created_on DATETIME,
            restriction VARCHAR(MAX) NULL,
            donationState VARCHAR(255),
            closureReason VARCHAR(MAX) NULL,
            resubmit varchar(500) NULL,
			rejectedReason varchar(500) NULL,
			recipientPhoneNumber varchar(500) NULL,
			recipientName varchar(500) NULL,
			approved_on DATETIME NULL,
			countdownEnded DATETIME NULL,
			newCountdownStart DATETIME NULL
        );

        -- Table to store filtered donations with original data
        INSERT INTO @OriginalFilteredDonations
        SELECT dp.*
        FROM donation_publish dp
        WHERE dp.status = 'Opened'
          AND EXISTS (
              SELECT 1
              FROM @Categories c
              WHERE CHARINDEX(c.Category, dp.itemCategory) = 0
          );

        -- Step 2: Further filter the stored donations based on the passed specificItems string
        SELECT DISTINCT ofd.*
        FROM @OriginalFilteredDonations ofd
        WHERE EXISTS (
            SELECT 1
            FROM STRING_SPLIT(@specificItems, ',') si
            CROSS APPLY (
                -- Split SpecificItemsForCategory by commas after removing brackets and parentheses
                SELECT TRIM(value) AS CleanedSpecificItem
                FROM STRING_SPLIT(
                    REPLACE(
                        REPLACE(
                            REPLACE(
                                REPLACE(ofd.SpecificItemsForCategory, '[', ''), ']', ''), '(', ''), ')', ''), 
                    ',')
            ) AS SplitSpecificItems
            WHERE LTRIM(RTRIM(SplitSpecificItems.CleanedSpecificItem)) LIKE '%' + LTRIM(RTRIM(si.value)) + '%'
        );

		  	SELECT * FROM @Categories;
		SELECT * FROM @ExistingCategories;
		SELECT * FROM @ItemList;
		SELECT * FROM @OriginalFilteredDonations;

	END
    ELSE
    BEGIN
        -- Split categories into table variable
        DECLARE @CategoryPos INT = 1;
        DECLARE @CategoryCommaPos INT;

        WHILE @CategoryPos <= LEN(@categoryName) + 1
        BEGIN
            SET @CategoryCommaPos = CHARINDEX(',', @categoryName + ',', @CategoryPos);
            INSERT INTO @Categories(Category)
            VALUES (LTRIM(RTRIM(SUBSTRING(@categoryName, @CategoryPos, @CategoryCommaPos - @CategoryPos))));

            SET @CategoryPos = @CategoryCommaPos + 1;
        END

        -- Split specific items into table variable
        DECLARE @ItemPos INT = 1;
        DECLARE @ItemCommaPos INT;
        DECLARE @CurrentCategory VARCHAR(255);
        DECLARE @CurrentItem VARCHAR(255);

        -- Loop through specific items and handle "All" case
        WHILE @ItemPos <= LEN(@specificItems) + 1
        BEGIN
            SET @ItemCommaPos = CHARINDEX(',', @specificItems + ',', @ItemPos);
            SET @CurrentItem = LTRIM(RTRIM(SUBSTRING(@specificItems, @ItemPos, @ItemCommaPos - @ItemPos)));
          
            BEGIN
                -- Assume specific items are categorized accordingly
                DECLARE @CategoryIndex INT = 1;
                DECLARE @CategoryCount INT = (SELECT COUNT(*) FROM @Categories);

                WHILE @CategoryIndex <= @CategoryCount
                BEGIN
                    SET @CurrentCategory = (SELECT Category FROM @Categories ORDER BY Category OFFSET @CategoryIndex - 1 ROWS FETCH NEXT 1 ROWS ONLY);
                    INSERT INTO @Items (Category, Item)
                    VALUES (@CurrentCategory, @CurrentItem);
                    SET @CategoryIndex = @CategoryIndex + 1;
                END
            END

            SET @ItemPos = @ItemCommaPos + 1;
        END

        -- Split states into table variable
        DECLARE @StatePos INT = 1;
        DECLARE @StateCommaPos INT;

        WHILE @StatePos <= LEN(@state) + 1
        BEGIN
            SET @StateCommaPos = CHARINDEX(',', @state + ',', @StatePos);
            INSERT INTO @States(State)
            VALUES (LTRIM(RTRIM(SUBSTRING(@state, @StatePos, @StateCommaPos - @StatePos))));

            SET @StatePos = @StateCommaPos + 1;
        END

        -- Filter donations based on categories, specific items, and state
        SELECT DISTINCT dp.*
        FROM donation_publish dp
        WHERE dp.status = 'Opened'
          AND (
              -- Match any of the selected states
              EXISTS (
                  SELECT 1
                  FROM @States s
                  WHERE dp.donationState LIKE '%' + s.State + '%'
              )
          )
          AND (
              -- Match donations that belong to any selected category
              EXISTS (
                  SELECT 1
                  FROM @Categories c
                  WHERE dp.itemCategory LIKE '%' + c.Category + '%'
                    AND (
                        -- Handle specific items filtering
                        EXISTS (
                            SELECT 1
                            FROM @Items i
                            WHERE i.Category = c.Category
                              AND (
                                  i.Item IS NULL
                                  OR dp.specificItemsForCategory LIKE '%' + i.Item + '%'
                              )
                        )
                    )
              )
          )
          AND (
              -- Ensure specific items are matched only for relevant categories
              EXISTS (
                  SELECT 1
                  FROM @Items i
                  WHERE i.Category IN (SELECT Category FROM @Categories)
                    AND (
                        i.Item IS NULL
                        OR (
                            dp.itemCategory LIKE '%' + i.Category + '%'
                            AND dp.specificItemsForCategory LIKE '%' + i.Item + '%'
                        )
                    )
              )
          );
    END


END






