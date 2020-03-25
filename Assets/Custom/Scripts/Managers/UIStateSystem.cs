using UnityEngine;
using UnityEngine.UI;
using helloVoRld.Test.UI;
using System.Collections.Generic;
using helloVoRld.Utilities.Debugging;

namespace helloVoRld.Test.Managers
{
    public class UIStateSystem : MonoBehaviour
    {
        [SerializeField] private UIPanel startPanel;

        [SerializeField] private UIPanel currentPanel;
        [SerializeField] private UIPanel previousPanel;

        [SerializeField] private Stack<UIPanel> panelsVisited = new Stack<UIPanel>();
        private void Start()
        {
            currentPanel = startPanel;
            ChangePanel(startPanel);
        }
        public virtual void ChangePanel(UIPanel newPanel)
        {
            if(newPanel)
            {
                if (previousPanel)
                {
                    panelsVisited.Push(previousPanel);
                }
                previousPanel = currentPanel;
                previousPanel.StopPanel();

                currentPanel = newPanel;
                currentPanel.StartPanel();
            }
        }


        public virtual void BackToPreviousPanel()
        {
            if(panelsVisited.Count > 0)
            {
                previousPanel = currentPanel;
                currentPanel.StopPanel();
                var newPanel = panelsVisited.Pop();
                DebugHelper.Log(newPanel.gameObject.name);

                currentPanel = newPanel;
                newPanel.StartPanel();

                //ChangePanel(newPanel);
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                DebugHelper.Log("Back");
                BackToPreviousPanel();
            }
        }
    }
}
