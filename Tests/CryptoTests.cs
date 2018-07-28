using LiveChatRoom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class CryptoTests
    {
        [TestMethod]
        public void Hash_TakeParameter_ReturnsHash()
        {
            string expected = "i7DPbrmxfQ99IrRW8SElfcElTh8BZlNwR2OD6ndt9BQ=";
            string parameter = "1234567";

            string result = Crypto.Hash(parameter);

            Assert.AreEqual(expected, result);
        }
    }
}
