using ZZ;

public class TitleController : ViewBase
{
    public TitleScene.EStateType NextState { get; private set; }

    protected override void OnButtonClick(EButtonType buttonType)
    {
        switch (buttonType)
        {
            case EButtonType.Title_Start:
                NextState = TitleScene.EStateType.Start;
                break;
        }
    }
}
