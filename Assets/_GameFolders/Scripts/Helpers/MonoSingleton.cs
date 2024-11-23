using UnityEngine;

namespace _GameFolders.Scripts.Helpers
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static volatile T instance;

        /// <summary>
        /// Returns the instance of the MonoSingleton.
        /// </summary>
        public static T Instance => instance;

        [SerializeField] private bool dontDestroyOnLoad; // true : One object in game | false : One object in scene (object destroyed when scene closes)
        
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                
                if (dontDestroyOnLoad)
                {
                    transform.parent = null; // Parent must be null for object to be "DontDestroyOnLoad"
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
