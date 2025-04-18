USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[create_notifications]    Script Date: 8/11/2024 3:56:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[create_notifications]
	@method NVARCHAR(10),                
    @id varchar(100) = NULL,         
    @userId VARCHAR(MAX) = NULL,      
    @link VARCHAR(max) = NULL,   
    @content VARCHAR(max) = NULL,
	@supportingId varchar(500)= NULL
AS
BEGIN
    SET NOCOUNT ON;

    ---- Generate the new notificationId if it is NULL
    IF @id IS NULL
    BEGIN
        -- Create the new notificationId 
        SELECT @id = 'N' + CAST(ISNULL(MAX(CAST(SUBSTRING(notificationId, 2, LEN(notificationId) - 1) AS INT)), 0) + 1 AS VARCHAR)
        FROM notifications;
    END

    IF (@method = 'INSERT')
    BEGIN
	BEGIN
        
        -- Insert into notifications
        INSERT INTO notifications (notificationId, userId, link, content, created_on, supportingId)
        VALUES (@id, @userId, @link, @content, GETDATE(), @supportingId);

        
    END
	END
    
END
