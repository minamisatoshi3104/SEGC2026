using ZZ;

/// <summary>
/// マップ選択の表示管理
/// </summary>
public class MapSelectController : ViewBase
{
    public MapSelectScene.EStateType NextState { get; private set; }

    protected override void OnButtonClick(EButtonType buttonType)
    {
        switch (buttonType)
        {
            case EButtonType.Common_Option:
                if(true)
                {
                    NextState = MapSelectScene.EStateType.MenuClose;
                }
                break;
            case EButtonType.Common_Close:
                NextState = MapSelectScene.EStateType.Menu;
                break;
            case EButtonType.MapSelect_Play:
                NextState = MapSelectScene.EStateType.ToInGame;
                break;
            case EButtonType.MapSelect_ToTitle:
                NextState = MapSelectScene.EStateType.ToTitle;
                break;
        }
    }
}
