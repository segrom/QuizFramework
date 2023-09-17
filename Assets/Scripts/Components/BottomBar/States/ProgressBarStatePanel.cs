using System.Collections;
using Models;
using UnityEngine;

namespace Components.BottomBar.States
{
    public abstract class BaseBottomBarState: MonoBehaviour
    {
        public abstract BottomBarStateType StateType { get; }

        public abstract IEnumerator Setup(TestModel test);
    }
}