using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Data;
using System.Data.SqlClient;

namespace auto_test
{
    [TestFixture]
    public class Atest
    {


        SFrameWork _fw = null;
        [TestFixtureSetUp]
        public void Init()
        {
            _fw = new SFrameWork();
        }
        [Test]
        public void startTesting()
        {
            bool rtn = false;
            int index = 0;
            _fw.configure(index);
            foreach (DataRow dr in _fw.dtTestCases.Rows)
            {
               
                _fw.loadCase(dr);
                foreach (DataRow _case in _fw.dtData.Rows)
                {
                    rtn = _fw.runCase(_case);
                    if (!rtn) { Console.WriteLine("Failed at:" + _fw.msg); }
                    else { Console.WriteLine("Passed at:" + _fw.msg); }

                    Assert.AreEqual(true, rtn);
                }
                index++;
                if (index < _fw.dtData.Rows.Count) { _fw.configure(index); }
                
            }
           
               
        }
        [TestFixtureTearDown]
        public void ShutDown()
        {
           // _fw.driver.Close();
        }

        private void excuteCases()
        {
            

        }
      



    }
}
