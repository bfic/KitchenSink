using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KitchenSink.Tests;

namespace KitchenSink.Test
{
    [TestFixture("firefox")]
    [TestFixture("chrome")]
    [TestFixture("edge")]
    //[TestFixture("internet explorer")]
    public class TextPageTest : BaseTest
    {
        public TextPageTest(string browser) : base(browser)
        {
        }

        [Test]
        public void TextPage_PageLoads()
        {
            driver.Navigate().GoToUrl(baseURL + "/Text");
            this.WaitUntil(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("html body puppet-client")));
        }

        [Test]
        public void TextPage_TextPropagationOnUnfocus()
        {
            driver.Navigate().GoToUrl(baseURL + "/Text");
            this.WaitUntil(ExpectedConditions.PresenceOfAllElementsLocatedBy(ByHelper.StarcounterIncludeWithInputText));
            var label = driver.FindElement(ByHelper.AnyControlLabel);
            var originalText = label.Text;
            driver.FindElement(ByHelper.AnyInput).Clear();
            driver.FindElement(ByHelper.AnyInput).SendKeys("Marcin");
            driver.FindElement(ByHelper.Body).ClickUsingMouse(driver);
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("Hi, Marcin!", driver.FindElement(ByHelper.AnyControlLabel).Text);
        }

        [Test]
        public void TextPage_TextPropagationWhileTyping()
        {
            driver.Navigate().GoToUrl(baseURL + "/Text");
            this.WaitUntil(ExpectedConditions.PresenceOfAllElementsLocatedBy(ByHelper.StarcounterIncludeWithInputText));
            var label = driver.FindElement(ByHelper.NthControlLabel(1));
            var originalText = label.Text;
            driver.FindElement(ByHelper.NthInput(1)).Clear();
            driver.FindElement(ByHelper.NthInput(1)).SendKeys("M");
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("Hi, M!", driver.FindElement(ByHelper.NthControlLabel(1)).Text);
        }
    }
}