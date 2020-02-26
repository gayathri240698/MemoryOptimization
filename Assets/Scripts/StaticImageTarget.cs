using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BharathARFoundation.ImageTargets
{
public class StaticImageTarget : NormalImageTarget
   {

        protected override void Awake()
        {
            base.Awake();
            trackImageManager.referenceLibrary = runtimeImageLibrary;
			trackImageManager.enabled = true;
		}
    } 
 }