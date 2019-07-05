create view rm.vCollocInfo
as
	select
		c.CollocId,
		CollocPic = c.CollocPic,
		CollocName = c.CollocName,
		CreationDate = c.CreationDate,
		r.RoomieId,
		Roomies = (select count(*) from rm.tiCollRoom ti where ti.CollocId = c.CollocId),		
		[RoomiesName] =coalesce ( [FirstName], r.FirstName)
	from rm.tColloc c
		inner join rm.tiCollRoom t on t.CollocId = c.CollocId
		left outer join rm.tRoomie r on r.RoomieId = t.RoomieId
	where c.CollocId <> 0;
