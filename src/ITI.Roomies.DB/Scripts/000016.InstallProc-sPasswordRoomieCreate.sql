create procedure rm.sPasswordRoomieCreate
(   
    @FirstName nvarchar(32),
    @LastName  nvarchar(32),
    @Email     nvarchar(64),
    @BirthDate datetime2,
    @Phone     char(10),
    @Password  varbinary(128),
	@RoomieId    int out
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if exists(select * from rm.tRoomie r where r.Email = @Email)
	begin
		rollback;
		return 1;
	end;

    insert into rm.tRoomie(FirstName, LastName, Email, BirthDate, Phone) 
				  values(@FirstName, @LastName, @Email, @BirthDate, @Phone);
    select @RoomieId = scope_identity();
    insert into rm.tPasswordRoomie(RoomieId, FirstName, LastName, Email, BirthDate, Phone,[Password])
                          values(@RoomieId, @FirstName, @LastName, @Email, @BirthDate, @Phone, @Password);
	commit;
    return 0;
end;
