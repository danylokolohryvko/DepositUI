using Bunit;
using DepositUI.Components;
using DepositUI.Core.Enums;
using NUnit.Framework;

namespace DepositUI.UnitTests.ComponentTests
{
    [TestFixture]
    public class NavTabsTests
    {
        [Test]
        public void RenderComponent_MainMode_ExpectFirstTabActive()
        {
            using var ctx = new Bunit.TestContext();
            var cut = ctx.RenderComponent<NavTabs>(opt=> opt.Add(c => c.Mode, ModeType.Main));

            var lis = cut.FindAll("ul > li");
            var firstA = lis[0].Children[0];
            var secondA = lis[1].Children[0];
            
            Assert.AreEqual(null, firstA.GetAttribute("href"));
            Assert.AreEqual("/deposit/history", secondA.GetAttribute("href"));

            Assert.AreEqual("nav-link active", firstA.ClassName);
            Assert.AreEqual("nav-link", secondA.ClassName);
        }

        [Test]
        public void RenderComponent_HistoryMode_ExpectSecondTabActive()
        {
            using var ctx = new Bunit.TestContext();

            var cut = ctx.RenderComponent<NavTabs>(opt => opt.Add(c => c.Mode, ModeType.History));
            var ul = cut.FindAll("ul > li");
            var firstA = ul[0].Children[0];
            var secondA = ul[1].Children[0];

            Assert.AreEqual("/deposit", firstA.GetAttribute("href"));
            Assert.AreEqual(null, secondA.GetAttribute("href"));

            Assert.AreEqual("nav-link", firstA.ClassName);
            Assert.AreEqual("nav-link active", secondA.ClassName);
        }

        [Test]
        public void RenderComponent_DetailsMode_ExpectSecondTabActiveWithLink()
        {
            using var ctx = new Bunit.TestContext();

            var cut = ctx.RenderComponent<NavTabs>(opt => opt.Add(c => c.Mode, ModeType.Details));
            var ul = cut.FindAll("ul > li");
            var firstA = ul[0].Children[0];
            var secondA = ul[1].Children[0];

            Assert.AreEqual("/deposit", firstA.GetAttribute("href"));
            Assert.AreEqual("/deposit/history", secondA.GetAttribute("href"));

            Assert.AreEqual("nav-link", firstA.ClassName);
            Assert.AreEqual("nav-link active", secondA.ClassName);
        }
    }
}
