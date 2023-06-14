CREATE TABLE Likes (
    UserId INT FOREIGN KEY REFERENCES Users(Id) ON DELETE NO ACTION NOT NULL,
    PostId INT FOREIGN KEY REFERENCES Posts(Id) ON DELETE NO ACTION NOT NULL,
    PRIMARY KEY(UserId,PostID)
)
GO
CREATE TABLE Comments (
    PostId INT FOREIGN KEY REFERENCES Posts(Id) ON DELETE NO ACTION NOT NULL,
    CommentId INT FOREIGN KEY REFERENCES Posts(Id) ON DELETE NO ACTION NOT NULL,
)
GO

CREATE FUNCTION dbo.CheckFriendshipExists(@RequestedUserId INT, @UserId INT)
RETURNS BIT
AS
BEGIN
    DECLARE @FriendshipExists BIT;

    IF EXISTS (
        SELECT 1
        FROM UserFriend
        WHERE (
            UserId1 = @RequestedUserId AND UserId2 = @UserId
        ) OR (
            UserId1 = @UserId AND UserId2 = @RequestedUserId
        )
    )
        SET @FriendshipExists = 1;
    ELSE
        SET @FriendshipExists = 0;

    RETURN @FriendshipExists;
END;
GO

ALTER TABLE UserRequest
ADD CONSTRAINT CHK_FriendshipExists CHECK (dbo.CheckFriendshipExists(RequestedUserId, UserId) = 0);

