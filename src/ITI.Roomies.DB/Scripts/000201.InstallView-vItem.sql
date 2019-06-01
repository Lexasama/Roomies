create view rm.vItem
as
	select  RoomieId = r.RoomieId,
			ItemId = i.ItemId,
			[Name] = i.ItemName,
			Price = i.ItemPrice 

	from rm.tItem i
	left outer join rm.tCourse c on c.CourseId = i.CourseId
	left outer join rm.tRoomie r on r.RoomieId = i.RoomieId
	where r.RoomieId <> 0;