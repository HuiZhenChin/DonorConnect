USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[auto_approve_item_arrival]    Script Date: 8/11/2024 3:52:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[auto_approve_item_arrival]
AS
BEGIN

    UPDATE delivery
    SET approvalDeadline = DATEADD(MINUTE, 25, reachTimeByRider)
    WHERE itemApproved = 0 
      AND approvalDeadline IS NULL 
      AND reachTimeByRider IS NOT NULL;

    UPDATE dr
    SET dr.walletAmount = dr.walletAmount + 
                          TRY_CAST(d.paymentAmount AS DECIMAL(10, 2))
    FROM delivery_rider dr
    INNER JOIN delivery d ON dr.riderId = d.riderId
    WHERE d.itemApproved = 0
      AND d.approvalDeadline <= GETDATE();

    UPDATE delivery
    SET itemApproved = 1,
        autoApproved = 1
    WHERE itemApproved = 0
      AND approvalDeadline <= GETDATE();

END;




