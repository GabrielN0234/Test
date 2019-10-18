using System;
using System.IO;

//librerias instalada mediante NuGet
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using iTextSharp.text;
using iTextSharp.text.pdf;   



namespace SeleniumTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Instansiacion del driver encargado de manejar el navagador
            IWebDriver driver = new ChromeDriver();                               
            driver.Navigate().GoToUrl("http://localhost/test/guerrilla/");         
            driver.Manage().Window.Maximize();                                      

            //instansiacion de la clase test
            Test test = new Test(driver);
            int cantidad_usuarios = 2;

            //Creacion de un archivo de registro
            test.start_log();                                                       

            //ejecucion del test
            if (test.funcionalidad_registrar_usuarios(cantidad_usuarios))                           
            {
                if(test.funcionalidad_cargar_usuarios())                            
                    test.write_log("\nTests completed successfully\n");             
            }
            else
            {
                test.write_log("\nThe test could not be completed\n");              
            }
        }
        }

    }

    //Esta clase modela los formularios HTML para ejecutar los tests
    public class Test {
        IWebDriver driver;
        String log="";
        public Test(IWebDriver driver) {
            this.driver = driver;
        }
        public Boolean funcionalidad_registrar_usuarios(int cantidadUsuarios)
        {
            this.log += "\nfuncionalidad_registrar_usuarios:\n";
            var r = new Random();
            Boolean value = true;

            for (int i = 0; i < cantidadUsuarios; i++)
            {
                var pass = r.Next(10000, 999999999);
                try
                {
                    //hace referencia al elemento html mediante el id
                    IWebElement btnSignUp = driver.FindElement(By.Id("signup"));
                    btnSignUp.Click();

                    IWebElement inputUser = driver.FindElement(By.Id("usuariotf"));
                    inputUser.SendKeys("user_" + r.Next(10000, 99999));

                    IWebElement inputEmail = driver.FindElement(By.Id("correotf"));
                    inputEmail.SendKeys("email_" + r.Next(10000, 99999) + "@domain.com");

                    IWebElement password = driver.FindElement(By.Id("passtf"));
                    password.SendKeys("" + pass);

                    IWebElement password2 = driver.FindElement(By.Id("passtf2"));
                    password2.SendKeys("" + pass);

                    IWebElement edad = driver.FindElement(By.Id("edadtf"));
                    edad.SendKeys("" + r.Next(18, 99));

                    IWebElement inputFaction = driver.FindElement(By.Id("mec"));
                    inputFaction.Click();

                    IWebElement inputTerms = driver.FindElement(By.Id("terms"));
                    inputTerms.Click();

                    IWebElement signUp = driver.FindElement(By.Id("Sign up"));
                    signUp.Click();
                    this.log += "       User registered successfully\n";
                    value = true;
                }
                catch (Exception e)
                {
                    this.log += "       An error occurred while registering a user. Exception caught:"+ e.Message+"\n";
                    value = false;
                }
            }
            return value;

        }

        public Boolean funcionalidad_cargar_usuarios()
        {
            this.log += "\nfuncionalidad_cargar_usuarios:\n";
            try
            {
                IWebElement verUsuarios = driver.FindElement(By.Name("rank"));
                verUsuarios.Click();
                this.log += "       Successful data loading\n";
                return true;
            }
            catch (Exception e)
        {
                this.log += "       An error occurred while loading data. Exception caught:" + e.Message + "\n";
            return false;
            }
        }
        
        //Crea un archivo log en formato pdf
        public void write_log(String result)
        {
            log += result;

            Document doc = new Document();
            String path = "log_" + DateTime.Now.ToString().Split(" ")[0].Replace("/","_") + ".pdf";
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();

            Paragraph title = new Paragraph();
            title.Font = FontFactory.GetFont(FontFactory.TIMES, 18f, BaseColor.BLACK);
            title.Add("Debugging log");
            doc.Add(title);

            doc.Add(new Paragraph(log));
            doc.Close();
        }
    public void start_log()
        {
        log += "\nDate: " + DateTime.Now + "\n" +
            "Application Name: Test\n\n" +
            "Driver data set:\n" +
            "       Starting ChromeDriver 77.0.3865.40 \n" +
            "       (f484704e052e0b556f8030b65b953dce96503217-refs/branch-heads/3865@{#442}) \n" +
            "       on port 60132\n"+
            "       Only local connections are allowed.\n"+
            "       Please protect ports used by ChromeDriver and related test\n" +
            "       frameworks to prevent access by malicious code.\n"+
            "       DevTools listening on ws:\n" +
            "       //127.0.0.1:60135/devtools/browser/f922f03b-c1ff-4445-a2cb-91470a9cf97b\n\n";
        }

    }
