create proc rm.sRoomiesUpdateCreate
(
	@RoomieId	 int, 
	@FirstName	 nvarchar(32),
	@LastName	 nvarchar(32),
	@Phone		 varchar(10),
	@BirthDate	 Datetime2
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from rm.tRoomie r where r.RoomieId = @RoomieId)
	begin
		rollback;
		return 1;
	end;

	update rm.tRoomie
	set FirstName = @FirstName, LastName = @LastName, BirthDate = @BirthDate, Phone = @Phone
	where RoomieId = @RoomieId;

	commit;
    return 0;
end;
