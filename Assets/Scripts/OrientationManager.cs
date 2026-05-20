using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    public GameObject rotateMessagePanel;
    
    void Update()
    {
        bool isMobile = Application.isMobilePlatform || SystemInfo.deviceType == DeviceType.Handheld;

        if (isMobile)
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
        else
        {
            rotateMessagePanel.SetActive(false);
        }
    }
}
