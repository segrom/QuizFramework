using System.Collections;
using Models;

namespace Components.BottomBar.States
{
    public class ResultsBottomBarState: BaseBottomBarState
    {
        public override BottomBarStateType StateType => BottomBarStateType.Results;

        public override IEnumerator Setup(TestModel test)
        {
            yield break;
        }
    }
}