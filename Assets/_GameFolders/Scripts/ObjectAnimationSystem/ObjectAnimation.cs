using System.Collections.Generic;
using System.Threading.Tasks;
using _GameFolders.Scripts.Interfaces;

namespace _GameFolders.Scripts.ObjectAnimationSystem
{
    public class ObjectAnimation
    {
        private static ObjectAnimation _instance;
        public static ObjectAnimation Instance => _instance ??= new ObjectAnimation();

        public async Task ExecuteAnimationsAsync(IEnumerable<IAnimation> animations)
        {
            foreach (var animation in animations)
            {
                await animation.ExecuteAsync();
            }
        }
    }
}