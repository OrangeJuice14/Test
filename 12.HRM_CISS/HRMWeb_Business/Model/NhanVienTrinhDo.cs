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
    [EdmEntityTypeAttribute(NamespaceName="ERPModel", Name="NhanVienTrinhDo")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class NhanVienTrinhDo : ERP_Core.BaseEntityObject//EntityObject
    {
    	static NhanVienTrinhDo()
    	{
    
    		AfterStaticConstruction();
        }
        static partial void AfterStaticConstruction();
    
    	public NhanVienTrinhDo()
    	{
    
    		this.AfterConstruction();
        }
        partial void AfterConstruction();
    
        #region Factory Method
    
        /// <summary>
        /// Create a new NhanVienTrinhDo object.
        /// </summary>
        /// <param name="oid">Initial value of the Oid property.</param>
        public static NhanVienTrinhDo CreateNhanVienTrinhDo(System.Guid oid)
        {
            NhanVienTrinhDo nhanVienTrinhDo = new NhanVienTrinhDo();
            nhanVienTrinhDo.Oid = oid;
            return nhanVienTrinhDo;
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
        public Nullable<System.Guid> TrinhDoVanHoa
        {
            get
            {
                return _trinhDoVanHoa;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _trinhDoVanHoa;
    			bool stopChanging = false;
                On_TrinhDoVanHoa_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("TrinhDoVanHoa");
    			if(!stopChanging)
    			{
    				_trinhDoVanHoa = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("TrinhDoVanHoa");
    				On_TrinhDoVanHoa_Changed(oldValue, _trinhDoVanHoa);//\\
    			}
            }
        }
    	public static String TrinhDoVanHoa_PropertyName { get { return "TrinhDoVanHoa"; } }
        private Nullable<System.Guid> _trinhDoVanHoa;
        partial void On_TrinhDoVanHoa_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_TrinhDoVanHoa_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> TrinhDoChuyenMon
        {
            get
            {
                return _trinhDoChuyenMon;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _trinhDoChuyenMon;
    			bool stopChanging = false;
                On_TrinhDoChuyenMon_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("TrinhDoChuyenMon");
    			if(!stopChanging)
    			{
    				_trinhDoChuyenMon = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("TrinhDoChuyenMon");
    				On_TrinhDoChuyenMon_Changed(oldValue, _trinhDoChuyenMon);//\\
    			}
            }
        }
    	public static String TrinhDoChuyenMon_PropertyName { get { return "TrinhDoChuyenMon"; } }
        private Nullable<System.Guid> _trinhDoChuyenMon;
        partial void On_TrinhDoChuyenMon_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_TrinhDoChuyenMon_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> ChuyenNganhDaoTao
        {
            get
            {
                return _chuyenNganhDaoTao;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _chuyenNganhDaoTao;
    			bool stopChanging = false;
                On_ChuyenNganhDaoTao_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("ChuyenNganhDaoTao");
    			if(!stopChanging)
    			{
    				_chuyenNganhDaoTao = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("ChuyenNganhDaoTao");
    				On_ChuyenNganhDaoTao_Changed(oldValue, _chuyenNganhDaoTao);//\\
    			}
            }
        }
    	public static String ChuyenNganhDaoTao_PropertyName { get { return "ChuyenNganhDaoTao"; } }
        private Nullable<System.Guid> _chuyenNganhDaoTao;
        partial void On_ChuyenNganhDaoTao_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_ChuyenNganhDaoTao_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> TruongDaoTao
        {
            get
            {
                return _truongDaoTao;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _truongDaoTao;
    			bool stopChanging = false;
                On_TruongDaoTao_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("TruongDaoTao");
    			if(!stopChanging)
    			{
    				_truongDaoTao = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("TruongDaoTao");
    				On_TruongDaoTao_Changed(oldValue, _truongDaoTao);//\\
    			}
            }
        }
    	public static String TruongDaoTao_PropertyName { get { return "TruongDaoTao"; } }
        private Nullable<System.Guid> _truongDaoTao;
        partial void On_TruongDaoTao_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_TruongDaoTao_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> HinhThucDaoTao
        {
            get
            {
                return _hinhThucDaoTao;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _hinhThucDaoTao;
    			bool stopChanging = false;
                On_HinhThucDaoTao_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("HinhThucDaoTao");
    			if(!stopChanging)
    			{
    				_hinhThucDaoTao = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("HinhThucDaoTao");
    				On_HinhThucDaoTao_Changed(oldValue, _hinhThucDaoTao);//\\
    			}
            }
        }
    	public static String HinhThucDaoTao_PropertyName { get { return "HinhThucDaoTao"; } }
        private Nullable<System.Guid> _hinhThucDaoTao;
        partial void On_HinhThucDaoTao_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_HinhThucDaoTao_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<int> NamTotNghiep
        {
            get
            {
                return _namTotNghiep;
            }
            set
            {
    			Nullable<int> oldValue =  _namTotNghiep;
    			bool stopChanging = false;
                On_NamTotNghiep_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("NamTotNghiep");
    			if(!stopChanging)
    			{
    				_namTotNghiep = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("NamTotNghiep");
    				On_NamTotNghiep_Changed(oldValue, _namTotNghiep);//\\
    			}
            }
        }
    	public static String NamTotNghiep_PropertyName { get { return "NamTotNghiep"; } }
        private Nullable<int> _namTotNghiep;
        partial void On_NamTotNghiep_Changing(Nullable<int> currentValue, ref Nullable<int> newValue, ref bool stopChanging);
        partial void On_NamTotNghiep_Changed(Nullable<int> oldValue, Nullable<int> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.DateTime> NgayCapBang
        {
            get
            {
                return _ngayCapBang;
            }
            set
            {
    			Nullable<System.DateTime> oldValue =  _ngayCapBang;
    			bool stopChanging = false;
                On_NgayCapBang_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("NgayCapBang");
    			if(!stopChanging)
    			{
    				_ngayCapBang = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("NgayCapBang");
    				On_NgayCapBang_Changed(oldValue, _ngayCapBang);//\\
    			}
            }
        }
    	public static String NgayCapBang_PropertyName { get { return "NgayCapBang"; } }
        private Nullable<System.DateTime> _ngayCapBang;
        partial void On_NgayCapBang_Changing(Nullable<System.DateTime> currentValue, ref Nullable<System.DateTime> newValue, ref bool stopChanging);
        partial void On_NgayCapBang_Changed(Nullable<System.DateTime> oldValue, Nullable<System.DateTime> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> TrinhDoTinHoc
        {
            get
            {
                return _trinhDoTinHoc;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _trinhDoTinHoc;
    			bool stopChanging = false;
                On_TrinhDoTinHoc_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("TrinhDoTinHoc");
    			if(!stopChanging)
    			{
    				_trinhDoTinHoc = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("TrinhDoTinHoc");
    				On_TrinhDoTinHoc_Changed(oldValue, _trinhDoTinHoc);//\\
    			}
            }
        }
    	public static String TrinhDoTinHoc_PropertyName { get { return "TrinhDoTinHoc"; } }
        private Nullable<System.Guid> _trinhDoTinHoc;
        partial void On_TrinhDoTinHoc_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_TrinhDoTinHoc_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> NgoaiNgu
        {
            get
            {
                return _ngoaiNgu;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _ngoaiNgu;
    			bool stopChanging = false;
                On_NgoaiNgu_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("NgoaiNgu");
    			if(!stopChanging)
    			{
    				_ngoaiNgu = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("NgoaiNgu");
    				On_NgoaiNgu_Changed(oldValue, _ngoaiNgu);//\\
    			}
            }
        }
    	public static String NgoaiNgu_PropertyName { get { return "NgoaiNgu"; } }
        private Nullable<System.Guid> _ngoaiNgu;
        partial void On_NgoaiNgu_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_NgoaiNgu_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<System.Guid> TrinhDoNgoaiNgu
        {
            get
            {
                return _trinhDoNgoaiNgu;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _trinhDoNgoaiNgu;
    			bool stopChanging = false;
                On_TrinhDoNgoaiNgu_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("TrinhDoNgoaiNgu");
    			if(!stopChanging)
    			{
    				_trinhDoNgoaiNgu = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("TrinhDoNgoaiNgu");
    				On_TrinhDoNgoaiNgu_Changed(oldValue, _trinhDoNgoaiNgu);//\\
    			}
            }
        }
    	public static String TrinhDoNgoaiNgu_PropertyName { get { return "TrinhDoNgoaiNgu"; } }
        private Nullable<System.Guid> _trinhDoNgoaiNgu;
        partial void On_TrinhDoNgoaiNgu_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_TrinhDoNgoaiNgu_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);
    
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
        public Nullable<System.Guid> HocHam
        {
            get
            {
                return _hocHam;
            }
            set
            {
    			Nullable<System.Guid> oldValue =  _hocHam;
    			bool stopChanging = false;
                On_HocHam_Changing(oldValue, ref value, ref stopChanging);
                ReportPropertyChanging("HocHam");
    			if(!stopChanging)
    			{
    				_hocHam = StructuralObject.SetValidValue(value);
    				ReportPropertyChanged("HocHam");
    				On_HocHam_Changed(oldValue, _hocHam);//\\
    			}
            }
        }
    	public static String HocHam_PropertyName { get { return "HocHam"; } }
        private Nullable<System.Guid> _hocHam;
        partial void On_HocHam_Changing(Nullable<System.Guid> currentValue, ref Nullable<System.Guid> newValue, ref bool stopChanging);
        partial void On_HocHam_Changed(Nullable<System.Guid> oldValue, Nullable<System.Guid> currentValue);

        #endregion

        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("ERPModel", "FK_NhanVien_NhanVienTrinhDo", "NhanVien")]
        public EntityCollection<NhanVien> NhanViens
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<NhanVien>("ERPModel.FK_NhanVien_NhanVienTrinhDo", "NhanVien");
            }
            set
            {
                if ((value != null))
                {
    				bool stopChanging = false;
    				On_NhanViens_Changing(ref value, ref stopChanging);//\\//
    				if(!stopChanging)
    				{
    					((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<NhanVien>("ERPModel.FK_NhanVien_NhanVienTrinhDo", "NhanVien", value);
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