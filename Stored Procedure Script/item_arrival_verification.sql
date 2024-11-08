USE [DONOR_CONNECT]
GO
/****** Object:  StoredProcedure [dbo].[item_arrival_verification]    Script Date: 8/11/2024 4:00:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[item_arrival_verification]
	@method NVARCHAR(10),                
    @deliveryId varchar(500) = NULL,         
	@orgId varchar(500)= NULL,
	@link varchar(max)= NULL


AS
BEGIN
    SET NOCOUNT ON;

	IF (@method = 'APPROVAL')
    BEGIN

	 DECLARE 
        @EMAIL_TITLE varchar(100) = NULL,
        @CONTENT varchar(max) = NULL,
        @EMAIL varchar(300) = NULL,
        @CC_EMAIL varchar(300) = NULL,
        @orgName varchar(max) = NULL
       
        SELECT 
        @orgName = orgName,
        @EMAIL = orgEmail,
        @CC_EMAIL = picEmail  
        FROM organization 
        WHERE orgId = @orgId;

		SET @EMAIL_TITLE = 'DONATION ITEM ARRIVAL VERIFICATION FOR - ' + @orgName + ' ' + CONVERT(varchar(20), GETDATE(), 120);

        SET @CONTENT = 'Greetings, <br>' +
                        'We are pleased to inform you that your donation items with Delivery ID: ' + @deliveryId + ' have arrived at the destination.' + '<br>' +
                        'Could you please help us to verify the arrival by pressing the link below or login to your account <br>' +
                         @link + '<br>' +
                        'After this is approved by you, the donor will receive an acknowledgment receipt and the rider can claim the earnings.<br>' +
                        'Thank you for your time.<br>' +
						'Regards,' + '<br>' +
                        'DonorConnect Team'  + '<br>' +
                        '<br><br>' +	  
                        '********************************(DONORCONNECT)********************************<br>' +
                        'This mail is sent by server automatically, please do not reply directly.<br>' +
                        '******************************************************************************<br>';

        EXEC msdb.dbo.sp_send_dbmail
            @profile_name = 'Notification',
            @recipients = @EMAIL,
            @copy_recipients = @CC_EMAIL,
            @subject = @EMAIL_TITLE,
            @body = @CONTENT,
            @body_format = 'HTML';

   
	END

    
    
END


