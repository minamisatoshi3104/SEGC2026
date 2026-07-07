using UnityEngine;

namespace ZZ
{
    public enum EButtonType
    {
        None = 0,

        // 汎用
        [InspectorName("Common/Yes")] Common_Yes = 1,
        [InspectorName("Common/No")] Common_No = 2,
        [InspectorName("Common/Close")] Common_Close = 3,
        [InspectorName("Common/Option")] Common_Option = 10,

        // タイトル
        [InspectorName("Title/Start")] Title_Start = 1001,

        // マップ選択
        [InspectorName("MapSelect/Play")] MapSelect_Play = 2001,
        [InspectorName("MapSelect/ToTitle")] MapSelect_ToTitle = 2002,

        // インゲーム
        [InspectorName("InGame/None")] InGame_None = 3001,

        // デバッグ
        [InspectorName("Debug/OpenDebugMenu")] Debug_OpenDebugMenu = 9001,
    }
}