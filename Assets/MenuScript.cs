using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] GameObject patchNotesWindow;
   public void Click_PlayGame(){
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
       Debug.Log("Go to game scenes");
   }
   public void Click_QuitGame(){
       Application.Quit();
       Debug.Log("Closing application");
   }

    public void Click_ClosePatchNotes(){
       patchNotesWindow.SetActive(false);
       this.transform.gameObject.SetActive(true);
       Debug.Log("Patch notes is closed."); 
    }
   public void Click_OpenPatchNotes(){
       patchNotesWindow.SetActive(true);
       HideMenu();
       Debug.Log("Patch notes is open.");
   }
   private void HideMenu(){
       this.transform.gameObject.SetActive(false);
   }
}
