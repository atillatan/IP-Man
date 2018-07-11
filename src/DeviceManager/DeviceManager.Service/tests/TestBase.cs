/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-04-17 12:11:47
 */
using System;
using Xunit;
using Core.Framework.Service;

namespace DeviceManager.Service.Tests
{
    [Collection("ServiceCollection")]
    public abstract class TestBase
    {
        internal TestUtil testUtil = new TestUtil();

        protected TestBase()
        { }

        public string String(int length)
        {
            return TestUtil.String(length);
        }

        public int Number(int length)
        {
            return TestUtil.Number(length);
        }

        public static string StringComplex(int length)
        {
            return TestUtil.StringComplex(length);
        }

        public static int Number(int minVal, int maxVal)
        {
            return TestUtil.Number(minVal, maxVal);
        }

        public static long LongNumber(int length)
        {
            return TestUtil.LongNumber(length);
        }
    }
}