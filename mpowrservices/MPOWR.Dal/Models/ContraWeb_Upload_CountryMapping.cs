//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MPOWR.Dal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ContraWeb_Upload_CountryMapping
    {
        public int ID { get; set; }
        public int FileID { get; set; }
        public Nullable<int> GeoMappingID { get; set; }
        public short CountryID { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual ContraWeb_Upload_DataFile ContraWeb_Upload_DataFile { get; set; }
        public virtual ContraWeb_Upload_GeoMapping ContraWeb_Upload_GeoMapping { get; set; }
    }
}
