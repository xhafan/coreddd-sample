-- this is sqlite DB view

drop view if exists ShipPolicyItemDto;

create view ShipPolicyItemDto
as
select 
    item.Id as PolicyItemId
    , p.Id as PolicyId
    , shipItem.ShipId
    , s.Name     as ShipName
from Policy p
join PolicyItem item on item.PolicyId = p.Id
join CargoPolicyItem cargoItem on cargoItem.PolicyItemId = item.Id
join ShipPolicyItem shipItem on shipItem.CargoPolicyItemId = item.Id
join Ship s on s.Id = shipItem.ShipId