using UnityEditor;
using UnityEngine.TestTools;

class CodeCoverageMenuItem
{
	const string EnableCodeCoverageItemName = "Code Coverage/Enable Code Coverage";

	[MenuItem(EnableCodeCoverageItemName, false)]
	static void EnableCodeCoverage()
	{
		Coverage.enabled = !Coverage.enabled;
	}

	[MenuItem(EnableCodeCoverageItemName, true)]
	static bool EnableCodeCoverageValidate()
	{
		Menu.SetChecked(EnableCodeCoverageItemName, Coverage.enabled);
		return true;
	}
}