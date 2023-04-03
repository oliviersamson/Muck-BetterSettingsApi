using HarmonyLib;
using MuckSettings;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using static MuckSettings.Settings;

namespace BetterSettingsApi.PagePatch
{
    [HarmonyPatch(typeof(Page), MethodType.Constructor, new Type[] { typeof(GameObject) })]
    class Page_Constructor
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            CodeMatcher codeMatcher = new(instructions);

            // Go after .ctor call
            codeMatcher = codeMatcher.MatchForward(false, new CodeMatch(OpCodes.Call));
            codeMatcher = codeMatcher.Advance(1);

            // Load current Page instance on top of stact
            codeMatcher = codeMatcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0));

            // Load keep Gameobject tab on top of stack
            codeMatcher = codeMatcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_1));

            // Run new Page constructor code here
            codeMatcher = codeMatcher.InsertAndAdvance(Transpilers.EmitDelegate<Action<Page, GameObject>>(
                (instance, tab) => {
                    
                    // Get ScrollRect and set it to preferred values for movement type and scroll sensitivity
                    var scroll = MonoBehaviour.Instantiate(Main.SettingsScroll);
                    ScrollRect scrollRect = scroll.GetComponent<ScrollRect>();
                    scrollRect.movementType = ScrollRect.MovementType.Clamped;
                    scrollRect.scrollSensitivity = 35;

                    // Get the RectTransform representing the content area of the ScrollRect
                    RectTransform content = scrollRect.content;

                    // Make a copy of the VerticalLayoutGroup with the content area as its parent, remove its padding and set it active
                    VerticalLayoutGroup copy = MonoBehaviour.Instantiate<VerticalLayoutGroup>(tab.GetComponent<VerticalLayoutGroup>(), content.transform);
                    copy.padding = new RectOffset(0, 0, 0, 0);
                    copy.gameObject.SetActive(true);                    

                    // Set the content's scaling to the original scaling
                    content.GetComponent<VerticalLayoutGroup>().spacing = copy.spacing;

                    // Force it to be rebuilt in order to get good values for preferredWidth and preferredHeight in order to resize it
                    LayoutRebuilder.ForceRebuildLayoutImmediate(copy.GetComponent<RectTransform>());
                    copy.GetComponent<RectTransform>().sizeDelta = new Vector2(copy.preferredWidth, copy.preferredHeight);

                    // Destroy background image (since it is already there)
                    GameObject.Destroy(copy.GetComponent<RawImage>());

                    // Destroy the copy's children since they need to be replaced by the originals for the callbacks to work
                    foreach (Transform rect in copy.transform)
                    {
                        GameObject.Destroy(rect.gameObject);
                    }

                    // Add the original children to the copy
                    List<Transform> children = new List<Transform>();
                    Transform originalTransform = tab.GetComponent<VerticalLayoutGroup>().transform;
                    foreach (Transform child in originalTransform)
                    {
                        children.Add(child);
                    }

                    foreach (Transform child in children)
                    {
                        child.SetParent(copy.transform);
                        child.localScale = Vector3.one;
                    }

                    // Destroy the original Vertical Layout Group
                    GameObject.Destroy(tab.GetComponent<VerticalLayoutGroup>());

                    // Set the ScrollRect's parent as the current tab
                    scroll.transform.SetParent(tab.transform, false);

                    // Set the RectTransform content field's value
                    var contentField = typeof(Page).GetField("content", BindingFlags.Instance | BindingFlags.NonPublic);
                    contentField.SetValue(instance, content);

                    // Set the height field's value
                    var heightField = typeof(Page).GetField("height", BindingFlags.Instance | BindingFlags.NonPublic);
                    float initialValue = (float)heightField.GetValue(instance);
                    heightField.SetValue(instance, initialValue + copy.GetComponent<RectTransform>().sizeDelta.y);
                }));

            codeMatcher = codeMatcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ret));

            return codeMatcher.InstructionEnumeration();
        }
    }
}
    