using System.Collections.Generic;

namespace core.Models
{
    public class Response
    {
        private string _Exception;

        public object Data { get; set; }

        public bool Success { get; set; }

        public string Exception
        {
            get
            {
                if (!Success && string.IsNullOrEmpty(_Exception))
                {
                    return "Beklenmeyen bir hata oluştu.";
                }

                return _Exception ?? string.Empty;
            }
            set
            {
                _Exception = value;
            }
        }
    }
}
