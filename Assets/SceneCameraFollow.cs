using UnityEngine;

namespace EFT
{
    public class SceneCameraFollow : MonoBehaviour
    {
        private Camera _camera;

        private Camera Camera
        {
            get
            {
                if (_camera == null)
                    _camera = GetComponent<Camera>();

                return _camera;
            }
        }

        private void Update()
        {
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!enabled)
                return;

            if (Camera == null)
                return;

            if (UnityEditor.SceneView.currentDrawingSceneView == null ||
                UnityEditor.SceneView.currentDrawingSceneView.camera == null)
                return;

            Follow(Camera, UnityEditor.SceneView.currentDrawingSceneView.camera);
        }
#endif

        private static void Follow(Camera follower, Camera target)
        {
            var targetTransform = target.transform;
            var followerTransform = follower.transform;

            followerTransform.position = targetTransform.position;
            followerTransform.rotation = targetTransform.rotation;
        }
    }
}