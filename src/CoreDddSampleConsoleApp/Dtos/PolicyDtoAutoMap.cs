using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace CoreDddSampleConsoleApp.Dtos
{
    public class PolicyDtoAutoMap : IAutoMappingOverride<PolicyDto>
    {
        public void Override(AutoMapping<PolicyDto> mapping)
        {
            mapping.Id(x => x.PolicyId);
        }
    }
}
