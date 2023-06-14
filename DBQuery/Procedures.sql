CREATE PROCEDURE CreateUser
    @Email NVARCHAR(50),
    @Password NVARCHAR(50),
    @Name NVARCHAR(50)
AS
BEGIN
    INSERT INTO users (Email,Password,Name)
    VALUES (@Email, @Password, @Name)
END
GO

CREATE PROCEDURE CreatePost
    @UserId INT,
    @Content NVARCHAR(MAX),
    @ImageURL NVARCHAR(MAX),
    @GroupId INT = NULL
AS
BEGIN 
    INSERT INTO Posts(UserId,COntent,ImageURL,GroupId)
    VALUES(@UserId,@Content,@ImageURL,@GroupId)
END
GO


CREATE PROCEDURE AcceptFriend
    @RequesteUserId INT,
    @UserId INT
AS
BEGIN
    DELETE FROM UserRequest
        WHERE RequestedUserId = @RequesteUserId AND UserId = @UserId;
    INSERT INTO UserFriend(UserId1,UserId2)
        VALUES(@RequesteUserId,@UserId)
END
GO

CREATE OR ALTER PROCEDURE GetRequests
    @MyUserId INT
AS
BEGIN
    SELECT [f].[Id],
        [f].[Name],
        [f].[ImageURL]
     FROM UserRequest ur
    JOIN Users u ON u.Id = ur.RequestedUserId 
        JOIN Users f ON f.Id = ur.UserId
    WHERE u.id = @myUserId;
END
GO

CREATE OR ALTER PROCEDURE GetFriends
    @MyUserId INT
AS
BEGIN
    SELECT [f].[Id],
        [f].[Name],
        [f].[ImageURL]
    FROM Users u
        JOIN UserFriend uf ON (uf.UserId1 = @MyUserId OR uf.UserId2 = @MyUserId) 
        JOIN Users f ON (uf.UserId1 = f.Id OR uf.UserId2 = f.Id)
    WHERE u.Id = @MyUserId
        AND f.Id != @MyUserId;
END

