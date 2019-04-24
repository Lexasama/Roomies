create procedure rm.sRoomiesCreate
(
    @FirstName   nvarchar(64),
    @LastName    nvarchar(64),
    @BirthDate   datetime2,
    @Phone       varchar(10),
    @UserId      nvarchar(64),
    @RoomieId    int out
)
as
begin
    set transaction isolation level serializable;
	begin tran;

	--if exists(select * from rm.tRoomies r where r.FirstName = @FirstName and r.LastName = @LastName)
	--begin
	--	rollback;
	--	return 1;
	--end;

	insert into rm.tRoomies(FirstName, LastName, BirthDate, Phone, Email)
	                  values(@FirstName, @LastName, @BirthDate, @Phone,(select u.Email from rm.tUser u where userId=@UserId));
	set @RoomieId = scope_identity();
	commit;
	return 0;
end;
