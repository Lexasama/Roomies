create view rm.vCollocPic
as
	select 
	CollocId = c.CollocId,
	CollocPic = c.CollocPic
	from rm.tColloc c
	where c.CollocId <> 0;