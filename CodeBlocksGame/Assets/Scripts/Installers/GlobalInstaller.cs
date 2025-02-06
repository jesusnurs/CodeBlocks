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
            Container.BindInterfacesAndSelfTo<UserBalanceSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputSystem>().AsSingle();
            
            //Data
            Container.BindInstance(playerMovementData).AsSingle();
        }
    }
}
