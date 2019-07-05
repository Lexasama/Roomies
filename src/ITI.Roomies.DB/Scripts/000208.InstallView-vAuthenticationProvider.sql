create view rm.vAuthenticationProvider
as
    select usr.RoomieId, usr.ProviderName
    from (select RoomieId = r.RoomieId,
              ProviderName = 'Roomies'
          from rm.tPasswordRoomie r
          union all
          select RoomieId = r.RoomieId,
              ProviderName = 'Google'
          from rm.tGoogleRoomie r) usr
    where usr.RoomieId <> 0;