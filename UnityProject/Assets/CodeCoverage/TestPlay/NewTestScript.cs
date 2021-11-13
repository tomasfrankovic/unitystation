using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
		var gameObject = new GameObject();
		var alienEggCycle = gameObject.AddComponent<Alien.AlienEggCycle>();
		//alienEggCycle.OnSpawnServer(new SpawnInfo( SpawnType.Default));
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
		Assert.Pass();
    }
}
