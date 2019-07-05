create view rm.vRoomieInfo 
as
	select
		RoomieId = i.RoomieId,
		FirstName = r.FirstName,
		LastName = r.LastName,
		CollocId
	from rm.tiCollRoom i
		inner join rm.tRoomie r on i.RoomieId = r.RoomieId
	where i.CollocId <>0;					 