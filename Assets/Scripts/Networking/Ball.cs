#region

using Fusion;

#endregion
public sealed class Ball : NetworkBehaviour
{
    [Networked] private TickTimer Life { get; set; }

    public override void FixedUpdateNetwork()
    {

        if (Life.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
        else
        {
            transform.position += 5 * transform.forward * Runner.DeltaTime;
        }
    }

    internal void Init()
    {
        Life = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }
}
