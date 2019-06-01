create view rm.vRoomieCategory
as
	select 
		CategoryId = ctgr.CategoryId,
		 Icon = ctgr.IconName,
		CategoryName = ctgr.CategoryName,
		CollocId = ctgr.CollocId,
		RoomieId = ri.RoomieId,
		RoomieFirstName = ri.FirstName,
		RoomieLastName = ri.LastName
	from rm.tCategory ctgr 
		left outer join rm.tColloc cllc on cllc.CollocId = ctgr.CollocId
		 join rm.vRoomieInfo ri on ri.CollocId = ctgr.CollocId
	where ctgr.CategoryId <> 0;

	select * from rm.vRoomieInfo