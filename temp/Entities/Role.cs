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
    
    
    [core.Attributes.TableAttribute(Name="role")]
    [Newtonsoft.Json.JsonObjectAttribute("Role")]
    public sealed class Role : core.Bases.EntityBase
    {
        
        private long _Id;
        
        private string _Name;
        
        [core.Attributes.ColumnAttribute(Name="id", Identity=true, Spatial=false, SequenceName="role_seq")]
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
    }
}
