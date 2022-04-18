using DeliverableIA.Managers;
using UnityEngine;

namespace DeliverableIA.Core.Utils
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            CreateObject<GameManager>("Managers");
            CreateObject<SoundManager>("Managers");

            Debug.Log("<color=yellow><b>Bootstrap:</b> executed</color>");
        }

        private static void CreateObject<T>(string parent) where T : MonoBehaviour
        {
            if (Object.FindObjectOfType<T>() != null) return;
            
            GameObject par = GameObject.Find(parent);
            
            if (par == null) par = new GameObject(parent);
            
            par.transform.SetParent(null);
            par.AddComponent<T>();
        }
    }
}