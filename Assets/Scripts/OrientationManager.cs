using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    public GameObject rotateMessagePanel;
    
    void Update()
    {
        if(Screen.height > Screen.width)
        {
            rotateMessagePanel.SetActive(true);
        }
        else
        {
            rotateMessagePanel.SetActive(false);
        }
    }
}
