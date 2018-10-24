using System;
using System.Web.UI;
using CoreDdd.Commands;
using CoreDdd.Queries;
using CoreDddSampleWebAppCommon.Commands;
using CoreIoC;

namespace CoreDddSampleAspNetWebFormsApp
{
    public partial class ShipManagement : Page
    {
        private ICommandExecutor _commandExecutor;
        private IQueryExecutor _queryExecutor;

        protected void Page_Load(object sender, EventArgs e)
        {
            _commandExecutor = IoC.Resolve<ICommandExecutor>();
            _queryExecutor = IoC.Resolve<IQueryExecutor>();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            IoC.Release(_commandExecutor);
            IoC.Release(_queryExecutor);
        }

        protected void CreateNewShipButton_Click(object sender, EventArgs e)
        {
            _commandExecutor.CommandExecuted += args =>
            {
                var generatedShipId = (int)args.Args;
                LastGeneratedShipIdLabel.Text = $"{generatedShipId}";
            };

            _commandExecutor.Execute(new CreateNewShipCommand
            {
                ShipName = CreateShipNameTextBox.Text,
                Tonnage = int.Parse(CreateTonnageTextBox.Text)
            });

        }

        protected void UpdateShipButton_OnClick(object sender, EventArgs e)
        {
            _commandExecutor.Execute(new UpdateShipDataCommand
            {
                ShipId = int.Parse(UpdateShipIdTextBox.Text),
                ShipName = UpdateShipNameTextBox.Text,
                Tonnage = int.Parse(UpdateTonnageTextBox.Text)
            });
        }
    }
}