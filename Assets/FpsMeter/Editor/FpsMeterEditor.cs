using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace vr.basics.components{
	[CustomEditor(typeof(FpsMeter))]
	public class FpsMeterEditor : Editor{
		private SerializedProperty mode;
		private SerializedProperty running;
		private SerializedProperty unityEvent;
		private SerializedProperty graph;
		private SerializedProperty elements;

		void OnEnable(){
			mode = serializedObject.FindProperty("mode");
			running = serializedObject.FindProperty("running");
			unityEvent = serializedObject.FindProperty("watchedMethods");

			graph = serializedObject.FindProperty("graph");
			elements= serializedObject.FindProperty("elements");
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();
			FpsMeter.DisplayType currentMode = ((FpsMeter)target).mode;
			EditorGUILayout.PropertyField (mode);
			if (currentMode != FpsMeter.DisplayType.ExternalActivation) {
				EditorGUILayout.PropertyField (running);
			}
			if (currentMode == FpsMeter.DisplayType.MethodExecutionTime) {
				EditorGUILayout.PropertyField (unityEvent);
			}
			EditorGUILayout.Space ();

			EditorGUILayout.PropertyField (graph,true);
			EditorGUILayout.PropertyField (elements,true);

			serializedObject.ApplyModifiedProperties ();
		}
	}
}