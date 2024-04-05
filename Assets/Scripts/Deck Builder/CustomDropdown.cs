using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

struct CustomDropdownOptions {
    public bool isEnabled;
    public bool hasCheckmark;
    public FontStyles? fontStyle;

    public CustomDropdownOptions(bool isEnabled, bool hasCheckmark, FontStyles? fontStyle) {
        this.isEnabled = isEnabled;
        this.hasCheckmark = hasCheckmark;
        this.fontStyle = fontStyle;
    }
}

// Thanks to: https://gist.github.com/h1ddengames/5a78674391e5b50dad8cdd638a46b706
public class CustomDropdown : TMP_Dropdown {
    Dictionary<int, CustomDropdownOptions> indexesCustomized = new Dictionary<int, CustomDropdownOptions>();

    GameObject mainLabel;
    GameObject placeholderLabel;

    protected override void Start() {
        base.Start();

        foreach (Transform t in transform) {
            switch (t.name) {
                case "mainLabel":
                    mainLabel = t.gameObject;
                    break;
                case "placeholderLabel":
                    placeholderLabel = t.gameObject;
                    break;
            }
        }
    }

    public void ShowMainLabel(bool show) {
        mainLabel.SetActive(show);
        placeholderLabel.SetActive(!show);
    }

    override public void OnPointerClick(PointerEventData eventData) {
        base.OnPointerClick(eventData);
        CustomizeItems();
    }

    public void CustomizeItems() {
        var dropdownList = GetComponentInChildren<Canvas>();
        if (!dropdownList) return;

        var items = dropdownList.GetComponentsInChildren<DropdownItem>();

        for (var i = 0; i < items.Length; i++) {
            var item = items[i];

            if (i == 0) {
                RemoveNoneItem(item, dropdownList.gameObject);
            }

            if (indexesCustomized.ContainsKey(i)) {
                var value = indexesCustomized[i];
                
                CustomizeEnabled(item, value.isEnabled);
                CustomizeFontStyle(item, value.fontStyle);
                CustomizeCheckmark(item, value.hasCheckmark);
            }
            if (value != i) {
                CustomizeCheckmark(item, false);
            }
        }
    }

    void RemoveNoneItem(DropdownItem item, GameObject dropdownList) {
        var containerTransform = item.transform.parent;
        var dropdownListTransform = dropdownList.transform;

        item.gameObject.SetActive(false);

        var containerRect = containerTransform.GetComponent<RectTransform>();
        var dropdownListRect = dropdownListTransform.GetComponent<RectTransform>();

        containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, containerRect.sizeDelta.y - 55);
        dropdownListRect.sizeDelta = new Vector2(dropdownListRect.sizeDelta.x, dropdownListRect.sizeDelta.y - 55);
    }

    void CustomizeEnabled(DropdownItem item, bool enabled) {
        var toggle = item.GetComponent<Toggle>();
        toggle.interactable = enabled;
    }
    void CustomizeFontStyle(DropdownItem item, FontStyles? fontStyle) {
        if (fontStyle != null) {
            var text = item.GetComponentInChildren<TextMeshProUGUI>();
            text.fontStyle = (FontStyles)fontStyle;
        }
    }
    void CustomizeCheckmark(DropdownItem item, bool hasCheckmark) {
        if (!hasCheckmark) {
            Transform checkMarkTranform = null;
            Transform labelTransform = null;

            foreach (Transform t in item.transform) {
                switch (t.name) {
                    case "Item Checkmark":
                        checkMarkTranform = t;
                        break;
                    case "Item Label":
                        labelTransform = t;
                        break;
                }
            }
            Destroy(checkMarkTranform.gameObject);

            var labelRect = labelTransform.GetComponent<RectTransform>();
            labelRect.offsetMin = new Vector2(15, labelRect.offsetMin.y);
        }
    }
    
    public void RemoveAllCustomization() {
        var tmpDict = indexesCustomized;
        for (int i = 0; i < tmpDict.Count; i++) {
            RemoveCustomization(i);   
        }
    }

    public void RemoveCustomization(int index) {
        if (indexesCustomized.ContainsKey(index)) {
            indexesCustomized.Remove(index);
        }
    }

    public void AddCustomization(int index, bool isEnabled, bool hasCheckmark = true, FontStyles? fontStyle = null) {
        var option = new CustomDropdownOptions(isEnabled, hasCheckmark, fontStyle);

        if (!indexesCustomized.ContainsKey(index)) {
            indexesCustomized.Add(index, option);
        }

        CustomizeItems();
    }

    public void ResetState() {
        value = 0;
        ShowMainLabel(false);
    }
}
