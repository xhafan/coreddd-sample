-- this is sqlite DB view

drop view if exists ShipCargoPolicyItemDto;

create view ShipCargoPolicyItemDto
as
select 
    item.Id			as PolicyItemId
    , p.Id			as PolicyId
    , shipCargoItem.ShipId
    , s.Name		as ShipName
from Policy p
join PolicyItem item on item.PolicyId = p.Id
join CargoPolicyItem cargoItem on cargoItem.PolicyItemId = item.Id
join ShipCargoPolicyItem shipCargoItem on shipCargoItem.CargoPolicyItemId = item.Id
join Ship s on s.Id = shipCargoItem.ShipId