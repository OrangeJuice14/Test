//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
//	  Code duoc tao boi SERVERERP\tai luc 09:33:22 ngay 03/10/2017
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
    [EdmEntityTypeAttribute(NamespaceName="ERPModel", Name="TinhTrang")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class TinhTrang : ERP_Core.BaseEntityObject//EntityObject
    {
    	static TinhTrang()
    	{
    
    		AfterStaticConstruction();
        }
        static partial void AfterStaticConstruction();
    
    	public TinhTrang()
    	{
    
    		this.AfterConstruction();
        }
        partial void AfterConstruction();
    
        #region Factory Method
    
        /// <summary>
        /// Create a new TinhTrang object.
        /// </summary>
        /// <param name="oid">Initial value of the Oid property.</param>
        public static TinhTrang CreateTinhTrang(System.Guid oid)
        {
            TinhTrang tinhTrang = new TinhTrang();
            tinhTrang.Oid = oid;
            return tinhTrang;
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
        public string MaQuanLy
        {
            get
            {
                return _maQuanLy;
            }
            set
            {
    			string oldValue =  _maQuanLy;
    			bool stopChanging = false;
                On_MaQuanLy_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("MaQuanLy");
    			if(!stopChanging)
    			{
    				_maQuanLy = StructuralObject.SetValidValue(value, true);
    				ReportPropertyChanged("MaQuanLy");
    				On_MaQuanLy_Changed(oldValue, _maQuanLy);//\\
    			}
            }
        }
    	public static String MaQuanLy_PropertyName { get { return "MaQuanLy"; } }
        private string _maQuanLy;
        partial void On_MaQuanLy_Changing(string currentValue, ref string newValue, ref bool stopChanging);
        partial void On_MaQuanLy_Changed(string oldValue, string currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public string TenTinhTrang
        {
            get
            {
                return _tenTinhTrang;
            }
            set
            {
    			string oldValue =  _tenTinhTrang;
    			bool stopChanging = false;
                On_TenTinhTrang_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("TenTinhTrang");
    			if(!stopChanging)
    			{
    				_tenTinhTrang = StructuralObject.SetValidValue(value, true);
    				ReportPropertyChanged("TenTinhTrang");
    				On_TenTinhTrang_Changed(oldValue, _tenTinhTrang);//\\
    			}
            }
        }
    	public static String TenTinhTrang_PropertyName { get { return "TenTinhTrang"; } }
        private string _tenTinhTrang;
        partial void On_TenTinhTrang_Changing(string currentValue, ref string newValue, ref bool stopChanging);
        partial void On_TenTinhTrang_Changed(string oldValue, string currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<bool> KhongConCongTacTaiTruong
        {
            get
            {
                return _khongConCongTacTaiTruong;
            }
            set
            {
    			Nullable<bool> oldValue =  _khongConCongTacTaiTruong;
    			bool stopChanging = false;
                On_KhongConCongTacTaiTruong_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("KhongConCongTacTaiTruong");
    			if(!stopChanging)
    			{
    				_khongConCongTacTaiTruong = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("KhongConCongTacTaiTruong");
    				On_KhongConCongTacTaiTruong_Changed(oldValue, _khongConCongTacTaiTruong);//\\
    			}
            }
        }
    	public static String KhongConCongTacTaiTruong_PropertyName { get { return "KhongConCongTacTaiTruong"; } }
        private Nullable<bool> _khongConCongTacTaiTruong;
        partial void On_KhongConCongTacTaiTruong_Changing(Nullable<bool> currentValue, ref Nullable<bool> newValue, ref bool stopChanging);
        partial void On_KhongConCongTacTaiTruong_Changed(Nullable<bool> oldValue, Nullable<bool> currentValue);
    
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
        public Nullable<bool> KhongTinhTNTT
        {
            get
            {
                return _khongTinhTNTT;
            }
            set
            {
    			Nullable<bool> oldValue =  _khongTinhTNTT;
    			bool stopChanging = false;
                On_KhongTinhTNTT_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("KhongTinhTNTT");
    			if(!stopChanging)
    			{
    				_khongTinhTNTT = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("KhongTinhTNTT");
    				On_KhongTinhTNTT_Changed(oldValue, _khongTinhTNTT);//\\
    			}
            }
        }
    	public static String KhongTinhTNTT_PropertyName { get { return "KhongTinhTNTT"; } }
        private Nullable<bool> _khongTinhTNTT;
        partial void On_KhongTinhTNTT_Changing(Nullable<bool> currentValue, ref Nullable<bool> newValue, ref bool stopChanging);
        partial void On_KhongTinhTNTT_Changed(Nullable<bool> oldValue, Nullable<bool> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<byte> LoaiTinhTrang
        {
            get
            {
                return _loaiTinhTrang;
            }
            set
            {
    			Nullable<byte> oldValue =  _loaiTinhTrang;
    			bool stopChanging = false;
                On_LoaiTinhTrang_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("LoaiTinhTrang");
    			if(!stopChanging)
    			{
    				_loaiTinhTrang = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("LoaiTinhTrang");
    				On_LoaiTinhTrang_Changed(oldValue, _loaiTinhTrang);//\\
    			}
            }
        }
    	public static String LoaiTinhTrang_PropertyName { get { return "LoaiTinhTrang"; } }
        private Nullable<byte> _loaiTinhTrang;
        partial void On_LoaiTinhTrang_Changing(Nullable<byte> currentValue, ref Nullable<byte> newValue, ref bool stopChanging);
        partial void On_LoaiTinhTrang_Changed(Nullable<byte> oldValue, Nullable<byte> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<int> PhanTramHuongLuong
        {
            get
            {
                return _phanTramHuongLuong;
            }
            set
            {
    			Nullable<int> oldValue =  _phanTramHuongLuong;
    			bool stopChanging = false;
                On_PhanTramHuongLuong_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("PhanTramHuongLuong");
    			if(!stopChanging)
    			{
    				_phanTramHuongLuong = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("PhanTramHuongLuong");
    				On_PhanTramHuongLuong_Changed(oldValue, _phanTramHuongLuong);//\\
    			}
            }
        }
    	public static String PhanTramHuongLuong_PropertyName { get { return "PhanTramHuongLuong"; } }
        private Nullable<int> _phanTramHuongLuong;
        partial void On_PhanTramHuongLuong_Changing(Nullable<int> currentValue, ref Nullable<int> newValue, ref bool stopChanging);
        partial void On_PhanTramHuongLuong_Changed(Nullable<int> oldValue, Nullable<int> currentValue);

        #endregion

        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_NhanVien_TinhTrang", "NhanVien")]
        public EntityCollection<NhanVien> NhanViens
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<NhanVien>("ERPModel.FK_NhanVien_TinhTrang", "NhanVien");
            }
            set
            {
                if ((value != null))
                {
    				bool stopChanging = false;
    				On_NhanViens_Changing(ref value, ref stopChanging);//\\//
    				if(!stopChanging)
    				{
    					((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<NhanVien>("ERPModel.FK_NhanVien_TinhTrang", "NhanVien", value);
    					On_NhanViens_Changed(this.NhanViens);//\\//
    				}
    			}
            }
        }
    	public static String NhanViens_EntityCollectionPropertyName { get { return "NhanViens"; } }
    	partial void On_NhanViens_Changing(ref EntityCollection<NhanVien> newValue, ref bool stopChanging);
    	partial void On_NhanViens_Changed(EntityCollection<NhanVien> currentValue);

        #endregion

    }
}