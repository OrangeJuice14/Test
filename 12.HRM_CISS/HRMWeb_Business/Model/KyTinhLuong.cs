//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
//	  Code duoc tao boi DESKTOP-PKPRC2J\NGOCBAO luc 12:14:05 ngay 06/04/2019
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
    [EdmEntityTypeAttribute(NamespaceName="ERPModel", Name="KyTinhLuong")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class KyTinhLuong : ERP_Core.BaseEntityObject//EntityObject
    {
    	static KyTinhLuong()
    	{
    
    		AfterStaticConstruction();
        }
        static partial void AfterStaticConstruction();
    
    	public KyTinhLuong()
    	{
    
    		this.AfterConstruction();
        }
        partial void AfterConstruction();
    
        #region Factory Method
    
        /// <summary>
        /// Create a new KyTinhLuong object.
        /// </summary>
        /// <param name="oid">Initial value of the Oid property.</param>
        public static KyTinhLuong CreateKyTinhLuong(System.Guid oid)
        {
            KyTinhLuong kyTinhLuong = new KyTinhLuong();
            kyTinhLuong.Oid = oid;
            return kyTinhLuong;
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
        public Nullable<System.Guid> CongTy
        {
            get
            {
                return _congTy;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _congTy;
    			bool stopChanging = false;
                On_CongTy_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("CongTy");
    			if(!stopChanging)
    			{
    				_congTy = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("CongTy");
    				On_CongTy_Changed(oldValue, _congTy);//\\
    			}
            }
        }
    	public static String CongTy_PropertyName { get { return "CongTy"; } }
        private Nullable<System.Guid> _congTy;
        partial void On_CongTy_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_CongTy_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<int> Thang
        {
            get
            {
                return _thang;
            }
            set
            {
    			Nullable<int> oldValue =  _thang;
    			bool stopChanging = false;
                On_Thang_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("Thang");
    			if(!stopChanging)
    			{
    				_thang = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("Thang");
    				On_Thang_Changed(oldValue, _thang);//\\
    			}
            }
        }
    	public static String Thang_PropertyName { get { return "Thang"; } }
        private Nullable<int> _thang;
        partial void On_Thang_Changing(Nullable<int> currentValue, ref Nullable<int> newValue, ref bool stopChanging);
        partial void On_Thang_Changed(Nullable<int> oldValue, Nullable<int> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<int> Nam
        {
            get
            {
                return _nam;
            }
            set
            {
    			Nullable<int> oldValue =  _nam;
    			bool stopChanging = false;
                On_Nam_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("Nam");
    			if(!stopChanging)
    			{
    				_nam = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("Nam");
    				On_Nam_Changed(oldValue, _nam);//\\
    			}
            }
        }
    	public static String Nam_PropertyName { get { return "Nam"; } }
        private Nullable<int> _nam;
        partial void On_Nam_Changing(Nullable<int> currentValue, ref Nullable<int> newValue, ref bool stopChanging);
        partial void On_Nam_Changed(Nullable<int> oldValue, Nullable<int> currentValue);
    
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
        public Nullable<decimal> SoNgay
        {
            get
            {
                return _soNgay;
            }
            set
            {
    			Nullable<decimal> oldValue =  _soNgay;
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
        private Nullable<decimal> _soNgay;
        partial void On_SoNgay_Changing(Nullable<decimal> currentValue, ref Nullable<decimal> newValue, ref bool stopChanging);
        partial void On_SoNgay_Changed(Nullable<decimal> oldValue, Nullable<decimal> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<bool> KhoaSo
        {
            get
            {
                return _khoaSo;
            }
            set
            {
    			Nullable<bool> oldValue =  _khoaSo;
    			bool stopChanging = false;
                On_KhoaSo_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("KhoaSo");
    			if(!stopChanging)
    			{
    				_khoaSo = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("KhoaSo");
    				On_KhoaSo_Changed(oldValue, _khoaSo);//\\
    			}
            }
        }
    	public static String KhoaSo_PropertyName { get { return "KhoaSo"; } }
        private Nullable<bool> _khoaSo;
        partial void On_KhoaSo_Changing(Nullable<bool> currentValue, ref Nullable<bool> newValue, ref bool stopChanging);
        partial void On_KhoaSo_Changed(Nullable<bool> oldValue, Nullable<bool> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> ThongTinChung
        {
            get
            {
                return _thongTinChung;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _thongTinChung;
    			bool stopChanging = false;
                On_ThongTinChung_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("ThongTinChung");
    			if(!stopChanging)
    			{
    				_thongTinChung = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("ThongTinChung");
    				On_ThongTinChung_Changed(oldValue, _thongTinChung);//\\
    			}
            }
        }
    	public static String ThongTinChung_PropertyName { get { return "ThongTinChung"; } }
        private Nullable<System.Guid> _thongTinChung;
        partial void On_ThongTinChung_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_ThongTinChung_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> MocTinhThueTNCN
        {
            get
            {
                return _mocTinhThueTNCN;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _mocTinhThueTNCN;
    			bool stopChanging = false;
                On_MocTinhThueTNCN_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("MocTinhThueTNCN");
    			if(!stopChanging)
    			{
    				_mocTinhThueTNCN = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("MocTinhThueTNCN");
    				On_MocTinhThueTNCN_Changed(oldValue, _mocTinhThueTNCN);//\\
    			}
            }
        }
    	public static String MocTinhThueTNCN_PropertyName { get { return "MocTinhThueTNCN"; } }
        private Nullable<System.Guid> _mocTinhThueTNCN;
        partial void On_MocTinhThueTNCN_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_MocTinhThueTNCN_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> QuanLyChamCong
        {
            get
            {
                return _quanLyChamCong;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _quanLyChamCong;
    			bool stopChanging = false;
                On_QuanLyChamCong_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("QuanLyChamCong");
    			if(!stopChanging)
    			{
    				_quanLyChamCong = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("QuanLyChamCong");
    				On_QuanLyChamCong_Changed(oldValue, _quanLyChamCong);//\\
    			}
            }
        }
    	public static String QuanLyChamCong_PropertyName { get { return "QuanLyChamCong"; } }
        private Nullable<System.Guid> _quanLyChamCong;
        partial void On_QuanLyChamCong_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_QuanLyChamCong_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
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
        public Nullable<System.DateTime> MocDongBaoHiem
        {
            get
            {
                return _mocDongBaoHiem;
            }
            set
            {
    			Nullable<System.DateTime> oldValue =  _mocDongBaoHiem;
    			bool stopChanging = false;
                On_MocDongBaoHiem_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("MocDongBaoHiem");
    			if(!stopChanging)
    			{
    				_mocDongBaoHiem = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("MocDongBaoHiem");
    				On_MocDongBaoHiem_Changed(oldValue, _mocDongBaoHiem);//\\
    			}
            }
        }
    	public static String MocDongBaoHiem_PropertyName { get { return "MocDongBaoHiem"; } }
        private Nullable<System.DateTime> _mocDongBaoHiem;
        partial void On_MocDongBaoHiem_Changing(Nullable<System.DateTime> currentValue, ref Nullable<System.DateTime> newValue, ref bool stopChanging);
        partial void On_MocDongBaoHiem_Changed(Nullable<System.DateTime> oldValue, Nullable<System.DateTime> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> QuanLyCongNgoaiGio
        {
            get
            {
                return _quanLyCongNgoaiGio;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _quanLyCongNgoaiGio;
    			bool stopChanging = false;
                On_QuanLyCongNgoaiGio_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("QuanLyCongNgoaiGio");
    			if(!stopChanging)
    			{
    				_quanLyCongNgoaiGio = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("QuanLyCongNgoaiGio");
    				On_QuanLyCongNgoaiGio_Changed(oldValue, _quanLyCongNgoaiGio);//\\
    			}
            }
        }
    	public static String QuanLyCongNgoaiGio_PropertyName { get { return "QuanLyCongNgoaiGio"; } }
        private Nullable<System.Guid> _quanLyCongNgoaiGio;
        partial void On_QuanLyCongNgoaiGio_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_QuanLyCongNgoaiGio_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> QuanLyCongKhac
        {
            get
            {
                return _quanLyCongKhac;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _quanLyCongKhac;
    			bool stopChanging = false;
                On_QuanLyCongKhac_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("QuanLyCongKhac");
    			if(!stopChanging)
    			{
    				_quanLyCongKhac = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("QuanLyCongKhac");
    				On_QuanLyCongKhac_Changed(oldValue, _quanLyCongKhac);//\\
    			}
            }
        }
    	public static String QuanLyCongKhac_PropertyName { get { return "QuanLyCongKhac"; } }
        private Nullable<System.Guid> _quanLyCongKhac;
        partial void On_QuanLyCongKhac_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_QuanLyCongKhac_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);

        #endregion

        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_KyTinhLuong_QuanLyChamCong", "CC_QuanLyChamCong")]
        public CC_QuanLyChamCong CC_QuanLyChamCong
        {//test
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<CC_QuanLyChamCong>("ERPModel.FK_KyTinhLuong_QuanLyChamCong", "CC_QuanLyChamCong").Value;
            }
            set
            {
    			bool stopChanging = false;
    			On_CC_QuanLyChamCong_Changing(ref value, ref stopChanging);//\\//
    			if(!stopChanging)
    			{
    				((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<CC_QuanLyChamCong>("ERPModel.FK_KyTinhLuong_QuanLyChamCong", "CC_QuanLyChamCong").Value = value;
    				On_CC_QuanLyChamCong_Changed(this.CC_QuanLyChamCong);//\\//
    			}
    	    }
        }
    	public static String CC_QuanLyChamCong_ReferencePropertyName { get { return "CC_QuanLyChamCong"; } }
    	partial void On_CC_QuanLyChamCong_Changing(ref CC_QuanLyChamCong newValue, ref bool stopChanging);
    	partial void On_CC_QuanLyChamCong_Changed(CC_QuanLyChamCong currentValue);
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<CC_QuanLyChamCong> CC_QuanLyChamCongReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<CC_QuanLyChamCong>("ERPModel.FK_KyTinhLuong_QuanLyChamCong", "CC_QuanLyChamCong");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<CC_QuanLyChamCong>("ERPModel.FK_KyTinhLuong_QuanLyChamCong", "CC_QuanLyChamCong", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_KyTinhLuong_QuanLyCongKhac", "CC_KyChamCong")]
        public CC_KyChamCong CC_KyChamCong
        {//test
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<CC_KyChamCong>("ERPModel.FK_KyTinhLuong_QuanLyCongKhac", "CC_KyChamCong").Value;
            }
            set
            {
    			bool stopChanging = false;
    			On_CC_KyChamCong_Changing(ref value, ref stopChanging);//\\//
    			if(!stopChanging)
    			{
    				((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<CC_KyChamCong>("ERPModel.FK_KyTinhLuong_QuanLyCongKhac", "CC_KyChamCong").Value = value;
    				On_CC_KyChamCong_Changed(this.CC_KyChamCong);//\\//
    			}
    	    }
        }
    	public static String CC_KyChamCong_ReferencePropertyName { get { return "CC_KyChamCong"; } }
    	partial void On_CC_KyChamCong_Changing(ref CC_KyChamCong newValue, ref bool stopChanging);
    	partial void On_CC_KyChamCong_Changed(CC_KyChamCong currentValue);
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<CC_KyChamCong> CC_KyChamCongReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<CC_KyChamCong>("ERPModel.FK_KyTinhLuong_QuanLyCongKhac", "CC_KyChamCong");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<CC_KyChamCong>("ERPModel.FK_KyTinhLuong_QuanLyCongKhac", "CC_KyChamCong", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_KyTinhLuong_QuanLyCongNgoaiGio", "CC_QuanLyCongNgoaiGio")]
        public CC_QuanLyCongNgoaiGio CC_QuanLyCongNgoaiGio
        {//test
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<CC_QuanLyCongNgoaiGio>("ERPModel.FK_KyTinhLuong_QuanLyCongNgoaiGio", "CC_QuanLyCongNgoaiGio").Value;
            }
            set
            {
    			bool stopChanging = false;
    			On_CC_QuanLyCongNgoaiGio_Changing(ref value, ref stopChanging);//\\//
    			if(!stopChanging)
    			{
    				((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<CC_QuanLyCongNgoaiGio>("ERPModel.FK_KyTinhLuong_QuanLyCongNgoaiGio", "CC_QuanLyCongNgoaiGio").Value = value;
    				On_CC_QuanLyCongNgoaiGio_Changed(this.CC_QuanLyCongNgoaiGio);//\\//
    			}
    	    }
        }
    	public static String CC_QuanLyCongNgoaiGio_ReferencePropertyName { get { return "CC_QuanLyCongNgoaiGio"; } }
    	partial void On_CC_QuanLyCongNgoaiGio_Changing(ref CC_QuanLyCongNgoaiGio newValue, ref bool stopChanging);
    	partial void On_CC_QuanLyCongNgoaiGio_Changed(CC_QuanLyCongNgoaiGio currentValue);
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<CC_QuanLyCongNgoaiGio> CC_QuanLyCongNgoaiGioReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<CC_QuanLyCongNgoaiGio>("ERPModel.FK_KyTinhLuong_QuanLyCongNgoaiGio", "CC_QuanLyCongNgoaiGio");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<CC_QuanLyCongNgoaiGio>("ERPModel.FK_KyTinhLuong_QuanLyCongNgoaiGio", "CC_QuanLyCongNgoaiGio", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_KyTinhLuong_CongTy", "CongTy")]
        public CongTy CongTy1
        {//test
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<CongTy>("ERPModel.FK_KyTinhLuong_CongTy", "CongTy").Value;
            }
            set
            {
    			bool stopChanging = false;
    			On_CongTy1_Changing(ref value, ref stopChanging);//\\//
    			if(!stopChanging)
    			{
    				((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<CongTy>("ERPModel.FK_KyTinhLuong_CongTy", "CongTy").Value = value;
    				On_CongTy1_Changed(this.CongTy1);//\\//
    			}
    	    }
        }
    	public static String CongTy1_ReferencePropertyName { get { return "CongTy1"; } }
    	partial void On_CongTy1_Changing(ref CongTy newValue, ref bool stopChanging);
    	partial void On_CongTy1_Changed(CongTy currentValue);
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<CongTy> CongTy1Reference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<CongTy>("ERPModel.FK_KyTinhLuong_CongTy", "CongTy");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<CongTy>("ERPModel.FK_KyTinhLuong_CongTy", "CongTy", value);
                }
            }
        }

        #endregion

    }
}