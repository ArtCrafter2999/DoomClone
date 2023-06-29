using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform spawnPoint;
    public override void InstallBindings()
    {
        var playerInstance = Container.InstantiatePrefabForComponent<Player>(
            player, spawnPoint.position, Quaternion.identity, null);
        Container
            .Bind<Player>()
            .FromInstance(playerInstance)
            .AsSingle();
    }
}
