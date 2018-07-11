-- this is sqlite DB view

drop view if exists PolicyDto;

create view PolicyDto
as
select 
    Id as PolicyId
    , Terms
from Policy 