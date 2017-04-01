using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoCoordination;
using System.IO;
using System.Reflection;

namespace DoCoordinationTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void DoWorkTest()
        {
            // ワークファイル作成
            // var path = Assembly.GetEntryAssembly().Location;
            var path = @"Z:\Source\Repos\CSharp\develop\MultiCore\DoCoordinationTest\bin\Debug";
            Console.WriteLine(path);
            var wkfile = Path.Combine(path, "workfile.txt");
            using (TextWriter writer = new StreamWriter(wkfile,false))
            {
                writer.WriteLine("{0},{1}", 1, 2);
            }

                //
            var form = new RequestForm();
            form.divisor = 2;
            form.remainder = 1;
            form.DoWork();

            // ワークファイル検査
            string except = "1,2";
            string result = "";
            using (TextReader reader = new StreamReader(wkfile, false))
            {
                result = reader.ReadLine();
            }
            Assert.AreEqual(except, result);

        }
    }
}
