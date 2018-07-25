using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace CoreDddSampleConsoleApp.Dtos
{
    public class ShipCargoPolicyItemDtoAutoMap : IAutoMappingOverride<ShipCargoPolicyItemDto>
    {
        public void Override(AutoMapping<ShipCargoPolicyItemDto> mapping)
        {
            mapping.Id(x => x.PolicyItemId);
        }
    }
}