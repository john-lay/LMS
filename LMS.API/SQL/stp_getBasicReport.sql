SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<John Lay>
-- Create date: <14 Jan 2015>
-- Description:	<Get a basic report>
-- =============================================
CREATE PROCEDURE stp_getBasicReport
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1000 [ClientId]
      ,[Name]
      ,[LogoTitle]
      ,[LogoResource]
	FROM [LMS].[dbo].[Client]
END
GO
