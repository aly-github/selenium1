using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Threading;





      
    public class SFrameWork
    {
      public IWebDriver driver;
      public Boolean testStatus;
      public DataTable dtTestCases = null;
      public DataTable dtData = null;
      public string msg="";
      IWebElement _element=null;

      public SFrameWork()
      {
          Console.WriteLine("Hello World");
         
          dtTestCases = DB_Connect.ExecuteReaderTable("Select * from tbl_config");
          

          
        
          //excuteCase();
      }
      public void configure(int index)
      {
          Console.WriteLine(dtTestCases.Rows[index]["URL"].ToString());
          if (dtTestCases.Rows[index]["browser"].ToString() == "firefox") { driver = new FirefoxDriver(); }
          driver.Navigate().GoToUrl(dtTestCases.Rows[index]["URL"].ToString());
          driver.Manage().Window.Maximize();

      }
        
     public void  loadCase(DataRow dr)
     {
         SqlCommand _cmd = new SqlCommand("Select * from tbl_testcases where testCaseName=@testCaseName order by pk");
         _cmd.Parameters.Add("@testCaseName", SqlDbType.VarChar).Value = dr["testCaseName"].ToString();
          dtData = DB_Connect.ExecuteReaderTable(_cmd);

     }


        public string browserCommands(DataRow dataRow)
        {
            string _val="";
            
            switch (dataRow["browser_command"].ToString())
            {
                case "getURL":
                    _val = driver.Url;
                  break;
                case "getTitle":
                  _val = driver.Title;
                  break;
                        


            }
           
            return _val;
            
        }
        public bool runCase(DataRow dataRow)
         {
            bool rtn=false;

            Console.WriteLine("runCase");
           
                    msg = dataRow["comments"].ToString();
                    if (dataRow["browser_command"] != null && dataRow["element_by"].ToString() == "browser") // Browser comannds non element commands
                    {
                        Console.WriteLine("browser");
           
                        rtn = browserCommandaction(dataRow);
                       
                    }
                    else if (dataRow["element_name"] != null)
                    {
                        Console.WriteLine("element");
                        rtn = elementCommandaction(dataRow);
                    }
 
               
            
            return rtn;
        }
        public bool browserCommandaction(DataRow dataRow )
        {
           bool rtn = false;
           try
           {
               switch (dataRow["action"].ToString())
               {

                   case "verify":

                       if (dataRow["value"].ToString() == browserCommands(dataRow)) { rtn = true; }
                       break;
                   case "screenShot":
                       Thread.Sleep(3000);
                       Console.WriteLine("screenShot");
                       Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                       string _fileName = "C:\\Users\\Aly\\Pictures\\hy\\" + DateTime.Now.Ticks + "_ss.png";
                       ss.SaveAsFile(_fileName, ImageFormat.Png);
                       rtn = true;
                       break;

               }
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.Message.ToString());
           
           }



            return rtn;


        }

        public bool elementCommandaction(DataRow dataRow)
        {
            bool rtn = false;

            try
            {
                switch (dataRow["action"].ToString())
                {

                    case "enterText":
                        Console.WriteLine("enterText");
                        
                         elementBy(dataRow["element_name"].ToString(), dataRow["element_by"].ToString());
                         _element.Clear();
                        _element.SendKeys(dataRow["value"].ToString());
                        Thread.Sleep(2000);
                        rtn = true;
                        break;
                    case "clearText":
                        Console.WriteLine("clearText");
                        elementBy(dataRow["element_name"].ToString(), dataRow["element_by"].ToString());
                        _element.Clear();
                        rtn = true;
                        break;
                    case "click":
                        if (dataRow["element_by"].ToString()=="linkText") Thread.Sleep(3000);
                        elementBy(dataRow["element_name"].ToString(), dataRow["element_by"].ToString());
                        _element.Click();
                        rtn = true;
                        break;
                    case "exist":
                        Console.WriteLine("exist");
                        elementBy(dataRow["element_name"].ToString(), dataRow["element_by"].ToString());
                        if (_element!=null){Console.WriteLine("Displayed"); rtn = true;}
                        else rtn = false;

                        break;
                    case "selectValue":
                        Console.WriteLine("exist");
                        elementBy(dataRow["element_name"].ToString(), dataRow["element_by"].ToString());
                       // var selectElement = new SelectElement(_element);
                        _element.SendKeys(dataRow["value"].ToString() + Keys.Enter);
                        rtn = true;
                        break;



                }
            }
            catch (Exception ex)
            {
                rtn = false;
                Console.WriteLine(ex.Message.ToString());
            }




            return rtn;


        }

        private  void  elementBy(string _name,string _by)
        {
            _element = null;
            Thread.Sleep(5000);
            switch(_by)
            {
                case "id":
                    _element = driver.FindElement(By.Id(_name));
                    Console.WriteLine("id");
                  break;
                case "xpath":
                  _element = driver.FindElement(By.XPath(_name));
                    break;
                case "linkText":
                    
                 _element   =driver.FindElement(By.PartialLinkText(_name));
                 break;
                case "css":
                 _element = driver.FindElement(By.CssSelector(_name));
                 break;
                case "name":
                    
                 _element = driver.FindElement(By.Name(_name));
                 break;

            }


        }
        
        private void executeCommands(){
            dtData = null;

        }

            
    }

