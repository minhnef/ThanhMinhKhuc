using Unity.Cinemachine;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    public static CameraShaking Instance;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //<summary>
    /// Shake the camera with a given intensity.
    public void ShakeCamera(float intensity)
    {
        impulseSource.ImpulseDefinition.AmplitudeGain = intensity;
        impulseSource.GenerateImpulse();
    }
}
