using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public Transform fingers;

    private SceneManagement sceneManagment;
    private TouchInput touchInput;

    private void Start()
    {
        sceneManagment = FindObjectOfType<SceneManagement>();
        touchInput = FindObjectOfType<TouchInput>();

        Player.instance.Coins = 25;

        Debug.Log(sceneManagment.CurrentLevelName());
        //Checks to see if the current scene is the tutorial level
        if(sceneManagment.CurrentLevelName() == "Tutorial")
        {
            StartCoroutine(PlayTutorial());
        }
    }

    private IEnumerator PlayTutorial()
    {
        DataHelper.instance.ShowData("Welcome to the Tutorial Level!", DataHelper.Position.CENTER);

        yield return new WaitForSeconds(1f);

        DataHelper.instance.ShowData("To exit the Tutorial Level, press the button in the top right-", DataHelper.Position.MIDDLE_RIGHT);
        DataHelper.instance.PingObject("Pause Button");

        yield return new WaitForSeconds(4f);

        DataHelper.instance.ShowData("Then click the Home Button to return to the main menu.", DataHelper.Position.MIDDLE_RIGHT);

        yield return new WaitForSeconds(3f);

        DataHelper.instance.CancelPing(true);

        DataHelper.instance.ShowData("Now, let's move the camera.", DataHelper.Position.BOTTOM_RIGHT);

        yield return new WaitForSeconds(3f);

#if unity_android
        DataHelper.instance.ShowData("Use one finger to move the camera side to side.", DataHelper.Position.BOTTOM_RIGHT);
        fingers.GetComponent<Animator>().SetBool("Move Camera", true);


        float touchTime = 0;
        while(touchTime < 5)
        {
            if (touchInput.movingCamera)
            {
                touchTime += Time.smoothDeltaTime;
            }
            yield return null;
        }
#endif

#if unity_android
        fingers.GetComponent<Animator>().SetBool("Move Camera", false);

        DataHelper.instance.ShowData("Use two fingers to zoom in and out.", DataHelper.Position.BOTTOM_RIGHT);

        fingers.GetComponent<Animator>().SetBool("Zoom", true);

        touchTime = 0;
        while (touchTime < 5)
        {
            if (touchInput.zooming)
            {
                fingers.GetComponent<Animator>().SetBool("Zoom", false);
                touchTime += Time.smoothDeltaTime;
            }
            yield return null;
        }
#endif
        DataHelper.instance.ShowData("Let's place an Arrow Tower!", DataHelper.Position.MIDDLE_TOP);

        yield return new WaitForSeconds(3f);

        DataHelper.instance.ShowData("Click on the Arrow Tower icon to preview it!", DataHelper.Position.MIDDLE_TOP);

        DataHelper.instance.PingObject("Shop Button/Shop/ShopTowerItem (1)");

        while (TowerManager.instance.selectedTower == null)
        {
            yield return null;
        }

        Debug.Log("false");
        DataHelper.instance.CancelPing(true);

        yield return new WaitForSeconds(2f);

        DataHelper.instance.ShowData("Towers can only be placed on flat tiles shown by the blue icons.", DataHelper.Position.BOTTOM_RIGHT);
        SnapToGrid.instance.PingSnapPositions();

        yield return new WaitForSeconds(4f);

        DataHelper.instance.ShowData("Press the boxes in the top right to comfirm or cancel a tower purchase.", DataHelper.Position.BOTTOM_RIGHT);

        while (!TowerManager.instance.selectedTower.purchased)
        {
            yield return null;
        }

        SnapToGrid.instance.StopPingSnapPositions();

        DataHelper.instance.ShowData("Place as many towers as you can afford! Then press the Start Round button!", DataHelper.Position.BOTTOM_RIGHT);

        while (!GameManager.instance.RoundStarted)
        {
            yield return null;
        }

        yield return new WaitForSeconds(5f);
        GameManager.instance.PauseRound(false);

        DataHelper.instance.ShowData("White enemies only take one hit to kill.", DataHelper.Position.MIDDLE_BOTTOM);

    }

}
