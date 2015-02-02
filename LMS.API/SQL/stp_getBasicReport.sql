SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<John Lay>
-- Create date: <02 Feb 2015>
-- Description:	<Get a basic report>
-- =============================================
CREATE PROCEDURE stp_getBasicReport
	(@client_id	int)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	select DISTINCT TOP 1000 
		usersinsession.LearningComplete, 
		[user].UserId,
		[user].FirstName, 
		[user].LastName,
		[user].EmailAddress,
		ugroup.Name AS UserGroupName,
		[session].StartDate,
		[session].EndDate,
		[session].IsRolling,
		course.Name AS CourseName,
		course.CourseType,
		category.Name AS CourseCategoryName
	FROM [LMS].[dbo].UsersInCourseSession usersinsession
	INNER JOIN [User] [user] ON usersinsession.UserId = [user].UserId
	INNER JOIN UsersInUserGroup uugroup ON usersinsession.UserId = uugroup.UserId
	INNER JOIN UserGroup ugroup ON usersinsession.UserId = [user].UserId
	INNER JOIN CourseSession [session] ON [session].CourseSessionId = usersinsession.CourseSessionId
	INNER JOIN Course course ON [session].CourseId = course.CourseId
	INNER JOIN CoursesInCourseGroup ccg ON course.CourseId = ccg.CourseId
	INNER JOIN CourseCategory category ON ccg.CourseCategoryId = category.CourseCategoryId
	INNER JOIN Client client ON course.ClientId = client.ClientId
	WHERE client.ClientId = @client_id;
END
GO
