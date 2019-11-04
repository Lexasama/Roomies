create view rm.vUser
as
    select UserId = u.UserId,
           Email = u.Email,
           [Password] = case when p.[Password] is null then 0x else p.[Password] end,
           GoogleRefreshToken = case when gl.RefreshToken is null then '' else gl.RefreshToken end,
           GoogleId = case when gl.GoogleId is null then '' else gl.GoogleId end
    from rm.tUser u
        left outer join rm.tPasswordUser p on p.UserId = u.UserId
        left outer join rm.tGoogleUser gl on gl.UserId = u.UserId
    where u.UserId <> 0;