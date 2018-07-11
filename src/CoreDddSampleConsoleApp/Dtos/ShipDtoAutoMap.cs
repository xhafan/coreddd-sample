using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace CoreDddSampleConsoleApp.Dtos
{
    public class ShipDtoAutoMap : IAutoMappingOverride<ShipDto>
    {
        public void Override(AutoMapping<ShipDto> mapping)
        {
            mapping.Id(x => x.ShipId);
        }
    }
}