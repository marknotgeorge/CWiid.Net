using CWiid.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static CWiid.Net.Bluetooth;

namespace UnitTests
{
    [TestClass]
    public class BluetoothTests
    {
        [TestMethod]
        public void TestBluetoothAddressStructString()
        {
            string testString = "00:01:02:03:04:05";

            var testStruct = new BluetoothDeviceAddress(testString);

            Assert.AreEqual(testString, testStruct.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBluetoothAddressStructInvalidString()
        {
            string testString = "invalid";
            string resultString = string.Empty;

            var testStruct = new BluetoothDeviceAddress(testString);
        }

        [TestMethod]
        public void TestBluetoothAddressStructBytes()
        {
            string resultString = "00:01:02:03:04:05";

            var testStruct = new BluetoothDeviceAddress(0, 1, 2, 3, 4, 5);

            Assert.AreEqual(resultString, testStruct.ToString());
        }

        [TestMethod]
        public void TestBluetoothDeviceAddressAny()
        {
            string resultString = "00:00:00:00:00:00";

            var testStruct = BluetoothDeviceAddressAny;

            Assert.AreEqual(resultString, testStruct.ToString());
        }

        [TestMethod]
        public void TestBluetoothDeviceAddressAll()
        {
            string resultString = "FF:FF:FF:FF:FF:FF";

            var testStruct = BluetoothDeviceAddressAll;

            Assert.AreEqual(resultString, testStruct.ToString());
        }

        [TestMethod]
        public void TestBluetoothDeviceAddressLocal()
        {
            string resultString = "00:00:00:FF:FF:FF";

            var testStruct = BluetoothDeviceAddressLocal;

            Assert.AreEqual(resultString, testStruct.ToString());
        }
    }
}