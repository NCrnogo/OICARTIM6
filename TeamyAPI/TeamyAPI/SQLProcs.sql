CREATE PROC createUser
@name varchar(50),
@roll int,
@salt varchar(36),
@date char(10),
@pwd varchar(50),
@id int output
as
INSERT INTO Users 
VALUES (HASHBYTES('SHA2_512', @pwd),@salt,@name,@date)
SET @id=SCOPE_IDENTITY()
INSERT INTO UserRollMappings VALUES (@id,@roll)
GO

CREATE PROC getUsers
AS
	SELECT Users.*,UR.Roll AS 'Roll' FROM Users 
	LEFT JOIN UserRollMappings as URM ON URM.UserFK = Users.IDUser
	LEFT JOIN UserRoles as UR ON UR.IDUserRole = URM.UserRoleFK
GO

CREATE PROC getUser
@id int
AS
	SELECT Users.*,UR.Roll AS 'Roll' FROM Users 
	LEFT JOIN UserRollMappings as URM ON URM.UserFK = Users.IDUser
	LEFT JOIN UserRoles as UR ON UR.IDUserRole = URM.UserRoleFK
	WHERE Users.IDUser = @id
GO


CREATE PROC getTeams
AS
	SELECT * FROM Teams
GO


CREATE PROC getTeam
@id int
AS
	SELECT * FROM Teams
	WHERE Teams.IDTeam = @id
GO

CREATE PROC CheckLogin
@name varchar(50),
@pwd varchar(50)
AS
SELECT * FROM Users as s Where s.LoginName=@name AND HASHBYTES('SHA2_512', @pwd) = s.PasswordHash
GO

CREATE PROC updateUser
@name varchar(50),
@id int
AS
UPDATE Users
SET LoginName=@name
WHERE IDUser =@id
GO



CREATE PROC getUsersByTeam
	@id int
AS
	SELECT * FROM TeamMembers tm 
	INNER JOIN Users as us ON us.IDUser = tm.UserID
	WHERE tm.TeamID = @id
GO

CREATE PROC getUserInTeam
	@idteam int,
	@iduser int
AS
	SELECT * FROM TeamMembers tm
	INNER JOIN Users u on u.IDUser = tm.UserID
	WHERE TeamID = @idteam and UserID = @iduser
GO

CREATE PROC getTeamTeacher
	@id int
AS
	SELECT * FROM Teams t
	INNER JOIN Users s ON s.IDUser = t.TeacherID
	WHERE t.IDTeam = @id
GO

CREATE PROC getTeamOwner
	@id int
AS
	SELECT Users.* FROM Teams
	INNER JOIN Users ON Users.IDUser = Teams.OwnerID
	WHERE Teams.IDTeam=@id
GO

--User sends request to join to a team
CREATE PROC joinRequestUser
	@teamName nvarchar(50),
	@userId int
AS
	INSERT INTO TeamInvites VALUES ((Select IDTeam from Teams where Team like @teamName),@userId,0)
GO
--Team sends invite to user
CREATE PROC joinRequestTeam
	@teamid int,
	@userName nvarchar(100)
AS
	INSERT INTO TeamInvites VALUES (@teamid,(Select IDUser from Users where LoginName like @userName),1)
GO

CREATE PROC createTeam
	@idUser int,
	@name varchar(50),
	@created varchar(50)
AS
INSERT INTO Teams VALUES(@name,@created,@idUser,NULL)
GO

CREATE PROC getTeamInvites
	@idUser int
AS
SELECT DISTINCT TeamID, UserID, Invited, t.Team FROM TeamInvites 
INNER JOIN Teams as t on TeamID =t.IDTeam
WHERE UserID=@idUser AND Invited=1 
GO

CREATE PROC joinTeam
	@idUser int,
	@teamName varchar(50)
	AS
	INSERT INTO TeamMembers VALUES((SELECT IDTeam FROM Teams WHERE Team=@teamName),@idUser)
	DELETE FROM TeamInvites WHERE TeamID = (SELECT IDTeam FROM Teams WHERE Team=@teamName) AND UserID=@idUser
GO

CREATE PROC dismissJoinTeam
	@idUser int,
	@teamName varchar(50)
	AS
	DELETE FROM TeamInvites WHERE TeamID = (SELECT IDTeam FROM Teams WHERE Team=@teamName) AND UserID=@idUser
GO



CREATE PROC makeOwnerOfTeam
	@idUser int,
	@teamName varchar(50)
	AS
	INSERT INTO TeamMembers VALUES ((SELECT IDTeam FROM Teams WHERE Team=@teamName), (SELECT OwnerID FROM Teams WHERE IDTeam = ((SELECT IDTeam FROM Teams WHERE Team=@teamName))) )
	UPDATE Teams
	SET OwnerID = @idUser
	WHERE IDTeam = (SELECT IDTeam FROM Teams WHERE Team=@teamName)
	DELETE FROM TeamMembers WHERE TeamID = (SELECT IDTeam FROM Teams WHERE Team=@teamName) AND TeamMembers.UserID =@idUser
GO

