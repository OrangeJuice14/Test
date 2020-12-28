using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HRMWebApp.KPI.Core.DTO
{

    [Serializable]
    [XmlRoot(Namespace = "mydomain.com")]
    public class VanbanGui
    {
        private MessageHeader messageheader = null;
        public MessageHeader Messageheader
        {
            get { return messageheader; }
            set { messageheader = value; }
        }
        private Document document = null;
        public Document Document
        {
            get { return document; }
            set { document = value; }
        }
        private string subject = string.Empty;
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        private string _idObject;
        public string IdObject
        {
            get { return _idObject; }
            set { _idObject = value; }
        }
        private string tinhChat = string.Empty;
        public string TinhChat
        {
            get { return tinhChat; }
            set { tinhChat = value; }
        }
        private string _loaiVanban = string.Empty;
        public virtual string LoaiVanban
        {
            get { return _loaiVanban; }
            set { _loaiVanban = value; }
        }
        private string _linhVuc = string.Empty;
        public string LinhVuc
        {
            get { return _linhVuc; }
            set { _linhVuc = value; }
        }
        private string _soKyhieu = string.Empty;
        public string SoKyhieu
        {
            get { return _soKyhieu; }
            set { _soKyhieu = value; }
        }

        private string _dateSign = string.Empty;
        public string DateSign
        {
            get { return _dateSign; }
            set { _dateSign = value; }
        }

        private string _nguoiKy = string.Empty;
        public string NguoiKy
        {
            get { return _nguoiKy; }
            set { _nguoiKy = value; }
        }
        private string _chucVu = string.Empty;
        public string ChucVu
        {
            get { return _chucVu; }
            set { _chucVu = value; }
        }
        private string soBan = string.Empty;
        public string SoBan
        {
            get { return soBan; }
            set { soBan = value; }
        }
        private string soTrang = string.Empty;
        public string SoTrang
        {
            get { return soTrang; }
            set { soTrang = value; }
        }
    }
    [Serializable]
    public class MessageHeader
    {
        From _from = null;
        public From From
        {
            get { return _from; }
            set { _from = value; }
        }
        List<To> _to = new List<To>();
        [XmlElement("To")]
        public List<To> To
        {
            get { return _to; }
            set { _to = value; }
        }
    }
    [Serializable]
    public class To
    {
        private string organId = string.Empty;
        public string OrganId
        {
            get { return organId; }
            set { organId = value; }
        }
        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string email = string.Empty;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public To(string orid, string name, string email)
        {
            OrganId = orid;
            Name = name;
            Email = email;
        }
        public To() { }
    }
    [Serializable]
    public class From : To
    {
        private string organizationInCharge = string.Empty;
        public string OrganizationInCharge
        {
            get { return organizationInCharge; }
            set { organizationInCharge = value; }
        }
        private string url = string.Empty;
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        public From()
        { }
        //public From(string orid, string orchange, string name, string email)
        //{
        //    OrganId = orid;
        //    OrganizationInCharge = orchange;
        //    Name = name;
        //    Email = email;
        //}

    }
    [Serializable]
    public class Code
    {
        private string codeNumber = string.Empty;
        public string CodeNumber
        {
            get { return codeNumber; }
            set { codeNumber = value; }
        }
        private string codeNotation = string.Empty;
        public string CodeNotation
        {
            get { return codeNotation; }
            set { codeNotation = value; }
        }
    }
    [Serializable]
    public class Attachment
    {
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private byte[] value = null;
        public byte[] Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
    [Serializable]
    public class Document
    {
        private List<Attachment> attach = null;
        public List<Attachment> Attach
        {
            get { return attach; }
            set { attach = value; }
        }
    }
}
