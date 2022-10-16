namespace WebControllers.Models
{
    public class DoConvertionFromTemplateRequest
    {
        [Newtonsoft.Json.JsonProperty("fontId", Required = Newtonsoft.Json.Required.Always)]
        public int FontId { get; set; }

        [Newtonsoft.Json.JsonProperty("configId", Required = Newtonsoft.Json.Required.Always)]
        public int ConfigId { get; set; }

        [Newtonsoft.Json.JsonProperty("template", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Template { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }
    }
}
