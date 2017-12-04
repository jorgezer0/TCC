using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
namespace vr.basics.components{
	public class FpsMeter : MonoBehaviour {
		public enum DisplayType
		{
			FPS,
			DeltaTime,
			MethodExecutionTime,
			ExternalActivation
		}

		[System.Serializable]
		public class GraphSettings
		{
			public int skipFrames = 5;
			public int frameCount = 180;
			[Space]
			public int greenFps = 60;
			public int redFps = 10;
			public float padding = 0.02f;
			public Gradient colorRange;
		}
		[System.Serializable]
		public class ElementSetup
		{
			public Text currentFpsDisplay;
			public Text maxFpsDisplay;
			public Text minFpsDisplay;
			public Text avgFpsDisplay;
			public RawImage graphDisplay;
			public RectTransform averageLine;
		}

		[Space]
		public DisplayType mode;
		public bool running = true;
		[Space]
		public UnityEvent watchedMethods;

		[Header("Advanced")]
		[SerializeField] private GraphSettings graph;
		[SerializeField] private ElementSetup elements;

		private RectTransform averageLineParent;

		private float minTime;
		private float maxTime;

		private float averageTime = -1;
		private float smaller = -1;
		private float bigger = -1;

		private long operations = 0;

		private List<Color> colors;
		private List<float> times;

		private Texture2D texture;
		private Stopwatch methodTimer;

		private bool externalRunning = false;

		void Awake(){
			texture = new Texture2D (graph.frameCount, 1);
			minTime = 1 / (float)graph.greenFps;
			maxTime = 1 / (float)graph.redFps;
			colors = new List<Color> ();
			times = new List<float> ();
			for (int i = 0; i < graph.frameCount; i++) {
				colors.Add (Color.clear);
				times.Add(0);
			}
			texture.SetPixels (colors.ToArray ());
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.Apply ();
			elements.graphDisplay.texture = texture;
			averageLineParent = elements.averageLine.parent.gameObject.GetComponent<RectTransform> ();
			elements.minFpsDisplay.text = 
				elements.maxFpsDisplay.text = 
					elements.avgFpsDisplay.text = 
						elements.currentFpsDisplay.text = "";
			
		}

		// Update is called once per frame
		void Update () {
			if (!running)
				return;
			switch (mode) {
			case DisplayType.FPS:
			case DisplayType.DeltaTime:
				UpdateDeltaTime (Time.deltaTime);
				break;
			case DisplayType.MethodExecutionTime:
				if (methodTimer == null) {
					methodTimer = new Stopwatch ();
				}
				methodTimer.Start ();
				watchedMethods.Invoke ();
				methodTimer.Stop ();
				long millis = methodTimer.ElapsedMilliseconds;
				methodTimer.Reset ();
				UpdateDeltaTime( (float)millis / 1000 );
				break;
			}

		}

		/// <summary>
		/// Starts measuring external code.
		/// The mode must be set to External Activation to use that.
		/// </summary>
		public void StartMeasuring(){
			if (mode != DisplayType.ExternalActivation) {
				Debug.LogError ("Trying to use external measuring on wrong mode");
				return;
			}
			if (externalRunning) {
				Debug.LogError ("Trying to start a measuring without stopping");
				return;
			}
			if (methodTimer == null) {
				methodTimer = new Stopwatch ();
			}
			externalRunning = true;
			methodTimer.Start ();
		}

		/// <summary>
		/// Stops measuring external code.
		/// The mode must be set to External Activation to use that.
		/// </summary>
		public void StopMeasuring(){
			if (!externalRunning) {
				Debug.LogError ("Trying to stop measuring but there's no one running");
				return;
			}
			externalRunning = false;
			methodTimer.Stop ();
			long millis = methodTimer.ElapsedMilliseconds;
			methodTimer.Reset ();
			UpdateDeltaTime( (float)millis / 1000 );
		}

		void UpdateDeltaTime(float deltaTime){
			if (graph.skipFrames > 0) {
				graph.skipFrames--;
				return;
			}
			elements.currentFpsDisplay.text = GetDisplayValue (deltaTime);
			Color currentColor = GetColorFromTime (deltaTime);
			elements.currentFpsDisplay.color = currentColor;
			bool changedMinMax = false;
			if (averageTime < 0) {
				averageTime = smaller = bigger = deltaTime;
				UpdateText (elements.maxFpsDisplay, smaller, mode == DisplayType.FPS ? "max: " : "min: ");
				UpdateText (elements.minFpsDisplay, bigger, mode == DisplayType.FPS ? "min: " : "max: ");
				operations = 1;
				changedMinMax = true;
			} else {
				if (deltaTime < smaller) {
					smaller = deltaTime;
					changedMinMax = true;
					UpdateText (elements.maxFpsDisplay, smaller, mode == DisplayType.FPS ? "max: " : "min: ");
				}
				if (deltaTime > bigger) {
					bigger = deltaTime;
					changedMinMax = true;
					UpdateText (elements.minFpsDisplay, bigger, mode == DisplayType.FPS ? "min: " : "max: ");
				}
				averageTime = (operations * averageTime + deltaTime) / (operations+ 1); 
				operations++;
				UpdateText (elements.avgFpsDisplay, averageTime, "Avg: ");
			}

			colors.RemoveAt (0);
			times.RemoveAt (0);

			float fromVal = mode == DisplayType.FPS ? bigger : 0;
			float toVal = mode == DisplayType.FPS ? 0 : bigger;

			Vector2 anchorPos = elements.averageLine.anchoredPosition;
			float avgLerp = Mathf.InverseLerp (fromVal, toVal, averageTime);
			if (mode == DisplayType.FPS) {
				avgLerp = 1 - avgLerp;
			}

			float visibleArea = 1 - (graph.padding * 2);

			anchorPos.y = Mathf.Lerp (0, averageLineParent.sizeDelta.y, graph.padding + visibleArea * avgLerp);
			elements.averageLine.anchoredPosition = anchorPos;
			currentColor.a = graph.padding + visibleArea * Mathf.InverseLerp (fromVal, toVal, deltaTime);
			colors.Add (currentColor);
			times.Add (deltaTime);
			if (changedMinMax) {
				float f;
				Color c;
				for (int i = 0; i < graph.frameCount; i++) {
					f = times [i];
					if (f > 0) {
						c = colors [i];
						c.a = graph.padding + visibleArea * Mathf.InverseLerp (fromVal, toVal, f);
						colors [i] = c;
					}
				}
			}
			texture.SetPixels (colors.ToArray ());
			texture.Apply ();
		}

		void UpdateText(Text text, float time, string prefix = ""){
			text.text = GetDisplayValue (time, prefix);
			text.color = GetColorFromTime (time);
		}

		Color GetColorFromTime(float time){
			return graph.colorRange.Evaluate (Mathf.InverseLerp (maxTime, minTime, time));
		}

		string GetDisplayValue(float time, string prefix = ""){
			if (mode == DisplayType.FPS) {
				return string.Format ("{1}{0:0.} fps", 1 / time, prefix); 
			} else {
				return string.Format ("{1}{0:0.}ms", time * 1000, prefix);
			}
		}
	}
}