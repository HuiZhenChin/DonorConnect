USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[create_organization]    Script Date: 8/11/2024 3:57:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[create_organization]
    @method varchar(300),
    @orgId varchar(500) OUTPUT, 
    @orgName varchar(500),
    @orgEmail varchar(max),
    @orgContactNumber varchar(500),
    @orgHashPassword varchar(max),
    @orgNewHashPassword varchar(max) = NULL,
    @orgAddress varchar(max) = NULL,
    @businessLicenseImageBase64 varchar(max),
    @picName varchar(500),
    @picEmail varchar(max),
    @picContactNumber varchar(500),
    @orgProfilePicBase64 varchar(max) = NULL,
    @orgDescription varchar(max) = NULL,
    @mostNeededItemCategory varchar(500) = NULL,
    @orgRegion varchar(100),
    @orgStatus varchar(100),
    @adminId varchar(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @MESSAGE VARCHAR(500);

    IF (@method = 'INSERT')
    BEGIN
        -- Check if the organization already exists 
        IF NOT EXISTS(SELECT * FROM organization WHERE LOWER(orgName) = LOWER(@orgName))
        BEGIN
            -- Generate new organization ID
            SELECT @orgId = 'org' + CAST(ISNULL(MAX(CAST(SUBSTRING(orgId, 4, LEN(orgId) - 3) AS INT)), 0) + 1 AS varchar)
            FROM organization;

            -- Insert new organization
            INSERT INTO organization(orgId, orgName, orgEmail, orgContactNumber, orgHashPassword, orgNewHashPassword, orgAddress, businessLicenseImageBase64, picName, picEmail, picContactNumber, orgProfilePicBase64, orgDescription, mostNeededItemCategory, orgRegion, orgStatus, adminId, createdOn)
            VALUES (@orgId, @orgName, @orgEmail, @orgContactNumber, @orgHashPassword, @orgNewHashPassword, @orgAddress, @businessLicenseImageBase64, @picName, @picEmail, @picContactNumber, @orgProfilePicBase64, @orgDescription, @mostNeededItemCategory, @orgRegion, @orgStatus, @adminId, GETDATE());

			INSERT INTO [user] (username, email, role)
			VALUES (@orgName, @orgEmail, 'organization');

            SET @MESSAGE = 'Successful! You have registered as an organization! Your application is pending for approval now.';
        END
        ELSE
        BEGIN
            SET @MESSAGE = 'Organization already exists';
        END

        SELECT @MESSAGE AS MESSAGE;
    END
END

