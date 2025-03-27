using Cysharp.Threading.Tasks;
using UnityEngine;
using Spine;
using Spine.Unity;

public class AttachObjectToBone : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation; // Referencia al SkeletonAnimation de Spine
    public string boneName; // El nombre del hueso al que deseas pegar el objeto
    public GameObject objectToAttach; // El objeto que deseas pegar al hueso

    public Vector2 positionOffset;
    
    private Bone bone; // Referencia al hueso de Spine

    void Start()
    {
        if (skeletonAnimation == null || objectToAttach == null)
        {
            // Debug.LogError("SkeletonAnimation or ObjectToAttach is not assigned.");
            return;
        }

        // Obtener el hueso por su nombre
        bone = skeletonAnimation.skeleton.FindBone(boneName);

        if (bone == null)
        {
            // Debug.LogError("Bone not found: " + boneName);
            return;
        }
    }

    public void Disable()
    {
        objectToAttach = null;
    }

    public async void SetObjectToAttach(GameObject _gameObject)
    {
        await UniTask.WaitUntil(() => skeletonAnimation != null);
        
        objectToAttach = _gameObject;
        SetupBone();
    }

    private void SetupBone()
    {
        bone = skeletonAnimation.skeleton.FindBone(boneName);
    }

    void LateUpdate()
    {
        if (bone == null) return;
        if (objectToAttach == null) return;
        
        // Obtener la posición y rotación del hueso en el espacio del mundo
        Vector3 boneWorldPosition = skeletonAnimation.transform.TransformPoint(new Vector3(bone.WorldX, bone.WorldY, 0));
        Quaternion boneWorldRotation = Quaternion.Euler(0, 0, 0);
        
        Vector3 offset = new Vector3(positionOffset.x, positionOffset.y, 0);
        
        // Actualizar la posición y rotación del objeto
        objectToAttach.transform.position = boneWorldPosition + offset;
        objectToAttach.transform.rotation = boneWorldRotation;
    }
}