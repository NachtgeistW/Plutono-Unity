using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This is a demo class that adds effects when user clicks on the scene
public class Clicker:MonoBehaviour{

	//This will hold the reference to FlatFX object. We'll call it to add new effects
	FlatFX flatfx;

	//This one will hold effect that is selected right now
	int selectedEffect=0;

	//This will be set to number of effects. Currently it's 4 but could be expanded in the future
	int maxEffect=0;

	//This will reference the main camera
	Camera cam;

	//This will reference the text label with number of effect
	Text text;

	void Start(){
		//Retrieving a reference to FlatFX object
		flatfx=GameObject.Find("FlatFX").GetComponent<FlatFX>();
		//Retrieving a reference to the the camera
		cam=GetComponent<Camera>();
		//Retrieving a number of defined effects
		maxEffect=FlatFXType.GetNames(typeof(FlatFXType)).Length;
		//Retrieving a reference to the text
		text=GameObject.Find("LabelText").GetComponent<Text>();
		//Setting label to the effect which is currently selected
		UpdateLabel();
	}

	//Each frame we listen to clicks and adding a new effect
	void Update(){
		//When user clicks and it's not a click on UI button
		if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()){
			//Getting position of user click and adding effect in that point
			Vector2 point=cam.ScreenToWorldPoint(Input.mousePosition);
			//Deciding which effect to show
			int effect=selectedEffect;
			//Effects are numbered from 0 to 4. If it's set to -1 we select a random one
			if(effect==-1) effect=Random.Range(0,maxEffect);
			//Adding a new effect to the FlatFX object
			flatfx.AddEffect(point,effect);
		}
	}

	//When user clicks "Next" button
	public void FXNext(){
		selectedEffect++;
		if(selectedEffect>maxEffect-1){
			selectedEffect=-1;
		}
		UpdateLabel();
	}

	//When user clicks "Previous" button
	public void FXPrev(){
		selectedEffect--;
		if(selectedEffect<-1){
			selectedEffect=maxEffect-1;
		}
		UpdateLabel();
	}

	//Updating text on the bottom of the screen
	public void UpdateLabel(){
		if(selectedEffect!=-1) text.text=selectedEffect.ToString();
		else text.text="Random";
	}

}
