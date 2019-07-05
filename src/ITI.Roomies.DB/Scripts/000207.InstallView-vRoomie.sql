create view rm.vRoomie
as
    select RoomieId = r.roomieId,
           Email = r.Email,
           [Password] = case when p.[Password] is null then 0x else p.[Password] end,
           GoogleRefreshToken = case when gl.RefreshToken is null then '' else gl.RefreshToken end,
           GoogleId = case when gl.GoogleId is null then '' else gl.GoogleId end
    from rm.tRoomie r
        left outer join rm.tPasswordRoomie p on p.RoomieId = r.RoomieId
        left outer join rm.tGoogleroomie gl on gl.roomieId = r.RoomieId
    where r.RoomieId <> 0;