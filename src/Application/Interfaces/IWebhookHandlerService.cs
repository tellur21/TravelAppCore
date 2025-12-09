using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWebhookHandlerService
    {
        Task ProcessWebhookEventAsync(string json, string signature);
    }
}