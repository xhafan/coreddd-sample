-- this is sqlite DB view

drop view if exists ShipDto;

create view ShipDto
as
select 
    Id
    , Name
from Ship 