CREATE PROC saveWork
	@idUser int,
	@teamName varchar(50),
	@name varchar(100),
	@description varchar(500),
	@start varchar(10),
	@end varchar(10),
	@date varchar(10)
	AS
	INSERT INTO UserWork VALUES (@date,@name,@description,@idUser,(SELECT IDTeam FROM Teams WHERE Team=@teamName),@start,@end)
GO

CREATE PROC getJoinRequests
	@idTeam int
AS
	SELECT * FROM TeamInvites ti 
	INNER JOIN Users u on u.IDUser = ti.UserID
	WHERE ti.TeamID = @idTeam AND ti.Invited = 0
GO

CREATE PROC JoinUserToTeam
	@idUser int,
	@idTeam int
AS
	INSERT INTO TeamMembers VALUES (@idTeam,@idUser)
	DELETE FROM TeamInvites WHERE TeamID = @idTeam and UserID = @idUser
GO

CREATE PROC DismissUserToTeam
	@idUser int,
	@idTeam int
AS
	DELETE FROM TeamInvites WHERE TeamID = @idTeam and UserID = @idUser
GO

CREATE PROC GetWork
	@idTeam int
AS
	SELECT * FROM UserWork 
	INNER JOIN Users on IDUser = UserFK
	where TeamFK = @idTeam
GO

ALTER PROC DismissWork
	@idWork int
AS
	INSERT INTO Deleted_UserWork(DateCreated,Name,Details,UserFK,TeamFK,StartWork,EndWork) SELECT uw.DateCreated,uw.Name,uw.Details,uw.UserFK,uw.TeamFK,uw.StartWork,uw.EndWork FROM UserWork uw where IDUserWork = @idWork
	DELETE FROM UserWork where IDUserWork = @idWork
GO

CREATE PROC AcceptUsersWork
	@idWork int
AS
	INSERT INTO UserWork(DateCreated,Name,Details,UserFK,TeamFK,StartWork,EndWork) SELECT uw.DateCreated,uw.Name,uw.Details,uw.UserFK,uw.TeamFK,uw.StartWork,uw.EndWork FROM Deleted_UserWork uw where uw.IDUserWork = @idWork
	DELETE FROM Deleted_UserWork where IDUserWork = @idWork
GO

CREATE PROC GetWorkDeleted
	@idTeam int
AS
	SELECT * FROM Deleted_UserWork
	INNER JOIN Users u on u.IDUser = UserFK
	where TeamFK = @idTeam
GO

CREATE PROC DeleteTeam
	@idTeam int
AS
	DELETE FROM TeamMembers WHERE TeamID = @idTeam
	DELETE FROM TeamInvites WHERE TeamID = @idTeam
	DELETE FROM UserWork WHERE TeamFK = @idTeam
	DELETE FROM Teams WHERE IDTeam = @idTeam
GO

-----NOVE PROCE

CREATE PROC MakeUserTeacher
	@idUser int,
	@idTeam int
AS
	IF((SELECT TeacherID FROM Teams WHERE IDTeam = @idTeam) IS NULL)
	BEGIN
		DELETE FROM TeamMembers WHERE UserID=@idUser 
		Update Teams SET TeacherID = @idUser where IDTeam = @idTeam
	END
	ELSE
	BEGIN
	SELECT * FROM TeamMembers
		INSERT INTO TeamMembers VALUES(@idTeam,(SELECT TeacherID from Teams where IDTeam=@idTeam))
		DELETE FROM TeamMembers WHERE UserID=@idUser
		Update Teams SET TeacherID = @idUser where IDTeam = @idTeam
	END
GO

ALTER PROC removeFromTeam
	@idUser int,
	@teamName varchar(50)
	AS
	DELETE FROM TeamMembers WHERE TeamID = (SELECT IDTeam FROM Teams WHERE Team=@teamName) AND TeamMembers.UserID =@idUser
	UPDATE Teams SET TeacherID = NULL where Team=@teamName
GO

ALTER PROC getTeamsByUser
	@id int
AS
	SELECT DISTINCT t.Team,t.* FROM Teams as t
	LEFT JOIN TeamMembers as tm ON tm.TeamID = t.IDTeam
	WHERE t.OwnerID = @id OR tm.UserID = @id OR t.TeacherID = @id
GO

CREATE PROC DeleteUser
	@idUser int
AS
	INSERT INTO Deleted_Users(LoginName,DateCreated,PasswordHash,Salt) select u.DateCreated,u.LoginName,u.PasswordHash,u.Salt from Users u where u.IDUser = @idUser
	DELETE tm FROM TeamMembers tm
	JOIN Teams t on t.IDTeam = tm.TeamID
	where t.OwnerID = @idUser
	DELETE ti FROM TeamInvites ti
	JOIN Teams t on t.IDTeam = ti.TeamID
	where t.OwnerID = @idUser
	DELETE FROM UserRollMappings where UserFK = @idUser
	DELETE t FROM UserWork t
	JOIN Teams u on t.TeamFK = u.IDTeam 
	where u.OwnerID = @idUser
	DELETE FROM Teams where OwnerID = @idUser
	update Teams set TeacherID = null where TeacherID = @idUser
	DELETE FROM Users where IDUser=@idUser
GO
