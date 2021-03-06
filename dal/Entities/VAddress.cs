//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace dal.Entities
{
    
    
    [core.Attributes.TableAttribute(Name="v_address")]
    [Newtonsoft.Json.JsonObjectAttribute("VAddress")]
    public sealed class VAddress : core.Bases.EntityBase
    {
        
        private long _Id;
        
        private long _UavtCode;
        
        private string _Name;
        
        private NetTopologySuite.Geometries.Geometry _Geoloc;
        
        [core.Attributes.ColumnAttribute(Name="id", Identity=true, Spatial=false, SequenceName="")]
        [Newtonsoft.Json.JsonPropertyAttribute("Id")]
        public override long Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this._Id = value;
            }
        }
        
        [core.Attributes.ColumnAttribute(Name="uavt_code", Identity=false, Spatial=false, SequenceName="")]
        [Newtonsoft.Json.JsonPropertyAttribute("Uavt Kodu")]
        public long UavtCode
        {
            get
            {
                return this._UavtCode;
            }
            set
            {
                this._UavtCode = value;
            }
        }
        
        [core.Attributes.ColumnAttribute(Name="name", Identity=false, Spatial=false, SequenceName="")]
        [Newtonsoft.Json.JsonPropertyAttribute("Name")]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }
        
        [core.Attributes.ColumnAttribute(Name="geoloc", Identity=false, Spatial=true, SequenceName="")]
        [Newtonsoft.Json.JsonIgnoreAttribute()]
        public NetTopologySuite.Geometries.Geometry Geoloc
        {
            get
            {
                return this._Geoloc;
            }
            set
            {
                this._Geoloc = value;
            }
        }
    }
}
