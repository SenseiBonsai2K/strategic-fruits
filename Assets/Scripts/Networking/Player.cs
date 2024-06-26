#region

using Fusion;
using UnityEngine;

#endregion
public sealed class Player : NetworkBehaviour
{

    [SerializeField] private Ball PrefabBall;
    [SerializeField] private PhysxBall PrefabPhysxBall;
    public Material Material;
    private ChangeDetector _changeDetector;
    private Vector3 _forward;
    private NetworkCharacterController _networkCharacterController;
    [Networked] public bool SpawnedProjectile { get; set; }
    [Networked] private TickTimer Delay { get; set; }

    private void Awake()
    {
        _networkCharacterController = GetComponent<NetworkCharacterController>();
        _forward = transform.forward;
        Material = GetComponentInChildren<MeshRenderer>().material;
    }

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void Render()
    {
        foreach (string change in _changeDetector.DetectChanges(this))
        {
            Material.color = change switch
            {
                nameof(SpawnedProjectile) => Color.white,
                _ => Material.color
            };
        }
        Material.color = Color.Lerp(Material.color, Color.blue, Time.deltaTime);
    }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out NetworkInputData data))
        {
            return;
        }
        data.Direction.Normalize();
        _networkCharacterController.Move(5 * data.Direction * Runner.DeltaTime);

        if (data.Direction.sqrMagnitude > 0)
        {
            _forward = data.Direction;
        }

        if (!HasStateAuthority || !Delay.ExpiredOrNotRunning(Runner))
        {
            return;
        }
        if (data.Buttons.IsSet(NetworkInputData.Mousebutton0))
        {
            Delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            Runner.Spawn(PrefabBall,
                transform.position + _forward,
                Quaternion.LookRotation(_forward),
                Object.InputAuthority, static (_, o) =>
                {
                    // Initialize the Ball before synchronizing it
                    o.GetComponent<Ball>().Init();
                });
            SpawnedProjectile = !SpawnedProjectile;
        }
        else if (data.Buttons.IsSet(NetworkInputData.Mousebutton1))
        {
            Delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            Runner.Spawn(PrefabPhysxBall,
                transform.position + _forward,
                Quaternion.LookRotation(_forward),
                Object.InputAuthority,
                (_, o) =>
                {
                    o.GetComponent<PhysxBall>().Init(10 * _forward);
                });
            SpawnedProjectile = !SpawnedProjectile;
        }
    }
}
