
using System.Text.Json;
using Scada_Demo.MQTT_Model;
using Scada_Demo.ViewModels.Transactions;

namespace Scada_Demo.Services
{
    public class ProductionTransferResponseService
    {
        private readonly Start_ProductionViewModel _viewModel;

        public ProductionTransferResponseService(
            Start_ProductionViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void HandleResponse(string json)
        {
            try
            {
                var response =
                    JsonSerializer.Deserialize<ProductionTransferResponseModel>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (response == null)
                    return;

                Console.WriteLine(
                    $"Transfer Response: {response.Success} | {response.Message}");

                _viewModel.MqttTransferResponse(
                    response.RequestId ?? string.Empty,
                    response.Success,
                    response.Message ?? string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Transfer Response Error: {ex.Message}");
            }
        }
    }
}
