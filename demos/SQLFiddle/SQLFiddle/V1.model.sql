﻿create table Fiddles
( SHA1 binary(16) primary key
, CreatedUtc datetime default(sysutcdatetime())
, Backend string(8)
, Model string(4096)
, Command string(4096)
, Valid bool
, Deleted bool default(false)
);

create index IX_Fiddles_Deleted on Fiddles(Deleted);

create table StandardFiddles
( Id int primary key autoincrement
, Title string(128)
, SHA1 binary(16) references Fiddles(SHA1)
);