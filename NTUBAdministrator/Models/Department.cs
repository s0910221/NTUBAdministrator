//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace NTUBAdministrator.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Department
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Department()
        {
            this.DepartmentAuthentication = new HashSet<DepartmentAuthentication>();
        }
    
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentEName { get; set; }
        public string CreateID { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string ModifyID { get; set; }
        public Nullable<System.DateTime> ModifyTime { get; set; }
        public string Record { get; set; }
    
        public virtual UserAccount UserAccount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DepartmentAuthentication> DepartmentAuthentication { get; set; }
        public virtual UserAccount UserAccount1 { get; set; }
    }
}
