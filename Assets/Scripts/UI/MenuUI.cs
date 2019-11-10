

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.JellyOwl.Tower.UI {
	public class MenuUI : MonoBehaviour {
		private static MenuUI instance;
		public static MenuUI Instance { get { return instance; } }

        public delegate void MenuEventHandler();
        public static event MenuEventHandler OnPlayClick;

        [SerializeField]
        protected Button playButton;
        [SerializeField]
        protected GameObject MenuCamera;

		private void Awake(){
			if (instance){
				Destroy(gameObject);
				return;
			}
			
			instance = this;
		}
		
		private void Start () {
            playButton.onClick.AddListener(Play);
		}

        private void Play()
        {
            playButton.interactable = false;
            OnPlayClick.Invoke();
            MenuCamera.SetActive(false);
        }

        private void Update () {
			
		}
		
		private void OnDestroy(){
			if (this == instance) instance = null;
		}
	}
}