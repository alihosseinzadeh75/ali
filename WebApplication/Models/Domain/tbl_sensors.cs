//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication.Models.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_sensors
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_sensors()
        {
            this.tbl_sensorinformation = new HashSet<tbl_sensorinformation>();
        }
    
        public int ID { get; set; }
        public int UserID { get; set; }
        public int BaseID { get; set; }
        public int TypeID { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public string State { get; set; }
    
        public virtual tbl_base tbl_base { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_sensorinformation> tbl_sensorinformation { get; set; }
        public virtual tbl_types tbl_types { get; set; }
        public virtual tbl_users tbl_users { get; set; }
    }
}
