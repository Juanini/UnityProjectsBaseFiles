using System.Threading;
using Cysharp.Threading.Tasks;

public class ActionBlocker
{
    private bool canDoAction = true;
    private int millisecondsToBlock = 200;
    private CancellationTokenSource cancelToken;

    public void Setup(int _milliseconds)
    {
        millisecondsToBlock = _milliseconds;
    }
    
    public bool CanDoAction()
    {
        if (!canDoAction) return canDoAction;

        cancelToken = new CancellationTokenSource();
        canDoAction = false;
        DoActionBlockTimer(cancelToken.Token);

        return true;
    }

    public void Reset()
    {
        canDoAction = true;
        cancelToken?.Cancel();
    }

    private async void DoActionBlockTimer(CancellationToken cancellationToken)
    {
        await UniTask.Delay(millisecondsToBlock);
        canDoAction = true;
    }
}
