using UnityEngine;

public class SubmitButton : MonoBehaviour {

    public void SubmitSelectedPlayer()
    {
        transform.parent.GetComponentInChildren<CookPlayerSelectButtonUI>().SubmitSelectPlayer();
        transform.parent.gameObject.SetActive(false);
    }
}
