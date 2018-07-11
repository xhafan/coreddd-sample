using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace CoreDddSampleConsoleApp.Dtos
{
    public class ShipPolicyItemDtoAutoMap : IAutoMappingOverride<ShipPolicyItemDto>
    {
        public void Override(AutoMapping<ShipPolicyItemDto> mapping)
        {
            mapping.Id(x => x.PolicyItemId);
        }
    }
}