namespace Application.Gateways.Dtos
{
    public class GatewayOutput
    {
        public bool Success { get; private set; }

        public string Message { get; private set; }

        private GatewayOutput(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static GatewayOutput Succeeded() => new GatewayOutput(true, string.Empty);

        public static GatewayOutput Failed(string message) => new GatewayOutput(false, message);
    }
}
