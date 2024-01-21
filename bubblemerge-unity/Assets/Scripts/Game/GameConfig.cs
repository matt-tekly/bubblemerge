using System;
using Tekly.Common.Utils.Tweening;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class BubbleProviderConfig
    {
        public int SizeMax = 4;
        public int[] StartingSizes;
        
        public float WorldLeft;
        public float WorldRight;
    }
    
    [Serializable]
    public class BubbleConfig
    {
        public int BaseSize = 4;
        public Color[] Colors;
        public float[] SizeRatios;
    }
    
    
    [CreateAssetMenu]
    public class GameConfig : ScriptableObject
    {
        public float StartHeight;
        public float GrowTime = 0.3f;
        public float MergeTime = 0.3f;
        
        public TweenSettings BubbleAppearTween;
        public float BubbleFallSpeed = 1f;
        public float FullTimeLose = 5f;

        public BubbleConfig BubbleConfig;
        public BubbleProviderConfig BubbleProviderConfig;
    }
    
}