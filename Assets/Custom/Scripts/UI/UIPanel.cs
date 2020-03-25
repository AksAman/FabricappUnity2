using UnityEngine;

namespace helloVoRld.Test.UI
{
    public class UIPanel : MonoBehaviour
    {
        public virtual void StartPanel()
        {
            this.gameObject.SetActive(true);

        }

        public virtual void StopPanel()
        {
            this.gameObject.SetActive(false);
        }
    } 
}
