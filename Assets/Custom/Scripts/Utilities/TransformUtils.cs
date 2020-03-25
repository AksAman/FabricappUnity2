using helloVoRld.Core.Pooling;
using UnityEngine;

namespace helloVoRld.Utilities
{
    public static class TransformUtils
    {
        public static void ClearChilds(Transform parentTransform, ObjectPooler poolerToReturn)
        {
            int childCount = parentTransform.childCount;

            while (parentTransform.childCount > 0)
            {
                GameObject toRemove = parentTransform.GetChild(0).gameObject;
                poolerToReturn.ReturnObject(toRemove);

            }
        }
    }

}
