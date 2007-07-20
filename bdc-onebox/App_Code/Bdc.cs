using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.Data;
using Microsoft.Office.Server.ApplicationRegistry.MetadataModel;
using Microsoft.Office.Server.ApplicationRegistry.Runtime;
using Microsoft.Office.Server.ApplicationRegistry.SystemSpecific;
using Microsoft.Office.Server.ApplicationRegistry.Infrastructure;
using WSSAdmin = Microsoft.SharePoint.Portal.Administration;
using OSSAdmin = Microsoft.Office.Server.Administration;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace OneBox
{
    public class Bdc
    {
        const string yourSSPName = "SharedServices1";
        static Hashtable _lobInsts = new Hashtable();
        static Hashtable _lobs = new Hashtable();
        public static BdcConfig _cfg;

        LobSystemInstance _lobInst = null;
        Lob _lob = null;
        Hashtable _keys = new Hashtable();
        Microsoft.Office.Server.ApplicationRegistry.MetadataModel.Entity _entityInst;
        Entity _entity;
        public String _lobName, _entityName;
        public Bdc()
        {
        }

        public static void init(String configFile)
        {
            //read config
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(BdcConfig));
            XmlTextReader reader = new XmlTextReader(configFile);
            _cfg = (BdcConfig)x.Deserialize(reader);
            reader.Close();
            // setup bdc
            SetupBDC();
        }
        public String find(String query)
        {
            return "";
        }
        public String find(String lob, String entityName, String fieldName, String query)
        {

            //DisplayLOBSystemsinBDC();
            GetLob(lob);
            GetEntity(entityName);
            IEntityInstanceEnumerator enums = FindStuff(fieldName, query);
            return this.GenerateXml(enums);
        }
        public static void SetupBDC()
        {
            SqlSessionProvider.Instance().SetSharedResourceProviderToUse(yourSSPName);
        }

        void GetLob(String slob)
        {
            if (!_lobs.ContainsKey(slob))
            {
                NamedLobSystemInstanceDictionary sysInstances = ApplicationRegistry.GetLobSystemInstances();
                LobSystemInstance l = sysInstances[slob];
                
                IEnumerator it = _cfg.lobCollection.GetEnumerator();
                while (it.MoveNext())
                {
                    Lob lob = (Lob)it.Current;
                    if (lob.name.Equals(slob))
                    {
                        _lobInsts.Add(slob, l); //the real lob
                        _lobs.Add(slob, lob); //the config of lob
                        break;
                    }
                }
            }
            _lobInst = (LobSystemInstance)_lobInsts[slob];
            _lob = (Lob)_lobs[slob];
            if (_lobInst == null)
            {
                throw new Exception(slob + " does not exist in this service provider, check the name to make sure it's correct.");
            }
            if (_lob == null)
            {
                throw new Exception(slob + " is not found in bdc.xml");
            }
            return;
        }

        void GetEntity(String entityName)
        {
            IEnumerator it = _lob.entityCollection.GetEnumerator();
            while (it.MoveNext())
            {
                Entity en = (Entity)it.Current;
                if (en.name.Equals(entityName))
                {
                    _entity = en;
                    break;
                }
            }
            _entityInst = _lobInst.GetEntities()[entityName];
        }

        /// <summary>
        /// Find by ID
        /// </summary>
        /// <returns></returns>
         
        IEntityInstanceEnumerator FindStuff()
        {
            IEnumerator it = _entityInst.GetMethodInstances().Keys.GetEnumerator();
            MethodInstance m = null;
            while (it.MoveNext())
            {
                String key = (String)it.Current;
                m = (MethodInstance)_entityInst.GetMethodInstances()[key];
                {
                    String b = m.MethodInstanceType.ToString();
                    if (b.Equals("IdEnumerator"))
                    {
                        break;
                    }
                }
            }
            IEntityInstanceEnumerator  itt = (IEntityInstanceEnumerator )   _entityInst.Execute(m, _lobInst);
            while (itt.MoveNext())
            {
                IEntityInstance e = (IEntityInstance)itt.Current;
                DataRow row = e.EntityAsDataRow(e.EntityAsDataTable);
                Object ob = row[0];
                IEntityInstance ent = _entityInst.FindSpecific(ob, _lobInst);
                FormatRecord(ent);
            }
            
            return itt;
        }

        void FormatRecord(IEntityInstance ie)
        {
            foreach (Field f in _entityInst.GetSpecificFinderView().Fields)
            {
                String key = f.Name;
                String value = ie[f].ToString();
                if (_keys.ContainsKey(key))
                {
            //        u = u.Replace("{" + key + "}", value);
                }
            }
        }
        /// <summary>
        /// 
        /// Find on a field value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IEntityInstanceEnumerator FindStuff(String name, String value)
        {
            FilterCollection fc = _entityInst.GetFinderFilters();
            FilterCollection newfc = new FilterCollection();
            for (int i = 0; i < fc.Count; i++)
            {
                if (fc[i].GetType().FullName == "Microsoft.Office.Server.ApplicationRegistry.Runtime.WildcardFilter")
                {
                    if (string.IsNullOrEmpty(name) || (0 == string.Compare(fc[i].Name.ToLower(), name.ToLower())))
                    {
                        newfc.Add
                            (fc[i]);
                    }
                }
            }
            ((WildcardFilter)newfc[0]).Value = "%" + value + "%";
            return _entityInst.FindFiltered(newfc, _lobInst);
        }


        String GenerateXml(IEntityInstanceEnumerator enums)
        {
            XmlDocument xmldoc = new XmlDocument();
            //let's add the header
            XmlNode xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmldoc.AppendChild(xmlnode);
            XmlElement xmlelem = xmldoc.CreateElement("", "OneBoxResults", "http://www.w3.org/2001/XMLSchema-instance");
            xmldoc.AppendChild(xmlelem);
            XmlElement xmlelem2 = xmldoc.CreateElement(null, "resultCode", null);
            xmlelem.AppendChild(xmlelem2);
            xmlelem2.AppendChild(xmldoc.CreateTextNode("Success"));
            xmlelem2 = xmldoc.CreateElement("provider");
            xmlelem.AppendChild(xmlelem2);
            xmlelem2.AppendChild(xmldoc.CreateTextNode("MOSS BDC"));
            xmlelem2 = xmldoc.CreateElement("title");
            xmlelem.AppendChild(xmlelem2);
            XmlElement xmlelem3 = xmldoc.CreateElement("urlText");
            xmlelem2.AppendChild(xmlelem3);
            xmlelem3.AppendChild(xmldoc.CreateTextNode("Results in MOSS - BDC"));
            xmlelem3 = xmldoc.CreateElement("urlLink");
            xmlelem2.AppendChild(xmlelem3);
            xmlelem3.AppendChild(xmldoc.CreateTextNode(_entity.listUrl));

            //find keys
            Regex reg = new Regex("{([^}]*)}");
            //process actions
            //since onebox only allows one url,we only use one action here
            IEnumerator ait = _entity.actionCollection.GetEnumerator();
            String url = null;
            while (ait.MoveNext())
            {
                Action a = (Action)ait.Current;
                url = a.url;
                MatchCollection col = reg.Matches(url);
                IEnumerator it = col.GetEnumerator();
                while (it.MoveNext())
                {
                    String match = ((Match)it.Current).Value;
                    String key = match.Substring(1, match.Length - 2);
                    _keys.Add(key, key);
                }
            }

            //now let's add body
            //let's add another element (child of the root)
            enums.Reset();
            while (enums.MoveNext())
            {
                xmlelem2 = xmldoc.CreateElement("MODULE_RESULT");
                xmlelem.AppendChild(xmlelem2);
                IEntityInstance IE = enums.Current;
                String u = url;
                foreach (Field f in _entityInst.GetFinderView().Fields)
                {
                    String key = f.Name;
                    String value = IE[f] == null? "": IE[f].ToString();
                    if (_keys.ContainsKey(key))
                    {
                        u = u.Replace("{" + key + "}", value); 
                    }
                    xmlelem3 = xmldoc.CreateElement("Field");
                    XmlAttribute attr = xmldoc.CreateAttribute("name");
                    attr.Value = key;
                    xmlelem3.Attributes.Append(attr);
                    xmlelem2.AppendChild(xmlelem3);
                    xmlelem3.AppendChild(xmldoc.CreateTextNode(value));
                }
                if (u != null)
                {
                    xmlelem3 = xmldoc.CreateElement("U");
                    xmlelem2.AppendChild(xmlelem3);
                    xmlelem3.AppendChild(xmldoc.CreateTextNode(u));
                }
            }
            return toString(xmldoc);
        }
        String toString(XmlDocument xmldoc)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter stringWriter =
              new System.IO.StringWriter(sb);
            System.Xml.XmlTextWriter xmlWriter =
              new System.Xml.XmlTextWriter(stringWriter);
            xmldoc.WriteContentTo(xmlWriter);
            return sb.ToString();
        }
        static void WriteConfig(String pwd)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(BdcConfig));
            BdcConfig cfg = new BdcConfig();
            cfg.ssp = "SharedService1";
            Lob lob = new Lob();
            Entity entity = new Entity();
            entity.listUrl = "http://abc.com";
            entity.name = "Product";
            Action action = new Action();
            action.url = "http://bbd";
            action.name = "view";
            entity.actionCollection.Add(action);

            lob.entityCollection.Add(entity);
            cfg.lobCollection.Add(lob);
            XmlTextWriter writer = new XmlTextWriter(pwd + "bdc.xml", Encoding.UTF8);
            x.Serialize(writer, cfg);
            writer.Close();
        }

        static void GetConfig(String pwd)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(BdcConfig));
            XmlTextReader reader = new XmlTextReader(pwd + "bdc.xml");
            _cfg = (BdcConfig)x.Deserialize(reader);
            reader.Close();
            /*
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(reader);
            XmlElement root = xmldoc.DocumentElement;
            XmlNodeList entityNodes, authorNodes;
            entityNodes = root.SelectNodes("entity");
            String aName, aValue;
            ArrayList articles = new ArrayList();
            articleNodes = section.SelectNodes("article");

            foreach (XmlNode NEntity in entityNodes)
            {
                aName = NArticle.Attributes.GetNamedItem("title").Value;
                aValue = NArticle.Attributes.GetNamedItem("url").Value;
                ArrayList authors = new ArrayList();
                authorNodes = NArticle.SelectNodes("authors//author");
                if (!(authorNodes == null))
                {
                    foreach (XmlNode NAuthor in authorNodes)
                    {
                        authors.Add(NAuthor.InnerText);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.Write("No authors found.");
                }
                articles.Add(new Article(aName, aValue, authors));
            }
            return articles;
             */
        }
    }
}
