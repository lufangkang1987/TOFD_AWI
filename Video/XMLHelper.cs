using System;
using System.Xml;
using System.Diagnostics;

namespace STARDC.Xml
{
    public static class XmlHelper
    {
        /// <summary>
        /// 生成一个XML文件
        /// </summary>
        /// <param name="encoding">gb2312</param>
        /// <param name="localName">XData</param>
        /// <returns></returns>
        public static XmlDocument Create(string encoding, string localName)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            try
            {
                if (localName.Trim().Length == 0)
                    localName = "XData";
                if (encoding.Trim().Length == 0)
                    encoding = "gb2312";
                XmlNode xmlnode = objXmlDoc.CreateXmlDeclaration("1.0", encoding, null);
                objXmlDoc.AppendChild(xmlnode);
                //加入一个根元素
                XmlElement xmlelem = objXmlDoc.CreateElement(localName);
                xmlelem.AppendChild(objXmlDoc.CreateTextNode(""));
                objXmlDoc.AppendChild(xmlelem);
                return objXmlDoc;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return objXmlDoc;
            }
        }
        /// <summary>
        /// 生成一个XML文件
        /// </summary>
        /// <returns></returns>
        public static XmlDocument Create()
        {
            XmlDocument objXmlDoc = new XmlDocument();
            try
            {
                XmlNode xmlnode = objXmlDoc.CreateXmlDeclaration("1.0", "gb2312", null);
                objXmlDoc.AppendChild(xmlnode);
                //加入一个根元素
                XmlElement xmlelem = objXmlDoc.CreateElement("", "XData", "");
                xmlelem.AppendChild(objXmlDoc.CreateTextNode(""));
                objXmlDoc.AppendChild(xmlelem);
                return objXmlDoc;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return objXmlDoc;
            }
        }
        /// <summary>
        /// 添加子节点及值
        /// </summary>
        /// <param name="aParentNode"></param>
        /// <param name="aChildName"></param>
        /// <returns></returns>
        public static XmlNode AddChildNode(XmlNode aParentNode, string aChildName, string aChildValue)
        {
            if (aParentNode == null)
                return null;
            XmlNode _node = aParentNode.OwnerDocument.CreateNode(XmlNodeType.Element, aChildName, string.Empty);
            _node.InnerText = aChildValue;
            aParentNode.AppendChild(_node);
            return _node;
        }
        /// <summary>
        /// 添加子节点及属性
        /// </summary>
        /// <param name="aParentNode"></param>
        /// <param name="aChildName"></param>
        /// <param name="aChildValue"></param>
        /// <param name="aAttName"></param>
        /// <param name="aValue"></param>
        /// <returns></returns>
        public static XmlNode AddChildNode(XmlNode aParentNode, string aChildName, string aChildValue, string aAttName, string aValue)
        {
            if (aParentNode == null)
                return null;
            XmlNode _node = aParentNode.OwnerDocument.CreateNode(XmlNodeType.Element, aChildName, string.Empty);
            _node.InnerText = aChildValue;
            if (aAttName.Trim().Length > 0)
                SetAttriText(_node, aAttName, aValue);
            aParentNode.AppendChild(_node);
            return _node;
        }
        /// <summary>
        /// 设置节点属性及值
        /// </summary>
        /// <param name="aNode"></param>
        /// <param name="aAttName"></param>
        /// <param name="aValue"></param>
        public static XmlNode SetAttriText(XmlNode aNode, string aAttName, string aValue)
        {
            try
            {
                if (aNode == null)
                    return null;
                else
                {
                    if (aNode.Attributes[aAttName] == null)
                    {
                        XmlNode TemAtt = aNode.OwnerDocument.CreateNode(XmlNodeType.Attribute, aAttName, string.Empty);
                        TemAtt.Value = aValue;
                        aNode.Attributes.SetNamedItem(TemAtt);
                        return TemAtt;
                    }
                    else
                    {
                        aNode.Attributes[aAttName].Value = aValue;
                        return aNode;
                    }                    
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return null;
            }
        }
        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="ValueName"></param>
        /// <returns></returns>
        public static string GetInnerText(XmlNode xmlNode, string ValueName)
        {
            string Value = string.Empty;
            try
            {
                if (xmlNode == null)
                    return Value;
                XmlNode Node = xmlNode.SelectSingleNode("./" + ValueName);
                if (Node != null)
                    return Node.InnerText;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
            return Value;
        }
        /// <summary>
        /// 设置节点值
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="ValueName"></param>
        /// <returns></returns>
        public static XmlNode SetInnerText(XmlNode xmlNode, string aChildName, string aChildValue)
        {
            try
            {
                if (xmlNode == null)
                    return xmlNode;
                XmlNode Node = xmlNode.SelectSingleNode("./" + aChildName);
                if (Node == null)
                    Node = AddChildNode(xmlNode, aChildName, aChildValue);

                Node.InnerText = aChildValue;
                return xmlNode;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return xmlNode;
            }
        }
        /// <summary>
        /// 获取节点属性值
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="ValueName"></param>
        /// <returns></returns>
        public static string GetAttriText(XmlNode xmlNode, string ValueName)
        {
            string Value = string.Empty;
            try
            {
                if (xmlNode == null)
                    return Value;
                if (xmlNode.Attributes[ValueName] != null)
                    Value = xmlNode.Attributes[ValueName].Value;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
            return Value;
        }
        /// <summary>
        /// 获取节点属性值 带默认参数
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="ValueName"></param>
        /// <param name="_Default"></param>
        /// <returns></returns>
        public static string GetAttriText(XmlNode xmlNode, string ValueName, string _Default)
        {
            string Value = _Default;
            try
            {
                if (xmlNode.Attributes[ValueName] != null)
                    Value = xmlNode.Attributes[ValueName].Value;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
            return Value;
        }         
    }
}