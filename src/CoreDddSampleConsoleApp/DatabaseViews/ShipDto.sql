-- this is sqlite DB view

drop view if exists ShipDto;

create view ShipDto
as
select 
    Id as ShipId
    , Name
from Ship 