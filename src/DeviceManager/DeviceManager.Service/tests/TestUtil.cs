/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-02 18:26:50
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceManager.DTO;
using DeviceManager.DTO.System;

namespace DeviceManager.Service.Tests
{
    public class TestUtil
    {
        public static string String(int length)
        {
            length = length = 4;
            char[] chars = "abcçdefgğhıijklmnoöpqrsştuüvwxyz1234567890ABCÇDEFGHIİJKLMNOÖPQRSŞTUÜVWXYZ".ToCharArray();
            string result = "Test";
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int x = random.Next(1, chars.Length);

                if (!result.Contains(chars.GetValue(x).ToString()))
                    result += chars.GetValue(x);
                else
                    i--;
            }
            return result;
        }

        public static string StringComplex(int length)
        {
            length = length = 4;
            char[] chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&".ToCharArray();
            string result = "Test";
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int x = random.Next(1, chars.Length);

                if (!result.Contains(chars.GetValue(x).ToString()))
                    result += chars.GetValue(x);
                else
                    i--;
            }
            return result;
        }

        public static int Number(int length)
        {
            if (length == 10) throw new Exception("INT deger icin en fazla 9 hane olabilir LongNumber fonksitonunu kullan. ");

            Random rn = new Random();
            int minVal = int.Parse("1000000000".Substring(0, length));
            int maxVal = int.Parse("9999999999".Substring(0, length));
            return (new Random()).Next(minVal, maxVal);
        }

        public static long LongNumber(int length)
        {
            string s = "10000000000000000000000000".Substring(0, length - 9) + TestUtil.Number(9).ToString();
            return long.Parse(s);
        }

        public static int Number(int minVal, int maxVal)
        {
            return (new Random()).Next(minVal, maxVal);
        }

        public SettingsDto SettingsDto()
        {
            return new SettingsDto { Key = String(10), Val = String(100) };
        }

        public LanguageDto LanguageDto()
        {
            return new LanguageDto { Key = String(10), Val = String(100) };
        }

        public PropertyDto PropertyDto()
        {
            return new PropertyDto { Name = String(10), Version = 1 };
        }

        public ModelDto ModelDto()
        {
            return new ModelDto { BrandName = String(10), ModelName = String(20) };
        }
        public TemplateDto TemplateDto()
        {
            return new TemplateDto { Name = String(10), Version = 1 };
        }
        public TemplatePropertyDto TemplatePropertyDto()
        {
            return new TemplatePropertyDto { TemplateId = 1, PropertyId = 1, DefaultValue = String(20), IsRequired = 0, Version = 1 };
        }

        public DeviceDto DeviceDto()
        {
            return new DeviceDto { DeviceCode = String(20), TemplateId = 1, ModelId = 1, State = 1, SerialNumber = String(20), LastAccessDate = new DateTime(), Version = 1 };
        }

        public DevicePropertyDto DevicePropertyDto()
        {
            return new DevicePropertyDto { DeviceId = 1, PropertyId = 1, PropertyValue = String(20), Version = 1 };
        }



    }
}