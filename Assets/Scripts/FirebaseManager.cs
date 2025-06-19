using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    public TextMeshProUGUI uidText;

    private string userId;

    // Call From Js
    public void OnFirebaseUserIdReceived(string uid)
    {
        userId = uid;
        Debug.Log("Firebase UID: " + userId);
        if (uidText != null)
            uidText.text = "UID: " + userId;
    }

    public string GetUserId() => userId;
}
