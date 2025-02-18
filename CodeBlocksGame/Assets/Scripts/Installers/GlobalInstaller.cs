using Data;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private PlayerMovementData playerMovementData;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UserBalanceSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerInputSystem>().AsSingle().NonLazy();
            
            //Data
            Container.BindInstance(playerMovementData).AsSingle().NonLazy();
        }
    }
}
