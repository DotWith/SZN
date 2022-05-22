using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Dot.SZN.UI
{
    public class OptionsPanel : MonoBehaviour
    {
        public TMP_Dropdown resolutionDropdown;
        public TMP_Dropdown refreshRateDropdown;
        public SZNButton applyButton;
        public Toggle fullscrennToggle;

        public void Start()
        {
            Resolution[] resolutions = Screen.resolutions;

            List<string> resolutionsOptions = new List<string>();
            List<string> refreshRateOptions = new List<string>();

            int currentResolutionIndex = 0;
            int currentRefreshRateIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string resolutionOption = resolutions[i].width + " x " + resolutions[i].height;
                string refreshRateOption = resolutions[i].refreshRate.ToString();

                if (!resolutionsOptions.Contains(resolutionOption))
                {
                    resolutionsOptions.Add(resolutionOption);

                    if (resolutions[i].width == Screen.width &&
                        resolutions[i].height == Screen.height)
                    {
                        currentResolutionIndex = i;
                    }
                }

                if (!refreshRateOptions.Contains(refreshRateOption))
                {
                    refreshRateOptions.Add(refreshRateOption);

                    if (resolutions[i].refreshRate == Application.targetFrameRate)
                    {
                        currentRefreshRateIndex = i;
                    }
                }
            }

            resolutionDropdown.AddOptions(resolutionsOptions);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            refreshRateDropdown.AddOptions(refreshRateOptions);
            refreshRateDropdown.value = currentRefreshRateIndex;
            refreshRateDropdown.RefreshShownValue();

            applyButton.OnClick.AddListener(() => ApplyButtonCallback());
        }

        void ApplyButtonCallback()
        {
            SetResoultion(resolutionDropdown.value);
            SetRefreshRate(refreshRateDropdown.value);
        }

        Resolution ConvertOption(TMP_Dropdown dropdown, int index)
        {
            string text = dropdown.options[index].text;
            Resolution newResolution = new Resolution()
            {
                height = Convert.ToInt32(text.Split(" x ")[1]),
                width = Convert.ToInt32(text.Split(" x ")[0])
            };
            return newResolution;
        }

        void SetResoultion(int resolutionIndex)
        {
            Resolution resolution = ConvertOption(resolutionDropdown, resolutionIndex);
            Screen.SetResolution(resolution.width, resolution.height, fullscrennToggle.isOn);
        }

        void SetRefreshRate(int refreshRateIndex)
        {
            int resolution = Convert.ToInt32(refreshRateDropdown.options[refreshRateIndex].text);
            Application.targetFrameRate = resolution;
        }
    }
}
