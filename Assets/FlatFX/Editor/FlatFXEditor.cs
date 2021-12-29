using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System;
using System.Reflection;

[CustomEditor(typeof(FlatFX))]
public class FlatFXEditor:Editor{

	private FlatFX script;

	private int particleCount=0;
	private int triangleCount=0;

	void Awake(){
		script=(FlatFX)target;
		//script.runInEditMode=true;
	}

	[MenuItem("GameObject/Effects/FlatFX")]
	static void Create(){
		GameObject go=new GameObject();
		go.AddComponent<FlatFX>();
		go.name="FlatFX";
		FlatFX script=go.GetComponent<FlatFX>();
		//Set default settings
		TextAsset jsonAsset=(TextAsset)Resources.Load("FlatFXSettingsDefault",typeof(TextAsset));
		JsonUtility.FromJsonOverwrite(jsonAsset.text,script);
		//Select the new object
		Selection.activeGameObject=go;
	}

	public override bool RequiresConstantRepaint(){
		return script.particles.Count>0;
	}

	public override void OnInspectorGUI(){
		int labelWidth=120;
		EditorGUIUtility.labelWidth=labelWidth+4;

		//Export/Import presets
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("Manage presets","Export settings for all effects to an external file or import them back"),GUILayout.Width(labelWidth));
		if(GUILayout.Button(new GUIContent("Import...","Import FlatFX settings tro a JSON file."))) LoadSettings();
		if(GUILayout.Button(new GUIContent("Export...","Export FlatFX settings from a previously exported JSON file."))) SaveSettings();
		EditorGUILayout.EndHorizontal();

		//Show particle count and triangle count
		EditorGUILayout.BeginHorizontal();
		GUILayout.Box(new GUIContent("Effect count: "+script.particleCount.ToString()),EditorStyles.helpBox);
		GUILayout.Box(new GUIContent("Triangle count: "+script.triangleCount.ToString()),EditorStyles.helpBox);
		EditorGUILayout.EndHorizontal();

		//FX toolbar
		string[] enumstrings=FlatFXType.GetNames(typeof(FlatFXType));
		Texture[] buttons=new Texture[enumstrings.Length];
		for(int i=0;i<buttons.Length;i++){
			buttons[i]=(Texture)Resources.Load("Icons/FX"+enumstrings[i]);
		}
		int switchState=GUILayout.Toolbar(script.selectedEffect,buttons,GUILayout.Height(28));
		if(switchState!=script.selectedEffect){
			GUI.FocusControl(null);
			script.selectedEffect=switchState;
		}

		//Decide which settings to show and edit
		FlatFXSettings showSettings=script.settings[switchState];

		//Sector count
		int sectorCount=EditorGUILayout.IntField(new GUIContent("Sector count","Determines how many sectors the effect will have"),showSettings.sectorCount);
		if(sectorCount!=showSettings.sectorCount){
			Undo.RecordObject(script,"Edit settings");
			showSettings.sectorCount=sectorCount;
			EditorUtility.SetDirty(script);
		}

		//Lifetime
		float lifetime=EditorGUILayout.FloatField(new GUIContent("Lifetime","How long the effect will exist"),showSettings.lifetime);
		if(lifetime!=showSettings.lifetime){
			Undo.RecordObject(script,"Edit settings");
			showSettings.lifetime=lifetime;
			EditorUtility.SetDirty(script);
		}

		//Easing function
		Easings.Functions easing=(Easings.Functions)EditorGUILayout.EnumPopup(new GUIContent("Easing function","The function that specifies how this effect will be animated"),showSettings.easing);
		if(easing!=showSettings.easing){
			Undo.RecordObject(script,"Edit settings");
			showSettings.easing=easing;
			EditorUtility.SetDirty(script);
		}

