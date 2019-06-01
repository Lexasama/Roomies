create table rm.tIcon
(
	IconId int identity(0,1),
	IconName nvarchar(32) not null,
	
constraint PK_rm_tIcon primary key (IconId),
constraint UK_rm_tIcon_IconName unique(IconName),
);
insert into rm.tIcon(IconName) values (N'');
insert into rm.tIcon(IconName) values ('Icon0');
insert into rm.tIcon(IconName) values ('Icon1');
insert into rm.tIcon(IconName) values ('Icon2');
insert into rm.tIcon(IconName) values ('Icon3');
insert into rm.tIcon(IconName) values ('Icon4');
insert into rm.tIcon(IconName) values ('Icon5');
insert into rm.tIcon(IconName) values ('Icon6');
insert into rm.tIcon(IconName) values ('Icon7');
insert into rm.tIcon(IconName) values ('Icon8');
insert into rm.tIcon(IconName) values ('Icon9');
insert into rm.tIcon(IconName) values ('Icon10');
insert into rm.tIcon(IconName) values ('Icon11');
insert into rm.tIcon(IconName) values ('Icon12');
insert into rm.tIcon(IconName) values ('Icon13');
insert into rm.tIcon(IconName) values ('Icon14');
insert into rm.tIcon(IconName) values ('Icon15');
insert into rm.tIcon(IconName) values ('Icon16');
insert into rm.tIcon(IconName) values ('Icon17');
insert into rm.tIcon(IconName) values ('Icon18');
insert into rm.tIcon(IconName) values ('Icon19');
insert into rm.tIcon(IconName) values ('Icon20');



