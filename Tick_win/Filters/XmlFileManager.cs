using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Tick_win.Models;

namespace Tick_win.Filters
{
    public class XmlFileManager
    {
        
        private string _connection = HttpContext.Current.Server.MapPath("~/XmlLogStorsge/XmlLog.xml"); //Строка доступа к файлу

        //Логирование действий полбзователя

        public void XmlSave(LogData data)
        {

            XmlDocument xDoc = new XmlDocument();
            try
            {

                xDoc.Load(_connection);

                XmlElement xRoot = xDoc.DocumentElement;


                XmlElement session = xDoc.CreateElement("Session");
                XmlElement userName = xDoc.CreateElement("UserName");
                XmlElement userAction = xDoc.CreateElement("UserAction");
                XmlElement Date = xDoc.CreateElement("Date");

                XmlText name = xDoc.CreateTextNode($"{data.UserName}");
                XmlText action = xDoc.CreateTextNode($"{data.UserAction}");
                XmlText date = xDoc.CreateTextNode($"{data.Date}");

                userName.AppendChild(name);
                userAction.AppendChild(action);
                Date.AppendChild(date);

                session.AppendChild(userName);
                session.AppendChild(userAction);
                session.AppendChild(Date);
                xRoot.AppendChild(session);

                xDoc.Save(_connection);

            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
        }

        //Логирование исключений пользователя

        public void XmlSave(ExViewModel data)
        {

            XmlDocument xDoc = new XmlDocument();
            try
            {

                xDoc.Load(_connection);

                XmlElement xRoot = xDoc.DocumentElement;


                XmlElement error = xDoc.CreateElement("Exception");
                XmlElement userName = xDoc.CreateElement("UserName");
                XmlElement exMessage = xDoc.CreateElement("ExceptionMessage");
                XmlElement userAction = xDoc.CreateElement("ControllerName");
                XmlElement stackTrace = xDoc.CreateElement("ExceptionStackTrace");
                XmlElement Date = xDoc.CreateElement("LogTime");

                XmlText name = xDoc.CreateTextNode($"{data.UserName}");
                XmlText exMes = xDoc.CreateTextNode($"{data.ExMessage}");
                XmlText action = xDoc.CreateTextNode($"{data.ControllerName}");
                XmlText sTrace = xDoc.CreateTextNode($"{data.ExTrace}");
                XmlText date = xDoc.CreateTextNode($"{data.LogTime}");

                userName.AppendChild(name);
                exMessage.AppendChild(exMes);
                userAction.AppendChild(action);
                stackTrace.AppendChild(sTrace);
                Date.AppendChild(date);

                error.AppendChild(userName);
                error.AppendChild(exMessage);
                error.AppendChild(userAction);
                error.AppendChild(stackTrace);
                error.AppendChild(Date);
                xRoot.AppendChild(error);

                xDoc.Save(_connection);


            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
        }
    }

    class FileNotFoundException : Exception
    {
        public FileNotFoundException() : base("Файл не найден")
        {
        }
    }
}