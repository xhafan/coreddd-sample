using System;
using CoreDdd.Commands;

namespace CoreDddSampleConsoleApp.Samples.Ddd
{
    public class CreateNewPolicyCommand : ICommand
    {
        public int PolicyHolderId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Terms { get; set; }
    }
}