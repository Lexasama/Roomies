create view rm.vAuthenticationProvider
as
    select usr.UserId, usr.ProviderName
    from (select UserId = u.UserId,
              ProviderName = 'Roomies'
          from rm.tPasswordUser u
          union all
          select UserId = u.UserId,
              ProviderName = 'Google'
          from rm.tGoogleUser u) usr
    where usr.UserId <> 0;