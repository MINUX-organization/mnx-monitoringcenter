using MediatR;

namespace MNX.MonitoringCenter.Inventory.UseCases.InternetAdapter;

using InternetAdapter = Contracts.InternetAdapter.InternetAdapter;

internal class GetInternetAdaptersInfoQuery : IStreamRequest<InternetAdapter>
{
}
