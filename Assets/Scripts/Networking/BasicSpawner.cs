#region

using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion
public sealed class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{

    [SerializeField] private NetworkPrefabRef PlayerPrefab;
    private readonly Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private bool _mouseButton0;
    private bool _mouseButton1;
    private NetworkRunner _runner;

    private void Update()
    {
        _mouseButton0 = _mouseButton0 || Input.GetMouseButton(0);
        _mouseButton1 = _mouseButton1 || Input.GetMouseButton(1);
    }

    private void OnGUI()
    {
        if (_runner)
        {
            return;
        }
        if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
        {
            StartGame(GameMode.Host);
        }
        if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
        {
            StartGame(GameMode.Client);
        }
    }

    public void OnPlayerJoined([NotNull] NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer)
        {
            return;
        }
        // Create a unique position for the player
        Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
        NetworkObject networkPlayerObject = runner.Spawn(PlayerPrefab, spawnPosition, Quaternion.identity, player);
        // Keep track of the player avatars for easy access
        _spawnedCharacters.Add(player, networkPlayerObject);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (!_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            return;
        }
        runner.Despawn(networkObject);
        _spawnedCharacters.Remove(player);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
        {
            data.Direction += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            data.Direction += Vector3.back;
        }

        if (Input.GetKey(KeyCode.A))
        {
            data.Direction += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            data.Direction += Vector3.right;
        }

        data.Buttons.Set(NetworkInputData.Mousebutton0, _mouseButton0);
        _mouseButton0 = false;
        data.Buttons.Set(NetworkInputData.Mousebutton1, _mouseButton1);
        _mouseButton1 = false;

        input.Set(data);
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)                               { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)                                          { }
    public void OnConnectedToServer(NetworkRunner runner)                                                                { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)                               { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)   { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)           { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)                              { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)                                { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)                    { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)                             { }
    public void OnSceneLoadDone(NetworkRunner runner)                                                                    { }
    public void OnSceneLoadStart(NetworkRunner runner)                                                                   { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)                               { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)                              { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)          { }

    private async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        gameObject.AddComponent<RunnerSimulatePhysics3D>();
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        SceneRef scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on the game mode) a session with a specific name
        await _runner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
}