		//Randomize position
		float randomizePosition=EditorGUILayout.FloatField(new GUIContent("Randomize position","Sets the max distance where the effect will appear"),showSettings.randomizePosition);
		if(randomizePosition!=showSettings.randomizePosition){
			Undo.RecordObject(script,"Edit settings");
			showSettings.randomizePosition=randomizePosition;
			EditorUtility.SetDirty(script);
		}

		//Labels for start and end
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(" ",GUILayout.Width(labelWidth));
		GUILayout.Label(new GUIContent("Start:","Starting conditions for the effect"));
		GUILayout.Label(new GUIContent("End:","Final conditions for the effect"));
		EditorGUILayout.EndHorizontal();

		//Size
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("Size:","Sets both width and height of the effect"),GUILayout.Width(labelWidth));
		float startSize=EditorGUILayout.FloatField(showSettings.start.size);
		if(startSize!=showSettings.start.size){
			Undo.RecordObject(script,"Edit settings");
			showSettings.start.size=startSize;
			EditorUtility.SetDirty(script);
		}
		float endSize=EditorGUILayout.FloatField(showSettings.end.size);
		if(endSize!=showSettings.end.size){
			Undo.RecordObject(script,"Edit settings");
			showSettings.end.size=endSize;
			EditorUtility.SetDirty(script);
		}
		EditorGUILayout.EndHorizontal();

		//Thickness
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("Thickness:","How much of the total size the effect will fill"),GUILayout.Width(labelWidth));
		float startThickness=EditorGUILayout.FloatField(showSettings.start.thickness);
		if(startThickness!=showSettings.start.thickness){
			Undo.RecordObject(script,"Edit settings");
			showSettings.start.thickness=startThickness;
			EditorUtility.SetDirty(script);
		}
		float endThickness=EditorGUILayout.FloatField(showSettings.end.thickness);
		if(endThickness!=showSettings.end.thickness){
			Undo.RecordObject(script,"Edit settings");
			showSettings.end.thickness=endThickness;
			EditorUtility.SetDirty(script);
		}
		EditorGUILayout.EndHorizontal();

		//Inner color
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("Inner color:","Sets color for inner vertices"),GUILayout.Width(labelWidth));
		Color startInnerColor=EditorGUILayout.ColorField(showSettings.start.innerColor);
		if(startInnerColor!=showSettings.start.innerColor){
			Undo.RecordObject(script,"Edit settings");
			showSettings.start.innerColor=startInnerColor;
			EditorUtility.SetDirty(script);
		}
		Color endInnerColor=EditorGUILayout.ColorField(showSettings.end.innerColor);
		if(endInnerColor!=showSettings.end.innerColor){
			Undo.RecordObject(script,"Edit settings");
			showSettings.end.innerColor=endInnerColor;
			EditorUtility.SetDirty(script);
		}
		EditorGUILayout.EndHorizontal();

		//Outer color
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(new GUIContent("Outer color:","Sets color for outer vertices"),GUILayout.Width(labelWidth));
		Color startOuterColor=EditorGUILayout.ColorField(showSettings.start.outerColor);
		if(startOuterColor!=showSettings.start.outerColor){
			Undo.RecordObject(script,"Edit settings");
			showSettings.start.outerColor=startOuterColor;
			EditorUtility.SetDirty(script);
		}
		Color endOuterColor=EditorGUILayout.ColorField(showSettings.end.outerColor);
		if(endOuterColor!=showSettings.end.outerColor){
			Undo.RecordObject(script,"Edit settings");
			showSettings.end.outerColor=endOuterColor;
			EditorUtility.SetDirty(script);
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(10);

		//Test button
		EditorGUILayout.BeginHorizontal();
		GUILayout.Box(new GUIContent("Effect #"+switchState.ToString()+": "+enumstrings[switchState]),EditorStyles.helpBox,GUILayout.Width(labelWidth));
		if(GUILayout.Button(new GUIContent("Test","Add this effect to the scene. Keep in mind that framerate is lower when in edit mode. Hit play to test effects in proper framerate."))) script.AddEffect(script.transform.position,switchState);
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(10);

		bool useUnscaledTime=EditorGUILayout.Toggle(new GUIContent("Ignore Time Scale","If you want this asset to keep animating even if you set the game on pause by setting TimeScale to 0."),script.useUnscaledTime);
		if(useUnscaledTime!=script.useUnscaledTime){
			Undo.RecordObject(script,"Changed Time Scale dependence");
			script.useUnscaledTime=useUnscaledTime;
			EditorUtility.SetDirty(script);
		}

		//Sprite sorting
		GUILayout.Space(10);
		//Get sorting layers
		int[] layerIDs=GetSortingLayerUniqueIDs();
		string[] layerNames=GetSortingLayerNames();
		//Get selected sorting layer
		int selected=-1;
		for(int i=0;i<layerIDs.Length;i++){
			if(layerIDs[i]==script.sortingLayer){
				selected=i;
			}
		}
		//Select Default layer if no other is selected
		if(selected==-1){
			for(int i=0;i<layerIDs.Length;i++){
				if(layerIDs[i]==0){
					selected=i;
				}
			}
		}
		//Sorting layer dropdown
		EditorGUI.BeginChangeCheck();
		GUIContent[] dropdown=new GUIContent[layerNames.Length+2];
		for(int i=0;i<layerNames.Length;i++){
			dropdown[i]=new GUIContent(layerNames[i]);
		}
		dropdown[layerNames.Length]=new GUIContent();
		dropdown[layerNames.Length+1]=new GUIContent("Add Sorting Layer...");
		selected=EditorGUILayout.Popup(new GUIContent("Sorting Layer","Name of the Renderer's sorting layer"),selected,dropdown);
		if(EditorGUI.EndChangeCheck()){
			Undo.RecordObject(script,"Change sorting layer");
			if(selected==layerNames.Length+1){
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/Tags and Layers");
			}else{
				script.sortingLayer=layerIDs[selected];
			}
			EditorUtility.SetDirty(script);
		}
		//Order in layer field
		EditorGUI.BeginChangeCheck();
		int order=EditorGUILayout.IntField(new GUIContent("Order in Layer","Renderer's order within a sorting layer"),script.orderInLayer);
		if(EditorGUI.EndChangeCheck()){
			Undo.RecordObject(script,"Change order in layer");
			script.orderInLayer=order;
			EditorUtility.SetDirty(script);
		}
	}

	public void SaveSettings(){
		string path=EditorUtility.SaveFilePanel("Export FlatFX settings",Application.dataPath,"FlatFXSettings "+System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")+".json","json");
		if(path.Length!=0){
			string json=JsonUtility.ToJson(script,true);
			File.WriteAllText(path,json);
			AssetDatabase.Refresh();
		}
	}

	public void LoadSettings(bool loadDefault=false){
		string path=EditorUtility.OpenFilePanel("Overwrite with png",Application.dataPath,"json");
		if(path.Length!=0){
			string json=File.ReadAllText(path);
			JsonUtility.FromJsonOverwrite(json,script);
		}
	}

	void OnSceneGUI(){
		//Repaint inspector when particleCount or triangleCount changes
		if(particleCount!=script.particleCount || triangleCount!=script.triangleCount){
			particleCount=script.particleCount;
			triangleCount=script.triangleCount;
			EditorUtility.SetDirty(target);
		}
	}

	//Get the sorting layer IDs
	public int[] GetSortingLayerUniqueIDs() {
		Type internalEditorUtilityType=typeof(InternalEditorUtility);
		PropertyInfo sortingLayerUniqueIDsProperty=internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs",BindingFlags.Static|BindingFlags.NonPublic);
		return (int[])sortingLayerUniqueIDsProperty.GetValue(null,new object[0]);
	}

	//Get the sorting layer names
	public string[] GetSortingLayerNames(){
		Type internalEditorUtilityType=typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty=internalEditorUtilityType.GetProperty("sortingLayerNames",BindingFlags.Static|BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null,new object[0]);
	}

}
