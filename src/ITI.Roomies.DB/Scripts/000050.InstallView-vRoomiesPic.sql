create view rm.vRoomiesPic
as
	select 
		RoomieId = r.RoomieId,
		RoomiePic = r.RoomiePic
	from rm.tRoomie r
	where r.RoomieId <>0;