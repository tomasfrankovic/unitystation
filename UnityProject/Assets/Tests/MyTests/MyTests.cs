using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.Linq;
using System.IO;
using UnityEditor.SceneManagement;

namespace Tests
{
	public class MyTests
	{
		[Test]
		public void CheckWeaponsComponents()
		{
			string path = Application.dataPath + "/Prefabs/Items/Weapons/Melee";
			string[] files = System.IO.Directory.GetFiles(path);

			bool failed = false;
			var failedPrefabs = new StringBuilder();
			foreach (string file in files)
			{
				if (!file.Contains(".prefab") || file.Contains(".meta"))
					continue;
				Object obj = AssetDatabase.LoadMainAssetAtPath(file.Replace(Application.dataPath + "/", "Assets/"));

				if (obj == null && !(obj is GameObject))
				{
					failed = true;
					failedPrefabs.AppendLine($"{obj.name}: Prefab load failed.");
				}

				GameObject go = (GameObject)obj;

				if (!go.GetComponent<PrefabTracker>())
				{
					failed = true;
					failedPrefabs.AppendLine($"{obj.name}: Prefab doesn't contain an PrefabTracker component.");
				}

				if (!go.GetComponent<Pickupable>())
				{
					failed = true;
					failedPrefabs.AppendLine($"{obj.name}: Prefab doesn't contain an Pickupable component.");
				}

			}

			if (failed)
				Assert.Fail(failedPrefabs.ToString());
			else
				Assert.Pass();
		}

		[Test]
		public void CheckNullReferenceObjects()
		{
			List<string> results = new List<string>();
			foreach (string prefab in SearchAndDestroy.GetAllPrefabs())
			{
				Object obj = AssetDatabase.LoadMainAssetAtPath(prefab);
				//Debug.Log(prefab);

				if (!(obj is GameObject))
				{
					Logger.LogFormat("For some reason, object {0} won't cast to GameObject", Category.Tests, prefab);
					continue;
				}

				var go = (GameObject)obj;
				foreach (Component component in go.GetComponentsInChildren<Component>(true))
					if (component == null)
						results.Add(prefab);
			}

			foreach (string s in results)
				Logger.LogFormat("Null reference on object {0}", Category.Tests, s);

			Assert.IsEmpty(results, "Null reference found: {0}", string.Join(", ", results));
		}

        [Test]
        public void ReviseDecals()
        {
            string[] files = Directory.GetFiles(Application.dataPath + "/Prefabs/Decal/Resources");

            var failedPrefabs = new StringBuilder();
            foreach (string file in files)
            {
                if (!file.Contains(".prefab") || file.Contains(".meta"))
                    continue;
                Object obj = AssetDatabase.LoadMainAssetAtPath(file.Replace(Application.dataPath + "/", "Assets/"));

                if (obj == null && !(obj is GameObject))
                {
                    failedPrefabs.AppendLine($"{obj.name}: Prefab load failed.");
                    continue;
                }

                GameObject go = (GameObject)obj;

                if (!go.GetComponent<CustomNetTransform>())
                    failedPrefabs.AppendLine($"{obj.name} is missing CustomNetTransform");

                if (!go.GetComponent<Objects.Construction.FloorDecal>())
                    failedPrefabs.AppendLine($"{obj.name} is missing FloorDecal");

                if (!go.GetComponent<RegisterItem>())
                    failedPrefabs.AppendLine($"{obj.name} is missing RegisterItem");

                if (!go.GetComponent<ObjectBehaviour>())
                    failedPrefabs.AppendLine($"{obj.name} is missing ObjectBehaviour");

                if (!go.GetComponent<UprightSprites>())
                    failedPrefabs.AppendLine($"{obj.name} is missing UprightSprites");

                if (!go.GetComponent<Mirror.NetworkIdentity>())
                    failedPrefabs.AppendLine($"{obj.name} is missing NetworkIdentity");

                if (!go.GetComponent<CustomNetSceneChecker>())
                    failedPrefabs.AppendLine($"{obj.name} is missing CustomNetSceneChecker");
            }

            if (failedPrefabs.ToString() != "")
                Assert.Fail(failedPrefabs.ToString());
            else
                Assert.Pass();
        }

        [Test]
		public void FindMissingPrefabs()
		{
			bool failed = true;
			var failedPrefabs = new StringBuilder();
			var scenesPaths = AssetDatabase.FindAssets("t:Scene").Select(AssetDatabase.GUIDToAssetPath);
			foreach (var scene in scenesPaths)
			{
				if (scene.Contains("DevScenes") || scene.StartsWith("Packages")) continue;

				foreach (var gameObject in EditorSceneManager.OpenScene(scene).GetRootGameObjects())
				{
					var ObjectLayer = gameObject.GetComponentInChildren<ObjectLayer>();
					if (ObjectLayer == null) continue;

					for (int i = 0; i < ObjectLayer.transform.childCount; i++)
					{
						var child = ObjectLayer.transform.GetChild(i);
						if (child.name.Contains("Missing Prefab"))
						{
							failed = false;
							failedPrefabs.AppendLine(
								$"{scene}: {child.name} Missing prefab");
						}
					}
				}
			}

			if (!failed)
				Assert.Fail(failedPrefabs.ToString());
			else
				Assert.Pass();
		}

		[Test]
		public void CheckAlienEggPrefab()
		{
			Object obj = AssetDatabase.LoadMainAssetAtPath("Assets/Prefabs/Objects/Alien/Alien Egg.prefab");

			if (obj == null && !(obj is GameObject))
				Assert.Fail("Prefab load failed.");

			GameObject go = (GameObject)obj;
			if(!go.GetComponent<Alien.AlienEggCycle>())
				Assert.Fail("Prefab doesn't contain an AlienEggCycle script.");
			//check important components for AlienEggCycle script to work


			if (!go.GetComponent<SpriteHandler>())
				Assert.Fail("Prefab doesn't contain an SpriteHandler script.");

			if (!go.GetComponent<RegisterObject>())
				Assert.Fail("Prefab doesn't contain an RegisterObject script.");

			if (!go.GetComponent<ObjectAttributes>())
				Assert.Fail("Prefab doesn't contain an ObjectAttributes script.");
		}
	}
}
