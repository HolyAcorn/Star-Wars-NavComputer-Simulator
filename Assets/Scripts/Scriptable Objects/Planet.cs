using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    [CreateAssetMenu(fileName = "Planet", menuName = "Scriptable Objects/Planet")]
    public class Planet : Node
    {


        public string displayName { get; private set; }
        public string wikiLink { get; private set; }

        public override void Initialize(string name, int coordX, int coordY, int type = 0)
        {
            base.Initialize(name, coordX, coordY, type);
            displayName = name;
            wikiLink = "https://starwars.fandom.com/wiki/" + name.Replace(' ', '_');
        }





    }
}
