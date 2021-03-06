// Copyright 2004, Microsoft Corporation
// Sample Code - Use restricted to terms of use defined in the accompanying license agreement (EULA.doc)

//--------------------------------------------------------------
// Autogenerated by XSDObjectGen version 1.4.4.1
// Schema file: bdc.xsd
// Creation Date: 12/31/2006 10:46:47 AM
//--------------------------------------------------------------

using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace OneBox
{

    public struct Declarations
    {
        public const string SchemaVersion = "http://schemas.google.com/enterprise/2006/12/bdc";
    }


    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class EntityCollection : ArrayList
    {
        public OneBox.Entity Add(OneBox.Entity obj)
        {
            base.Add(obj);
            return obj;
        }

        public OneBox.Entity Add()
        {
            return Add(new OneBox.Entity());
        }

        public void Insert(int index, OneBox.Entity obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(OneBox.Entity obj)
        {
            base.Remove(obj);
        }

        new public OneBox.Entity this[int index]
        {
            get { return (OneBox.Entity)base[index]; }
            set { base[index] = value; }
        }
    }

    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class LobCollection : ArrayList
    {
        public OneBox.Lob Add(OneBox.Lob obj)
        {
            base.Add(obj);
            return obj;
        }

        public OneBox.Lob Add()
        {
            return Add(new OneBox.Lob());
        }

        public void Insert(int index, OneBox.Lob obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(OneBox.Lob obj)
        {
            base.Remove(obj);
        }

        new public OneBox.Lob this[int index]
        {
            get { return (OneBox.Lob)base[index]; }
            set { base[index] = value; }
        }
    }

    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class ActionCollection : ArrayList
    {
        public OneBox.Action Add(OneBox.Action obj)
        {
            base.Add(obj);
            return obj;
        }

        public OneBox.Action Add()
        {
            return Add(new OneBox.Action());
        }

        public void Insert(int index, OneBox.Action obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(OneBox.Action obj)
        {
            base.Remove(obj);
        }

        new public OneBox.Action this[int index]
        {
            get { return (OneBox.Action)base[index]; }
            set { base[index] = value; }
        }
    }



    [XmlType(TypeName = "Entity", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class Entity
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return actionCollection.GetEnumerator();
        }

        public OneBox.Action Add(OneBox.Action obj)
        {
            return actionCollection.Add(obj);
        }

        [XmlIgnore]
        public OneBox.Action this[int index]
        {
            get { return (OneBox.Action)actionCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return actionCollection.Count; }
        }

        public void Clear()
        {
            actionCollection.Clear();
        }

        public OneBox.Action Remove(int index)
        {
            OneBox.Action obj = actionCollection[index];
            actionCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            actionCollection.Remove(obj);
        }

        [XmlAttribute(AttributeName = "name", DataType = "string")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __name;

        [XmlIgnore]
        public string name
        {
            get { return __name; }
            set { __name = value; }
        }

        [XmlAttribute(AttributeName = "listUrl", DataType = "string")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __listUrl;

        [XmlIgnore]
        public string listUrl
        {
            get { return __listUrl; }
            set { __listUrl = value; }
        }

        [XmlElement(Type = typeof(OneBox.Action), ElementName = "action", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ActionCollection __actionCollection;

        [XmlIgnore]
        public ActionCollection actionCollection
        {
            get
            {
                if (__actionCollection == null) __actionCollection = new ActionCollection();
                return __actionCollection;
            }
            set { __actionCollection = value; }
        }

        public Entity()
        {
            name = string.Empty;
        }
    }


    [XmlType(TypeName = "Lob", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class Lob
    {
        [System.Runtime.InteropServices.DispIdAttribute(-4)]
        public IEnumerator GetEnumerator()
        {
            return entityCollection.GetEnumerator();
        }

        public OneBox.Entity Add(OneBox.Entity obj)
        {
            return entityCollection.Add(obj);
        }

        [XmlIgnore]
        public OneBox.Entity this[int index]
        {
            get { return (OneBox.Entity)entityCollection[index]; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return entityCollection.Count; }
        }

        public void Clear()
        {
            entityCollection.Clear();
        }

        public OneBox.Entity Remove(int index)
        {
            OneBox.Entity obj = entityCollection[index];
            entityCollection.Remove(obj);
            return obj;
        }

        public void Remove(object obj)
        {
            entityCollection.Remove(obj);
        }

        [XmlAttribute(AttributeName = "name", DataType = "string")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __name;

        [XmlIgnore]
        public string name
        {
            get { return __name; }
            set { __name = value; }
        }

        [XmlElement(Type = typeof(OneBox.Entity), ElementName = "entity", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public EntityCollection __entityCollection;

        [XmlIgnore]
        public EntityCollection entityCollection
        {
            get
            {
                if (__entityCollection == null) __entityCollection = new EntityCollection();
                return __entityCollection;
            }
            set { __entityCollection = value; }
        }

        public Lob()
        {
            name = string.Empty;
        }
    }


    [XmlType(TypeName = "BdcConfig", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class BdcConfig
    {

        [XmlElement(ElementName = "ssp", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __ssp;

        [XmlIgnore]
        public string ssp
        {
            get { return __ssp; }
            set { __ssp = value; }
        }

        [XmlElement(Type = typeof(OneBox.Lob), ElementName = "lob", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public LobCollection __lobCollection;

        [XmlIgnore]
        public LobCollection lobCollection
        {
            get
            {
                if (__lobCollection == null) __lobCollection = new LobCollection();
                return __lobCollection;
            }
            set { __lobCollection = value; }
        }

        public BdcConfig()
        {
            ssp = string.Empty;
        }
    }


    [XmlType(TypeName = "Action", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class Action
    {

        [XmlAttribute(AttributeName = "name", DataType = "string")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __name;

        [XmlIgnore]
        public string name
        {
            get { return __name; }
            set { __name = value; }
        }

        [XmlAttribute(AttributeName = "url", DataType = "string")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public string __url;

        [XmlIgnore]
        public string url
        {
            get { return __url; }
            set { __url = value; }
        }

        public Action()
        {
        }
    }


    [XmlRoot(ElementName = "bdc", Namespace = Declarations.SchemaVersion, IsNullable = false), Serializable]
    public class bdc : OneBox.BdcConfig
    {

        public bdc()
            : base()
        {
        }
    }
}
