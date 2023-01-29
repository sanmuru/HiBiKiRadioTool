using SamLu.Utility.HiBiKiRadio.Info;
using SamLu.Utility.HiBiKiRadio.Json;

namespace SamLu.Utility.HiBiKiRadio.Tasks;

public class InformationListTask : ApiTaskBase
{
    public InformationListTask() : base() { }

    protected virtual InformationInfo[] FetchCore(CancellationToken cancellationToken)
    {
        var informations = this.FetchAsAsync<information[]>(new Uri(ApiBase, "informations"), cancellationToken).Result;
        var length = informations.Length;
        var infos = new InformationInfo[informations.Length];
        for (int i = 0; i < length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            infos[i] = new(informations[i]);
        }

        return infos;
    }

    public Task<InformationInfo[]> FetchAsync(CancellationToken cancellationToken = default) =>
        Task.Factory.StartNew(() => this.FetchCore(cancellationToken), cancellationToken);
}
