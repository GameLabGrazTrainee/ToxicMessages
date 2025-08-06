using UnityEngine;

public class ThreatTrigger : MonoBehaviour


{

    public ToxicType Type;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            { return; }
        var playerController = other.gameObject.GetComponent<PlayerController>();
        var messageType = playerController.GetCurrentMessage().isToxic;
        if (messageType == Type)
        {
            playerController.IncreaseLife();
            playerController.ResetPosition();
            playerController.isCategoryMessageShown = true;
            playerController.rightCategoryMessage.gameObject.SetActive(true);
            playerController.wrongCategoryMessage.gameObject.SetActive(false);
            playerController.goodMessage.gameObject.SetActive(false);


        }
        else
        {
            playerController.ResetPosition();
            playerController.isCategoryMessageShown = true;
            playerController.wrongCategoryMessage.gameObject.SetActive(true);
            playerController.goodMessage.gameObject.SetActive(false);
            playerController.rightCategoryMessage.gameObject.SetActive(false);
        }
    }   

   


}
