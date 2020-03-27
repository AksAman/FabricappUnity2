using helloVoRld.Test.UI;
using UnityEngine;

namespace helloVoRld.Test.Managers
{
    public class SlideUIStateSystem : UIStateSystem
    {
        private AnimationManager animationManager => AnimationManager.Instance;
        [SerializeField] private readonly UIPanel fabricPanel;

        public override void ChangePanel(UIPanel newPanel)
        {
            base.ChangePanel(newPanel);
            fabricPanel.StopPanel();
            if (!animationManager.isSelectionOpen)
            {
                animationManager.AnimateSelectionPanel();
            }
        }
    }
}
