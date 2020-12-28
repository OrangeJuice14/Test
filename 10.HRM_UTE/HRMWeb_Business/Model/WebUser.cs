//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
//	  Code duoc tao boi SERVERERP\tai luc 09:33:23 ngay 03/10/2017
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
//using System.Data.EntityClient;
using System.Data.Entity.Core.EntityClient;
//using System.Data.Objects;
using System.Data.Entity.Core.Objects;
//using System.Data.Objects.DataClasses;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace HRMWeb_Business.Model
{
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="ERPModel", Name="WebUser")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class WebUser : ERP_Core.BaseEntityObject//EntityObject
    {
    	static WebUser()
    	{
    
    		AfterStaticConstruction();
        }
        static partial void AfterStaticConstruction();
    
    	public WebUser()
    	{
    
    		this.AfterConstruction();
        }
        partial void AfterConstruction();
    
        #region Factory Method
    
        /// <summary>
        /// Create a new WebUser object.
        /// </summary>
        /// <param name="oid">Initial value of the Oid property.</param>
        public static WebUser CreateWebUser(System.Guid oid)
        {
            WebUser webUser = new WebUser();
            webUser.Oid = oid;
            return webUser;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public System.Guid Oid
        {
            get
            {
                return _oid;
            }
            set
            {
                if (_oid != value)
                {
        			System.Guid oldValue =  _oid;
        			bool stopChanging = false;
                    On_Oid_Changing(oldValue, ref value, ref stopChanging);
                    ReportPropertyChanging("Oid");
        			if(!stopChanging)
        			{
        				_oid = StructuralObject.SetValidValue(value);
        				ReportPropertyChanged("Oid");
        				On_Oid_Changed(oldValue, _oid);//\\
        			}
                }
            }
        }
    	public static String Oid_PropertyName { get { return "Oid"; } }
        private System.Guid _oid;
        partial void On_Oid_Changing(System.Guid currentValue, ref System.Guid newValue, ref bool stopChanging);
        partial void On_Oid_Changed(System.Guid oldValue, System.Guid currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
    			string oldValue =  _userName;
    			bool stopChanging = false;
                On_UserName_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("UserName");
    			if(!stopChanging)
    			{
    				_userName = StructuralObject.SetValidValue(value, true);
    				ReportPropertyChanged("UserName");
    				On_UserName_Changed(oldValue, _userName);//\\
    			}
            }
        }
    	public static String UserName_PropertyName { get { return "UserName"; } }
        private string _userName;
        partial void On_UserName_Changing(string currentValue, ref string newValue, ref bool stopChanging);
        partial void On_UserName_Changed(string oldValue, string currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public string PassWord
        {
            get
            {
                return _passWord;
            }
            set
            {
    			string oldValue =  _passWord;
    			bool stopChanging = false;
                On_PassWord_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("PassWord");
    			if(!stopChanging)
    			{
    				_passWord = StructuralObject.SetValidValue(value, true);
    				ReportPropertyChanged("PassWord");
    				On_PassWord_Changed(oldValue, _passWord);//\\
    			}
            }
        }
    	public static String PassWord_PropertyName { get { return "PassWord"; } }
        private string _passWord;
        partial void On_PassWord_Changing(string currentValue, ref string newValue, ref bool stopChanging);
        partial void On_PassWord_Changed(string oldValue, string currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<bool> HoatDong
        {
            get
            {
                return _hoatDong;
            }
            set
            {
    			Nullable<bool> oldValue =  _hoatDong;
    			bool stopChanging = false;
                On_HoatDong_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("HoatDong");
    			if(!stopChanging)
    			{
    				_hoatDong = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("HoatDong");
    				On_HoatDong_Changed(oldValue, _hoatDong);//\\
    			}
            }
        }
    	public static String HoatDong_PropertyName { get { return "HoatDong"; } }
        private Nullable<bool> _hoatDong;
        partial void On_HoatDong_Changing(Nullable<bool> currentValue, ref Nullable<bool> newValue, ref bool stopChanging);
        partial void On_HoatDong_Changed(Nullable<bool> oldValue, Nullable<bool> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<bool> UserChamCong
        {
            get
            {
                return _userChamCong;
            }
            set
            {
    			Nullable<bool> oldValue =  _userChamCong;
    			bool stopChanging = false;
                On_UserChamCong_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("UserChamCong");
    			if(!stopChanging)
    			{
    				_userChamCong = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("UserChamCong");
    				On_UserChamCong_Changed(oldValue, _userChamCong);//\\
    			}
            }
        }
    	public static String UserChamCong_PropertyName { get { return "UserChamCong"; } }
        private Nullable<bool> _userChamCong;
        partial void On_UserChamCong_Changing(Nullable<bool> currentValue, ref Nullable<bool> newValue, ref bool stopChanging);
        partial void On_UserChamCong_Changed(Nullable<bool> oldValue, Nullable<bool> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> WebGroupID
        {
            get
            {
                return _webGroupID;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _webGroupID;
    			bool stopChanging = false;
                On_WebGroupID_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("WebGroupID");
    			if(!stopChanging)
    			{
    				_webGroupID = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("WebGroupID");
    				On_WebGroupID_Changed(oldValue, _webGroupID);//\\
    			}
            }
        }
    	public static String WebGroupID_PropertyName { get { return "WebGroupID"; } }
        private Nullable<System.Guid> _webGroupID;
        partial void On_WebGroupID_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_WebGroupID_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> ThongTinNhanVien
        {
            get
            {
                return _thongTinNhanVien;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _thongTinNhanVien;
    			bool stopChanging = false;
                On_ThongTinNhanVien_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("ThongTinNhanVien");
    			if(!stopChanging)
    			{
    				_thongTinNhanVien = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("ThongTinNhanVien");
    				On_ThongTinNhanVien_Changed(oldValue, _thongTinNhanVien);//\\
    			}
            }
        }
    	public static String ThongTinNhanVien_PropertyName { get { return "ThongTinNhanVien"; } }
        private Nullable<System.Guid> _thongTinNhanVien;
        partial void On_ThongTinNhanVien_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_ThongTinNhanVien_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<int> OptimisticLockField
        {
            get
            {
                return _optimisticLockField;
            }
            set
            {
    			Nullable<int> oldValue =  _optimisticLockField;
    			bool stopChanging = false;
                On_OptimisticLockField_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("OptimisticLockField");
    			if(!stopChanging)
    			{
    				_optimisticLockField = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("OptimisticLockField");
    				On_OptimisticLockField_Changed(oldValue, _optimisticLockField);//\\
    			}
            }
        }
    	public static String OptimisticLockField_PropertyName { get { return "OptimisticLockField"; } }
        private Nullable<int> _optimisticLockField;
        partial void On_OptimisticLockField_Changing(Nullable<int> currentValue, ref Nullable<int> newValue, ref bool stopChanging);
        partial void On_OptimisticLockField_Changed(Nullable<int> oldValue, Nullable<int> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<int> GCRecord
        {
            get
            {
                return _gCRecord;
            }
            set
            {
    			Nullable<int> oldValue =  _gCRecord;
    			bool stopChanging = false;
                On_GCRecord_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("GCRecord");
    			if(!stopChanging)
    			{
    				_gCRecord = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("GCRecord");
    				On_GCRecord_Changed(oldValue, _gCRecord);//\\
    			}
            }
        }
    	public static String GCRecord_PropertyName { get { return "GCRecord"; } }
        private Nullable<int> _gCRecord;
        partial void On_GCRecord_Changing(Nullable<int> currentValue, ref Nullable<int> newValue, ref bool stopChanging);
        partial void On_GCRecord_Changed(Nullable<int> oldValue, Nullable<int> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public string AdminEmail
        {
            get
            {
                return _adminEmail;
            }
            set
            {
    			string oldValue =  _adminEmail;
    			bool stopChanging = false;
                On_AdminEmail_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("AdminEmail");
    			if(!stopChanging)
    			{
    				_adminEmail = StructuralObject.SetValidValue(value, true);
    				ReportPropertyChanged("AdminEmail");
    				On_AdminEmail_Changed(oldValue, _adminEmail);//\\
    			}
            }
        }
    	public static String AdminEmail_PropertyName { get { return "AdminEmail"; } }
        private string _adminEmail;
        partial void On_AdminEmail_Changing(string currentValue, ref string newValue, ref bool stopChanging);
        partial void On_AdminEmail_Changed(string oldValue, string currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<int> AgentObjectTypeId
        {
            get
            {
                return _agentObjectTypeId;
            }
            set
            {
    			Nullable<int> oldValue =  _agentObjectTypeId;
    			bool stopChanging = false;
                On_AgentObjectTypeId_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("AgentObjectTypeId");
    			if(!stopChanging)
    			{
    				_agentObjectTypeId = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("AgentObjectTypeId");
    				On_AgentObjectTypeId_Changed(oldValue, _agentObjectTypeId);//\\
    			}
            }
        }
    	public static String AgentObjectTypeId_PropertyName { get { return "AgentObjectTypeId"; } }
        private Nullable<int> _agentObjectTypeId;
        partial void On_AgentObjectTypeId_Changing(Nullable<int> currentValue, ref Nullable<int> newValue, ref bool stopChanging);
        partial void On_AgentObjectTypeId_Changed(Nullable<int> oldValue, Nullable<int> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> DepartmentId
        {
            get
            {
                return _departmentId;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _departmentId;
    			bool stopChanging = false;
                On_DepartmentId_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("DepartmentId");
    			if(!stopChanging)
    			{
    				_departmentId = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("DepartmentId");
    				On_DepartmentId_Changed(oldValue, _departmentId);//\\
    			}
            }
        }
    	public static String DepartmentId_PropertyName { get { return "DepartmentId"; } }
        private Nullable<System.Guid> _departmentId;
        partial void On_DepartmentId_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_DepartmentId_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);

        #endregion

        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_WebUsers_HoSo", "HoSo")]
        public HoSo HoSo
        {//test
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<HoSo>("ERPModel.FK_WebUsers_HoSo", "HoSo").Value;
            }
            set
            {
    			bool stopChanging = false;
    			On_HoSo_Changing(ref value, ref stopChanging);//\\//
    			if(!stopChanging)
    			{
    				((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<HoSo>("ERPModel.FK_WebUsers_HoSo", "HoSo").Value = value;
    				On_HoSo_Changed(this.HoSo);//\\//
    			}
    	    }
        }
    	public static String HoSo_ReferencePropertyName { get { return "HoSo"; } }
    	partial void On_HoSo_Changing(ref HoSo newValue, ref bool stopChanging);
    	partial void On_HoSo_Changed(HoSo currentValue);
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<HoSo> HoSoReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<HoSo>("ERPModel.FK_WebUsers_HoSo", "HoSo");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<HoSo>("ERPModel.FK_WebUsers_HoSo", "HoSo", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_WebUsers_ThongTinNhanVien", "ThongTinNhanVien")]
        public ThongTinNhanVien ThongTinNhanVien1
        {//test
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<ThongTinNhanVien>("ERPModel.FK_WebUsers_ThongTinNhanVien", "ThongTinNhanVien").Value;
            }
            set
            {
    			bool stopChanging = false;
    			On_ThongTinNhanVien1_Changing(ref value, ref stopChanging);//\\//
    			if(!stopChanging)
    			{
    				((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<ThongTinNhanVien>("ERPModel.FK_WebUsers_ThongTinNhanVien", "ThongTinNhanVien").Value = value;
    				On_ThongTinNhanVien1_Changed(this.ThongTinNhanVien1);//\\//
    			}
    	    }
        }
    	public static String ThongTinNhanVien1_ReferencePropertyName { get { return "ThongTinNhanVien1"; } }
    	partial void On_ThongTinNhanVien1_Changing(ref ThongTinNhanVien newValue, ref bool stopChanging);
    	partial void On_ThongTinNhanVien1_Changed(ThongTinNhanVien currentValue);
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<ThongTinNhanVien> ThongTinNhanVien1Reference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<ThongTinNhanVien>("ERPModel.FK_WebUsers_ThongTinNhanVien", "ThongTinNhanVien");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<ThongTinNhanVien>("ERPModel.FK_WebUsers_ThongTinNhanVien", "ThongTinNhanVien", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_Web_UpdateHoSoNhanVien_WebUsers", "Web_UpdateHoSoNhanVien")]
        public EntityCollection<Web_UpdateHoSoNhanVien> Web_UpdateHoSoNhanVien
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<Web_UpdateHoSoNhanVien>("ERPModel.FK_Web_UpdateHoSoNhanVien_WebUsers", "Web_UpdateHoSoNhanVien");
            }
            set
            {
                if ((value != null))
                {
    				bool stopChanging = false;
    				On_Web_UpdateHoSoNhanVien_Changing(ref value, ref stopChanging);//\\//
    				if(!stopChanging)
    				{
    					((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<Web_UpdateHoSoNhanVien>("ERPModel.FK_Web_UpdateHoSoNhanVien_WebUsers", "Web_UpdateHoSoNhanVien", value);
    					On_Web_UpdateHoSoNhanVien_Changed(this.Web_UpdateHoSoNhanVien);//\\//
    				}
    			}
            }
        }
    	public static String Web_UpdateHoSoNhanVien_EntityCollectionPropertyName { get { return "Web_UpdateHoSoNhanVien"; } }
    	partial void On_Web_UpdateHoSoNhanVien_Changing(ref EntityCollection<Web_UpdateHoSoNhanVien> newValue, ref bool stopChanging);
    	partial void On_Web_UpdateHoSoNhanVien_Changed(EntityCollection<Web_UpdateHoSoNhanVien> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_WebUsers_WebGroup", "WebGroup")]
        public WebGroup WebGroup
        {//test
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<WebGroup>("ERPModel.FK_WebUsers_WebGroup", "WebGroup").Value;
            }
            set
            {
    			bool stopChanging = false;
    			On_WebGroup_Changing(ref value, ref stopChanging);//\\//
    			if(!stopChanging)
    			{
    				((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<WebGroup>("ERPModel.FK_WebUsers_WebGroup", "WebGroup").Value = value;
    				On_WebGroup_Changed(this.WebGroup);//\\//
    			}
    	    }
        }
    	public static String WebGroup_ReferencePropertyName { get { return "WebGroup"; } }
    	partial void On_WebGroup_Changing(ref WebGroup newValue, ref bool stopChanging);
    	partial void On_WebGroup_Changed(WebGroup currentValue);
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<WebGroup> WebGroupReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<WebGroup>("ERPModel.FK_WebUsers_WebGroup", "WebGroup");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<WebGroup>("ERPModel.FK_WebUsers_WebGroup", "WebGroup", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_WebUser_BoPhan_WebUsers", "WebUser_BoPhan")]
        public EntityCollection<WebUser_BoPhan> WebUser_BoPhan
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<WebUser_BoPhan>("ERPModel.FK_WebUser_BoPhan_WebUsers", "WebUser_BoPhan");
            }
            set
            {
                if ((value != null))
                {
    				bool stopChanging = false;
    				On_WebUser_BoPhan_Changing(ref value, ref stopChanging);//\\//
    				if(!stopChanging)
    				{
    					((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<WebUser_BoPhan>("ERPModel.FK_WebUser_BoPhan_WebUsers", "WebUser_BoPhan", value);
    					On_WebUser_BoPhan_Changed(this.WebUser_BoPhan);//\\//
    				}
    			}
            }
        }
    	public static String WebUser_BoPhan_EntityCollectionPropertyName { get { return "WebUser_BoPhan"; } }
    	partial void On_WebUser_BoPhan_Changing(ref EntityCollection<WebUser_BoPhan> newValue, ref bool stopChanging);
    	partial void On_WebUser_BoPhan_Changed(EntityCollection<WebUser_BoPhan> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_CC_ChamCongNgayNghi_IDWebUsers", "CC_ChamCongNgayNghi")]
        public EntityCollection<CC_ChamCongNgayNghi> CC_ChamCongNgayNghi
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<CC_ChamCongNgayNghi>("ERPModel.FK_CC_ChamCongNgayNghi_IDWebUsers", "CC_ChamCongNgayNghi");
            }
            set
            {
                if ((value != null))
                {
    				bool stopChanging = false;
    				On_CC_ChamCongNgayNghi_Changing(ref value, ref stopChanging);//\\//
    				if(!stopChanging)
    				{
    					((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<CC_ChamCongNgayNghi>("ERPModel.FK_CC_ChamCongNgayNghi_IDWebUsers", "CC_ChamCongNgayNghi", value);
    					On_CC_ChamCongNgayNghi_Changed(this.CC_ChamCongNgayNghi);//\\//
    				}
    			}
            }
        }
    	public static String CC_ChamCongNgayNghi_EntityCollectionPropertyName { get { return "CC_ChamCongNgayNghi"; } }
    	partial void On_CC_ChamCongNgayNghi_Changing(ref EntityCollection<CC_ChamCongNgayNghi> newValue, ref bool stopChanging);
    	partial void On_CC_ChamCongNgayNghi_Changed(EntityCollection<CC_ChamCongNgayNghi> currentValue);

        #endregion

    }
}