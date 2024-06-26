#region

using Fusion;
using UnityEngine;

#endregion
struct NetworkInputData : INetworkInput
{
    public const byte Mousebutton0 = 1;
    public const byte Mousebutton1 = 2;

    public NetworkButtons Buttons;
    public Vector3 Direction;
}
