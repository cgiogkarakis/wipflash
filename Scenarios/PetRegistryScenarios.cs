﻿#region

using Examples.ExampleUtils;
using NUnit.Framework;
using WiPFlash;
using WiPFlash.Component;
using WiPFlash.Components;

#endregion

namespace Scenarios
{
    [TestFixture]
    public class PetRegistryScenarios : UIBasedExamples
    {
        [Test]
        public void ICanRegisterAPet()
        {
            Application application = new ApplicationLauncher().Launch(EXAMPLE_APP_PATH);
            Window window = application.FindWindow(EXAMPLE_APP_WINDOW_NAME);
            window.Find<TextBox>("petNameInput").Text = "Snowdrop";
            window.Find<ComboBox>("petTypeInput").Select("Rabbit");
            window.Find<TextBox>("petPriceInput").Text = "100.00";
            window.Find<ListBox>("petRulesInput").Select("Dangerous", "No Children");
            window.Find<Button>("petSaveButton").Click();
        }

    }
}