//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
//	  Code duoc tao boi DESKTOP-PKPRC2J\NGOCBAO luc 04:17:16 ngay 30/08/2016
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
    [EdmEntityTypeAttribute(NamespaceName="ERPModel", Name="CC_ChamCongNgayNghi")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class CC_ChamCongNgayNghi : ERP_Core.BaseEntityObject//EntityObject
    {
    	static CC_ChamCongNgayNghi()
    	{
    
    		AfterStaticConstruction();
        }
        static partial void AfterStaticConstruction();
    
    	public CC_ChamCongNgayNghi()
    	{
    
    		this.AfterConstruction();
        }
        partial void AfterConstruction();
    
        #region Factory Method
    
        /// <summary>
        /// Create a new CC_ChamCongNgayNghi object.
        /// </summary>
        /// <param name="oid">Initial value of the Oid property.</param>
        public static CC_ChamCongNgayNghi CreateCC_ChamCongNgayNghi(System.Guid oid)
        {
            CC_ChamCongNgayNghi cC_ChamCongNgayNghi = new CC_ChamCongNgayNghi();
            cC_ChamCongNgayNghi.Oid = oid;
            return cC_ChamCongNgayNghi;
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
        public Nullable<System.Guid> IDBoPhan
        {
            get
            {
                return _iDBoPhan;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _iDBoPhan;
    			bool stopChanging = false;
                On_IDBoPhan_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("IDBoPhan");
    			if(!stopChanging)
    			{
    				_iDBoPhan = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("IDBoPhan");
    				On_IDBoPhan_Changed(oldValue, _iDBoPhan);//\\
    			}
            }
        }
    	public static String IDBoPhan_PropertyName { get { return "IDBoPhan"; } }
        private Nullable<System.Guid> _iDBoPhan;
        partial void On_IDBoPhan_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_IDBoPhan_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> IDNhanVien
        {
            get
            {
                return _iDNhanVien;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _iDNhanVien;
    			bool stopChanging = false;
                On_IDNhanVien_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("IDNhanVien");
    			if(!stopChanging)
    			{
    				_iDNhanVien = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("IDNhanVien");
    				On_IDNhanVien_Changed(oldValue, _iDNhanVien);//\\
    			}
            }
        }
    	public static String IDNhanVien_PropertyName { get { return "IDNhanVien"; } }
        private Nullable<System.Guid> _iDNhanVien;
        partial void On_IDNhanVien_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_IDNhanVien_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> IDHinhThucNghi
        {
            get
            {
                return _iDHinhThucNghi;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _iDHinhThucNghi;
    			bool stopChanging = false;
                On_IDHinhThucNghi_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("IDHinhThucNghi");
    			if(!stopChanging)
    			{
    				_iDHinhThucNghi = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("IDHinhThucNghi");
    				On_IDHinhThucNghi_Changed(oldValue, _iDHinhThucNghi);//\\
    			}
            }
        }
    	public static String IDHinhThucNghi_PropertyName { get { return "IDHinhThucNghi"; } }
        private Nullable<System.Guid> _iDHinhThucNghi;
        partial void On_IDHinhThucNghi_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_IDHinhThucNghi_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.DateTime> TuNgay
        {
            get
            {
                return _tuNgay;
            }
            set
            {
    			Nullable<System.DateTime> oldValue =  _tuNgay;
    			bool stopChanging = false;
                On_TuNgay_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("TuNgay");
    			if(!stopChanging)
    			{
    				_tuNgay = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("TuNgay");
    				On_TuNgay_Changed(oldValue, _tuNgay);//\\
    			}
            }
        }
    	public static String TuNgay_PropertyName { get { return "TuNgay"; } }
        private Nullable<System.DateTime> _tuNgay;
        partial void On_TuNgay_Changing(Nullable<System.DateTime> currentValue, ref Nullable<System.DateTime> newValue, ref bool stopChanging);
        partial void On_TuNgay_Changed(Nullable<System.DateTime> oldValue, Nullable<System.DateTime> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.DateTime> DenNgay
        {
            get
            {
                return _denNgay;
            }
            set
            {
    			Nullable<System.DateTime> oldValue =  _denNgay;
    			bool stopChanging = false;
                On_DenNgay_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("DenNgay");
    			if(!stopChanging)
    			{
    				_denNgay = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("DenNgay");
    				On_DenNgay_Changed(oldValue, _denNgay);//\\
    			}
            }
        }
    	public static String DenNgay_PropertyName { get { return "DenNgay"; } }
        private Nullable<System.DateTime> _denNgay;
        partial void On_DenNgay_Changing(Nullable<System.DateTime> currentValue, ref Nullable<System.DateTime> newValue, ref bool stopChanging);
        partial void On_DenNgay_Changed(Nullable<System.DateTime> oldValue, Nullable<System.DateTime> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.DateTime> NgayTao
        {
            get
            {
                return _ngayTao;
            }
            set
            {
    			Nullable<System.DateTime> oldValue =  _ngayTao;
    			bool stopChanging = false;
                On_NgayTao_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("NgayTao");
    			if(!stopChanging)
    			{
    				_ngayTao = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("NgayTao");
    				On_NgayTao_Changed(oldValue, _ngayTao);//\\
    			}
            }
        }
    	public static String NgayTao_PropertyName { get { return "NgayTao"; } }
        private Nullable<System.DateTime> _ngayTao;
        partial void On_NgayTao_Changing(Nullable<System.DateTime> currentValue, ref Nullable<System.DateTime> newValue, ref bool stopChanging);
        partial void On_NgayTao_Changed(Nullable<System.DateTime> oldValue, Nullable<System.DateTime> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public string DienGiai
        {
            get
            {
                return _dienGiai;
            }
            set
            {
    			string oldValue =  _dienGiai;
    			bool stopChanging = false;
                On_DienGiai_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("DienGiai");
    			if(!stopChanging)
    			{
    				_dienGiai = StructuralObject.SetValidValue(value, true);
    				ReportPropertyChanged("DienGiai");
    				On_DienGiai_Changed(oldValue, _dienGiai);//\\
    			}
            }
        }
    	public static String DienGiai_PropertyName { get { return "DienGiai"; } }
        private string _dienGiai;
        partial void On_DienGiai_Changing(string currentValue, ref string newValue, ref bool stopChanging);
        partial void On_DienGiai_Changed(string oldValue, string currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> IDWebUser
        {
            get
            {
                return _iDWebUser;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _iDWebUser;
    			bool stopChanging = false;
                On_IDWebUser_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("IDWebUser");
    			if(!stopChanging)
    			{
    				_iDWebUser = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("IDWebUser");
    				On_IDWebUser_Changed(oldValue, _iDWebUser);//\\
    			}
            }
        }
    	public static String IDWebUser_PropertyName { get { return "IDWebUser"; } }
        private Nullable<System.Guid> _iDWebUser;
        partial void On_IDWebUser_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_IDWebUser_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> BangChamCongNgayNghi
        {
            get
            {
                return _bangChamCongNgayNghi;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _bangChamCongNgayNghi;
    			bool stopChanging = false;
                On_BangChamCongNgayNghi_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("BangChamCongNgayNghi");
    			if(!stopChanging)
    			{
    				_bangChamCongNgayNghi = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("BangChamCongNgayNghi");
    				On_BangChamCongNgayNghi_Changed(oldValue, _bangChamCongNgayNghi);//\\
    			}
            }
        }
    	public static String BangChamCongNgayNghi_PropertyName { get { return "BangChamCongNgayNghi"; } }
        private Nullable<System.Guid> _bangChamCongNgayNghi;
        partial void On_BangChamCongNgayNghi_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_BangChamCongNgayNghi_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<int> SoNgay
        {
            get
            {
                return _soNgay;
            }
            set
            {
    			Nullable<int> oldValue =  _soNgay;
    			bool stopChanging = false;
                On_SoNgay_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("SoNgay");
    			if(!stopChanging)
    			{
    				_soNgay = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("SoNgay");
    				On_SoNgay_Changed(oldValue, _soNgay);//\\
    			}
            }
        }
    	public static String SoNgay_PropertyName { get { return "SoNgay"; } }
        private Nullable<int> _soNgay;
        partial void On_SoNgay_Changing(Nullable<int> currentValue, ref Nullable<int> newValue, ref bool stopChanging);
        partial void On_SoNgay_Changed(Nullable<int> oldValue, Nullable<int> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<byte> XepLoaiDanhGia
        {
            get
            {
                return _xepLoaiDanhGia;
            }
            set
            {
    			Nullable<byte> oldValue =  _xepLoaiDanhGia;
    			bool stopChanging = false;
                On_XepLoaiDanhGia_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("XepLoaiDanhGia");
    			if(!stopChanging)
    			{
    				_xepLoaiDanhGia = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("XepLoaiDanhGia");
    				On_XepLoaiDanhGia_Changed(oldValue, _xepLoaiDanhGia);//\\
    			}
            }
        }
    	public static String XepLoaiDanhGia_PropertyName { get { return "XepLoaiDanhGia"; } }
        private Nullable<byte> _xepLoaiDanhGia;
        partial void On_XepLoaiDanhGia_Changing(Nullable<byte> currentValue, ref Nullable<byte> newValue, ref bool stopChanging);
        partial void On_XepLoaiDanhGia_Changed(Nullable<byte> oldValue, Nullable<byte> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> IDWebUsers
        {
            get
            {
                return _iDWebUsers;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _iDWebUsers;
    			bool stopChanging = false;
                On_IDWebUsers_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("IDWebUsers");
    			if(!stopChanging)
    			{
    				_iDWebUsers = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("IDWebUsers");
    				On_IDWebUsers_Changed(oldValue, _iDWebUsers);//\\
    			}
            }
        }
    	public static String IDWebUsers_PropertyName { get { return "IDWebUsers"; } }
        private Nullable<System.Guid> _iDWebUsers;
        partial void On_IDWebUsers_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_IDWebUsers_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
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
        public Nullable<System.DateTime> TuNgayOld
        {
            get
            {
                return _tuNgayOld;
            }
            set
            {
    			Nullable<System.DateTime> oldValue =  _tuNgayOld;
    			bool stopChanging = false;
                On_TuNgayOld_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("TuNgayOld");
    			if(!stopChanging)
    			{
    				_tuNgayOld = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("TuNgayOld");
    				On_TuNgayOld_Changed(oldValue, _tuNgayOld);//\\
    			}
            }
        }
    	public static String TuNgayOld_PropertyName { get { return "TuNgayOld"; } }
        private Nullable<System.DateTime> _tuNgayOld;
        partial void On_TuNgayOld_Changing(Nullable<System.DateTime> currentValue, ref Nullable<System.DateTime> newValue, ref bool stopChanging);
        partial void On_TuNgayOld_Changed(Nullable<System.DateTime> oldValue, Nullable<System.DateTime> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.DateTime> DenNgayOld
        {
            get
            {
                return _denNgayOld;
            }
            set
            {
    			Nullable<System.DateTime> oldValue =  _denNgayOld;
    			bool stopChanging = false;
                On_DenNgayOld_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("DenNgayOld");
    			if(!stopChanging)
    			{
    				_denNgayOld = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("DenNgayOld");
    				On_DenNgayOld_Changed(oldValue, _denNgayOld);//\\
    			}
            }
        }
    	public static String DenNgayOld_PropertyName { get { return "DenNgayOld"; } }
        private Nullable<System.DateTime> _denNgayOld;
        partial void On_DenNgayOld_Changing(Nullable<System.DateTime> currentValue, ref Nullable<System.DateTime> newValue, ref bool stopChanging);
        partial void On_DenNgayOld_Changed(Nullable<System.DateTime> oldValue, Nullable<System.DateTime> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<int> TrangThai
        {
            get
            {
                return _trangThai;
            }
            set
            {
    			Nullable<int> oldValue =  _trangThai;
    			bool stopChanging = false;
                On_TrangThai_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("TrangThai");
    			if(!stopChanging)
    			{
    				_trangThai = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("TrangThai");
    				On_TrangThai_Changed(oldValue, _trangThai);//\\
    			}
            }
        }
    	public static String TrangThai_PropertyName { get { return "TrangThai"; } }
        private Nullable<int> _trangThai;
        partial void On_TrangThai_Changing(Nullable<int> currentValue, ref Nullable<int> newValue, ref bool stopChanging);
        partial void On_TrangThai_Changed(Nullable<int> oldValue, Nullable<int> currentValue);

        #endregion

        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_CC_ChamCongNgayNghi_HinhThucNghi", "HinhThucNghi")]
        public HinhThucNghi HinhThucNghi
        {//test
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<HinhThucNghi>("ERPModel.FK_CC_ChamCongNgayNghi_HinhThucNghi", "HinhThucNghi").Value;
            }
            set
            {
    			bool stopChanging = false;
    			On_HinhThucNghi_Changing(ref value, ref stopChanging);//\\//
    			if(!stopChanging)
    			{
    				((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<HinhThucNghi>("ERPModel.FK_CC_ChamCongNgayNghi_HinhThucNghi", "HinhThucNghi").Value = value;
    				On_HinhThucNghi_Changed(this.HinhThucNghi);//\\//
    			}
    	    }
        }
    	public static String HinhThucNghi_ReferencePropertyName { get { return "HinhThucNghi"; } }
    	partial void On_HinhThucNghi_Changing(ref HinhThucNghi newValue, ref bool stopChanging);
    	partial void On_HinhThucNghi_Changed(HinhThucNghi currentValue);
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<HinhThucNghi> HinhThucNghiReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<HinhThucNghi>("ERPModel.FK_CC_ChamCongNgayNghi_HinhThucNghi", "HinhThucNghi");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<HinhThucNghi>("ERPModel.FK_CC_ChamCongNgayNghi_HinhThucNghi", "HinhThucNghi", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_CC_ChamCongNgayNghi_BoPhan", "BoPhan")]
        public BoPhan BoPhan
        {//test
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<BoPhan>("ERPModel.FK_CC_ChamCongNgayNghi_BoPhan", "BoPhan").Value;
            }
            set
            {
    			bool stopChanging = false;
    			On_BoPhan_Changing(ref value, ref stopChanging);//\\//
    			if(!stopChanging)
    			{
    				((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<BoPhan>("ERPModel.FK_CC_ChamCongNgayNghi_BoPhan", "BoPhan").Value = value;
    				On_BoPhan_Changed(this.BoPhan);//\\//
    			}
    	    }
        }
    	public static String BoPhan_ReferencePropertyName { get { return "BoPhan"; } }
    	partial void On_BoPhan_Changing(ref BoPhan newValue, ref bool stopChanging);
    	partial void On_BoPhan_Changed(BoPhan currentValue);
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<BoPhan> BoPhanReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<BoPhan>("ERPModel.FK_CC_ChamCongNgayNghi_BoPhan", "BoPhan");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<BoPhan>("ERPModel.FK_CC_ChamCongNgayNghi_BoPhan", "BoPhan", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_CC_ChamCongNgayNghi_IDNhanVien", "ThongTinNhanVien")]
        public ThongTinNhanVien ThongTinNhanVien
        {//test
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<ThongTinNhanVien>("ERPModel.FK_CC_ChamCongNgayNghi_IDNhanVien", "ThongTinNhanVien").Value;
            }
            set
            {
    			bool stopChanging = false;
    			On_ThongTinNhanVien_Changing(ref value, ref stopChanging);//\\//
    			if(!stopChanging)
    			{
    				((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<ThongTinNhanVien>("ERPModel.FK_CC_ChamCongNgayNghi_IDNhanVien", "ThongTinNhanVien").Value = value;
    				On_ThongTinNhanVien_Changed(this.ThongTinNhanVien);//\\//
    			}
    	    }
        }
    	public static String ThongTinNhanVien_ReferencePropertyName { get { return "ThongTinNhanVien"; } }
    	partial void On_ThongTinNhanVien_Changing(ref ThongTinNhanVien newValue, ref bool stopChanging);
    	partial void On_ThongTinNhanVien_Changed(ThongTinNhanVien currentValue);
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<ThongTinNhanVien> ThongTinNhanVienReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<ThongTinNhanVien>("ERPModel.FK_CC_ChamCongNgayNghi_IDNhanVien", "ThongTinNhanVien");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<ThongTinNhanVien>("ERPModel.FK_CC_ChamCongNgayNghi_IDNhanVien", "ThongTinNhanVien", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_CC_ChamCongNgayNghi_IDWebUsers", "WebUser")]
        public WebUser WebUser
        {//test
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<WebUser>("ERPModel.FK_CC_ChamCongNgayNghi_IDWebUsers", "WebUser").Value;
            }
            set
            {
    			bool stopChanging = false;
    			On_WebUser_Changing(ref value, ref stopChanging);//\\//
    			if(!stopChanging)
    			{
    				((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<WebUser>("ERPModel.FK_CC_ChamCongNgayNghi_IDWebUsers", "WebUser").Value = value;
    				On_WebUser_Changed(this.WebUser);//\\//
    			}
    	    }
        }
    	public static String WebUser_ReferencePropertyName { get { return "WebUser"; } }
    	partial void On_WebUser_Changing(ref WebUser newValue, ref bool stopChanging);
    	partial void On_WebUser_Changed(WebUser currentValue);
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<WebUser> WebUserReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<WebUser>("ERPModel.FK_CC_ChamCongNgayNghi_IDWebUsers", "WebUser");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<WebUser>("ERPModel.FK_CC_ChamCongNgayNghi_IDWebUsers", "WebUser", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_CC_ChamCongNgayNghi_WebUsers", "WebUser")]
        public WebUser WebUser1
        {//test
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<WebUser>("ERPModel.FK_CC_ChamCongNgayNghi_WebUsers", "WebUser").Value;
            }
            set
            {
    			bool stopChanging = false;
    			On_WebUser1_Changing(ref value, ref stopChanging);//\\//
    			if(!stopChanging)
    			{
    				((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<WebUser>("ERPModel.FK_CC_ChamCongNgayNghi_WebUsers", "WebUser").Value = value;
    				On_WebUser1_Changed(this.WebUser1);//\\//
    			}
    	    }
        }
    	public static String WebUser1_ReferencePropertyName { get { return "WebUser1"; } }
    	partial void On_WebUser1_Changing(ref WebUser newValue, ref bool stopChanging);
    	partial void On_WebUser1_Changed(WebUser currentValue);
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<WebUser> WebUser1Reference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<WebUser>("ERPModel.FK_CC_ChamCongNgayNghi_WebUsers", "WebUser");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<WebUser>("ERPModel.FK_CC_ChamCongNgayNghi_WebUsers", "WebUser", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_CC_ChamCongNgayNghi_HoSo", "HoSo")]
        public HoSo HoSo
        {//test
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<HoSo>("ERPModel.FK_CC_ChamCongNgayNghi_HoSo", "HoSo").Value;
            }
            set
            {
    			bool stopChanging = false;
    			On_HoSo_Changing(ref value, ref stopChanging);//\\//
    			if(!stopChanging)
    			{
    				((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<HoSo>("ERPModel.FK_CC_ChamCongNgayNghi_HoSo", "HoSo").Value = value;
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
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<HoSo>("ERPModel.FK_CC_ChamCongNgayNghi_HoSo", "HoSo");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<HoSo>("ERPModel.FK_CC_ChamCongNgayNghi_HoSo", "HoSo", value);
                }
            }
        }

        #endregion

    }
}