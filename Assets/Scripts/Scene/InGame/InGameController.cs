using Game;
using UnityEngine;
using ZZ;

public class InGameController : ViewBase
{
    public InGameScene.EStateType NextState { get; private set; }

    protected override void OnButtonClick(EButtonType buttonType)
    {
        switch (buttonType)
        {
            case EButtonType.InGame_None:
                break;
            case EButtonType.Debug_OpenDebugMenu:
                break;
        }
    }
}
