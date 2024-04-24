using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems.Lychnis
{
    [CreateAssetMenu]
    public class MintPrefabs : ScriptableObject
    {
        public GameObject StemPrefab;
        public GameObject LeafPrefab;
        public GameObject DryLeafPrefab;
        public GameObject RustLeafPrefab;
        public GameObject FlowerPrefab;
        public GameObject BudPrefab;
        public GameObject FruitPrefab;
    }
}
